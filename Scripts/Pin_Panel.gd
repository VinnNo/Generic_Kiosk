extends Panel

signal Button_Pressed(Out_Val);
signal PinBox(Out_Box);
signal PinAnim(Out_Anim);


func _ready():
	emit_signal("PinBox", $Pin_Panel/Pin_Label);
	emit_signal("PinAnim", $AnimationPlayer);
	pass;


func bPressed(Out_Val):
	emit_signal("Button_Pressed", Out_Val);
