using Godot;
using Godot.Collections;

public partial class console : PopupPanel
{
    string[] gamepadCheatcode = { "ui_up", "ui_up", "ui_down", "ui_down", "ui_left", "ui_right", "ui_left", "ui_right", "ui_cancel", "ui_accept", "cheat_start" };
    int gpCcIndexer = 0;
    InputEvent inputEvent;
    private static RichTextLabel textblock;
    LineEdit line;
    Dictionary commandDict;
    string error = "Not found! :(";

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
        //Cheatcode
        if (Input.IsActionJustPressed(gamepadCheatcode[gpCcIndexer]))
        {
            gpCcIndexer++;
            GetNode<Timer>("cheatcode_timer").Start();
            if (gpCcIndexer == gamepadCheatcode.Length)
            {
                gpCcIndexer = 0;
                ToggleVisible();
            }
        }
        if (Input.IsActionJustPressed("ui_cancel"))
            Visible = false;
        //Normal keyboard hotkey
        if (Input.IsActionJustPressed("console"))
            ToggleVisible();
    }
    void ToggleVisible()
    {
        Visible = !Visible;
        player.allowMovement = !Visible;
        line.GrabFocus();
    }
    void OnPopupHide() { if (dialog_bubble.isTalking == false) player.allowMovement = true; }
    void OnLineEditTextSubmitted(string command)
    {
        line.Clear();
        //repeat user input to console
        if (command.Length != 0) Print(player_variables.PlayerName + " > " + command);
        //splits command into arguments and uses the right call functions for the given amount of split string arguments by the user
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
        else if (command.Length != 0) Print(error);
    }
    public static void Print(string text)
    {
        GD.Print(text);
        textblock.AddText(text + "\n");
    }


    void help()
    {
        Print("==================================== Help ====================================");
        for (int i = 0; i < commandDict.Count; i++)
        {
            Print((i + 1) + ". " + Json.ParseString(commandDict.Keys.ToString()).AsStringArray()[i]
            + Json.ParseString(commandDict.Values.ToString()).AsStringArray()[i]);
        }
    }
    void help(string key) //Optional parameters aren't optional in Call()/Callv() so i use overloads instead
    {
        key = key.ToLower();
        if (key.Length != 0 && commandDict.ContainsKey(key))
        {
            Print(key);
            Print(commandDict[key].ToString());
        }
        else
        {
            Print(error);
            help("help");
        };
    }
    void consoleclear() => textblock.Clear();
    void speed(float multiplier)
    {
        player.speed = Mathf.Clamp(multiplier, 0.01f, 15f);
        Print("Set player speed to " + Mathf.Clamp(multiplier, 0.01f, 15f));
    }
    void noclip()
    {
        try { Print(player.CollisionToggle()); }
        catch
        {
            Print("Player is not accessable");
            help("noclip");
        }
    }
    void stickycamera()
    {
        try { Print(player.CheatCam()); }
        catch
        {
            Print("Player is not accessable");
            help("stickycamera");
        }
    }
    void playername(string name)
    {
        string tmpPlayerName = player_variables.PlayerName;
        player_variables.PlayerName = name;
        if (player_variables.PlayerName == "")
        {
            player_variables.PlayerName = tmpPlayerName;
            Print("The name had too much incorrect symbols and would be empty when changed.");
        }
        else
            Print("Your new name is now: " + player_variables.PlayerName);
    }
    void closedialogue()
    {
        dialog_bubble.forceClose = true;
        Print("Dialogue got closed!");
    }
    void reload()
    {
        GetTree().ReloadCurrentScene();
        Print("Level got reloaded!");
    }
    void visiblecollision()
    {
        GetTree().DebugCollisionsHint = !GetTree().DebugCollisionsHint;
        Print("Visible collision shapes and hitmarker now set to: " + GetTree().DebugCollisionsHint + "Use 'reload' to see changes!");
    }
}