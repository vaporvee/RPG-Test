using Godot;
using System.Text.RegularExpressions;

public partial class dialog_bubble : CanvasLayer
{
    public Variant parsedDlg;
    public Variant dlgLines;
    public int dlgPointer = 0;
    public RichTextLabel richText;

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
        Visible = true;
    }

    public void GatherDialog(string key)
    {
        dlgLines = parsedDlg.AsGodotDictionary()[key].AsGodotArray();
        if (dlgLines.VariantType == Variant.Type.Array)
            dlgLines = dlgLines.AsGodotArray()[GD.RandRange(0, dlgLines.AsGodotArray().Count)];
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
        if (richText.VisibleCharacters < Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length && GetNode<Timer>("typewriter_timer").IsStopped())
        {
            richText.VisibleCharacters++;
            GetNode<Timer>("typewriter_timer").Start();
        }
        if (Input.IsActionJustPressed("ui_cancel") && Visible) richText.VisibleCharacters = richText.Text.Length;
        if (Input.IsActionJustPressed("ui_accept") && GetNode<PanelContainer>("box/panel_container").Visible == false && Visible && richText.VisibleCharacters == -1 | Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length <= richText.VisibleCharacters)
        {
            if (dlgPointer < dlgLines.AsGodotArray().Count)
            {
                if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.String)
                {
                    richText.Text = dlgLines.AsGodotArray()[dlgPointer].ToString();
                    richText.VisibleCharacters = 0;
                    GetNode<Timer>("typewriter_timer").Start();
                }
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.Dictionary)
                {
                    MakeAnswerBox(dlgLines.AsGodotArray()[dlgPointer].AsGodotDictionary().Keys.ToString().Trim('[', ']').Split(","));
                    GetNode<PanelContainer>("box/panel_container").Visible = true;
                }

            }
            if (dlgLines.AsGodotArray()[dlgPointer].VariantType != Variant.Type.Dictionary)
                dlgPointer++;
        }
        if (dlgPointer > dlgLines.AsGodotArray().Count)
            CloseDialog();
    }
    public void MakeAnswerBox(string[] dialogOptions)
    {
        var parent = GetNode("box/panel_container/margin_container/v_box_container");
        for (int i = 0; parent.GetChildCount() < dialogOptions.Length; i++)
        {
            parent.AddChild(GD.Load<PackedScene>("res://scenes/gui/dlg_answer_button.tscn").Instantiate());
        }
        for (int i = 0; i < dialogOptions.Length; i++)
            parent.GetChild<Button>(i).Text = dialogOptions[i];
    }
}