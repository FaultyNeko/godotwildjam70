using Godot;
using System;

public partial class DeathScreen : Control
{
	private void OnRetryPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Main.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//StartScreen.tscn");
	}
	
	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
