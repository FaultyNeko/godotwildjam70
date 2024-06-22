using Godot;
using System;

public partial class StartScreen : Control
{
	public override void _Ready()
	{
		CallDeferred("_ReadyDeferred");
		var volumeConfigDic = ResourceLoader.Load<SaveConfig>("user://saves/settings.tres", "", ResourceLoader.CacheMode.Ignore).DataDic;
		
		foreach (string key in volumeConfigDic.Keys)
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(key), Mathf.LinearToDb((float)volumeConfigDic[key]));
	}

	private void _ReadyDeferred() {
		GD.Print(HasNode("/root/GlobalMusicPlayer"));
		var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
		mp.PlayTrack("Title");
	}
	
	private void OnStartPressed()
	{
		GD.Print(HasNode("/root/GlobalMusicPlayer"));
		var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
		mp.PlayTrack("Regal");

		//AudioManager.PlayerAudio.PlayAudio(this, "StartGame", "Music");
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
