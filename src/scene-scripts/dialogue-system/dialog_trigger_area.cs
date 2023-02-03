using Godot;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")] string file;
    [Export] string title;
    public Variant actor;
    public override void _Ready()
    {
        actor = GetParent(); //must be the scene's root node
    }
}
