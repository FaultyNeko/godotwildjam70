using Godot;
using Godot.Collections;

public partial class InputConfig : Resource
{
    [Export]
    public Dictionary<string, InputEvent> DataDic;
}