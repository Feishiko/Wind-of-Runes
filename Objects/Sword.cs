using Godot;
using System;

public partial class Sword : Equipment
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
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Sword";
		weight = 20;
		part = "Weapon";
		damage = random.Next(1, controller.currentFloor);
		damageDiceNumber = random.Next(1, Mathf.Max((int)Mathf.Floor(controller.currentFloor*.2f), 1));
		strength = random.Next(controller.currentFloor + 1);
		agility = 0;
		intelligence = 0;
		toughness = random.Next(controller.currentFloor);
		AV = 0;
		DV = 0;
		MakeDescription();
	}
}
