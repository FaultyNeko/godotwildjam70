using Godot;
using System;

public partial class StartScreen : Control
{
	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Main.tscn");
	}
	
	private void OnSettingsPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Settings.tscn");
	}
	
	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
