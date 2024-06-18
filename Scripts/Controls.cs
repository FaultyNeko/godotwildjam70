using Godot;
using System;
using Godot.Collections;

public partial class Controls : Control
{
	const string SaveDir = "user://saves";
	const string SaveName = "controls.tres";

	private Button _currentButton;
	private string _currentAction;
	private bool _remapping;

	public override void _Ready()
	{
		LoadControls(SaveName);
		var inputConfig = ResourceLoader.Load<SaveConfig>(SaveDir + "/" + SaveName, "", ResourceLoader.CacheMode.Ignore);
		
		foreach (Button button in GetTree().GetNodesInGroup("RemapButtons"))
		{
			button.Pressed += () => OnButtonPressed(button);
			button.Text = _ReturnText((InputEvent)inputConfig.DataDic[button.Name]);
		}
	}

	private void OnButtonPressed(Button button)
	{
		if (!_remapping)
		{
			_remapping = true;
			_currentButton = button;
			_currentAction = button.Name;
			button.Text = "-";
		}
	}
	
	private void OnBackPressed()
	{
		if (!_remapping)
			QueueFree();
	}

	private void OnResetPressed()
	{
		if (!_remapping)
		{
			LoadControls("defaultcontrols.tres");
			SaveControls();
		}
	}
	
	// Returns the character to display of an InputEvent
	public string _ReturnText(InputEvent @event)
	{
		string str = "";
		if (@event is InputEventKey keyEvent)
		{
			str = keyEvent.AsTextKeycode();
		}
		else if (@event is InputEventMouseButton mouseEvent)
		{
			switch (mouseEvent.ButtonIndex)
			{
				case MouseButton.Left:
					str = "M1";
					break;
				case MouseButton.Middle:
					str = "M3";
					break;
				case MouseButton.Right:
					str = "M2";
					break;
			}
		}
		return str;
	}
	
	// Detects input and remaps the control
	public override void _Input(InputEvent @event)
	{
		InputEvent temp = @event;

		if (_remapping && _ReturnText(@event) != "")
		{
			_currentButton.Text = _ReturnText(temp);
			InputMap.EraseAction(_currentAction);
			InputMap.AddAction(_currentAction);
			InputMap.ActionAddEvent(_currentAction, temp);
			_remapping = false;
			SaveControls();
		}
	}
	
	public void SaveControls()
	{
		DirAccess.MakeDirAbsolute(SaveDir);
		var inputConfig = new SaveConfig();
		inputConfig.DataDic = new Dictionary<string, Variant>();

		foreach (Button button in GetTree().GetNodesInGroup("RemapButtons"))
			inputConfig.DataDic.Add(button.Name, InputMap.ActionGetEvents(button.Name)[0]);
		
		ResourceSaver.Save(inputConfig, SaveDir + "/" + SaveName, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
	}
	
	public void LoadControls(String name)
	{
		var inputConfig = ResourceLoader.Load<SaveConfig>(SaveDir + "/" + name, "", ResourceLoader.CacheMode.Ignore);

		foreach (string str in inputConfig.DataDic.Keys)
		{
			InputMap.EraseAction(str);
			InputMap.AddAction(str);
			InputMap.ActionAddEvent(str, (InputEvent)inputConfig.DataDic[str]);
		}
	}

	private void PlayHover()
	{
		AudioManager.PlayerAudio.PlayAudio(this, "HoverButton", "SFX");
	}
}
