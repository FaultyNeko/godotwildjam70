using Godot;
using System;

public partial class PauseScreen : Control
{
	private void OnSettingsPressed()
	{
		AddChild(GD.Load<PackedScene>("res://Scenes/Settings.tscn").Instantiate());
	}
	
	private void OnResumePressed()
	{
		Visible = false;
		AudioManager.PlayerAudio.PlayAudio(this, "Unpaused", "SFX");
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
			if (Visible)
				AudioManager.PlayerAudio.PlayAudio(this, "Unpaused", "SFX");
			else
				AudioManager.PlayerAudio.PlayAudio(this, "Paused", "SFX");
			Visible = !Visible;
			GetTree().Paused = !GetTree().Paused;
		}
	}

	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
