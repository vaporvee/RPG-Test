using Godot;
using Godot.Collections;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDialog;
    public Variant test;
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_filedialog_refresh"))//F5
        {
            if (test.VariantType == Variant.Type.Array)
                test = test.AsGodotArray()[0];
            GD.Print(test);
        }
    }
    public void GetDialog(string dialogFile)
    {
        parsedDialog = Json.ParseString(FileAccess.Open(dialogFile, FileAccess.ModeFlags.Read).GetAsText());
        GetNode<Label>("name_label").Text = parsedDialog.AsGodotDictionary()["dialogTitle"].AsString();
        Array<string> dialogLinestest = new Array<string>();
        dialogLinestest.Add("test");
        dialogLinestest.Add("test2");
        GD.Print();
        test = parsedDialog.AsGodotDictionary()["tipp"];
        if (GetParent().Name == "player") GetParent<player>().allowMovement = true;
    }
}
