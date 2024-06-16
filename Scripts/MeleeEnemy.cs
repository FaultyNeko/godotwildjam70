using Godot;
namespace WildGameJam70.Scripts;

public partial class MeleeEnemy : CharacterBody2D
{
	private Player _knight;
	private NavigationAgent2D _nav;
	private float _gravity;
	private bool _didJump;

	public float Speed = 100;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_knight = GetNode<Player>("%Knight");
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		
		Vector2 velocity = Velocity;
		_nav.TargetPosition = _knight.GlobalPosition;
		velocity = ToLocal(_nav.GetNextPathPosition()).Normalized() * Speed;
		_nav.Velocity = velocity;
	}
	
	private void VelocityComputed(Vector2 safeVelocity)
	{
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

		if (_nav.TargetPosition.Y + 30 < GlobalPosition.Y && !_didJump)
			_didJump = true;
		if (_didJump)
			safeVelocity.Y -= 300f;
		Velocity = safeVelocity;
		MoveAndSlide();
	}
}