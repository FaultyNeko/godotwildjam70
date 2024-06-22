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
		temp.Name = name;
		temp.Stream = GD.Load<AudioStream>("res://Assets/" + bus + "/" + name + ".wav");
		temp.Bus = bus;
		parent.AddChild(temp);
		try
		{
			temp.Finished += () => QueueFree();
		} catch (Exception ignored) { }
		temp.Play();
	}
	
	public void PlayPositionalAudio(Node parent, string name, string bus)
	{
		AudioStreamPlayer2D temp = new AudioStreamPlayer2D();
		temp.Name = name;
		temp.Stream = GD.Load<AudioStream>("res://Assets/" + bus + "/" + name + ".wav");
		temp.Bus = bus;
		parent.AddChild(temp);
		try
		{
			temp.Finished += () => QueueFree();
		} catch (Exception ignored) { }
		temp.Play();
	}
}
