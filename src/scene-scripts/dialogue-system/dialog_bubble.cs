using Godot;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class dialog_bubble : CanvasLayer
{
	public Variant parsedDlg;
	public Variant dlgLines;
	public int dlgPointer = 0;
	public RichTextLabel richText;
	public Timer typewriterTimer;
	public string name;
	public Area2D triggerArea;

	/*TODO: 
    - Dont repeat the same randomized dialogue after you get asked do you need something "else"
    - add tree support (example: "story" key)
    - ability to add dialogue begin answers on the fly (special ones are colored)
    they will be in a dictionary with event IDs or Dictionary keys it also needs an array wich ones are active*/
	public override void _Ready()
	{
		richText = GetNode<RichTextLabel>("box/rich_text_label");
		typewriterTimer = GetNode<Timer>("typewriter_timer");
	}
	public void GetDialog(string file, string title, Area2D actor, string playerName, bool introducedVillager)
	{
		triggerArea = actor;
		name = title;

		parsedDlg = Json.ParseString(FileAccess.Open(file, FileAccess.ModeFlags.Read).GetAsText()
		.Replace("{player}", "[color=cyan]" + playerName + "[/color]").Replace("{title}", "[color=purple]" + title + "[/color]"));

		if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() != "villager" || introducedVillager)
			GetNode<Label>("box/name_label").Text = title;
		if (GetParent().Name == "player") GetParent<player>().allowMovement = false;

		//Get first key
		if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "villager")
			if (introducedVillager)
				GatherDialog("welcome");
			else
				GatherDialog("intro");

		else if (parsedDlg.AsGodotDictionary()["dialogType"].AsString() == "message")
			GatherDialog("message");

		Visible = true;
	}

	public void GatherDialog(string key)
	{
		dlgPointer = 0;
		dlgLines = parsedDlg.AsGodotDictionary()[key].AsGodotArray();
		dlgLines = dlgLines.AsGodotArray()[GD.RandRange(0, dlgLines.AsGodotArray().Count - 1)];
		//TODO:copy a clean default array and remove already used indexes and copy from clean array when its empty
	}

	public override void _Process(double delta)
	{
		DialogControlls();
		AnswerBoxControlls();
	}
	public void DialogControlls()
	{
		if (Input.IsActionJustPressed("ui_cancel") && Visible) richText.VisibleCharacters = richText.Text.Length;

		if (Input.IsActionJustPressed("ui_accept") && Visible && GetNode<PanelContainer>("box/panel_container").Visible == false
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
		if (dlgPointer > dlgLines.AsGodotArray().Count)
			CloseDialog();
	}
	public void UpdateDialog()
	{
		richText.Text = dlgLines.AsGodotArray()[dlgPointer].ToString();
		richText.VisibleCharacters = 0;
		typewriterTimer.Start();
	}
	public void OnTypewriterTimerTimeout()
	{
		if (richText.VisibleCharacters < Regex.Replace(richText.Text, @"\[[^]]+\]", "").Length)
			richText.VisibleCharacters++;
		else typewriterTimer.Stop();
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
			if (dialogOptions[i].StartsWith("<!>")) parent.GetChild<Button>(i).Disabled = true;
			parent.GetChild<Button>(i).Text = dialogOptions[i].Replace("<!>", "");
		}
		GetNode<PanelContainer>("box/panel_container").Visible = true;
		parent.GetChild<Button>(0).GrabFocus();
	}
	public void AnswerBoxControlls()
	{
		if (GetNode<PanelContainer>("box/panel_container").Visible == true
		&& GetNode("box/panel_container/margin_container").GetChild(0).GetChild<Button>(0).ButtonGroup.GetPressedButton() != null)
		{
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
	public void InDialogEvents(int eventID) //maybe replaceable with jsonrpc?
	{
		switch (eventID)
		{
			case 0:
				GetNode<Label>("box/name_label").Text = name;
				triggerArea.Set("introducedVillager", true);
				GatherDialog("begindialog");
				UpdateDialog();
				break;
		}
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
}