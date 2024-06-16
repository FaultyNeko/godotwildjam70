using Godot;
using System;

public partial class VolumeSlider : HSlider
{
	private int _busIndex;
	[Export] private String _busName;
	
	public override void _Ready()
	{
		_busIndex = AudioServer.GetBusIndex(_busName);
		Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(_busIndex));
	}

	private void _OnValueChanged(float value)
	{
		AudioServer.SetBusVolumeDb(_busIndex, Mathf.LinearToDb(value));
	}
}
