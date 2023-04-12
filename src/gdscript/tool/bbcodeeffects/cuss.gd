@tool
extends RichTextEffect


# Syntax: [cuss][/cuss]
var bbcode = "cuss"

var VOWELS : PackedInt32Array = [97,101,105,111,117,65,69,73,79,85]#a,e,i,o,u,A,E,I,O,U
var CHARS : PackedInt32Array = [38,36,33,64,42,35,37]#&,$,!,@,*,#,%
var SPACE : int = 32

var _was_space = false

func get_text_server():
	return TextServerManager.get_primary_interface()

func _process_custom_fx(char_fx):
	var c = char_fx.glyph_index
	
	var vowelBool : bool = false
	for x in VOWELS:
		if char_fx.glyph_index == get_text_server().font_get_glyph_index(char_fx.font, 1, x, 0):
			vowelBool = true
	
	if not _was_space and not char_fx.relative_index == 0 and not c == get_text_server().font_get_glyph_index(char_fx.font, 1, SPACE, 0):
		var t = char_fx.elapsed_time + char_fx.glyph_index * 10.2 + char_fx.relative_index * 2
		t *= 4.3
		if vowelBool or sin(t) > 0.0:
			char_fx.glyph_index = get_text_server().font_get_glyph_index(char_fx.font, 1, CHARS[int(t) % len(CHARS)], 0)
	
	_was_space = c == get_text_server().font_get_glyph_index(char_fx.font, 1, SPACE, 0)
	
	return true
