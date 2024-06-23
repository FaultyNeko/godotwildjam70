using Godot;
using System;

public partial class Camera : Camera2D
{
	private Player _player1, _player2;
	private bool _isPlayer1Active, _isTweenPlaying;
	private HUD _hud;
	private Timer _timer;
	private AudioStreamPlayer _swapSFX;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isPlayer1Active = true;
		_isTweenPlaying = false;
		_hud = (HUD)GetTree().GetFirstNodeInGroup("HUD");
		_timer = GetNode<Timer>("Timer");
		_swapSFX = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		
		foreach (Node node in GetTree().GetNodesInGroup("Player"))
		{
			if (_player1 == null)
				_player1 = node as Player;
			else
				_player2 = node as Player;
		}
		Global.CurrentPlayer = _player1;
		Position = _player1.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_player1.IsActivated = _isPlayer1Active;
		_player2.IsActivated = !_isPlayer1Active;
		PositionSmoothingEnabled = !_isTweenPlaying;

		if (!_isTweenPlaying && GlobalPosition != Global.CurrentPlayer.GlobalPosition)
			GlobalPosition = Global.CurrentPlayer.GlobalPosition;

		if (Input.IsActionPressed("Swap") && !_isTweenPlaying && Global.CurrentPlayer.IsOnFloor())
		{
			Global.CurrentPlayer.IsActivated = false;
			if (!_swapSFX.Playing)
				_swapSFX.Play();
			if (_timer.IsStopped())
				_timer.Start();
		}

		if (Input.IsActionJustReleased("Swap"))
		{
			Global.CurrentPlayer.IsActivated = true;
			_swapSFX.Stop();
			_timer.Stop();
		}
	}

	private void OnTimerTimeout()
	{
		SwitchPlayers();
	}

	public void SwitchPlayers()
	{
		if (Global.CurrentPlayer == _player1)
		{
			AudioManager.PlayerAudio.PlayAudio(this, "SwapToHell", "SFX");
			Global.CurrentPlayer = _player2;

			GD.Print(HasNode("/root/GlobalMusicPlayer"));
			var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
			mp.PlayTrack("Hell");
			(mp.CurrentSong as AudioStreamLayered).LayersPlaying = 0b1;
		}
		else
		{
			AudioManager.PlayerAudio.PlayAudio(this, "SwapToRegal", "SFX");
			Global.CurrentPlayer = _player1;

			GD.Print(HasNode("/root/GlobalMusicPlayer"));
			var mp = GetNode<MusicPlayer>("/root/GlobalMusicPlayer");
			mp.PlayTrack("Regal");
			(mp.CurrentSong as AudioStreamLayered).LayersPlaying = 0b1;
		}

		_hud.UpdateStats(Global.CurrentPlayer);
		_hud.UpdateHealth(Global.CurrentPlayer);
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
