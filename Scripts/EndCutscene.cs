using Godot;
using System;

public partial class EndCutscene : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureRect background = GetNode<TextureRect>("TextureRect");
		Tween fadeIn = background.CreateTween();
		fadeIn.TweenProperty(background, "modulate:a", 1, 2);
		fadeIn.Finished += () => FadeInLabels();
		fadeIn.Play();
	}

	public void FadeInLabels()
	{
		if (GetTree().GetFirstNodeInGroup("EndCutsceneLabels") != null)
		{
			Label label = (Label)GetTree().GetFirstNodeInGroup("EndCutsceneLabels");
			Tween labelTween = label.CreateTween();
			labelTween.TweenProperty(label, "modulate:a", 1, 2);
			labelTween.Finished += () => FadeInLabels();
			label.RemoveFromGroup("EndCutsceneLabels");
			labelTween.Play();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
			GetTree().ChangeSceneToFile("Scenes//WinScreen.tscn");
	}
}
