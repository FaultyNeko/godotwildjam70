using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -450.0f;
    public float DashSpeed = 600.0f;
    public float DashDuration = 0.2f;
    public const float DashCooldown = 1.0f;
    public const float WallJumpVelocity = -400.0f;
    public const float WallSlideSpeed = 100.0f;

    public int Agility, Strength, Vitality, Health;
    
    private bool _isDashing, _isWallSliding, _isOnGround, _wallJump, _doubleJump;
    private float _dashTimeLeft, _dashCooldownTimeLeft, _dashMult;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Marker2D _positionMarker;
    private HUD _hud;
    private AudioStreamPlayer _stepPlayer;
    private Area2D _hurtbox;
    
    public bool IsActivated = true;
    public int Damage = 15;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _stateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
        _positionMarker = GetNode<Marker2D>("Marker2D");
        Health = 100;
        _hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
        _stepPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _hurtbox = _positionMarker.GetNode<Area2D>("Hurtbox");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (!IsActivated)
            return;
        
        Vector2 velocity = Velocity;
        int direction = Input.GetAxis("Left", "Right").CompareTo(0);
        
        // Handle Idle
        if (direction == 0 && !_isDashing && IsOnFloor())
            _stateMachine.Travel("Idle");

        // Add the gravity.
        if (!IsOnFloor() && !_isWallSliding && !_isDashing)
            velocity.Y += Gravity * (float)delta;

        // Handle Dash input
        if (Input.IsActionJustPressed("Dash") && _dashCooldownTimeLeft <= 0 && direction != 0)
        {
            _dashTimeLeft = DashDuration;
            _dashCooldownTimeLeft = DashCooldown;
            _stateMachine.Travel("Dash");
            _dashMult = direction;
            _positionMarker.Scale = new Vector2(direction, 1);
            AudioManager.PlayerAudio.PlayAudio(this, "Dash", "SFX");
        }
        
        // Handle Dashing
        if (_dashTimeLeft > 0)
        {
            _isDashing = true;
            _dashTimeLeft -= (float)delta;
            velocity.X = DashSpeed * _dashMult;
            velocity.Y = 0; // Maintain horizontal movement during dash
        }
        else
        {
            _isDashing = false;
            velocity.X = Speed * direction;
            _dashCooldownTimeLeft -= (float)delta;
        }
        
        // Handle Running
        if (direction != 0 && !_isDashing)
        {
            _positionMarker.Scale = new Vector2(direction, 1);
            if (IsOnFloor())
                _stateMachine.Travel("Run");
            if (!_isDashing && IsOnFloor() && !_stepPlayer.Playing && FindChild("Land") == null)
                _stepPlayer.Play();
        }

        // Handle Wall Slide
        if (IsOnWall() && !_isWallSliding && !IsOnFloor() && velocity.Y > 0)
            AudioManager.PlayerAudio.PlayAudio(this, "WallGrab", "SFX");
        _isWallSliding = IsOnWall() && !IsOnFloor() && velocity.Y > 0;
        if (_isWallSliding)
        {
            velocity.Y = WallSlideSpeed;
            _stateMachine.Travel("Wall Slide");
        }
        
        // Handle Jump.
        if (Input.IsActionJustPressed("Jump") && ((_wallJump && _isWallSliding) || _doubleJump || IsOnFloor()))
        {
            if (_isWallSliding && _wallJump)
            {
                velocity.Y = WallJumpVelocity;
                _wallJump = false;
            }
            else if (_doubleJump)
            {
                velocity.Y = JumpVelocity;
                _doubleJump = false;
            }
            else if (IsOnFloor())
            {
                velocity.Y = JumpVelocity;
            }
            AudioManager.PlayerAudio.PlayAudio(this, "Jump", "SFX");
            _stateMachine.Travel("Jump");
        }
        
        if (IsOnFloor())
        {
            _wallJump = true;
            _doubleJump = true;
        }
        
        // Handle Landing SFX
        if (!_isOnGround && IsOnFloor())
        {
            _stateMachine.Travel("Land");
            AudioManager.PlayerAudio.PlayAudio(this, "Land", "SFX");
        }
        _isOnGround = IsOnFloor();
        
        // Handle attacking
        if (Input.IsActionJustPressed("Attack"))
        {
            if (!_hurtbox.Monitorable)
                AudioManager.PlayerAudio.PlayAudio(this, "PlayerAttack", "SFX");
            _stateMachine.Travel("Attack");
        }
        
        Velocity = velocity;
        
        MoveAndSlide();
    }
    
    public void ChangeStat(string stat, int value)
    {
        switch (stat)
        {
            case "Agility":
                Agility += value;
                DashSpeed += 25;
                DashDuration += .025f;
                break;
            case "Strength":
                Strength += value;
                Damage += 5;
                break;
            case "Vitality":
                Vitality += value;
                Health += 20;
                _hud.UpdateHealth(this);
                break;
        }
        _hud.UpdateStats(this);
    }

    public void TakeDamage(int damage)
    {
        if (IsActivated)
        {
            AudioManager.PlayerAudio.PlayAudio(this, "PlayerHit", "SFX");
            Health -= damage;
            _hud.UpdateHealth(this);
            if (Health <= 0)
                GetTree().ChangeSceneToFile("Scenes//DeathScreen.tscn");
        }
    }

    private void OnHurtboxEntered(Node2D body)
    {
        if (body is FlyingEnemy)
        {
            ((FlyingEnemy)body).TakeDamage(Damage);
        }
        else if (body is MeleeEnemy)
        {
            ((MeleeEnemy)body).TakeDamage(Damage);
        }
        else if (body is RangedEnemy)
        {
            ((RangedEnemy)body).TakeDamage(Damage);
        }
    }
}
