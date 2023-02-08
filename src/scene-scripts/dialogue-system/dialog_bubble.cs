using Godot;
using System.Text.RegularExpressions;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDlg;
    public Variant dlgLines;
    public int dlgPointer = 0;
    public RichTextLabel richText;

    /*TODO: 
    - Dont repeat the same randomized dialogue after you get asked do you need something "else"
    - add controller support for answerboxes
    - add tree support (example: "story" key)
    - ability to add dialogue begin answers on the fly (special ones are colored)
    they will be in a dictionary with event IDs or Dictionary keys*/
    public override void _Ready()
    {
        richText = GetNode<RichTextLabel>("box/rich_text_label");
    }
    public void GetDialog(string file, string title, Variant actor, string playerName)
    {
        playerName = "[color=blue]" + playerName + "[/color]";
        parsedDlg = Json.ParseString(FileAccess.Open(file, FileAccess.ModeFlags.Read).GetAsText().Replace("{player}", playerName));
        GetNode<Label>("box/name_label").Text = title;
        if (GetParent().Name == "player") GetParent<player>().allowMovement = false;
        if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "villager")
            GatherDialog("welcome");
        else if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "message")
            GatherDialog("message");
        Visible = true;
    }

    public void GatherDialog(string key)
    {
        dlgPointer = 0;
        dlgLines = parsedDlg.AsGodotDictionary()[key].AsGodotArray();
        if (dlgLines.VariantType == Variant.Type.Array)
            dlgLines = dlgLines.AsGodotArray()[GD.RandRange(0, dlgLines.AsGodotArray().Count - 1)];
    }

    public void CloseDialog()
    {
        Visible = false;
        dlgPointer = 0;
        richText.VisibleCharacters = -1;
        GetNode<Label>("box/name_label").Text = "???";
        richText.Text = "";
        if (GetParent().Name == "player") GetParent<player>().allowMovement = true;
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_cancel") && Visible) richText.VisibleCharacters = richText.Text.Length;

        if (Input.IsActionJustPressed("ui_accept") && Visible && GetNode<PanelContainer>("box/panel_container").Visible == false
        && richText.VisibleCharacters == -1 | Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length <= richText.VisibleCharacters)
        {
            if (dlgPointer < dlgLines.AsGodotArray().Count)
            {
                GD.Print(dlgLines.AsGodotArray()[dlgPointer].VariantType);
                if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.Float && ((float)dlgLines.AsGodotArray()[dlgPointer]) == 0)
                    InDialogEvents((int)dlgLines.AsGodotArray()[dlgPointer]);
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.String)
                    UpdateDialog();
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.Dictionary)
                    MakeAnswerBox(Json.ParseString(dlgLines.AsGodotArray()[dlgPointer].AsGodotDictionary().Keys.ToString()).AsStringArray());
            }
            dlgPointer++;
        }
        if (richText.VisibleCharacters < Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length && GetNode<Timer>("typewriter_timer").IsStopped())
        {
            richText.VisibleCharacters++;
            GetNode<Timer>("typewriter_timer").Start();
        }
        if (dlgPointer > dlgLines.AsGodotArray().Count)
        {
            if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "villager")
            {
                GatherDialog("else");
                UpdateDialog();
                dlgPointer++;
            }
            else CloseDialog();
        }

        if (GetNode<PanelContainer>("box/panel_container").Visible == true
        && GetNode("box/panel_container/margin_container").GetChild(0).GetChild<Button>(0).ButtonGroup.GetPressedButton() != null)
        {
            var answer = dlgLines.AsGodotArray()[dlgPointer - 1].AsGodotDictionary()[GetNode<Button>(GetNode("box/panel_container/margin_container")
            .GetChild(0).GetChild<Button>(0).ButtonGroup.GetPressedButton().GetPath()).Text];
            GetNode<PanelContainer>("box/panel_container").Visible = false;
            if (answer.VariantType == Variant.Type.String)
            {
                GatherDialog(answer.AsString());
                UpdateDialog();
            }
            dlgPointer++;
        }
    }
    public void UpdateDialog()
    {
        richText.Text = dlgLines.AsGodotArray()[dlgPointer].ToString();
        richText.VisibleCharacters = 0;
        GetNode<Timer>("typewriter_timer").Start();
    }
    public void MakeAnswerBox(string[] dialogOptions)
    {
        var parent = GetNode("box/panel_container/margin_container");
        if (parent.GetChildCount() == 1) parent.GetChild(0).Free();
        parent.AddChild(new VBoxContainer());
        parent = parent.GetChild(0);
        for (int i = 0; parent.GetChildCount() < dialogOptions.Length; i++)
        {
            parent.AddChild(GD.Load<PackedScene>("res://scenes/gui/dlg_answer_button.tscn").Instantiate());
            parent.GetChild<Button>(i).Text = dialogOptions[i];
        }
        GetNode<PanelContainer>("box/panel_container").Visible = true;
    }
    public void InDialogEvents(int eventID) //maybe replaceable with jsonrpc?
    {
        switch (eventID)
        {
            case 0:
                CloseDialog();
                break;
        }
    }
}