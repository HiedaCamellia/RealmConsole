namespace RealmConsole;

#if TOOLS
using Godot;
using System;

[Tool]
public partial class Plugin : EditorPlugin
{
    private static void Print(string what)
    {
        GD.Print($"[RealmConsole] {what}");
    }

	public override void _EnterTree()
    {
        if (!ProjectSettings.HasSetting("input/realm_console_toggle"))
        {
            Print("Adding realm_console_toggle to input map");

            ProjectSettings.SetSetting("input/realm_console_toggle",
                new Godot.Collections.Dictionary
                {
                    { "deadzone", 0.5f },
                    { "events", new Godot.Collections.Array { new InputEventKey { Keycode = Key.Quoteleft } } }
                });

            ProjectSettings.Save();
        }

        if (!ProjectSettings.HasSetting("input/realm_console_hide"))
        {
            Print("Adding realm_console_hide to input map");

            ProjectSettings.SetSetting("input/realm_console_hide",
                new Godot.Collections.Dictionary
                {
                    { "deadzone", 0.5f },
                    { "events", new Godot.Collections.Array { new InputEventKey { Keycode = Key.Escape } } }
                });

            ProjectSettings.Save();
        }

        AddAutoloadSingleton("RealmConsole", "res://addons/realm_console/RealmConsole.cs");
    }

	public override void _ExitTree()
	{
		RemoveAutoloadSingleton("RealmConsole");
	}
}
#endif
