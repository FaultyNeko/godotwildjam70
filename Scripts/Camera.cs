using Godot;
using System;

public partial class Camera : Camera2D
{
	private Player _player1, _player2;
	private bool _isPlayer1Active, _isTweenPlaying;
	private HUD _hud;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isPlayer1Active = true;
		_isTweenPlaying = false;
		_hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
		
		foreach (Node node in GetTree().GetNodesInGroup("Player"))
		{
			if (_player1 == null)
				_player1 = node as Player;
			else
				_player2 = node as Player;
		}
		Global.CurrentPlayer = _player1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_player1.IsActivated = _isPlayer1Active;
		_player2.IsActivated = !_isPlayer1Active;
		PositionSmoothingEnabled = !_isTweenPlaying;
			
		if (Input.IsActionJustPressed("Swap") && !_isTweenPlaying)
			SwitchPlayers();
	}

	public void SwitchPlayers()
	{
		if (Global.CurrentPlayer == _player1)
			Global.CurrentPlayer = _player2;
		else
			Global.CurrentPlayer = _player1;
		_hud.UpdateStats(Global.CurrentPlayer);
		_isTweenPlaying = true;
		Reparent(GetTree().Root.GetChild(0));
		Tween tween = CreateTween();
		if (_isPlayer1Active)
		{
			tween.TweenProperty(this, "position", _player2.GlobalPosition, 1.0f)
				.SetTrans(Tween.TransitionType.Back);
			tween.TweenCallback(Callable.From(() => Reparent(_player2)));
		}
		else
		{
			tween.TweenProperty(this, "position", _player1.GlobalPosition, 1.0f).SetTrans(Tween.TransitionType.Back);
			tween.TweenCallback(Callable.From(() => Reparent(_player1)));
		}

		tween.TweenCallback(Callable.From(() => _isTweenPlaying = false));
		_isPlayer1Active = !_isPlayer1Active;
		tween.Play();
	}
}
