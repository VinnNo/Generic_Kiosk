using Godot;
using System;

public class Button : Godot.Button
{
    [Signal]
    public delegate void Alt_Pressed(string Out_Val);

    [Export]
    public string Value = "";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Pressed()
    {
        EmitSignal(nameof(Alt_Pressed), Value);
    }
}
