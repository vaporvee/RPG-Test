using Godot;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")] string file;
    [Export] string title;
    [Export] bool introducedVillager;
}
