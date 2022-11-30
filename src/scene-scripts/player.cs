using Godot;
using System;
public partial class player : CharacterBody2D
{
	[Export]
	public int speed = 400;

    public override void _Ready()
    {
    }
    public override void _PhysicsProcess(double delta)
	{
        MoveAndCollide(new Vector2
            (
            Input.GetActionStrength("move_right")
            - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_down")
            - Input.GetActionStrength("move_up")
            ).LimitLength(1)
            * speed * (float)delta
            );
    }
    public override void _Process(double delta)
    {
        //rotates just interaction_area instead of whole player
        if (Input.IsActionJustPressed("move_right")) GetNode<Area2D>("interaction_area").Rotation = Mathf.Pi / -2;
        if (Input.IsActionJustPressed("move_left")) GetNode<Area2D>("interaction_area").Rotation = Mathf.Pi / 2;
        if (Input.IsActionJustPressed("move_down")) GetNode<Area2D>("interaction_area").Rotation = 0;
        if (Input.IsActionJustPressed("move_up")) GetNode<Area2D>("interaction_area").Rotation = Mathf.Pi;
    }
}
