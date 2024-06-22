using Godot;
using System;

public partial class MusicPlayer : Node
{
	[Export] private NodePath CurrentSong = "Regal";

	public override void _Ready() {
		GetNode<AudioStreamPlayer>($"{CurrentSong}/0").Play();
	}

	/// <summary>
	/// 	Plays a song in the music player
	/// </summary>
	/// <param name="song">Name of the song. So far, the only tracks are "Regal" and "Hell"</param>
	public void PlayTrack(NodePath song) {

	}
}
