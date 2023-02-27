using Godot;
using System;

public partial class slime : CharacterBody2D
{
    [Export] int speed = 70;
    Vector2 motion = Vector2.Zero;
    public override void _Ready() => GetNode<AnimatedSprite2D>("animated_sprite_2d").Play();
    public override void _PhysicsProcess(double delta)
    {
        if (GetNode<VisibleOnScreenNotifier2D>("visible_notifier_2d").IsOnScreen())
            motion = Position.DirectionTo(player.globalPlayerPosition) * speed;
        else motion = Vector2.Zero;
        Velocity = motion;
        MoveAndSlide();
    }
}
