using Godot;
using System;

public partial class PickUp : BaseObject
{
	[Export]
	public Texture2D icon { get; set; }
	public string name { get; set; }
	public string description { get; set; }
	public int weight { get; set; }
	public bool selected { get; set; } = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
