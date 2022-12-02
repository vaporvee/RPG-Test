using Godot;
using Godot.Collections;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public string printedDialog;
    public int debugCounter;
    public void ImportString(string dialogTitle, string dialogFile, string playerName)
    {
        Visible = true;
        GD.Print("test");
        GetNode("/root/main/player").Call("ChangeProcess",false);
        string currentKey = "randomWelcomeText";
        using var file = FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read);
        string text = file.GetAsText();

        var jsonFile = JSON.ParseString(text);
        Dictionary allDialog = (Dictionary)jsonFile;
        if (currentKey.BeginsWith("random"))
        {
            string[] dialogPart = allDialog[currentKey].AsStringArray();
            printedDialog = dialogPart[GD.Randi() % dialogPart.Length];
        }
        else printedDialog = allDialog[currentKey].AsString();
        printedDialog = String.Format(printedDialog, playerName);
        GetNode<Label>("TextLabel").Text = printedDialog;
        GetNode<Label>("NameLabel").Text = dialogTitle;
    }
    public void EndDialog()
    {
        Visible = false;
        GetNode("/root/main/player").Call("ChangeProcess", true);
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            debugCounter++;
        }
        if(debugCounter == 2)
        {
            EndDialog();
            debugCounter = 0;
        }
    }
}
