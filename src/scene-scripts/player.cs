using Godot;
using System;
using System.Diagnostics;

public partial class player : CharacterBody2D
{
    [Export]
    public string playerName;
	[Export]
	public int speed = 400;

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
    public void ChangeProcess(bool process){ if(process) ProcessMode = ProcessModeEnum.Inherit; else ProcessMode = ProcessModeEnum.Disabled; }
    public override void _Process(double delta)
    {
        //set ray_cast target position
        if (Input.IsActionJustPressed("move_right")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(128, 0);
        if (Input.IsActionJustPressed("move_left")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(-128, 0);
        if (Input.IsActionJustPressed("move_down")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(0, 128);
        if (Input.IsActionJustPressed("move_up")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(0, -128);

        //call event in raycasted object
        if (Input.IsActionJustPressed("ui_accept") && GetNode<RayCast2D>("ray_cast_2d").IsColliding()) 
            GetNode<RayCast2D>("ray_cast_2d").GetCollider().Call("OnInteraction", playerName);
    }
}
