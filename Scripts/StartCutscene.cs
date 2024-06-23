using Godot;
using System;

public partial class StartCutscene : Control
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
		if (GetTree().GetFirstNodeInGroup("StartCutsceneLabels") != null)
		{
			Label label = (Label)GetTree().GetFirstNodeInGroup("StartCutsceneLabels");
			Tween labelTween = label.CreateTween();
			labelTween.TweenProperty(label, "modulate:a", 1, 2);
			labelTween.Finished += () => FadeInLabels();
			label.RemoveFromGroup("StartCutsceneLabels");
			labelTween.Play();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
			GetTree().ChangeSceneToFile("Scenes//Level.tscn");
	}
}
