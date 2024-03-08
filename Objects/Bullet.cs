using Godot;
using System;

public partial class Bullet : Equipment
{
	private Controller controller;
	public int numbers = 1;
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
		// var random = new Random();
		name = "Bullet";
		weight = 0;
		part = "Ammo";
		damage = 0;
		damageDiceNumber = 0;
		strength = 0;
		agility = 0;
		intelligence = 0;
		toughness = 0;
		AV = 0;
		DV = 0;
		MakeDescription();
	}
}
