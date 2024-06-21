using Godot;
using System;

public partial class tutorialtriggers : Area2D
{
	private Popup _popupInstance;

	public override void _Ready()
	{
		// Locate the popup node within the parent's children and store the reference
		_popupInstance = GetParent().GetNode<Popup>("Tutorialpopup");
		if (_popupInstance != null)
		{
			_popupInstance.Visible = false; // Initially hidden
		}

		// Connect the body_entered and body_exited signals to their respective methods
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	// Method to set the tutorial text on the popup
	public void SetTutorialText(string text)
	{
		_popupInstance?.SetText(text);
	}

	// Called when a body enters the area
	private void OnBodyEntered(Node body)
	{
		// Check if the body is on layer 1 and make the popup visible
		if (body is PhysicsBody2D physicsBody && (physicsBody.CollisionLayer & 1) != 0)
		{
			_popupInstance.Visible = true;
		}
	}

	// Called when a body exits the area
	private void OnBodyExited(Node body)
	{
		// Check if the body is on layer 1 and hide the popup
		if (body is PhysicsBody2D physicsBody && (physicsBody.CollisionLayer & 1) != 0)
		{
			_popupInstance.Visible = false;
		}
	}
}
