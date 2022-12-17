using Godot;
using System;

public partial class dialog_trigger_area : Area2D
{
    [Export(PropertyHint.File, "*json")]
    string dialogFile;
    public void OnInteraction(string playerName)
    {
        GetNode("/root/main/dialog_bubble").Call("ImportString",dialogFile,playerName);
    }
}
