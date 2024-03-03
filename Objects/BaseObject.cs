using Godot;
using System;

public partial class BaseObject : Node2D
{
	public int gridX;
	public int gridY;
	public bool isVisible = false;
	public bool isMemorized = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
