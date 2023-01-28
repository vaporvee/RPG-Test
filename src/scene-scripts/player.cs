using Godot;
using System;

public partial class player : CharacterBody2D
{
    [Export] public string playerName;
	[Export] public int speed = 200;
    public Vector2 movement;
    public AnimatedSprite2D animatedSprite;
    public Marker2D rotCenter;
    public RayCast2D rayCast;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("animated_sprite_2d");
        rotCenter = GetNode<Marker2D>("rotation_center");
        rayCast = GetNode<RayCast2D>("rotation_center/ray_cast_2d");
    }
    public void ChangeProcess(bool process) 
    {
        if (process) ProcessMode = ProcessModeEnum.Inherit; else ProcessMode = ProcessModeEnum.Disabled; 
        animatedSprite.Frame = 0;
    }
    public override void _PhysicsProcess(double delta)
	{
        movement = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if(movement.Length() != 0) rotCenter.Rotation = new Vector2((float)Math.Round(movement.x,0),(float)Math.Round(movement.y,0)).Angle();
        MoveAndCollide(movement * speed * (float)delta);
    }
    public override void _Process(double delta)
    {
        //call event in raycasted object
        if (Input.IsActionJustPressed("ui_accept") && rayCast.IsColliding())
            rayCast.GetCollider().Call("OnInteraction", playerName);

        //animation system (with controller support wich cant get normalized vector)
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
            animatedSprite.SpeedScale = Math.Abs(movement.x * speed/150);
        }
        else if (Math.Round(movement.y, 0) != 0)
        {
            if (movement.y > 0.05) animatedSprite.Animation = "move_down";
            if (movement.y < 0.05) animatedSprite.Animation = "move_up";
            animatedSprite.FlipH = false;
            animatedSprite.SpeedScale = Math.Abs(movement.y * speed/150);
        }
    }
}
