using Godot;
using System;

public class Player : KinematicBody2D
{
    private Vector2 velocity;

    [Export]
    public int speed = 400;

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        MoveAndCollide(velocity * delta);
    }
  
    public void GetInput()
    {
        velocity = new Vector2();
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= speed;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += speed;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.y -= speed;
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.y += speed;
        }
    }


}
