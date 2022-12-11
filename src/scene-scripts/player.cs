using Godot;
using System;

public partial class player : CharacterBody2D
{
    [Export]
    public string playerName;
	[Export]
	public int speed = 400;
    public Vector2 velocity;
    public AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    public void ChangeProcess(bool process) 
    {
        if (process) ProcessMode = ProcessModeEnum.Inherit; else ProcessMode = ProcessModeEnum.Disabled; 
        animatedSprite.Frame = 0;
    }
    public override void _PhysicsProcess(double delta)
	{
        velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down").LimitLength(1);
        MoveAndCollide(velocity * speed * (float)delta);
    }
    public override void _Process(double delta)
    {
        //set ray_cast target position
        int raylength = 64;
        if (Input.IsActionJustPressed("move_right")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(raylength, 0);
        if (Input.IsActionJustPressed("move_left")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(-raylength, 0);
        if (Input.IsActionJustPressed("move_down")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(0, raylength);
        if (Input.IsActionJustPressed("move_up")) GetNode<RayCast2D>("ray_cast_2d").TargetPosition = new Vector2(0, -raylength);
        //call event in raycasted object
        if (Input.IsActionJustPressed("ui_accept") && GetNode<RayCast2D>("ray_cast_2d").IsColliding())
            GetNode<RayCast2D>("ray_cast_2d").GetCollider().Call("OnInteraction", playerName);
        //animation system (with controller support wcih cant get normalized vector)
        if (velocity.Length() != 0) 
            animatedSprite.Play();
        else
        {
            animatedSprite.Frame = 0;
            animatedSprite.Stop();
        }
        if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left"))
        {
            animatedSprite.Animation = "move_side";
            animatedSprite.FlipH = velocity.x < 0.5;
            animatedSprite.SpeedScale = Math.Abs(velocity.x);
        }
        else if (Input.IsActionPressed("move_up") || Input.IsActionPressed("move_down"))
        {
            if (velocity.y > 0.05) animatedSprite.Animation = "move_down";
            if (velocity.y < 0.05) animatedSprite.Animation = "move_up";
            animatedSprite.FlipH = false;
            animatedSprite.SpeedScale = Math.Abs(velocity.y);
        }
    }
}
