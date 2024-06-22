using Godot;
using System;

public partial class AudioStreamLayered : Node
{
	[Export] public float VolumeScale = 1.0f;

	/// <summary>
	/// 	Plays each audio track from the given from_position, in seconds.
	/// </summary>
	/// <param name="fromPosition">Starting point of the audio track</param>
	public void Play(float fromPosition = 0.0f) 
	{
		foreach (AudioStreamPlayer p in this.GetChildren()) {
			if (p is AudioStreamPlayer) {
				p.Play(fromPosition);
			}
		}
	}
}
