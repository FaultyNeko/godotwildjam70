using Godot;
using System;

public partial class AudioStreamLayered : Node
{
	[Export] public bool Playing = false;
	[Export(PropertyHint.Layers2DPhysics)] public uint LayersPlaying { get; set; }
	[Export] public float VolumeDb = 0.0f;

	private const float MIN_VOL = -1000.0f;
	private const float MAX_VOL = 0.0f;
	private uint _layersPlayingPrev = 0;

	[Signal] public delegate void SignalChangedEventHandler();

    public override void _Ready()
    {
		_layersPlayingPrev = LayersPlaying;
    }

    /// <summary>
    /// 	Plays each audio track from the given from_position, in seconds.
    /// </summary>
    /// <param name="fromPosition">Starting point of the audio track</param>
    public void Play(float fromPosition = 0.0f) 
	{
		uint i = 1;
		foreach (AudioStreamPlayer p in GetChildren()) {
			if (p is AudioStreamPlayer) {
				p.Play(fromPosition);
				if ((LayersPlaying & i) == i) {
					p.VolumeDb = 0.0f;
				} else {
					p.VolumeDb = -80.0f;
				}
			}
			i = i << 1;	// Bitwise stuff lmao
		}
		Playing = true;
	}

    public override void _Process(double delta)
    {
		if (_layersPlayingPrev != LayersPlaying) {
			
		}
    }

    /// <summary>
    /// 	Stops each audio track
    /// </summary>
    public void Stop() 
	{
		foreach (AudioStreamPlayer p in GetChildren()) {
			if (p is AudioStreamPlayer) {
				p.Stop();
			}
		}
		Playing = false;
	}

	// Mutes and unmutes each layer according to LayersPlaying
	private void MuteLayers() {
		int i = 1;
		foreach (AudioStreamPlayer p in GetChildren()) {
			if (p is AudioStreamPlayer) {
				if ((LayersPlaying & i) == i) {
					p.VolumeDb = 0.0f;
				} else {
					p.VolumeDb = -80.0f;
				}
			}
			i = i << 1;
		}
	}
}
