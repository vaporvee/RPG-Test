using Godot;
using System;

public partial class main : Node2D
{
    [Export]
    public string currentController = Input.GetJoyName(0);

    public override void _Ready()
    {
    }
    public override void _Process(double delta)
    {
        //CHANGE INPUT FOR NINTENDO CONTROLLER
        currentController = Input.GetJoyName(0);
        if (currentController.StartsWith("Nintendo"))
        {
            InputMap.ActionAddEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.B});
            InputMap.ActionEraseEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionAddEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionEraseEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
        }
        else
        {
            InputMap.ActionAddEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
            InputMap.ActionEraseEvent("ui_accept", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
            InputMap.ActionAddEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.B });
            InputMap.ActionEraseEvent("ui_cancel", new InputEventJoypadButton() { ButtonIndex = JoyButton.A });
        }
        if (Input.IsActionJustPressed("ui_accept"))
        {
            GD.Print(currentController + " accept");
        }
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GD.Print(currentController + " cancel");
        }

        //FULLSCREEN
        if (Input.IsActionJustPressed("hotkey_fullscreen"))
        {
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            }
        }
    }
}
