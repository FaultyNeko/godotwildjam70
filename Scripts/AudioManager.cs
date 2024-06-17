using Godot;
using System;

public partial class AudioManager : Node
{
	private AudioStreamPlayer _uiButtonHover;

	public AudioManager()
	{
		_uiButtonHover = new AudioStreamPlayer();
		_uiButtonHover.Stream = GD.Load<AudioStream>("res://Assets/SFX/HoverButton.wav");
	}
	
	public override void _Ready()
	{
		_uiButtonHover = new AudioStreamPlayer();
		_uiButtonHover.Stream = GD.Load<AudioStream>("res://Assets/SFX/HoverButton.wav");
	}

	public void PlayHover()
	{
		_uiButtonHover.Play();
	}
}
