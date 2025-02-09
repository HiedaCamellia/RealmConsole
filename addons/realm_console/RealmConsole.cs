namespace RealmConsole;

using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class RealmConsole : CanvasLayer
{
    public static RealmConsole Default = null!;

    public Node Host { get; set; } = default!;

    public Dictionary HostVars { get; } = new Dictionary();

    private TabBar _tabBar = default!;
    private Button _addButton = default!;
    private PanelContainer _scriptTabContainer = default!;

    private List<NodeScriptTab> _nodeScriptTabs = [];

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

        _tabBar = console.GetNode<TabBar>("%TabBar");
        _addButton = console.GetNode<Button>("%AddButton");
        _scriptTabContainer = console.GetNode<PanelContainer>("%ScriptTabContainer");

        _tabBar.TabChanged += TabChanged;
        _addButton.Pressed += AddTab;
        _tabBar.TabClosePressed += CloseTab;
    }

    private void CloseTab(long tab)
    {
        int t = (int)tab;
        if(t >= 0 && t < _nodeScriptTabs.Count)
        {
            var nodeScriptTab = _nodeScriptTabs[t];
            nodeScriptTab.Stop();
            nodeScriptTab.QueueFree();
            _nodeScriptTabs.RemoveAt(t);
            _tabBar.RemoveTab(t);
        }
    }

    private void TabChanged(long tab)
    {
        int t = (int)tab;
        _nodeScriptTabs.ForEach((tab) => tab.Visible = false);
        if(t >= 0 && t < _nodeScriptTabs.Count)
        {
            _nodeScriptTabs[t].Visible = true;
        }
    }

    private void AddTab()
    {
        var tab = GD.Load<PackedScene>("res://addons/realm_console/component/NodeScriptTab.tscn")
            .Instantiate<NodeScriptTab>();
        tab.RealmConsole = this;
        _scriptTabContainer.AddChild(tab);

        _nodeScriptTabs.Add(tab);

        _tabBar.AddTab($"Script {_nodeScriptTabs.Count}");
        _tabBar.CurrentTab = _nodeScriptTabs.Count - 1;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("realm_console_toggle") && !Visible)
        {
            Visible = true;

        }
        else if (@event.IsActionPressed("realm_console_hide") && Visible)
        {
            Visible = false;
        }
    }
}
