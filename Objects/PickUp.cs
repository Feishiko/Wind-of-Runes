using Godot;
using System;

public class JsonPickUp
{
	public string name { get; set; }
	public string description { get; set; }
	public int weight { get; set; }
	// Equipment
	public string part { get; set; }
	public int damage { get; set; }
	public int damageDiceNumber { get; set; }
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public bool isEquipped { get; set; }
	// Food
	public int nutrition { get; set; }
	// Bread, Corpse
	public string type { get; set; }
	// Bullet
	public int numbers { get; set; }
	// Shrink Gun && Laser Gun
	public int ammo { get; set; }
	public int maxAmmo { get; set; }
	// Micro
	public string rune { get; set; }
	// Type
	public string pickUpType { get; set; }
}

public partial class PickUp : BaseObject
{
	[Export]
	public Texture2D icon { get; set; }
	public string name { get; set; }
	public string description { get; set; }
	public int weight { get; set; }
	public bool selected { get; set; } = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
