using Godot;
using System;

public partial class Glove : Equipment
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
		var random = new Random();
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Gloves";
		weight = 10;
		part = "Hand";
		damage = 0;
		damageDiceNumber = 0;
		strength = random.Next(controller.currentFloor + 1);
		agility = random.Next(controller.currentFloor);
		intelligence = random.Next(controller.currentFloor);
		toughness = random.Next(controller.currentFloor);
		AV = random.Next(controller.currentFloor);
		DV = random.Next(controller.currentFloor);
		MakeDescription();
	}
}
