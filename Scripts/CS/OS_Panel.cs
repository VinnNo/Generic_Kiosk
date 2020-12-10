using Godot;
using System;

public class OS_Panel : Panel
{

    Pin_Label PIN = null;
    Godot.AnimationPlayer PinAnim = null;
    object rtl = null;

    [Export]
    public bool bActive = false;

    [Export]
    private int KEY = 1101;

    public string sk = "";



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rtl = GetNode("Rich_Text_Device");
        GetNode("TouchScreenButton").Connect("pressed", this, "Touch_Button");
    }

    public  void Add_Header(string Header)
    {
        string a = "[b][u][color=#6df]{Header}[/color][/u][/b]\n";
        a = String.Format("a", "Header");
        //rtl.AppendBbcode(a);
        //rtl.AppendBbcode("\n[b][u][color=#6df]{Header}[/color][/u][/b]\n".Format({Header = Header}));
    }

    public void Add_Line(string Key, string Value)
    {
        //rtl.AppendBbcode("[b][color=#d98191]{key}:[/color][/b] {value}\n".Format({Key = Key, Value = Value if String(Value) != "" else "[color=#8fff](empty)[/color]"}));
    }

    public void Touch_Button()
    {
        var a = GetNode("AnimationPlayer") as AnimationPlayer;
        if (bActive)
        {
            a.PlayBackwards("InCS");
        }
        else
        {
            a.Play("InCS");
        }

    }

    public void Button_Pressed(string Out_Val)
    {
        if (PIN == null || PinAnim == null)
        {
            return;
        }

        var p = PIN;
        var a = PinAnim;
        string o = Out_Val;

        if (o == "CLEAR")
        {
            p.KEY = "";
            p.Text = "";
        }
        else if (o == "SEND")
        {
            if (p.KEY == Convert.ToString(KEY))
            {
                a.Play("out");
                PIN = null;
                PinAnim = null;
                GD.Print(Convert.ToString(KEY));
            }
            else
            {
                p.KEY = "";
                p.Text = "";
            }

        }
        else if (p.Text.Length < 4)
        {
            p.Text = p.Text + "*";
            p.KEY = p.KEY + Out_Val;
        }
        GD.Print("Pressed");
    }

    public void Set_Lock()
    {
        if (PIN != null)
        {
            return;
        }
        var l = ResourceLoader.Load("res://Scenes/Pin_PanelCS.tscn") as PackedScene;
        Panel i = (Panel)l.Instance();
        i.Connect("PinBox", this, nameof(Set_PIN));
        i.Connect("PinAnim", this, nameof(Set_Anim_PIN));
        AddChild(i);
        i.Connect("Button_Pressed", this, "Button_Pressed");
    }

    public void Set_PIN(Pin_Label Out_Type)
    {
        PIN = Out_Type;
        sk = PIN.KEY;
    }

    public void Set_Anim_PIN(AnimationPlayer Out_Type)
    {
        PinAnim = Out_Type;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
