using Godot;
using Godot.Collections;

namespace WildGameJam70.Scripts;

public partial class InputConfig : Resource
{
	[Export]
	public Dictionary<string, InputEvent> DataDic;
}