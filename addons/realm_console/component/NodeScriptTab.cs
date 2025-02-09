using Godot;
using System;
using Godot.Collections;

public partial class NodeScriptTab : Control
{
    private ScriptStatu _statu = ScriptStatu.Stop;

    public enum ScriptStatu
    {
        Stop,
        Running,
        Pause
    }

    public ScriptStatu Statu
    {
        get => _statu;
        set
        {
            var oldValue = _statu;

            var timeString = DateTime.Now.ToString("HH:mm:ss");
            switch (value)
            {
                case ScriptStatu.Stop:
                    Host.Call("unmount", ScriptNode);
                    ScriptNode = null;
                    OutputText.PrintErr($"[{timeString}] 脚本已停止");
                    _statu = ScriptStatu.Stop;
                    CodeEdit.Editable = true;
                    break;
                case ScriptStatu.Running:
                    switch (oldValue)
                    {
                        case ScriptStatu.Stop:
                            {
                                var result = Host.Call("mount", CodeEdit.Text, Vars);
                                switch (result.Obj)
                                {
                                    case Node node:
                                        ScriptNode = node;
                                        ScriptNode.SetProcess(true);
                                        ScriptNode.SetPhysicsProcess(true);
                                        _statu = ScriptStatu.Running;
                                        CodeEdit.Editable = false;
                                        OutputText.PrintSuccess($"[{timeString}] 脚本已启动");
                                        break;
                                    case string err:
                                        OutputText.PrintErr($"[{timeString}] 脚本启动失败: {err}");
                                        break;
                                    default:
                                        OutputText.PrintErr($"[{timeString}] 脚本启动失败: 未知错误");
                                        break;
                                }
                                break;
                            }
                        case ScriptStatu.Pause:
                            ScriptNode.SetProcess(true);
                            ScriptNode.SetPhysicsProcess(true);
                            OutputText.PrintWarn($"[{timeString}] 脚本已恢复");
                            _statu = ScriptStatu.Running;
                            break;
                    }
                    break;
                case ScriptStatu.Pause:
                    ScriptNode.SetProcess(false);
                    ScriptNode.SetPhysicsProcess(false);
                    OutputText.PrintWarn($"[{timeString}] 脚本已暂停");
                    _statu = ScriptStatu.Pause;
                    break;
            }

            ColorRect.Color = Statu switch
            {
                ScriptStatu.Stop => new Color("c94f4f"),
                ScriptStatu.Running => new Color("5fad65"),
                ScriptStatu.Pause => new Color("ced0d6"),
                _ => throw new ArgumentOutOfRangeException()
            };
            RunButton.Disabled = Statu == ScriptStatu.Running;
            PauseButton.Disabled = Statu != ScriptStatu.Running;
            StopButton.Disabled = Statu == ScriptStatu.Stop;
        }
    }

    public RealmConsole.RealmConsole RealmConsole { get; set; } = default!;
    public Node Host => RealmConsole.Host;
    public Node? ScriptNode { get; set; } = default;
    public Dictionary Vars { get; set; } = default!;

    public ColorRect ColorRect { get; set; } = default!;
    public Button RunButton { get; set; } = default!;
    public Button PauseButton { get; set; } = default!;
    public Button StopButton { get; set; } = default!;
    public CodeEdit CodeEdit { get; set; } = default!;
    public OutputText OutputText { get; set; } = default!;

	public override void _Ready()
    {
        Vars = RealmConsole.HostVars.Duplicate();

        ColorRect = GetNode<ColorRect>("%ColorRect");
        RunButton = GetNode<Button>("%RunButton");
        PauseButton = GetNode<Button>("%PauseButton");
        StopButton = GetNode<Button>("%StopButton");
        CodeEdit = GetNode<CodeEdit>("%CodeEdit");
        OutputText = GetNode<OutputText>("%OuputText");

        RunButton.Pressed += Run;
        PauseButton.Pressed += Pause;
        StopButton.Pressed += Stop;

        Vars["Output"] = OutputText;
	}

	public override void _Process(double delta)
	{

	}

    public void Run()
    {
        Statu = ScriptStatu.Running;
    }

    public void Pause()
    {
        Statu = ScriptStatu.Pause;
    }

    public void Stop()
    {
        Statu = ScriptStatu.Stop;
    }
}
