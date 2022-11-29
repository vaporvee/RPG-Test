using Godot;
using Godot.Collections;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")]
    string dialogFile;

    public string currentKey = "randomWelcomeText";
    public override void _Ready()
    {
        using var file = FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read);
        string text = file.GetAsText();

        var jsonFile = JSON.ParseString(text);
        Dictionary allDialog = (Dictionary)jsonFile;
        try
        {
            string[] dialogPart = allDialog[currentKey].AsStringArray();
            GD.Print(dialogPart[GD.Randi() % dialogPart.Length]);
        } 
        catch 
        { 
            GD.Print(allDialog[currentKey]);
        }
    }

}
