using Godot;
using Godot.Collections;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")]
    string dialogFile;
    [Export]
    string dialogTitle;
    public string printedDialog;
    public void OnInteraction(string playerName)
    {
        string currentKey = "randomWelcomeText";
        using var file = FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read);
        string text = file.GetAsText();

        var jsonFile = JSON.ParseString(text);
        Dictionary allDialog = (Dictionary)jsonFile;
        if (currentKey.BeginsWith("random")) { 
            string[] dialogPart = allDialog[currentKey].AsStringArray();
            printedDialog = dialogPart[GD.Randi() % dialogPart.Length];
        } else printedDialog = allDialog[currentKey].AsString();
        printedDialog = String.Format(printedDialog, playerName);

        GetNode("/root/main/dialog_bubble").Call("ImportString",dialogTitle, printedDialog);
    }
}
