using Godot;
using System;

public partial class Wall : BaseObject
{
	[Export]
	private Texture2D textureWall1;
	[Export]
	private Texture2D textureWall2;
	private Controller controller;
	public override void _Ready()
	{
		controller = Controller.GetInstance();
		if (controller.currentFloor <= 5)
		{
			GetNode<Sprite2D>("Sprite2D").Texture = textureWall1;
		}
		if (controller.currentFloor > 5 && controller.currentFloor <= 10)
		{
			GetNode<Sprite2D>("Sprite2D").Texture = textureWall2;
		}
	}

	public override void _Process(double delta)
	{
	}
}
