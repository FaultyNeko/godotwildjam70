using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class RangedEnemy : CharacterBody2D
{
	private PackedScene _arrow;
	private Tween _shoot;
	private Vector2 _position;
	private Marker2D _positionMarker;
	private int _health;
	
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
		_positionMarker = GetNode<Marker2D>("Marker2D");
		_health = 100;
	}

	private void Shoot()
	{
		// Should not shoot if player is too far
		if (_position.DistanceTo(Position) > 1000)
			return;
		Area2D arrow = _arrow.Instantiate<Area2D>();
		AudioManager.PlayerAudio.PlayPositionalAudio(this, "Dash", "SFX");
		AddChild(arrow);
		arrow.Rotation = Position.AngleToPoint(_position);
		Tween arrowTween = arrow.CreateTween();
		arrow.BodyEntered += body => ArrowCollision(body, arrow);
		arrowTween.TweenProperty(arrow, "position", ToLocal(_position), .5);
		arrowTween.TweenCallback(Callable.From(() => arrow.CallDeferred(MethodName.QueueFree)));
		arrowTween.Play();
		Modulate = Colors.White;
		_positionMarker.Scale = new Vector2((_position.X - Position.X).CompareTo(0), 1);
	}

	private void ArrowCollision(Node2D body, Node2D arrow)
	{
		if (body.IsInGroup("Player"))
		{
			((Player)body).TakeDamage(10);
			arrow.CallDeferred(MethodName.QueueFree);
		}
	}
	
	public void TakeDamage(int damage)
	{
		_health -= damage;
		AudioManager.PlayerAudio.PlayPositionalAudio(this, "EnemyHit", "SFX");
		if (_health <= 0)
		{
			AudioManager.PlayerAudio.PlayPositionalAudio(GetParent(), "EnemyHit", "SFX");
			QueueFree();
		}
	}
}
