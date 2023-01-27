using Godot;

public partial class audioplayer_tile : StaticBody2D
{
	[Export] bool soundOn = true;
	public void OnAudioFinished() 
	{
		if (soundOn) GetNode<AudioStreamPlayer2D>("audio_stream_player_2d").Play();
	} 
		
}
