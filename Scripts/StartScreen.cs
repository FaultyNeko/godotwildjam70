using Godot;
using System;

public partial class StartScreen : Control
{
	public override void _Ready()
	{
		var volumeConfigDic = ResourceLoader.Load<SaveConfig>("user://saves/settings.tres", "", ResourceLoader.CacheMode.Ignore).DataDic;

		foreach (string key in volumeConfigDic.Keys)
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(key), Mathf.LinearToDb((float)volumeConfigDic[key]));
	}
	
	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Main.tscn");
	}
	
	private void OnSettingsPressed()
	{
		AddChild(GD.Load<PackedScene>("res://Scenes/Settings.tscn").Instantiate());
	}
	
	private void OnQuitPressed()
	{
		GetTree().Quit();
	}

	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
