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
    public const float WallJumpHorizontalSpeed = 300.0f; // Horizontal speed for wall jumps
    public const float WallSlideGravityFactor = 0.5f; // Gravity factor during wall slide

    private int _jumpCount;
    private bool _hasDoubleJumped;
    private bool _isDashing;
    private float _dashTimeLeft;
    private float _dashCooldownTimeLeft;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    
    private RayCast2D _rayCastLeft;
    private RayCast2D _rayCastRight;
    private AnimationPlayer _animationPlayer;
    
    public override void _Ready()
    {
        _rayCastLeft = GetNode<RayCast2D>("RayCast2DLeft");
        _rayCastRight = GetNode<RayCast2D>("RayCast2DRight");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor() && !_isDashing)
        {
            if (IsOnWall())
            {
                if (velocity.Y > 0)
                {
                    velocity.Y = 0; // Immediately set Y velocity to 0 when touching the wall
                }
                velocity.Y += gravity * WallSlideGravityFactor * (float)delta; // Apply reduced gravity for wall sliding
            }
            else
            {
                velocity.Y += gravity * (float)delta;
            }
        }
        // Handle Dash input
        if (Input.IsActionJustPressed("dash") && _dashCooldownTimeLeft <= 0)
        {
            _isDashing = true;
            _dashTimeLeft = DashDuration;
            _dashCooldownTimeLeft = DashCooldown;
        }
        // Handle dashing
        if (_isDashing)
        {
            _dashTimeLeft -= (float)delta;
            if (_dashTimeLeft > 0)
            {
                velocity.X = DashSpeed * (Input.IsActionPressed("right") ? 1 : (Input.IsActionPressed("left") ? -1 : 0));
                velocity.Y = 0; // Maintain horizontal movement during dash
            }
            else
            {
                _isDashing = false;
            }
        }
        else
        {
            if (Input.IsActionPressed("right"))
            {
                velocity.X = Speed;
            }
            else if (Input.IsActionPressed("left"))
            {
                velocity.X = -Speed;
            }
            else
            {
                velocity.X = 0;
            }
        }
        // Update dash cooldown timer
        if (_dashCooldownTimeLeft > 0)
        {
            _dashCooldownTimeLeft -= (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor())
            {
                // Ground jump
                velocity.Y = JumpVelocity;
                _jumpCount = 1;
                _hasDoubleJumped = false; // Reset double jump flag
            }
            else if (IsOnWall() && !_isDashing)
            {
                // Wall jump
                velocity.Y = WallJumpVelocity;
                if (_rayCastLeft.IsColliding())
                {
                    velocity.X = WallJumpHorizontalSpeed; // Jump to the right if on the left wall
                }
                else if (_rayCastRight.IsColliding())
                {
                    velocity.X = -WallJumpHorizontalSpeed; // Jump to the left if on the right wall
                }
                _jumpCount = 1; // Reset jump count for a double jump
                _hasDoubleJumped = false;
            }
            else if (_jumpCount == 1 && !_hasDoubleJumped)
            {
                // Double jump
                velocity.Y = JumpVelocity;
                _hasDoubleJumped = true; // Set double jump flag
                _jumpCount = 2; // Register the double jump
            }
            else if (_jumpCount < 1)
            {
                // Allow single jump in air if player hasn't jumped yet
                velocity.Y = JumpVelocity;
                _jumpCount = 1;
                _hasDoubleJumped = true;
            }
        }
        // Reset jump count when on the floor after movement.
        if (IsOnFloor())
        {
            // Ensure the velocity is stable before resetting
            if (Velocity.Y == 0)
            {
                _jumpCount = 0;
                _hasDoubleJumped = false;
            }
        }
        Velocity = velocity;
        MoveAndSlide();
    }
    private bool IsOnWall()
    {
        // Check if either RayCast2D is colliding
        return _rayCastLeft.IsColliding() || _rayCastRight.IsColliding();
    }
}
   

