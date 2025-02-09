using Godot;
using System;

public partial class OutputText : RichTextLabel
{
	public void Print(string text)
    {
        Text += $"{EscapeBBCode(text)}\n";
    }

    public void PrintErr(string text)
    {
        Text += $"[color=red]{EscapeBBCode(text)}[/color]\n";
    }

    public void PrintWarn(string text)
    {
        Text += $"[color=yellow]{EscapeBBCode(text)}[/color]\n";
    }

    public void PrintSuccess(string text)
    {
        Text += $"[color=green]{EscapeBBCode(text)}[/color]\n";
    }

    public void PrintInfo(string text)
    {
        Text += $"[color=blue]{EscapeBBCode(text)}[/color]\n";
    }

    public void PrintRich(string text)
    {
        Text += text + "\n";
    }

    public new void Clear()
    {
        Text = string.Empty;
    }

    public override void _Ready()
    {

    }

    // Returns escaped BBCode that won't be parsed by RichTextLabel as tags.
    private string EscapeBBCode(string bbcodeText)
    {
        // We only need to replace opening brackets to prevent tags from being parsed.
        return bbcodeText.Replace("[", "[lb]");
    }

    // Appends the user's message as-is, without escaping. This is dangerous!
    private void AppendChatLine(string username, string message)
    {
        AppendText($"{username}: [color=green]{message}[/color]\n");
    }

    // Appends the user's message with escaping.
    // Remember to escape both the player name and message contents.
    private void AppendChatLineEscaped(string username, string message)
    {
        AppendText($"{EscapeBBCode(username)}: [color=green]{EscapeBBCode(message)}[/color]\n");
    }
}
