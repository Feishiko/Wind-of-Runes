using Godot;
using System;

public partial class Clothes : Equipment
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
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Clothes";
		weight = 40;
		part = "Body";
		damage = 0;
		damageDiceNumber = 0;
		strength = random.Next(controller.currentFloor);
		agility = random.Next(controller.currentFloor);
		intelligence = random.Next(controller.currentFloor);
		toughness = random.Next(controller.currentFloor);
		AV = random.Next(controller.currentFloor + 2);
		DV = random.Next(controller.currentFloor + 1);
		MakeDescription();
	}
}
