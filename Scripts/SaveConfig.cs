using Godot;
using System;

public partial class SaveConfig : Resource
{
    [Export]
    public Godot.Collections.Dictionary<string, Variant> DataDic;
}
