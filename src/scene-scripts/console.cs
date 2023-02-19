using Godot;

public partial class console : PopupPanel
{
    public RichTextLabel textblock;
    public LineEdit line;

    public string error = "Not found! :(";

    //functions with capital letters can't be used inside the console
    public override void _Ready()
    {
        textblock = GetNode<RichTextLabel>("v_box_container/rich_text_label");
        line = GetNode<LineEdit>("v_box_container/line_edit");
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("console"))
        {
            Visible = !Visible;
            line.GrabFocus();
            if (GetParent().Name == "player") GetParent<player>().allowMovement = !Visible;
        }
    }
    public void OnPopupHide()
    {
        if (GetParent().Name == "player") GetParent<player>().allowMovement = true;
    }
    public void OnLineEditTextSubmitted(string command)
    {
        line.Clear();
        if (command.Length != 0) textblock.AddText("\n>" + command);
        Variant args;
        if (command.Split(' ').Length == 2)
        {
            int i = command.IndexOf(" ") + 1;
            args = command.Substring(i);
            Call(command.Split(' ')[0].ToLower(), args);
        }
        if (command.Split(' ').Length > 2)
        {
            int i = command.IndexOf(" ") + 1;
            args = command.Substring(i).Split(' ');
            Callv(command.Split(' ')[0].ToLower(), args.AsGodotArray());
        }
        else Call(command.ToLower());
    }



    public void help()
    {
        textblock.AddText("\n============ Help ============");
        textblock.AddText("\n1. consoleclear - Clears the console");
        textblock.AddText("\n2. speed <multiplier number> - Multiplies the player speed by the given value");
        textblock.AddText("\n3. playername <new name> - Renames the player");
    }
    public void consoleclear() => textblock.Clear();
    public void speed(float multiplier)
    {
        if (GetParent().Name == "player") GetParent<player>().speed = Mathf.Clamp(multiplier, 0.01f, 15f);
        textblock.AddText("\nSet speed to " + Mathf.Clamp(multiplier, 0.01f, 15f));
    }
    public void playername(string name)
    {
        if (GetParent().Name == "player")
        {
            GetParent<player>().playerName = name;
            GetParent<player>().ClearPlayerName();
            textblock.AddText("\nYour new name is now: " + GetParent<player>().playerName);
        }

    }
}
