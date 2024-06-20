using Godot;
using System;

public partial class UpgradeScreen : Control
{
	private Player _knight, _demon;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var players = GetTree().GetNodesInGroup("Player");
		_knight = (Player)players[0];
		_demon = (Player)players[1];
		
		GetTree().Paused = true;
		
		foreach (Button button in GetTree().GetNodesInGroup("UpgradeButtons"))
		{
			button.Pressed += () => OnButtonPressed(button);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed(Button button)
	{
		Global.CurrentPlayer.ChangeStat(button.Name, 1);
		GetTree().Paused = false;
		QueueFree();
	}

	private void OnExitPressed()
	{
		GetTree().Paused = false;
		QueueFree();
	}
	
	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
