using Godot;
using System;

public partial class dialog_bubble : CanvasLayer
{
    
    public override void _Process(double delta)
    {
    }
    public static void SetDialog(string dialogFile)
    {
        var parsedDialog = Json.ParseString(FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read).GetAsText());
        json
    }
}
