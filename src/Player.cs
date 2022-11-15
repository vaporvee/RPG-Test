using Godot;
using System;

public class player : KinematicBody2D
{
    [Export]
    public int speed = 400;

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
        int currentCellID = 1;
        var tilemap = GetNode<TileMap>("/root/Main/World/Foreground");
        Vector2 coordinates = tilemap.WorldToMap(Position);
        if (Input.IsActionJustReleased("debug"))
        { 
        GD.Print("All Number 1 tiles: " + tilemap.GetUsedCellsById(1));
        GD.Print("Player coordinate: " + coordinates);
        }
        if (Input.IsKeyPressed((int)KeyList.F1))
        {
            tilemap.SetCell((int)coordinates.x, (int)coordinates.y, -1);
        }
        if (Input.IsKeyPressed((int)KeyList.F2))
        {
            if(tilemap.GetCell((int)coordinates.x, (int)coordinates.y) == -1)
            {
            tilemap.SetCell((int)coordinates.x, (int)coordinates.y, currentCellID); //place offset coordinates += offset = build coordinates
            }
        }
    }
}
