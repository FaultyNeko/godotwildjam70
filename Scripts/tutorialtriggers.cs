using Godot;
using System;

public partial class tutorialtriggers : Area2D
{
	private Popup popupInstance;

	public override void _Ready()
	{
		popupInstance = GetNode<Popup>("Tutorialpopup");
		popupInstance.Visible = false; // Initially hidden
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	public void SetTutorialText(string text)
	{
		if (popupInstance != null)
		{
			popupInstance.SetText(text);
		}
	}

	private void OnBodyEntered(Node body)
	{
		// Check if the body is on layer 1
		if (body is PhysicsBody2D physicsBody && (physicsBody.CollisionLayer & 1) != 0)
		{
			if (popupInstance != null)
			{
				popupInstance.Visible = true; // Show the popup
			}
		}
	}

	private void OnBodyExited(Node body)
	{
		// Check if the body is on layer 1
		if (body is PhysicsBody2D physicsBody && (physicsBody.CollisionLayer & 1) != 0)
		{
			if (popupInstance != null)
			{
				popupInstance.Visible = false; // Hide the popup
			}
		}
	}
}
