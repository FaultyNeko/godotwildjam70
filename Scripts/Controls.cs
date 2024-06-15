using Godot;
using System;
using Godot.Collections;

public partial class Controls : Control
{
	private String[] _defaults = { "W" };
	
	const string SaveDir = "user://saves";
	const string SaveName = "controls.tres";

	private Button _currentButton;
	private string _currentAction;
	private bool _remapping;

	public override void _Ready()
	{
		foreach (Button button in GetTree().GetNodesInGroup("RemapButtons"))
			button.Pressed += () => OnButtonPressed(button);
	}

	private void OnButtonPressed(Button button)
	{
		if (!_remapping)
		{
			_remapping = true;
			button.Text = "-";
		}
	}
	
	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("Scenes//Settings.tscn");
	}

	private void OnResetPressed()
	{
		Godot.Collections.Array<Node> buttons = GetTree().GetNodesInGroup("RemapButtons");
		for (int i = 0; i < buttons.Count; i++)
		{
			((Button)buttons[i]).Text = _defaults[i];
		}
	}
	
	//For setting the text of the buttons at the start
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
					str = "M2";
					break;
				case MouseButton.Right:
					str = "M3";
					break;
			}
		}
		return str;
	}
	
	//Detects input and remaps the control
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
		var inputConfig = new InputConfig();
		var saveData = new Dictionary<string, InputEvent>
		{
			{ "Up", InputMap.ActionGetEvents("Up")[0]}
		};

		inputConfig.DataDic = saveData;
		ResourceSaver.Save(inputConfig, SaveDir + "/" + SaveName, ResourceSaver.SaverFlags.ReplaceSubresourcePaths);
	}
	
	public void LoadControls()
	{
		var inputConfig = ResourceLoader.Load<InputConfig>(SaveDir + "/" + SaveName, "", ResourceLoader.CacheMode.Ignore);

		foreach (string str in inputConfig.DataDic.Keys)
		{
			InputMap.EraseAction(str);
			InputMap.AddAction(str);
			InputMap.ActionAddEvent(str, inputConfig.DataDic[str]);
		}
		
	}
}
