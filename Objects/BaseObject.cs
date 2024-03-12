using Godot;
using System;

public class JsonBaseObject
{
	public int gridX { get; set; }
	public int gridY { get; set; }
	public bool isVisible { get; set; }
	public bool isMemorized { get; set; }
}

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

	public void Copy(BaseObject baseObject)
	{
		gridX = baseObject.gridX;
		gridY = baseObject.gridY;
		isVisible = baseObject.isVisible;
		isMemorized = baseObject.isMemorized;
	}
}
