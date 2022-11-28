using Godot;
using System;

public partial class dialog_object_area : Area2D
{
    [Export]
    public string dialogKey = "";
    public bool areaActive = false;

    public override void _Input(InputEvent @event)
    {
        if(areaActive && @event.IsActionPressed("ui_accept"))
        {
           // signal_bus.EmitSignal("DialogDisplay", dialogKey);
        }
    }
    public void onDialogAreaEntered()
    {
        areaActive = true;
    }
    public void onDialogAreaExited()
    {
        areaActive = false;
    }
}
