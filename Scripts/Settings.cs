using Godot;
using System;

public partial class Settings : Control
{
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
}
