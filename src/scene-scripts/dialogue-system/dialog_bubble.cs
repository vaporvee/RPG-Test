using Godot;
using System.Text.RegularExpressions;

public partial class dialog_bubble : CanvasLayer
{
    Variant parsedDlg;
    Variant dlgLines;
    int dlgPointer = 0;
    RichTextLabel richText;
    Timer typewriterTimer;
    string title;
    Area2D triggerArea;
    public static bool forceClose;
    public static bool isTalking;
    /*TODO: 
    - Dont repeat the same randomized dialogue after you get asked do you need something "else"
    - add tree support (example: "story" key)
    - ability to add dialogue begin answers on the fly (special ones are colored)
    they will be in a dictionary with event IDs or Dictionary keys it also needs an array wich ones are active
	-strings like in the "goodbye" key should be randomized without the array brackets so they are only needed for multiline texts
	-answers should work more like dialogue for tree support*/
    public override void _Ready()
    {
        richText = GetNode<RichTextLabel>("box/rich_text_label");
        typewriterTimer = GetNode<Timer>("typewriter_timer");
    }
    public void GetDialog(string file, Area2D actor)
    {
        console.Print("Loaded dialogue from: " + file + "\nClose dialogue with 'closedialogue'");
        triggerArea = actor;
        title = actor.Get("title").AsString();
        bool introducedVillager = actor.Get("introducedVillager").AsBool();

        parsedDlg = Json.ParseString(FileAccess.Open(file, FileAccess.ModeFlags.Read).GetAsText()
        .Replace("{player}", "[color=cyan]" + player_variables.PlayerName + "[/color]").Replace("{title}", "[color=purple]" + title + "[/color]"));

        if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() != "villager" || introducedVillager)
            GetNode<Label>("box/name_label").Text = title;

        player.allowMovement = false;

        //Get first key
        if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "villager")
            if (introducedVillager)
                GatherDialog("welcome");
            else
                GatherDialog("intro");

        else if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "message")
            GatherDialog("message");

        Visible = true;
        isTalking = true;
    }
    void GatherDialog(string key)
    {
        dlgPointer = 0;
        if (parsedDlg.AsGodotDictionary()[key].VariantType == Variant.Type.Array)
            dlgLines = parsedDlg.AsGodotDictionary()[key].AsGodotArray();
        if (dlgLines.AsGodotArray()[0].VariantType == Variant.Type.Array)
            dlgLines = dlgLines.AsGodotArray()[GD.RandRange(0, dlgLines.AsGodotArray().Count - 1)];
        //TODO:copy a clean default array and remove already used indexes and copy from clean array when its empty
    }
    void OnVisibilityChanged()
    {
        if (Visible)
            ProcessMode = ProcessModeEnum.Inherit;
        else if (richText != null)
        {
            dlgPointer = 0;
            richText.VisibleCharacters = -1;
            GetNode<Label>("box/name_label").Text = "???";
            richText.Text = "";
            if (GetParent().Name == "player") player.allowMovement = true;
            isTalking = false;
            forceClose = false;
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }
    public override void _Process(double delta)
    {
        DialogControlls();
        AnswerBoxControlls();
    }
    void DialogControlls()
    {
        if (Input.IsActionJustPressed("ui_cancel")) richText.VisibleCharacters = richText.Text.Length;

        if (Input.IsActionJustPressed("ui_accept") && GetNode<console>("/root/Console").Visible == false && GetNode<PanelContainer>("box/panel_container").Visible == false
        && richText.VisibleCharacters == -1 | Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length <= richText.VisibleCharacters)
        {
            if (dlgPointer < dlgLines.AsGodotArray().Count)
            {
                //read and write the dialogue
                if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.Float)
                    InDialogEvents((int)dlgLines.AsGodotArray()[dlgPointer]);
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.String && !dlgLines.AsGodotArray()[dlgPointer].AsString().StartsWith("<goto:>"))
                    UpdateDialog();
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.String && dlgLines.AsGodotArray()[dlgPointer].AsString().StartsWith("<goto:>"))
                {
                    GatherDialog(dlgLines.AsGodotArray()[dlgPointer].AsString().Replace("<goto:>", ""));
                    UpdateDialog();
                }
                else if (dlgLines.AsGodotArray()[dlgPointer].VariantType == Variant.Type.Dictionary)
                    MakeAnswerBox(Json.ParseString(dlgLines.AsGodotArray()[dlgPointer].AsGodotDictionary().Keys.ToString()).AsStringArray());
            }
            dlgPointer++;
        }
        Visible = !(dlgPointer > dlgLines.AsGodotArray().Count || forceClose);
    }
    void UpdateDialog()
    {
        richText.Text = dlgLines.AsGodotArray()[dlgPointer].ToString();
        richText.VisibleCharacters = 0;
        typewriterTimer.Start();
    }
    public void OnTypewriterTimerTimeout()
    {
        if (richText.VisibleCharacters < Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length)
        {
            richText.VisibleCharacters++;
            GetNode<AudioStreamPlayer>("typewriter_audio_stream").Play();
        }
        else typewriterTimer.Stop();
    }
    void MakeAnswerBox(string[] dialogOptions)
    {
        var parent = GetNode("box/panel_container/margin_container");
        if (parent.GetChildCount() == 1) parent.GetChild(0).Free();
        parent.AddChild(new VBoxContainer());
        parent = parent.GetChild(0);
        for (int i = 0; parent.GetChildCount() < dialogOptions.Length; i++)
        {
            parent.AddChild(GD.Load<PackedScene>("res://scenes/gui/dlg_answer_button.tscn").Instantiate());
            if (dialogOptions[i].StartsWith("<!>")) parent.GetChild<Button>(i).Disabled = true;
            parent.GetChild<Button>(i).Text = dialogOptions[i].Replace("<!>", "");
        }
        GetNode<PanelContainer>("box/panel_container").Visible = true;
        parent.GetChild<Button>(0).GrabFocus();
    }
    void AnswerBoxControlls()
    {
        if (GetNode<PanelContainer>("box/panel_container").Visible == true
        && GetNode("box/panel_container/margin_container").GetChild(0).GetChild<Button>(0).ButtonGroup.GetPressedButton() != null)
        {
            GetNode<AudioStreamPlayer>("answerbtn_audio_stream").Play(); //BUG: dialogue box breaks while game console is open.
            var answer = dlgLines.AsGodotArray()[dlgPointer - 1].AsGodotDictionary()[GetNode<Button>(GetNode("box/panel_container/margin_container")
            .GetChild(0).GetChild<Button>(0).ButtonGroup.GetPressedButton().GetPath()).Text];
            GetNode<PanelContainer>("box/panel_container").Visible = false;
            if (answer.VariantType == Variant.Type.String && answer.AsString().StartsWith("<goto:>"))
            {
                GatherDialog(answer.AsString().Replace("<goto:>", ""));
                UpdateDialog();
            }
            dlgPointer++;
        }
    }
    void InDialogEvents(int eventID)
    {
        switch (eventID)
        {
            case 0:
                GetNode<Label>("box/name_label").Text = title;
                triggerArea.Set("introducedVillager", true);
                GatherDialog("begindialog");
                UpdateDialog();
                break;
        }
    }
}