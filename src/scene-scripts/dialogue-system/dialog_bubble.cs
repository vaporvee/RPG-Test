using Godot;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDlg;
    public ArrayList dlgLines = new ArrayList();
    public int dlgPointer = 0;
    public RichTextLabel richText;

    public override void _Ready()
    {
        richText = GetNode<RichTextLabel>("box/rich_text_label");
        dlgLines.Add("Hello! I'm a [color=purple]debug character[/color] and...");
        dlgLines.Add("[center][b][wave amp=50 freq=15][rainbow]This is cool test text[/rainbow][/wave][/b][/center]"); //bbcode gets counted to so typewrite effect takes decades //make a seperate variable without bbcode and count that instead 
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
        richText.VisibleCharacters = -1;
        GetNode<Label>("box/name_label").Text = "???";
        richText.Text = "";
    }
    public override void _Process(double delta)
    {
        if (richText.VisibleCharacters < Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length && GetNode<Timer>("typewriter_timer").IsStopped())
        {
            richText.VisibleCharacters++;
            GetNode<Timer>("typewriter_timer").Start();
        }
        if (Input.IsActionJustPressed("ui_accept") && Visible == true && richText.VisibleCharacters == -1 | Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length == richText.VisibleCharacters)
        {
            if (dlgPointer < dlgLines.Count && dlgLines[dlgPointer] is string)
            {
                richText.Text = dlgLines[dlgPointer].ToString();
                richText.VisibleCharacters = 0;
                GetNode<Timer>("typewriter_timer").Start();
            }
            dlgPointer++;
        }
        if (dlgPointer > dlgLines.Count)
            CloseDialog();
    }
    //get dialogue inside dialogLines and add them to 
    //richtextlabel and detect dictionarys in them and display them as prompts
}
