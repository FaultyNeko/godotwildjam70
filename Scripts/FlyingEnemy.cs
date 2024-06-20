using Godot;
using System;

public partial class FlyingEnemy : CharacterBody2D
{
	private Player _knight;
	private NavigationAgent2D _nav;
	private Marker2D _marker;
	private PackedScene _egg;
	private Tween _layEgg;

	public float Speed = 100;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_knight = GetNode<Player>("%Knight");
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_marker = GetNode<Marker2D>("Marker2D");
		_egg = GD.Load<PackedScene>("res://Scenes/Egg.tscn");
		_layEgg = CreateTween();
		_layEgg.TweenCallback(Callable.From(LayEgg));
		_layEgg.TweenInterval(1);
		_layEgg.SetLoops();
		_layEgg.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		
		// Flying enemies can't go up or down
		_nav.TargetPosition = _knight.GlobalPosition;
		Vector2 velocity = Vector2.Zero;
		if (Math.Abs(GlobalPosition.X - _nav.TargetPosition.X) > 10f)
			velocity = new Vector2(ToLocal(_nav.GetNextPathPosition()).X.CompareTo(0) * Speed, 0);
		_nav.Velocity = velocity;
		
		
	}
	
	private void VelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	private void LayEgg()
	{
		RigidBody2D egg = _egg.Instantiate<RigidBody2D>();
		AddChild(egg);
		egg.Position = _marker.Position with {Y = _marker.Position.Y + 40};
		egg.BodyEntered += (body) => EggCollision(body);
		egg.BodyEntered += (body) => egg.QueueFree();
	}

	private void EggCollision(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			// Damage the player TODO
		}
	}
}