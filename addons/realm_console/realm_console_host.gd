extends Node

func mount(code: String, vars = {}):
	var script := GDScript.new()
	script.source_code = code

	if script.reload() == OK:
		var obj = script.new()

		if obj is Node:
			obj.Vars = vars
			add_child(obj)
			return obj
			
		else:
			return "Error: Script is not a Node."
			
	return "Error: Script reload fail."
			
	
func unmount(obj: Node):
	obj.queue_free()
