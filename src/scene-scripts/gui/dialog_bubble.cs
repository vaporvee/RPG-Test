using Godot;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public void ImportString(string dialogTitle, string printedDialog)
    {
        Visible = !Visible;
        GetNode<Label>("TextLabel").Text = printedDialog;
        GetNode<Label>("NameLabel").Text = dialogTitle;
    }
}
