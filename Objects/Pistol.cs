using Godot;
using System;

public partial class Pistol : Equipment
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
		name = $"{RandomName.RandomCharacterName()} {RandomName.RandomSpecies()}'s Pistol";
		weight = 30;
		part = "RangeWeapon";
		damage = random.Next(controller.currentFloor);
		damageDiceNumber = 0;
		strength = 0;
		agility = random.Next(controller.currentFloor + 1);
		intelligence = 0;
		toughness = random.Next(controller.currentFloor);
		AV = 0;
		DV = 0;
		MakeDescription();
		description += "Need ammos to shoot, damage depend on your agility\n";
	}
}
