using System.Collections.Generic;
using Godot;
using System;
using System.Text.RegularExpressions;
using System.Text;
//using System.IO;

public class TerminalCS : Control
{
	string dir_path = ".";
	string[] cmd_directory;

	bool history_loaded = false;
	string[] command_history = {"ls", "dir"};
	string history_file = "user://.gdterm_history";
	bool history_enabled = false;
	int current_history_index = -1;
	RegEx command_split = new RegEx();
	string pipe_file = "user://.gdterm_pipe";
	Godot.Directory cd;
	Godot.File file = new File();
	string base_dir = "res://";
	string[] sudo_args = {};
	string[] password_commands;
	string help_full;

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Godot.ConfigFile config = new ConfigFile();
		config.Load("addons/terminal.plugin.cfg");
		var verison = config.GetValue("plugin", "version");
		config = null;
		//help_full = help_full.Replace("{version}", "version");
		TextEdit t = (TextEdit) GetNode("TextEdit");
		t?.InsertTextAtCursor("");
		Load_History();
	}

	public void Load_History()
	{
		if (file.FileExists(history_file))
		{
			file.Open(history_file, File.ModeFlags.Read);
			var content = file.GetAsText();
			file.Close();
			foreach (string cmd in content.Split('\n'))
			{
				Array.Resize(ref command_history, command_history.Length + 1);
				command_history[command_history.Length - 1] = cmd;
			}
			current_history_index = command_history.Length;
		}
		history_loaded = true;
	}

	public void _on_LineEdit_GUI_Input(InputEvent @event)
	{
		/*
		int KEY_LEFT = 16777231;
		int KEY_UP = 16777232;
		int KEY_RIGHT = 16777233;
		int KEY_DOWN = 16777234;
		int KEY_ENTER = 16777221;
		*/

		var e = @event;
		if (!history_loaded)
		{
			Load_History();
		}

		if (e is InputEventKey keyEvent)
		{
			int sc = (int)keyEvent.Scancode;
			

			if ( (sc == (int)KeyList.Up || sc == (int)KeyList.Down) && sc == command_history.Length)
			{
				LineEdit l = (LineEdit)GetNode("HBoxContainer/LineEdit");
				history_enabled = true;

				l.Disconnect("gui_input", this, "_on_LineEdit_GUI_Input");
				if (sc == (int)KeyList.Up && current_history_index > 0)
				{
					current_history_index -= 1;
				}

				if (sc == (int)KeyList.Down && current_history_index < command_history.Length - 1)
				{
					current_history_index += 1;
				}

				l.Text = command_history[current_history_index];
				l.Connect("gui_input", this, "_on_LineEdit_GUI_Input");
				l.GrabFocus();
			}

			bool a = (sc == (int)KeyList.Up || sc == (int)KeyList.Down || sc == (int)KeyList.Left 
			|| sc == (int)KeyList.Right || sc == (int)KeyList.Enter);
			if (a)
			{
				history_enabled = false;
			}
		}
	}

	public string Parse_Command(string text, bool pipe = false)
	{
		// '["\'][^"\']* +[^"\']*["\']'
		string s = "[\'][^" + "\']* +[^" + "\']*[" + "\']";
		command_split.Compile(s);

		foreach (string g in command_split.SearchAll(text))
		{
			string t = g;
			text = text.Replace(t, t.Replace(" ", "."));
		}

		string[] result = text.Split(" ", false);
		string command = result[0];
		//string[] args;
		result[0].Remove(0);

		foreach (string a in result)
		{
			//RichTextLabel rtl = new RichTextLabel();
			//rtl.bbcode = a;
			string b = a;
			if (command == "awk")
			{
				b = b.Replace("$", "\\$");
			}
			b = b.Replace("'", "").Replace("'", " ");

		}


		return Convert.ToString(result);

	}

	public void Open_Dialog(string command, bool username = false)
	{
		int height = 32;
		
		WindowDialog d = (WindowDialog) GetNode("Dialog");
		LineEdit du = (LineEdit) GetNode("Dialog/User");
		LineEdit dp = (LineEdit) GetNode("Dialog/Password");

		dp.MarginTop = -12;
		du.Visible = false;
		d.SetMeta("command", command);

		if (username)
		{
			height = 64;
			dp.MarginTop = 3;
			du.Visible = true;
		}

		dp.Text = "";
		Vector2 v = new Vector2(330.0f, (float)height);
		d.PopupCentered(v);
		dp.GrabFocus();

	}

	public void Print_Results(string result)
	{
		TextEdit t = (TextEdit) GetNode("TextEdit");
		t.CursorSetLine(t.GetLineCount() - 1);
		t.CursorSetColumn(0);
		t.InsertTextAtCursor(result);
		t.ClearUndoHistory();
	}

	public void Update_History(string text)
	{
		if (history_enabled)
		{
			return;
		}

		//string a = "";
		StringBuilder sb = new StringBuilder();

		foreach (string i in command_history)
		{
			sb.Append(i);
		}

		sb.Append(text);
		Array.Clear(command_history, 0, command_history.Length);

		for (int i = 0; i < sb.Length; ++i)
		{
			command_history[i] = sb[i].ToString();
		}

		Godot.File file = new File();
		var mode = File.ModeFlags.ReadWrite;
		if (!file.FileExists(history_file))
		{
			mode = File.ModeFlags.Write;
		}
		file.Open(history_file, mode);
		file.SeekEnd();
		file.StoreString(text + "\n");
		file.Close();
	}

	public void Enter_Text(string new_text)
	{
		Update_History(new_text);
		current_history_index = command_history.Length;
		string[] pipe_command = new_text.Split("|");
		string result;
		LineEdit le = (LineEdit) GetNode("HBoxContainer/LineEdit");
		le.Text = "";
		if (pipe_command.Length > 1)
		{
			File f = new File();
			StringBuilder array_cmd = new StringBuilder();
			string[] array_c;
			foreach (string cmd in pipe_command)
			{
				string s = cmd.Trim(' ');
				array_cmd.Append(s);
			}

			string first = Convert.ToString(array_cmd[0]);
			result = Parse_Command(first);
			f.Open(pipe_file, File.ModeFlags.Write);
			f.StoreString(result);
			f.Close();
			
			/*
			foreach (string cmd in array_cmd)
			{
				result = Parse_Command(cmd, true);
				cd.Remove(pipe_file);
				f.Open(pipe_file, File.ModeFlags.Write);
				f.StoreString(result);
				f.Close();
			}
			*/
			
			AnimatedSprite ansp = (AnimatedSprite) GetNode("AnimatedSprite");
			ansp?.Play("Idle");
		}
	}
