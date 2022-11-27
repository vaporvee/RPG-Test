using Godot;
using System;

public partial class main : Node2D
{
    [Export]
    public string currentController = Input.GetJoyName(0);

    public override void _Ready()
    {
        GD.Print(Input.GetConnectedJoypads());
    }
    public override void _Process(double delta)
    {
        //Checks if using Keyboard or controller and giving out current controller
        //this is a terrible way of doing this and will be reworked
        if (Input.IsMouseButtonPressed(MouseButton.Left) || currentController == "" || currentController == null)
            currentController = "PC";
        if (Input.IsJoyButtonPressed(0, JoyButton.A) || Input.IsJoyButtonPressed(0, JoyButton.B))
            currentController = Input.GetJoyName(0);

        //CHANGE INPUT FOR NINTENDO CONTROLLER
        if (currentController.StartsWith("Nintendo"))
        {
            InputMap.ActionEraseEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionEraseEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
            InputMap.ActionAddEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
            InputMap.ActionAddEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
        } else
        {
            InputMap.ActionEraseEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
            InputMap.ActionEraseEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionAddEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionAddEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
        }

        if (Input.IsActionJustPressed("ui_accept")) GD.Print(currentController + " accept");
        if (Input.IsActionJustPressed("ui_cancel")) GD.Print(currentController + " cancel");

        //FULLSCREEN HOTKEY
        if (Input.IsActionJustPressed("hotkey_fullscreen"))
        {
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            else  DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }
}
