extends StaticBody2D

@export var soundOn = true

func _ready():
	$audio_stream_player_2d.autoplay = soundOn
	if(soundOn): 
		$audio_stream_player_2d.play()
func OnAudioFinished():
	if(soundOn): 
		$audio_stream_player_2d.play()
