using Godot;
using System;

public partial class Popup : Node2D
{
	private Label label;

	public override void _Ready()
	{
		label = GetNode<Label>("tutorial1/Label");
	}

	public void SetText(string text)
	{
		label.Text = text;
	}
}
