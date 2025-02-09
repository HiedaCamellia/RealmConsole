namespace RealmConsole;

using Godot;

public partial class Game : Control
{
    public int Coin = 0;

    private Button _button0 = default!;
    private RichTextLabel _text = default!;


    public override void _Ready()
    {
        RealmConsole.Default.HostVars["Game"] = this;

        _button0 = GetNode<Button>("%Button0");
        _text = GetNode<RichTextLabel>("%Text");

        _button0.Pressed += () => { Coin += 1; };
    }

    public override void _Process(double delta)
    {
        _text.Text = $"当前资源: {Coin}";
    }
}
