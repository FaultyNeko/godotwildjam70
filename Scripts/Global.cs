using Godot;
using System;

public partial class Global : Node
{
	public static Player CurrentPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentPlayer = (Player)GetTree().GetFirstNodeInGroup("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
