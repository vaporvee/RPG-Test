using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public int speed = 400;
    public override void _Ready()
    {
        
    }

  public override void _Process(float delta)
  {
        if (Input.IsActionPressed("move_left"))
        {
            Position -= new Vector2(speed, 0) * delta;
        }
        if (Input.IsActionPressed("move_right"))
        {
            Position += new Vector2(speed, 0) * delta;
        }
        if (Input.IsActionPressed("move_up"))
        {
            Position -= new Vector2(0, speed) * delta;
        }
        if (Input.IsActionPressed("move_down"))
        {
            Position += new Vector2(0, speed) * delta;
        }
    }
}
