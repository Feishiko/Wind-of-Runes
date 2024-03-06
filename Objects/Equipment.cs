using Godot;
using System;

public partial class Equipment : PickUp
{
	public string part { get; set; } // Hand Head Body Foot Weapon RangeWeapon Ammo
	public int defence { get; set; }
	public int damage { get; set; }
	public int damageDiceNumber { get; set; }
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public int hungryness { get; set; }
	public bool isEquipped { get; set; } = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
