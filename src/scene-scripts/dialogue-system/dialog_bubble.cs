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
    public int dialogOptionsLength;

    public override void _Ready()
    {
        richText = GetNode<RichTextLabel>("box/rich_text_label");
    }
    public void GetDialog(string file, string title, Variant actor, string playerName)
    {
        parsedDlg = Json.ParseString(FileAccess.Open(file, FileAccess.ModeFlags.Read).GetAsText().Replace("{player}", playerName));
        GetNode<Label>("box/name_label").Text = title;
        GD.Print("Now talking to: " + actor);
        if (GetParent().Name == "player") GetParent<player>().allowMovement = false;
        if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "villager")
            WelcomeDialog();
        Visible = true;
    }
    public void WelcomeDialog()
    {
        string[] welcomeText = parsedDlg.AsGodotDictionary()["welcome"].AsStringArray();
        Godot.Collections.Dictionary playerbeginoptions = parsedDlg.AsGodotDictionary()["playerbeginoptions"].AsGodotDictionary();
        GD.Randomize();
        dlgLines.Add(welcomeText[GD.Randi() % welcomeText.Length]);
        MakeAnswerBox(new string[] { playerbeginoptions["talk"].AsStringArray()[GD.Randi() % playerbeginoptions["talk"].AsStringArray().Length], playerbeginoptions["goaway"].AsStringArray()[GD.Randi() % playerbeginoptions["goaway"].AsStringArray().Length] });
    }
    public void CloseDialog()
    {
        Visible = false;
        dlgPointer = 0;
        dlgLines.Clear();
        richText.VisibleCharacters = -1;
        GetNode<Label>("box/name_label").Text = "???";
        richText.Text = "";
        if (GetParent().Name == "player") GetParent<player>().allowMovement = true;
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

        //AnswerBox wait for typewrite effect to finish (garbage code)
        GetNode<PanelContainer>("box/panel_container").Visible = richText.VisibleCharacters == -1 | Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length == richText.VisibleCharacters && GetNode("box/panel_container/margin_container/v_box_container").GetChildCount() == dialogOptionsLength;
    }
    public void MakeAnswerBox(string[] dialogOptions)
    {
        var button = GD.Load<PackedScene>("res://scenes/gui/dlg_answer_button.tscn");
        var parent = GetNode("box/panel_container/margin_container/v_box_container");
        for (int i = 0; parent.GetChildCount() < dialogOptions.Length; i++)
        {
            //remove button nodes for randomizing
            parent.AddChild(button.Instantiate());
            parent.GetChild<Button>(i).Text = dialogOptions[i];
        }
        dialogOptionsLength = dialogOptions.Length;
    }
}