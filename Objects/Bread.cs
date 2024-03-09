using Godot;
using System;

public partial class Bread : Food
{
	private Controller controller;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void Init()
	{
		controller = Controller.GetInstance();
		switch (controller.player.species)
		{
			case "Human": nutrition = 200; break;
			case "Kobold": nutrition = 100; break;
			case "Avian": nutrition = 50; break;
			case "Avali": nutrition = 50; break;
			case "Robot": nutrition = 0; break;
		}
		name = "Bread";
		weight = 10;
	}
}
