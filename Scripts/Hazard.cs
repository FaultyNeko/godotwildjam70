using Godot;
using System;

public partial class Hazard : Area2D
{
	private void _OnHazardEntered(Node2D body)
	{
		if (body is Player)
		{
			((Player)body).TakeDamage(10);
			if (body.Name == "Demon")
				body.GlobalPosition = new Vector2(3782, 4786);
			else
				body.GlobalPosition = new Vector2(3616, -1008);
		}
	}
}
