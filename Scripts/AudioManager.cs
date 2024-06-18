using Godot;
using System;

public partial class AudioManager : Node
{
	public static AudioManager PlayerAudio;
	
	public override void _Ready()
	{
		PlayerAudio = new AudioManager();
	}
	public void PlayAudio(Node parent, string name, string bus)
	{
		AudioStreamPlayer temp = new AudioStreamPlayer();
		temp.Stream = GD.Load<AudioStream>("res://Assets/SFX/" + name + ".wav");
		temp.Bus = bus;
		parent.AddChild(temp);
		try
		{
			temp.Finished += () => QueueFree();
		} catch (Exception ignored) { }
		temp.Play();
	}
}
