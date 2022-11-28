using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class dialog_bubble : CanvasLayer
{
	[Export(PropertyHint.File, "*.json,")]
	public string textfile;

	private Dictionary text;
	private string[] selectedText;
	private bool inProgress = false;

	//ITS FUCKING GARBAGE I HATE THIS DOCUMENTATION

    public override void _Ready()
	{
		var background = GetNode<ColorRect>("Background");
        var textLabel = GetNode<ColorRect>("TextLabel");

		background.Visible = false;
		//text.Add(LoadSceneText(textVariant));
		//signal_bus.Connect("DisplayDialog", this, "onDisplayDialog");
		
    }
	public string LoadSceneText(string textVariant)
	{
		if (File.Exists(textfile))
		{
			File.Open(textfile, FileMode.Open);
			textVariant = JSON.ParseString(File.ReadAllText(textfile)).ToString();
		}
		return textVariant;

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
