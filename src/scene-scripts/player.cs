using Godot;
using System;

public partial class player : CharacterBody2D
{
    [Export] public string playerName;
	[Export] public int speed = 400;
    public float rayCastLength;
    public Vector2 movement;
    public AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        rayCastLength = GetNode<RayCast2D>("ray_cast_2d").TargetPosition.y;
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    public void ChangeProcess(bool process) 
    {
        if (process) ProcessMode = ProcessModeEnum.Inherit; else ProcessMode = ProcessModeEnum.Disabled; 
        animatedSprite.Frame = 0;
    }
    public override void _PhysicsProcess(double delta)
	{
        movement = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        MoveAndCollide(movement * speed * (float)delta);
    }
    public override void _Process(double delta)
    {
        //set ray_cast target position
        RayCast2D rayCast = GetNode<RayCast2D>("ray_cast_2d");
        Vector2 rayCastPosition = new Vector2((float)Math.Round(movement.x), (float)Math.Round(movement.y)) * rayCastLength;
        if (rayCastPosition.Length() != 0) rayCast.TargetPosition = rayCastPosition;
        //call event in raycasted object
        if (Input.IsActionJustPressed("ui_accept") && rayCast.IsColliding())
            rayCast.GetCollider().Call("OnInteraction", playerName);
        //animation system (with controller support wcih cant get normalized vector)
        if (movement.Length() != 0)
            animatedSprite.Play();
        else
        {
            animatedSprite.Frame = 0;
            animatedSprite.Stop();
        }
        if (Math.Round(movement.x, 0) != 0)
        {
            animatedSprite.Animation = "move_side";
            animatedSprite.FlipH = movement.x < 0.5;
            animatedSprite.SpeedScale = Math.Abs(movement.x);
        }
        else if (Math.Round(movement.y, 0) != 0)
        {
            if (movement.y > 0.05) animatedSprite.Animation = "move_down";
            if (movement.y < 0.05) animatedSprite.Animation = "move_up";
            animatedSprite.FlipH = false;
            animatedSprite.SpeedScale = Math.Abs(movement.y);
        }
    }
}
