using Godot;
using System;

public partial class HUD : Control
{
	private Player _knight, _demon;
	private VBoxContainer _statList;
	private TextureProgressBar _healthBar;
	private Label _healthBarText;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var players = GetTree().GetNodesInGroup("Player");
		_knight = (Player)players[0];
		_demon = (Player)players[1];
		_statList = GetNode<VBoxContainer>("StatList");
		_healthBar = GetNode<TextureProgressBar>("HealthBar");
		_healthBarText = _healthBar.GetNode<Label>("HPValue"); // HealthBar/HPValue?

		_knight.Agility = 3;
		_knight.Strength = 1;
		_knight.Vitality = 1;
		
		_demon.Agility = 1;
		_demon.Strength = 3;
		_demon.Vitality = 1;
		
		UpdateStats(_knight);
		UpdateHealth(_knight);
	}

	public void UpdateStats(Player player)
	{
		var stats = _statList.GetChildren();
		((Label)stats[0]).Text = "Agility - " + player.Agility;
		((Label)stats[1]).Text = "Strength - " + player.Strength;
		((Label)stats[2]).Text = "Vitality - " + player.Vitality;
	}

	public void UpdateHealth(Player player)
	{
		if (player.Health > _healthBar.MaxValue)
			_healthBar.MaxValue = player.Health;
		_healthBar.Value = player.Health;
		_healthBarText.Text = player.Health + "/" + _healthBar.MaxValue;
	}

	private void UpgradeNodeEntered(Node body)
	{
		if (body is Player)
		{
			AddChild(GD.Load<PackedScene>("res://Scenes/UpgradeScreen.tscn").Instantiate());
		}
	}
}
