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
		
		var inputConfig = ResourceLoader.Load<SaveConfig>("user://saves/controls.tres", "", ResourceLoader.CacheMode.Ignore);
		foreach (string str in inputConfig.DataDic.Keys)
		{
			InputMap.EraseAction(str);
			InputMap.AddAction(str);
			InputMap.ActionAddEvent(str, (InputEvent)inputConfig.DataDic[str]);
		}
	}

	private void _ReadyDeferred() {
		//GD.Print(HasNode("/root/GlobalMusicPlayer"));
		var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
		mp.PlayTrack("Title");
	}
	
	private void OnStartPressed()
	{
		//GD.Print(HasNode("/root/GlobalMusicPlayer"));
		var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
		mp.PlayTrack("Regal");
		(mp.CurrentSong as AudioStreamLayered).LayersPlaying = 0b1;

		AddChild(GD.Load<PackedScene>("res://Scenes/StartCutscene.tscn").Instantiate());
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
