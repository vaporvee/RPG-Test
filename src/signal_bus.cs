using Godot;
using System;

public partial class signal_bus : Node
{
    [Signal]
    public delegate void DialogDisplayEventHandler(string textKey);
}
