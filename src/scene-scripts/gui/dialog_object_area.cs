using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class dialog_object_area : Area2D
{
    [Export]
    public string dialogKey = "";
    public bool areaActive = false;

    public override void _Input(InputEvent @event)
    {
        if(areaActive && @event.IsActionPressed("ui_accept"))
        {
            EmitSignal("DialogDisplayEventHandler", dialogKey);
        }
    }
    public void onAreaEntered()
    {
        areaActive = true;
    }
    public void onAreaExited()
    {
        areaActive = false;
    }
}
