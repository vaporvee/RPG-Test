using Godot;
using System;

public class main : Node2D
{

    public override void _Ready()
    {

    }

  public override void _Process(float delta)
  {
        if (Input.IsActionJustReleased("fullscreen"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
  }
}
