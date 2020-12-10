extends Button

export var Value = ""

signal alt_pressed(Out_Val);

func _pressed():
	emit_signal("alt_pressed", Value)
