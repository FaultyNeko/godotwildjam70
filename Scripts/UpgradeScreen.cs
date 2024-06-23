using Godot;
using System;
using Godot.Collections;

public partial class UpgradeScreen : Control
{
	private HUD _hud;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Paused = true;
		_hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
		foreach (Button button in GetTree().GetNodesInGroup("UpgradeButtons"))
		{
			button.Pressed += () => OnButtonPressed(button);
		}
		
		AudioManager.PlayerAudio.PlayAudio(GetParent(), "Upgrade", "SFX");
	}

	private void OnButtonPressed(Button button)
	{
		Global.CurrentPlayer.ChangeStat(button.Name, 1);
		GetTree().Paused = false;
		_hud.EraseNodes();
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
