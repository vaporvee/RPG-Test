using Godot;
using Godot.Collections;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDialog;
    public override void _Process(double delta)
    {
    }
    public void GetDialog(string dialogFile)
    {
        parsedDialog = Json.ParseString(FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read).GetAsText());
        GetNode<Label>("name_label").Text = parsedDialog.AsGodotDictionary()["dialogTitle"].AsString();
        Array<string> dialogLinestest = new Array<string>();
        dialogLinestest.Add("test");
        dialogLinestest.Add("test2");
        GD.Print(dialogLinestest);
        GD.Print((parsedDialog.AsGodotDictionary()["tipp"]).AsGodotArray()[2].AsGodotArray()[4].VariantType);
        if(GetParent().Name == "player") GetParent<player>().allowMovement = true;
    }
}
