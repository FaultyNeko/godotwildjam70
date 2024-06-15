using Godot;
using System;

public partial class StartScreen : Control
{
	private void _OnStartPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Main.tscn");
	}
	
	private void _OnSettingsPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Settings.tscn");
	}
	
	private void _OnQuitPressed()
	{
		GetTree().Quit();
	}
}
