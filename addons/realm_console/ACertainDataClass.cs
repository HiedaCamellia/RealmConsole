namespace RealmConsole;

using Godot;

public partial class ACertainDataClass : GodotObject
{
    public float AlphaNumber { get; set; }
    public string BetaString { get; set; } = default!;
    public int GammaInt { get; set; }
    public override string ToString() => $"Alpha: {AlphaNumber}, Beta: {BetaString}, Gamma: {GammaInt}";
}
