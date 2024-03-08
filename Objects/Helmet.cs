using Godot;
using System;

public partial class Helmet : Equipment
{
	private Controller controller;
	public override void _Ready()
	{
		controller = Controller.GetInstance();
	}

	public override void _Process(double delta)
	{
	}

	public void Init()
	{
		controller = Controller.GetInstance();
		var random = new Random();
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Helmet";
		weight = 30;
		part = "Head";
		damage = 0;
		damageDiceNumber = 0;
		strength = random.Next(controller.currentFloor);
		agility = random.Next(controller.currentFloor);
		intelligence = random.Next(controller.currentFloor + 1);
		toughness = random.Next(controller.currentFloor + 1);
		AV = random.Next(controller.currentFloor + 1);
		DV = random.Next(controller.currentFloor);
		MakeDescription();
	}
}
