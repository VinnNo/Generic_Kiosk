extends Control

enum HL {GD, CS, CPP};
var HL_STATE = HL.GD;
var bHighlighted = false;

export var nGD = preload("res://Scenes/Kiosk_GD.tscn");

export var nCS = preload("res://Scenes/Kiosk_CS.tscn");

var Info_GD = "This module of the kiosk shows functionality using Godot's built-in, run-time language; GDScript!";
var Info_CS = "This module of the kiosk shows functionality using C#!";
var Info_CPP = "This module of the kiosk shows functionality using CPP as bindings! (can be used with other languages too!)";
var Info_Clear = "";

var InfoText = "";

func _ready():
	Set_Info(InfoText);

func _input(event):
	if (event.is_action_pressed("kEscape")):
		get_tree().quit();

func _on_Button_GD_mouse_entered():
	InfoText = Info_GD;
	Set_Info(InfoText);
	pass


func _on_Button_GD_mouse_exited():
	InfoText = Info_Clear;
	Set_Info(InfoText);
	pass


func _on_Button_C_mouse_entered():
	InfoText = Info_CS;
	Set_Info(InfoText);
	pass


func _on_Button_C_mouse_exited():
	InfoText = Info_Clear;
	Set_Info(InfoText);
	pass


func _on_Button_CPP_mouse_entered():
	InfoText = Info_CPP;
	Set_Info(InfoText);
	pass


func _on_Button_CPP_mouse_exited():
	InfoText = Info_Clear;
	Set_Info(InfoText);
	pass


func _on_Button_GD_pressed():
	var i = nGD.instance();
	var p = get_parent();
	p.add_child(i);
	p.remove_child(self);
	pass


func _on_Button_C_pressed():
	var i = nCS.instance();
	var p = get_parent();
	p.add_child(i);
	p.remove_child(self);
	pass


func _on_Button_CPP_pressed():
	pass

func Set_Info(text):
	$Panel/Info.text = text;
