using Godot;
using System;

public partial class PauseScreen : Control
{
	private void OnResumePressed()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	private void OnQuitPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("Scenes//StartScreen.tscn");
	}
    
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Pause"))
		{
			Visible = !Visible;
			GetTree().Paused = !GetTree().Paused;
		}
	}

	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
