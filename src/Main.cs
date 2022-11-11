using Godot;
using System;

public class Main : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

  public override async void _Process(float delta)
  {
        if (Input.IsActionJustReleased("fullscreen"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
  }
}
