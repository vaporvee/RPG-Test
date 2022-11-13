using Godot;
using System;

public class Player : KinematicBody2D
{
    private Vector2 velocity;
    private Vector2 strength;
    [Export]
    public int speed = 400;

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        MoveAndCollide(velocity * strength * delta);
    }

    public void GetInput()
    {
        velocity = new Vector2();
        strength = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),Input.GetActionStrength("move_up") - Input.GetActionStrength("move_down")).LimitLength(1).Abs();
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
        if (Input.IsKeyPressed((int)KeyList.F1))
        {
            GD.Print(strength);
        }
    }

    public override async void _Process(float delta)
    {
        //debug the grid
        var tilemap = GetNode<TileMap>("/root/Main/World/Foreground");
        Vector2 coordinates = tilemap.WorldToMap(Position);
        if (Input.IsKeyPressed((int)KeyList.F3))
        { 
        GD.Print(coordinates);
        }
        if (Input.IsKeyPressed((int)KeyList.F2))
        {
            tilemap.SetCell((int)coordinates.x, (int)coordinates.y, 1);
        }
    }

}
