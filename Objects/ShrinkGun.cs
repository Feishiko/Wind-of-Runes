using Godot;
using System;

public partial class ShrinkGun : Equipment
{
	private Controller controller;
	public int ammo = 10;
	public int maxAmmo = 10;
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
		name = $"Avali's ShrinkGun";
		weight = 30;
		part = "RangeWeapon";
		damage = 0;
		damageDiceNumber = 0;
		strength = 0;
		agility = 0;
		intelligence = random.Next(controller.currentFloor);
		toughness = random.Next(controller.currentFloor);
		AV = 0;
		DV = 0;
		MakeDescription();
		description += "Shrink enemies, auto charge after battle(need to be equipped), charge speed depend on your intelligence\n";
	}
}
