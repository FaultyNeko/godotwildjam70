using Godot;
using System;
using Godot.Collections;

public partial class VolumeSlider : HSlider
{
	private int _busIndex;
	[Export] private String _busName;
	
	const string SaveDir = "user://saves";
	const string SaveName = "settings.tres";
	
	public override void _Ready()
	{
		_busIndex = AudioServer.GetBusIndex(_busName);
		Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(_busIndex));
	}

	private void _OnValueChanged(float value)
	{
		AudioServer.SetBusVolumeDb(_busIndex, Mathf.LinearToDb(value));
		SaveVolume();
	}
	
	public void SaveVolume()
	{
		DirAccess.MakeDirAbsolute(SaveDir);
		var saveData = ResourceLoader.Load<SaveConfig>(SaveDir + "/" + SaveName, "", ResourceLoader.CacheMode.Ignore);
		
		saveData.DataDic.Remove(Name);
		saveData.DataDic.Add(Name, Value);
		
		ResourceSaver.Save(saveData, SaveDir + "/" + SaveName, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
	}
}
