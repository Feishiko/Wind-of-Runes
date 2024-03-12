using Godot;
using System;

public class JsonFood : JsonPickUp
{
	public int nutrition { get; set; }
	public string type { get; set; } // Bread, Corpse..
}

public partial class Food : PickUp
{
	public int nutrition { get; set; }
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
