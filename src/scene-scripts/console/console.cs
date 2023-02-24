using Godot;
using Godot.Collections;

public partial class console : PopupPanel
{
    private string[] gamepadCheatcode = { "ui_up", "ui_up", "ui_down", "ui_down", "ui_left", "ui_right", "ui_left", "ui_right", "ui_cancel", "ui_accept", "cheat_start" };
    private int gpCcIndexer = 0;
    private InputEvent inputEvent;
    private static RichTextLabel textblock;
    private LineEdit line;
    private Dictionary commandDict;
    private string error = "Not found! :(";

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
        //OS console
        /*if (OS.ReadStringFromStdIn() != "") //not tested yet
            OnLineEditTextSubmitted(OS.ReadStringFromStdIn());*/
    }
    private void ToggleVisible()
    {
        Visible = !Visible;
        player.allowMovement = !Visible;
        line.GrabFocus();
    }
    private void OnPopupHide() { if (dialog_bubble.isTalking == false) player.allowMovement = true; }
    private void OnLineEditTextSubmitted(string command)
    {
        line.Clear();
        if (command.Length != 0) Print(player_variables.PlayerName + " > " + command + "\n");
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
        textblock.AddText(text + "\n");
    }


    private void help()
    {
        Print("==================================== Help ====================================\n");
        for (int i = 0; i < commandDict.Count; i++)
        {
            Print((i + 1) + ". " + Json.ParseString(commandDict.Keys.ToString()).AsStringArray()[i]);
            Print(Json.ParseString(commandDict.Values.ToString()).AsStringArray()[i]);
        }
    }
    private void help(string key) //Optional parameters aren't optional in Call()/Callv() so i use overloads instead
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
    private void consoleclear() => textblock.Clear();
    private void speed(float multiplier)
    {
        player.speed = Mathf.Clamp(multiplier, 0.01f, 15f);
        Print("Set player speed to " + Mathf.Clamp(multiplier, 0.01f, 15f));
    }
    private void noclip()
    {
        try { Print(player.CollisionToggle()); }
        catch
        {
            Print("Player is not accessable");
            help("noclip");
        }
    }
    private void stickycamera()
    {
        try { Print(player.CheatCam()); }
        catch
        {
            Print("Player is not accessable");
            help("stickycamera");
        }
    }
    private void playername(string name)
    {
        player_variables.PlayerName = name;
        Print("Your new name is now: " + player_variables.PlayerName);
    }
    private void closedialogue()
    {
        dialog_bubble.forceClose = true;
        Print("Dialogue got closed!");
    }
    private void reload()
    {
        GetTree().ReloadCurrentScene();
        Print("Level got reloaded!");
    }
    private void visiblecollision()
    {
        GetTree().DebugCollisionsHint = !GetTree().DebugCollisionsHint;
        Print("Visible collision shapes and hitmarker now set to: " + GetTree().DebugCollisionsHint + "\nUse 'reload' to see changes!");
    }
}