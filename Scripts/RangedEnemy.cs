using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class RangedEnemy : CharacterBody2D
{
	private PackedScene _arrow;
	private Tween _shoot;
	private Vector2 _position;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_arrow = GD.Load<PackedScene>("res://Scenes/Arrow.tscn");
		_shoot = CreateTween();
		_shoot.TweenInterval(2);
		_shoot.TweenCallback(Callable.From(() => _position = Global.CurrentPlayer.GlobalPosition));
		_shoot.TweenCallback(Callable.From(() => Modulate = Colors.Red));
		_shoot.TweenInterval(1);
		_shoot.TweenCallback(Callable.From(Shoot));
		_shoot.SetLoops();
		_shoot.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void Shoot()
	{
		// Should not shoot if player is too far
		if (_position.DistanceTo(Position) > 1000)
			return;
		Area2D arrow = _arrow.Instantiate<Area2D>();
		AddChild(arrow);
		arrow.Rotation = Position.AngleToPoint(_position);
		Tween arrowTween = arrow.CreateTween();
		arrowTween.TweenProperty(arrow, "position", ToLocal(_position), .5);
		arrowTween.TweenCallback(Callable.From(arrow.QueueFree));
		arrowTween.Play();
		Modulate = Colors.White;
	}
	
}
