using Godot;
using System;

public partial class player : CharacterBody2D
{

    [Export] public string playerName;
	[Export] public int speed = 200;
    public bool allowMovement = true;
    public Vector2 movement;
    public AnimatedSprite2D animatedSprite;
    public Marker2D rotCenter;
    public RayCast2D dialogRayCast;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("animated_sprite_2d");
        rotCenter = GetNode<Marker2D>("rotation_center");
        dialogRayCast = GetNode<RayCast2D>("rotation_center/ray_cast_2d");
    }
    public override void _PhysicsProcess(double delta)
	{
        if(allowMovement) movement = Input.GetVector("move_left", "move_right", "move_up", "move_down"); 
            else movement = Vector2.Zero;
        if(movement.Length() != 0) rotCenter.Rotation = new Vector2((float)Math.Round(movement.X,0),(float)Math.Round(movement.Y,0)).Angle();
        MoveAndCollide(movement * speed * (float)delta);
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept") && dialogRayCast.IsColliding() && allowMovement)
            GetNode<dialog_bubble>("dialog_bubble").GetDialog(dialogRayCast.GetCollider().Get("dialogFile").AsString());

        //animation system (with controller support wich cant get normalized vector)
        if(allowMovement == false)
        {
            animatedSprite.Stop();
            animatedSprite.Frame = 0;
        }
        if (movement.Length() != 0)
            animatedSprite.Play();
        else
        {
            animatedSprite.Frame = 0;
            animatedSprite.Stop();
        }
        if (Math.Round(movement.X, 0) != 0)
        {
            animatedSprite.Animation = "move_side";
            animatedSprite.FlipH = movement.X < 0.5;
            animatedSprite.SpeedScale = Math.Abs(movement.X * speed/150);
        }
        else if (Math.Round(movement.Y, 0) != 0)
        {
            if (movement.Y > 0.05) animatedSprite.Animation = "move_down";
            if (movement.Y < 0.05) animatedSprite.Animation = "move_up";
            animatedSprite.FlipH = false;
            animatedSprite.SpeedScale = Math.Abs(movement.Y * speed/150);
        }
    }
}
