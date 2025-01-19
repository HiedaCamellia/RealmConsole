namespace RealmConsole;

using Godot;
using System;
using Godot.Collections;

public partial class RealmConsole : CanvasLayer
{
    public static RealmConsole Default = null!;

    public Node Host { get; set; } = default!;

    public Dictionary HostVars => (Dictionary)Host.Get("Vars");

    private ACertainDataClass Data { get; set; } = new ACertainDataClass();

    private RichTextLabel _outputTextBox;
    private CodeEdit _codeEdit;
    private Button _exeButton;

    public RealmConsole()
    {
        if (Default is null)
        {
            Default = this;
        }

        Layer = 9999;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Ready()
    {
        Visible = false;

        Host = (Node)GD.Load<GDScript>("res://addons/realm_console/realm_console_host.gd").New();
        AddChild(Host);

        var console = GD.Load<PackedScene>("res://addons/realm_console/RealmConsole.tscn").Instantiate();
        AddChild(console);

        _outputTextBox = console.GetNode<RichTextLabel>("%OutputTextBox");
        _codeEdit = console.GetNode<CodeEdit>("%CodeEdit");
        _exeButton = console.GetNode<Button>("%ExeButton");

        _exeButton.Pressed += () =>
        {
            Host.Call("mount", _codeEdit.Text);
            Data.GammaInt++;
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("realm_console_toggle") && !_codeEdit.HasFocus())
        {
            Visible = !Visible;
            if (Visible)
            {
                Callable.From(() => _codeEdit.GrabFocus()).CallDeferred();
            }

        }
        else if (@event.IsActionPressed("realm_console_hide") && Visible)
        {
            Visible = false;
        }
    }
}
