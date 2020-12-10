using Godot;
using System;

public class Pin_Panel : Panel
{
    [Signal]
    public delegate void Button_Pressed(String Out_Val);

    [Signal]
    public delegate void PinBox(Pin_Label Out_Box);

    [Signal]
    public delegate void PinAnim(AnimationPlayer Out_Anim);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //EmitSignal(nameof(PinBox), GetNode("Pin_Panel/Pin_Label"));
        //EmitSignal(nameof(PinAnim), GetNode("AnimationPlayer"));

        EmitSignal(nameof(PinBox), (Label)GetNode("Pin_Panel/Pin_Label"));
        EmitSignal(nameof(PinAnim), (AnimationPlayer)GetNode("AnimationPlayer"));

        //GetNode("GridContainer/Button1").Connect("Alt_Pressed", this, "bPressed");
    }

    public void bPressed(string Out_Val)
    {
        EmitSignal(nameof(Button_Pressed), Out_Val);
        GD.Print("Pressed Panel");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
