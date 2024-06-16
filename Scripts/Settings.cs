using Godot;
using System;
using Godot.Collections;

public partial class Settings : Control
{
	private TextureButton _fullscreenButton;
	private Array<Node> _sliders;
	
	const string SaveDir = "user://saves";
	const string SaveName = "settings.tres";
	
	public override void _Ready()
	{
		_fullscreenButton = (TextureButton)GetNode("Fullscreen");
		_sliders = GetTree().GetNodesInGroup("VolumeSliders");
		
		_fullscreenButton.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
		LoadVolume();
	}
	
	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//StartScreen.tscn");
	}

	private void OnControlsPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Controls.tscn");
	}

	private void OnFullscreenToggled(bool toggledOn)
	{
		if (toggledOn)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
	}
	
	public void LoadVolume()
	{
		var volumeConfig = ResourceLoader.Load<SaveConfig>(SaveDir + "/" + SaveName, "", ResourceLoader.CacheMode.Ignore);

		foreach (HSlider slider in _sliders)
			slider.Value = (float)volumeConfig.DataDic[slider.Name];
	}
}
