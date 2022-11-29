using Godot;
using System;
public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")]
    string dialogFile;
    public override void _Ready()
	{
        using var file = FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read);
        string text = file.GetAsText();
        GD.Print(text);
	}

}
