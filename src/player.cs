using Godot;
using System;
public partial class player : CharacterBody2D
{
	[Export]
	public int speed = 400;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
}
