using Godot;
using System.Collections;
using System;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDlg;
    public ArrayList dlgLines = new ArrayList();
    public int dlgPointer = 0;

    public override void _Ready()
    {
        dlgLines.Add("Hello! I'm a debug character and...");
        dlgLines.Add("[center][b][wave amp=50 freq=15][rainbow]This is cool test text[/rainbow][/wave][/b][/center]");
    }
    public void GetDialog(string file, string title, Variant actor)
    {
        parsedDlg = Json.ParseString(FileAccess.Open(file, FileAccess.ModeFlags.Read).GetAsText());
        GetNode<Label>("box/name_label").Text = title;
        GD.Print("Now talking to: " + actor);
        if (GetParent().Name == "player") GetParent<player>().allowMovement = false;
        Visible = true;
    }
    public void CloseDialog()
    {
        if (GetParent().Name == "player") GetParent<player>().allowMovement = true;
        Visible = false;
        dlgPointer = 0;
        GetNode<RichTextLabel>("box/rich_text_label").Text = "";
        GetNode<Label>("box/name_label").Text = "???";
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            var textBlock = GetNode<RichTextLabel>("box/rich_text_label");
            if (dlgPointer < dlgLines.Count && dlgLines[dlgPointer] is string) textBlock.Text = dlgLines[dlgPointer].ToString();
            dlgPointer++;
        }
        if (dlgPointer > dlgLines.Count)
            CloseDialog();
    }
    //get dialogue inside dialogLines and add them to 
    //richtextlabel and detect dictionarys in them and display them as prompts
}
