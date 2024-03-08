using Godot;
using System;

public partial class Boot : Equipment
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
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Boot";
		weight = 20;
		part = "Foot";
		damage = 0;
		damageDiceNumber = 0;
		strength = random.Next(controller.currentFloor);
		agility = random.Next(controller.currentFloor + 1);
		intelligence = random.Next(controller.currentFloor);
		toughness = random.Next(controller.currentFloor);
		AV = random.Next(controller.currentFloor);
		DV = random.Next(controller.currentFloor);
		MakeDescription();
	}
}
