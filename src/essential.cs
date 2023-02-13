using Godot;
using System;

public partial class essential : Node
{
    public string currentController = Input.GetJoyName(0);

    public override void _Input(InputEvent @event)
    {
        //Checks if using Keyboard or controller and giving out current controller
        if (@event is InputEventKey || @event is InputEventMouseButton || currentController == "")
            currentController = "PC";
        if (@event is InputEventJoypadButton)
            currentController = Input.GetJoyName(0);
    }
    public override void _Process(double delta)
    {
        //CHANGE INPUT FOR NINTENDO CONTROLLER 
        InputEventJoypadButton JoyButtonA = new InputEventJoypadButton() { ButtonIndex = JoyButton.A };
        InputEventJoypadButton JoyButtonB = new InputEventJoypadButton() { ButtonIndex = JoyButton.B };
        if (currentController.StartsWith("Nintendo") && InputMap.ActionHasEvent("ui_accept", JoyButtonA))
        {
            InputMap.ActionEraseEvent("ui_accept", JoyButtonA);
            InputMap.ActionEraseEvent("ui_cancel", JoyButtonB);
            InputMap.ActionAddEvent("ui_accept", JoyButtonB);
            InputMap.ActionAddEvent("ui_cancel", JoyButtonA);
        }
        else if (InputMap.ActionHasEvent("ui_accept", JoyButtonB))
        {
            InputMap.ActionEraseEvent("ui_accept", JoyButtonB);
            InputMap.ActionEraseEvent("ui_cancel", JoyButtonA);
            InputMap.ActionAddEvent("ui_accept", JoyButtonA);
            InputMap.ActionAddEvent("ui_cancel", JoyButtonB);
        }

        //FULLSCREEN HOTKEY
        if (Input.IsActionJustPressed("hotkey_fullscreen"))
        {
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }
}
