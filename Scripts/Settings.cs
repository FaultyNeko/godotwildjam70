using Godot;
using System;

public partial class Settings : Control
{
	private void _OnBackPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Main.tscn");
	}

	private void _OnControlsPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Controls.tscn");
	}
}
