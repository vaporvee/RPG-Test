using Godot;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Signal] public delegate void InteractDialogueEventHandler();
    [Export(PropertyHint.File, "*json")] string dialogFile;
    public void OnInteraction(string playerName)
    {
        
    }
}
