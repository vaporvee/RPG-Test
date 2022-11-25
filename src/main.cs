using Godot;
using System;

public partial class main : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
