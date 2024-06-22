using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class MusicPlayer : Node
{
	public NodePath CurrentSong { get; private set; }
	private const string PATH_TO_TRACKLIST = "res://Assets/Tracklist.json";
	
	// Structs for json deserialization
	public class Track 
	{
		public string name;
		public List<string> stream;
	}

	public class Tracklist 
	{
		public List<Track> tracks;
	}

	public override void _Ready() 
	{
		CurrentSong = "Regal";

		if (!CurrentSong.IsEmpty) {
			PlayTrack(CurrentSong);
		}

		CallDeferred(MethodName.CreateNodes);
	}

	private void CreateNodes()
	{
		// Get the tracklist json and deserialize
		using var file = FileAccess.Open(PATH_TO_TRACKLIST, FileAccess.ModeFlags.Read);
		string json = file.GetAsText();

		GD.Print(json);
		var tl = JsonConvert.DeserializeObject<Tracklist>(json);
		GD.Print(tl.tracks.Count);

		// Create the nodes for each AudioStreamPlayer
		foreach (Track t in tl.tracks) {
			if (t.stream.Count > 1) {
				// For when the stream array has more than 1 entry
				AudioStreamLayered au = new AudioStreamLayered();
				au.Name = t.name;

				int i = 1;
				foreach (string s in t.stream) {
					AudioStreamPlayer ap = new AudioStreamPlayer();
					ap.Name = $"{i}";
					ap.Stream = GD.Load<AudioStream>(s);
					ap.Bus = "Music";
					au.AddChild(ap);
					i++;
				}

				AddChild(au);
			} 
			else {
				AudioStreamPlayer ap = new AudioStreamPlayer();
				ap.Name = t.name;
				ap.Stream = GD.Load<AudioStream>(t.stream[0]);
				ap.Bus = "Music";
				AddChild(ap);
			}
		}
	}

	/// <summary>
	/// 	Plays a song in the music player
	/// </summary>
	/// <param name="song">Name of the song. So far, the only tracks are "Regal" and "Hell"</param>
	public void PlayTrack(NodePath song)
	{
		Node curr = GetNode($"{CurrentSong}");
		if (IsPlaying(curr)) {

		}

		Node n = GetNode($"{CurrentSong}");
		if (n is not null) {
			PlayTrack(n);
		}
	}

	/* ------------------------------------------------------------------------------- */

	// If this node is an audio player, play the track
	private void PlayTrack(Node n) 
	{
		if (n is AudioStreamPlayer) {
			((AudioStreamPlayer)n).Play();
		}
		if (n is AudioStreamLayered) {
			((AudioStreamLayered)n).Play();
		}
	}

	private void StopTrack(Node n) 
	{
		if (n is AudioStreamPlayer) {
			((AudioStreamPlayer)n).Stop();
		}
		if (n is AudioStreamLayered) {
			((AudioStreamLayered)n).Stop();
		}
	}

	private bool IsPlaying(Node n) 
	{
		if (n is AudioStreamPlayer) {
			return ((AudioStreamPlayer)n).Playing;
		}
		if (n is AudioStreamLayered) {
			return ((AudioStreamLayered)n).Playing;
		}
		return false;
	}
}
