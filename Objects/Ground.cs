using Godot;
using System;

public partial class Ground : BaseObject
{
	[Export]
	private Texture2D textureGround1;
	[Export]
	private Texture2D textureGround2;
	private Controller controller;
	public override void _Ready()
	{
		controller = Controller.GetInstance();
		if (controller.currentFloor <= 5)
		{
			GetNode<Sprite2D>("Sprite2D").Texture = textureGround1;
		}
		if (controller.currentFloor > 5 && controller.currentFloor <= 10)
		{
			GetNode<Sprite2D>("Sprite2D").Texture = textureGround2;
		}
	}

	public override void _Process(double delta)
	{
	}
}
