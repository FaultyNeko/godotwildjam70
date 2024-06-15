using Godot;
using System;

public partial class Controls : Control
{
	private void _OnBackPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Settings.tscn");
	}
}
