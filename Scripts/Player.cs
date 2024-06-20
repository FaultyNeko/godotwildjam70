using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -450.0f;
    public const float DashSpeed = 600.0f;
    public const float DashDuration = 0.2f;
    public const float DashCooldown = 1.0f;
    public const float WallJumpVelocity = -400.0f;
    public const float WallSlideSpeed = 100.0f;

    public int Agility, Strength, Vitality, Health;
    
    private int _jumpCount;
    private bool _isDashing;
    private bool _isWallSliding;
    private float _dashTimeLeft;
    private float _dashCooldownTimeLeft;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Marker2D _positionMarker;
    private HUD _hud;
    
    public bool IsActivated = true;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _stateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
        _positionMarker = GetNode<Marker2D>("Marker2D");
        Health = 100;
        _hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        int direction = Input.GetAxis("Left", "Right").CompareTo(0);

        // Add the gravity.
        if (!IsOnFloor() && !_isWallSliding && !_isDashing)
            velocity.Y += Gravity * (float)delta;
        
        if (IsOnFloor())
            _jumpCount = 0;
        
        // Handle Dash input
        if (Input.IsActionJustPressed("Dash") && _dashCooldownTimeLeft <= 0)
        {
            _dashTimeLeft = DashDuration;
            _dashCooldownTimeLeft = DashCooldown;
        }
        
        // Handle Dashing
        if (_dashTimeLeft > 0)
        {
            _dashTimeLeft -= (float)delta;
            velocity.X = DashSpeed * direction;
            velocity.Y = 0; // Maintain horizontal movement during dash
        }
        else
        {
            velocity.X = Speed * direction;
            _dashCooldownTimeLeft -= (float)delta;
        }
        
        if (direction != 0)
            _positionMarker.Scale = new Vector2(direction, 1);
        
        // Handle Wall Slide
        _isWallSliding = IsOnWall() && !IsOnFloor() && velocity.Y > 0;
        if (_isWallSliding)
            velocity.Y = WallSlideSpeed;
        
        // Handle Jump.
        if (Input.IsActionJustPressed("Jump"))
        {
            if (_isWallSliding && _jumpCount < 3)
            {
                velocity.Y = WallJumpVelocity;
                _jumpCount++;
            }
            else if (_jumpCount < 2)
            {
                velocity.Y = JumpVelocity;
                _jumpCount++;
            }
        }
        
        Velocity = velocity;
        
        if (!IsActivated)
            Velocity = Vector2.Zero;
        
        MoveAndSlide();
    }
    
    public void ChangeStat(string stat, int value)
    {
        switch (stat)
        {
            case "Agility":
                Agility += value;
                break;
            case "Strength":
                Strength += value;
                break;
            case "Vitality":
                Vitality += value;
                break;
        }
        _hud.UpdateStats(this);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _hud.UpdateHealth(this);
        if (Health <= 0)
            GetTree().ChangeSceneToFile("Scenes//DeathScreen.tscn");
    }
}
