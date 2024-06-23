using Godot;
using System;

public partial class MeleeEnemy : CharacterBody2D
{
	public int Health { get => _health; }
	private NavigationAgent2D _nav;
	private float _gravity;
	private bool _didJump;
	private Marker2D _positionMarker;
	private AnimationNodeStateMachinePlayback _stateMachine;
	private Timer _timer;
	[Export] private int _health = 10;

	public float Speed = 100;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_positionMarker = GetNode<Marker2D>("Marker2D");
		_stateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
		_timer = GetNode<Timer>("Timer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		
		if (Global.CurrentPlayer.GlobalPosition.DistanceTo(Position) > 1000)
			return;
		
		// We don't want Melee enemies to attempt to resist gravity
		_nav.TargetPosition = Global.CurrentPlayer.GlobalPosition;
		if (Math.Abs(GlobalPosition.X - _nav.TargetPosition.X) < 750f)
		{
			Vector2 velocity = Velocity;
			velocity = new Vector2(ToLocal(_nav.GetNextPathPosition()).X.CompareTo(0) * Speed, 0);
			_nav.Velocity = velocity;
			_positionMarker.Scale = new Vector2(velocity.X.CompareTo(0), 1);
		}

		if (Math.Abs(GlobalPosition.X - _nav.TargetPosition.X) < 175f && _timer.TimeLeft == 0)
		{
			Modulate = Colors.Red;
			_timer.Start();
		}
	}

	private void OnTimerTimeout()
	{
		if (Modulate == Colors.Red)
		{
			AudioManager.PlayerAudio.PlayPositionalAudio(this, "EnemyAttack", "SFX");
			_stateMachine.Travel("Attack");
			Modulate = Colors.White;
			_timer.Start();
		}
	}
	
	private void VelocityComputed(Vector2 safeVelocity)
	{
		// Allow the gravity to accumulate over time, because safe velocity would keep resetting it
		if (!IsOnFloor())
		{
			_gravity += 9.81f;
			safeVelocity.Y = _gravity;
		}
		else
		{
			_gravity = 0;
			_didJump = false;
		}
		
		// Allows the effects of the jump to persist over time, to make the jump more natural.
		// The amount being subtracted is constant, so gravity will eventually overcome it.
		if (_nav.TargetPosition.Y + 30 < GlobalPosition.Y && !_didJump && IsOnWall())
			_didJump = true;
		if (_didJump)
			safeVelocity.Y -= 300f;
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	private void OnHurtboxEntered(Node2D body)
	{
		if (body is Player)
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