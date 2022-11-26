using Godot;
using System;

public partial class main : Node2D
{
    [Export]
    public string currentController = Input.GetJoyName(0);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //gamepad
        currentController = Input.GetJoyName(0);
        if (currentController.StartsWith("Nintendo"))
        {
            
        }
        //fullscreen
        if (Input.IsActionJustPressed("hotkey_fullscreen"))
        {
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            }
        }
    }

}
