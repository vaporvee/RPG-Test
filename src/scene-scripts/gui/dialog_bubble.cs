using Godot;
using Godot.Collections;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public string currentDialogLine;
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

        //Todo: add multiline text and actual running dialog. And go through string for other dictionaries wich always will be playeranswers

        if (currentKey.StartsWith("random"))
        {
            string[] dialogRand = allDialog[currentKey].AsStringArray();
            currentDialogLine = dialogRand[GD.Randi() % dialogRand.Length];
        }
        else currentDialogLine = allDialog[currentKey].AsString();
        currentDialogLine = String.Format(currentDialogLine, playerName);
        GetNode<Label>("TextLabel").Text = currentDialogLine;
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
        if(debugCounter > 1)
        {
            EndDialog();
            debugCounter = 0;
        }
    }
}
