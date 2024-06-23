using Godot;
using System;

public partial class FlyingEnemy : CharacterBody2D
{
	private NavigationAgent2D _nav;
	private Marker2D _marker;
	private PackedScene _egg;
	private Tween _layEgg;
	private int _health;

	public float Speed = 100;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_marker = GetNode<Marker2D>("Marker2D");
		_egg = GD.Load<PackedScene>("res://Scenes/Egg.tscn");
		_layEgg = CreateTween();
		_layEgg.TweenCallback(Callable.From(LayEgg));
		_layEgg.TweenInterval(1);
		_layEgg.SetLoops();
		_health = 100;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		
		if (Global.CurrentPlayer.GlobalPosition.DistanceTo(Position) > 1000)
			return;
		
		// Flying enemies can't go up or down
		_nav.TargetPosition = Global.CurrentPlayer.GlobalPosition;
		Vector2 velocity = Vector2.Zero;
		float distance = Math.Abs(GlobalPosition.X - _nav.TargetPosition.X);
		if (distance > 10f && distance < 750f)
			velocity = new Vector2(ToLocal(_nav.GetNextPathPosition()).X.CompareTo(0) * Speed, 0);
		
		if (distance < 100f && !_layEgg.IsRunning())
			_layEgg.Play();
		else
			_layEgg.Pause();

		_nav.Velocity = velocity;
		_marker.Scale = new Vector2(velocity.X.CompareTo(0), 1);
	}
	
	private void VelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	private void LayEgg()
	{
		RigidBody2D egg = _egg.Instantiate<RigidBody2D>();
		AudioManager.PlayerAudio.PlayPositionalAudio(this, "EnemyEgg", "SFX");
		AddChild(egg);
		egg.Position = _marker.Position with {Y = _marker.Position.Y + 20};
		egg.BodyEntered += body => EggCollision(body);
		egg.BodyEntered += body => egg.CallDeferred(MethodName.QueueFree);
	}

	private void EggCollision(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			((Player)body).TakeDamage(10);
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
