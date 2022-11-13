using Godot;
using System;

public class Main : Node2D
{

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
