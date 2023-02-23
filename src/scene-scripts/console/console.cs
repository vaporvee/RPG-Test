using Godot;
using Godot.Collections;

public partial class console : PopupPanel
{
    private RichTextLabel textblock;
    private LineEdit line;
    private Dictionary commandDict;
    private string error = "Not found! :(\n";

    //functions with capital letters can't be used inside the console
    public override void _Ready()
    {
        Visible = false;
        textblock = GetNode<RichTextLabel>("v_box_container/rich_text_label");
        line = GetNode<LineEdit>("v_box_container/line_edit");
        commandDict = Json.ParseString(FileAccess.GetFileAsString("res://src/scene-scripts/console/commands.json").ToString()).AsGodotDictionary();
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("console"))
        {
            Visible = !Visible;
            line.GrabFocus();
            player.allowMovement = !Visible;
        }
        /*if (OS.ReadStringFromStdIn() != "") //not tested yet
            OnLineEditTextSubmitted(OS.ReadStringFromStdIn());*/
    }
    private void OnPopupHide() => player.allowMovement = true;
    private void OnLineEditTextSubmitted(string command)
    {
        line.Clear();
        if (command.Length != 0) textblock.AddText(player_variables.PlayerName + " > " + command + "\n");
        Variant args;
        if (command.Split(' ').Length == 2 && commandDict.ContainsKey(command.Split(' ')[0].ToLower()))
        {
            int i = command.IndexOf(" ") + 1;
            args = command.Substring(i);
            commandDict.ContainsKey(command.Split(' ')[0].ToLower());
            Call(command.Split(' ')[0].ToLower(), args);
        }
        else if (command.Split(' ').Length > 2 && commandDict.ContainsKey(command.Split(' ')[0].ToLower()))
        {
            int i = command.IndexOf(" ") + 1;
            args = command.Substring(i).Split(' ');
            commandDict.ContainsKey(command.Split(' ')[0].ToLower());
            Callv(command.Split(' ')[0].ToLower(), args.AsGodotArray());
        }
        else if (commandDict.ContainsKey(command.ToLower()))
        {
            Call(command.ToLower());
        }
        else if (command.Length != 0) textblock.AddText(error);
    }



    private void help()
    {
        textblock.AddText("==================================== Help ====================================\n");
        for (int i = 0; i < commandDict.Count; i++)
        {
            textblock.AddText((i + 1) + ". " + Json.ParseString(commandDict.Keys.ToString()).AsStringArray()[i]);
            textblock.AddText(Json.ParseString(commandDict.Values.ToString()).AsStringArray()[i]);
        }
    }
    private void help(string key) //Optional parameters aren't optional in Call()/Callv() so i use overloads instead
    {
        key = key.ToLower();
        if (key.Length != 0 && commandDict.ContainsKey(key))
        {
            textblock.AddText(key);
            textblock.AddText(commandDict[key].ToString());
        }
        else textblock.AddText(error);
    }
    private void consoleclear() => textblock.Clear();
    private void speed(float multiplier)
    {
        player.speed = Mathf.Clamp(multiplier, 0.01f, 15f);
        textblock.AddText("Set player speed to " + Mathf.Clamp(multiplier, 0.01f, 15f) + "\n");
    }
    private void noclip()
    {
        try { textblock.AddText(player.CollisionToggle()); } catch { textblock.AddText("Player is not accessable\n"); }
    }
    private void stickycamera()
    {
        try { textblock.AddText(player.CheatCam()); } catch { textblock.AddText("Player is not accessable\n"); }
    }
    private void playername(string name)
    {
        player_variables.PlayerName = name;
        textblock.AddText("Your new name is now: " + player_variables.PlayerName + "\n");
    }
    private void reload()
    {
        GetTree().ReloadCurrentScene();
        textblock.AddText("Level got reloaded!\n");
    }
    private void visiblecollision()
    {
        GetTree().DebugCollisionsHint = !GetTree().DebugCollisionsHint;
        textblock.AddText("Visible collision shapes and hitmarker now set to: " + GetTree().DebugCollisionsHint + "\nUse 'reload' to see changes!\n");
    }
}