//WORK IN PROGRESS this code is very messy and will be fixed when it works
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class dialog_bubble : CanvasLayer
{
    public Dictionary allDialog;
    public List<string> currentDialogList = new List<string>();
    public string currentKey;
    public string currentDialogLine;
    public int dialogCounter;
    public string userName;
    public void ImportString(string dialogFile, string playerName)
    {
        userName = playerName;
        GetNode("/root/main/player").Call("ChangeProcess", false);
        currentKey = "multiTipp0";//2 is a selection menu needs to be planned before coding
        using var file = FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read);
        string text = file.GetAsText();
        allDialog = (Dictionary)JSON.ParseString(text);
        GetNode<Label>("NameLabel").Text = allDialog["dialogTitle"].ToString();
        addText();


        //Todo: add multiline text and actual running dialog. And go through string for other dictionaries wich always will be playeranswers

    }
    public void EndDialog()
    {
        currentDialogList = new List<string>();
        dialogCounter = 0;
        Visible = false;
        GetNode("/root/main/player").Call("ChangeProcess", true);
    }
    public void addText()
    {
        if (currentKey.StartsWith("random"))
        {
            string[] dialogRand = allDialog[currentKey].AsStringArray();
            currentDialogList.Add(dialogRand[GD.Randi() % dialogRand.Length]);
        }
        if (currentKey.StartsWith("multi"))
        {
            string[] dialogMulti = allDialog[currentKey].AsStringArray();
            currentDialogList.AddRange(dialogMulti);
        }
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
            if (dialogCounter > currentDialogList.Count)
                EndDialog();
        }
    }
}
