//WORK IN PROGRESS this code is very messy and will be fixed when it works
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class dialog_bubble : CanvasLayer
{
    public List<string> currentDialogList = new List<string>();
    public string currentDialogLine;
    public int dialogCounter;
    public string userName;
    public void ImportString(string dialogTitle, string dialogFile, string playerName)
    {
        userName = playerName;
        GetNode<Label>("NameLabel").Text = dialogTitle;
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
            currentDialogList.Add(dialogRand[GD.Randi() % dialogRand.Length]);
            currentDialogList.Add("Test!");
        }
        else currentDialogList.Add(allDialog[currentKey].AsString());

    }
    public void EndDialog()
    {
        currentDialogList = new List<string>();
        dialogCounter = 0;
        Visible = false;
        GetNode("/root/main/player").Call("ChangeProcess", true);
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            dialogCounter++;
            if (currentDialogList.Count >= dialogCounter)
            {
                Visible = true;
                currentDialogLine = currentDialogList[dialogCounter - 1];
                currentDialogLine = String.Format(currentDialogLine, userName);
                GetNode<Label>("TextLabel").Text = currentDialogLine;
            }
            else GD.Print("No valid dialog available");
        }
        if(dialogCounter > currentDialogList.Count) 
            EndDialog();
    }
}
