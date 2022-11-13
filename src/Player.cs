using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public int speed = 600;

    public override void _PhysicsProcess(float delta)
    {
        MoveAndCollide(new Vector2
            (
            Input.GetActionStrength("move_right") 
            - Input.GetActionStrength("move_left"), 
            Input.GetActionStrength("move_down") 
            - Input.GetActionStrength("move_up")
            ).LimitLength(1) 
            * speed * delta
            );
    }

    public override void _Process(float delta)
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
