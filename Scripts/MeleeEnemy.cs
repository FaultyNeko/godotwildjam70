using Godot;
namespace WildGameJam70.Scripts;

public partial class MeleeEnemy : CharacterBody2D
{
	private NavigationAgent2D _nav;
	private float _gravity;
	private bool _didJump;

	public float Speed = 100;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		
		// We don't want Melee enemies to attempt to resist gravity
		Vector2 velocity = Velocity;
		_nav.TargetPosition = Global.CurrentPlayer.GlobalPosition;
		velocity = new Vector2(ToLocal(_nav.GetNextPathPosition()).X.CompareTo(0) * Speed, 0);
		_nav.Velocity = velocity;
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
}