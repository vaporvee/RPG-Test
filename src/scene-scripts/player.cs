using Godot;
using System;
using System.Text.RegularExpressions;

public partial class player : CharacterBody2D
{
    [Export] public static float speed = 1;
    public static bool allowMovement = true;
    public Vector2 movement;
    public AnimatedSprite2D animatedSprite;
    public Marker2D rotCenter;
    public RayCast2D dialogRayCast;
    //console cheats:
    private static Camera2D cheatCam;
    private static Camera2D mainCam;
    private static CollisionShape2D collision;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("animated_sprite_2d");
        rotCenter = GetNode<Marker2D>("rotation_center");
        dialogRayCast = GetNode<RayCast2D>("rotation_center/ray_cast_2d");
        cheatCam = GetNode<Camera2D>("cheat_cam");
        mainCam = GetNode<Camera2D>("main_cam");
        collision = GetNode<CollisionShape2D>("collision_shape");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (allowMovement)
            movement = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        else movement = Vector2.Zero;
        if (Math.Round(movement.Length(), 0) != 0) rotCenter.Rotation = new Vector2((float)Math.Round(movement.X, 0), (float)Math.Round(movement.Y, 0)).Angle();
        MoveAndCollide(movement * speed * 200 * (float)delta);
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept") && dialogRayCast.IsColliding() && allowMovement)
            GetNode<dialog_bubble>("dialog_bubble").GetDialog(dialogRayCast.GetCollider().Get("file").AsString(), (Area2D)dialogRayCast.GetCollider());


        //animation system (with controller support wich cant get normalized vector)
        if (allowMovement == false)
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
            animatedSprite.SpeedScale = Math.Abs(movement.X * speed * 1.3f);
        }
        else if (Math.Round(movement.Y, 0) != 0)
        {
            if (movement.Y > 0.05) animatedSprite.Animation = "move_down";
            if (movement.Y < 0.05) animatedSprite.Animation = "move_up";
            animatedSprite.FlipH = false;
            animatedSprite.SpeedScale = Math.Abs(movement.Y * speed * 1.3f);
        }
    }
    public int BeginDialogue(int i)
    {
        return 0;
    }
    public void OnAnimationChanged()
    {
        if (animatedSprite.Animation == "move_side")
        {
            //GetNode<CollisionShape2D>("collision_shape").Shape
        }
        else
        {

        }
    }

    //CONSOLE CHEATS
    public static string CheatCam()
    {

        if (mainCam.Enabled)
        {
            cheatCam.Enabled = true;
            mainCam.Enabled = false;
            return "cheat_cam has been enabled\n";
        }
        else
        {
            cheatCam.Enabled = false;
            mainCam.Enabled = true;
            return "cheat_cam has been disabled\n";
        }
    }
    public static string CollisionToggle()
    {
        collision.Disabled = !collision.Disabled;
        return ("Noclip is now set to: " + collision.Disabled + "\n");
    }
}
