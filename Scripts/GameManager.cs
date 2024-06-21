using Godot;
using System;

public partial class GameManager : Node
{
	public override void _Ready()
	{
		// Assuming you have nodes named Tutorial1Instance1, Tutorial1Instance2, etc.
		var tutorial1Instance1 = GetNode<tutorialtriggers>("Tutorial1");
		tutorial1Instance1.SetTutorialText("Press jump while touching\na wall to perform a wall jump.");

		var tutorial1Instance2 = GetNode<tutorialtriggers>("Tutorial2");
		tutorial1Instance2.SetTutorialText("You got 2 types of attacks\ntry out your melee and ranged one.");

		// Add more instances as needed
	}
}
