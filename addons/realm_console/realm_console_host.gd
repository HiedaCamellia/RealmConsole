extends Node

var Vars = {}

func mount(code: String) -> void:
	var script := GDScript.new()
	script.source_code = code

	if script.reload() == OK:
		var obj = script.new()

		if obj is Node:
			obj.Vars = Vars
			add_child(obj)
			
		else:
			printerr("Error: Script is not a Node.")
