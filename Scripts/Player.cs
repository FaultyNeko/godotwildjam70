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
    private bool _hasDoubleJumped;
    private bool _isDashing;
    private bool _isWallSliding;
    private float _dashTimeLeft;
    private float _dashCooldownTimeLeft;
    private bool _isTouchingWall;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor() && !_isDashing && !_isWallSliding)
        {
            velocity.Y += gravity * (float)delta;
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
            else if (_isTouchingWall && !_isDashing)
            {
                // Wall jump
                velocity.Y = WallJumpVelocity;
                velocity.X = (Input.IsActionPressed("right") ? -Speed : (Input.IsActionPressed("left") ? Speed : 0));
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

        // Handle Wall Slide
        if (_isTouchingWall && !IsOnFloor() && velocity.Y > 0)
        {
            _isWallSliding = true;
            velocity.Y = Mathf.Min(velocity.Y, WallSlideSpeed);
            GD.Print("Wall sliding.");
        }
        else
        {
            _isWallSliding = false;
        }
        Velocity = velocity;
        MoveAndSlide();

        // Check if touching a wall
        _isTouchingWall = IsOnWall();

        // Reset jump count when on the floor after movement.
        if (IsOnFloor())
        {
            // Ensure the velocity is stable before resetting
            if (Velocity.Y == 0)
            {
                _jumpCount = 0;
                _hasDoubleJumped = false;
                GD.Print("Landed. Jump count reset to: " + _jumpCount);
            }
        }
    }

    private bool IsOnWall()
    {
        //placeholder method untill i figure out how to handel wall jumping further
        return false;
    }
}
