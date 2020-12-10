extends Panel

export var nLock_Panel = preload("res://Scenes/Pin_Panel.tscn");
#onready var iLock_Panel = nLock_Panel.instance();

export var bActive = false;
onready var rtl = $Rich_Text_Device;
var bAM = false;

export var KEY = str(1101);
var PIN;
var PinAnim;

func _ready():
	$Rich_Text_Device.clear();
	Set_Text();
	print(str(KEY))
	pass;

func add_header(header):
	rtl.append_bbcode("\n[b][u][color=#6df]{header}[/color][/u][/b]\n".format({
		header = header,
	}));


func add_line(key, value):
	rtl.append_bbcode("[b][color=#d98191]{key}:[/color][/b] {value}\n".format({
		key = key,
		value = value if str(value) != "" else "[color=#8fff](empty)[/color]",
	}));

func add_blank():
	rtl.append_bbcode("\n");

func datetime_to_string(date):
	if (
		date.has("year")
		and date.has("month")
		and date.has("day")
		and date.has("hour")
		and date.has("minute")
		and date.has("second")
	):
		# Date and time.
		return "{year}-{month}-{day} {hour}:{minute}:{second}".format({
			year = str(date.year).pad_zeros(2),
			month = str(date.month).pad_zeros(2),
			day = str(date.day).pad_zeros(2),
			hour = str(date.hour).pad_zeros(2),
			minute = str(date.minute).pad_zeros(2),
			second = str(date.second).pad_zeros(2),
		});
	elif date.has("year") and date.has("month") and date.has("day"):
		# Date only.
		return "{year}-{month}-{day}".format({
			year = str(date.year).pad_zeros(2),
			month = str(date.month).pad_zeros(2),
			day = str(date.day).pad_zeros(2),
		});
	else:
		# Time only.
		var h = date.hour;
		if (h > 12):
			h -= 12;
			bAM = false;
		else:
			bAM = true;
		return "{hour}:{minute}:{second}".format({
			hour = str(h).pad_zeros(2),
			minute = str(date.minute).pad_zeros(2),
			second = str(date.second).pad_zeros(2),
		});


func Set_Text():
	var t = $Rich_Text_Device;
	
	add_header("System");
	add_line("OS", str(OS.get_name()));
	add_line("System ID", str(OS.get_unique_id()));
	
	add_blank();
	
	add_header("Hardware");
	add_line("CPU Logical Cores", str(OS.get_processor_count()));
	add_line("Memory", str((OS.get_static_memory_usage() / 1000000)) + "MB");
	add_line("GPU Vendor", str(VisualServer.get_video_adapter_vendor()));
	add_line("GPU Model", str(VisualServer.get_video_adapter_name()));
	
	add_blank();
	
	add_header("Date and Time");
	add_line("Date", datetime_to_string(OS.get_date(false)));
	add_line("Time (STANDARD)", datetime_to_string(OS.get_time(false)) + " PM" if (!bAM) else " AM");


func _on_TouchScreenButton_pressed():
	if (bActive):
		$AnimationPlayer.play_backwards("In");
	else:
		$AnimationPlayer.play("In");


func _on_Button_alt_pressed(Out_Val):
	if (PIN == null || PinAnim == null):
		return;
		
	var p = PIN;
	var a = PinAnim;
	var o = Out_Val
	if (o == "CLEAR"):
		p.KEY = "";
		p.text = "";
	elif (o == "SEND"):
		if (p.KEY == str(KEY)):
			a.play("out");
			PIN = null;
			PinAnim = null;
		else:
			p.KEY = "";
			p.text = "";
	elif (p.text.length() < 4):
		p.text = p.text + "*";
		p.KEY = p.KEY + Out_Val;

func Set_Lock():
	if (PIN != null):
		return;
	var i = nLock_Panel.instance();
	i.connect("PinBox", self, "Set_PIN");
	i.connect("PinAnim", self, "Set_Anim_PIN");
	add_child(i);
	i.connect("Button_Pressed", self, "_on_Button_alt_pressed");

func Set_PIN(Out_Type):
	PIN = Out_Type;

func Set_Anim_PIN(Out_Type):
	PinAnim = Out_Type;

func _on_Pin_Panel_Button_Pressed(Out_Type):
	_on_Button_alt_pressed(Out_Type);
