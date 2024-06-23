using Godot;
using System;

public partial class WinArea : Area2D
{
	private bool _knightIn, _demonIn, _gameWon;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_knightIn && _demonIn && !_gameWon)
		{
			_gameWon = true;
			((Player)GetTree().GetNodesInGroup("Player")[0]).IsActivated = false;
			((Player)GetTree().GetNodesInGroup("Player")[1]).IsActivated = false;
			GetTree().GetFirstNodeInGroup("HUD").AddChild(GD.Load<PackedScene>("res://Scenes/EndCutscene.tscn").Instantiate());
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Knight")
			_knightIn = true;
		else if (body.Name == "Demon")
			_demonIn = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.Name == "Knight")
			_knightIn = false;
		else if (body.Name == "Demon")
			_demonIn = false;
	}
}
