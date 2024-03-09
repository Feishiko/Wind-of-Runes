using Godot;
using System;

public partial class LaserGun : Equipment
{
	private Controller controller;
	public int ammo = 20;
	public int maxAmmo = 20;
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
		name = $"Avali's LaserGun";
		weight = 30;
		part = "RangeWeapon";
		damage = random.Next(controller.currentFloor);
		damageDiceNumber = 0;
		strength = 0;
		agility = random.Next(controller.currentFloor);
		intelligence = random.Next(controller.currentFloor);
		toughness = random.Next(controller.currentFloor);
		AV = 0;
		DV = 0;
		MakeDescription();
		description += "Stolen from avali, auto charge after battle(need to be equipped), charge speed depend on your intelligence\n";
	}
}
