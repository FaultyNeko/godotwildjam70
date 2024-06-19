using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    public const float DashSpeed = 600.0f;
    public const float DashDuration = 0.2f;
    public const float DashCooldown = 1.0f;
    public const float WallJumpVelocity = -400.0f;
    public const float WallSlideSpeed = 100.0f;

    private int _jumpCount;
    private bool _isDashing;
    private bool _isWallSliding;
    private float _dashTimeLeft;
    private float _dashCooldownTimeLeft;
    public bool IsActivated = true;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor() && !_isWallSliding && !_isDashing)
            velocity.Y += Gravity * (float)delta;
        else
            _jumpCount = 0;
        
        // Handle Dash input
        if (Input.IsActionJustPressed("dash") && _dashCooldownTimeLeft <= 0)
        {
            _dashTimeLeft = DashDuration;
            _dashCooldownTimeLeft = DashCooldown;
        }
        
        // Handle Dashing
        if (_dashTimeLeft > 0)
        {
            _dashTimeLeft -= (float)delta;
            velocity.X = DashSpeed * Input.GetAxis("left", "right");
            velocity.Y = 0; // Maintain horizontal movement during dash
        }
        else
        {
            velocity.X = Speed * Input.GetAxis("left", "right");
            _dashCooldownTimeLeft -= (float)delta;
        }
        
        // Handle Wall Slide
        _isWallSliding = IsOnWall() && !IsOnFloor() && velocity.Y > 0;
        if (_isWallSliding)
            velocity.Y = WallSlideSpeed;
        
        
        // Handle Jump.
        if (Input.IsActionJustPressed("jump"))
        {
            if (_isWallSliding)
                velocity.Y = WallJumpVelocity;
            else if (_jumpCount < 2)
                velocity.Y = JumpVelocity;
            _jumpCount++;
        }
        
        Velocity = velocity;
        if (!IsActivated)
            Velocity = Vector2.Zero;
        
        MoveAndSlide();
    }
}
