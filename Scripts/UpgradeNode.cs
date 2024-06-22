using Godot;
using System;

public partial class UpgradeNode : Area2D
{
	private HUD _hud;

	[Export] public int Index;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
		BodyEntered += body => _hud.UpgradeNodeEntered(this, body);
	}

	public void Break()
	{
		Monitoring = false;
		Modulate = Colors.DarkGray;
	}
}
