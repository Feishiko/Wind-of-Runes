using Godot;
using System;

public class JsonEquipment : JsonPickUp
{
	public void CopiedFrom(Equipment equipment)
	{
		name = equipment.name;
		description = equipment.description;
		weight = equipment.weight;
		part = equipment.part;
		damage = equipment.damage;
		damageDiceNumber = equipment.damageDiceNumber;
		strength = equipment.strength;
		agility = equipment.agility;
		intelligence = equipment.intelligence;
		toughness = equipment.toughness;
		AV = equipment.AV;
		DV = equipment.DV;
		isEquipped = equipment.isEquipped;
		pickUpType = "Equipment";
	}
}

public partial class Equipment : PickUp
{
	public string part { get; set; } // Hand Head Body Foot Weapon RangeWeapon Ammo
									 // public int defence { get; set; }
	public int damage { get; set; }
	public int damageDiceNumber { get; set; }
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public bool isEquipped { get; set; } = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void MakeDescription()
	{
		description = $"Name: {name}\n";
		description += $"Part: {part}\n";
		description += damage == 0 ? "" : $"Damage: {damage}\n";
		description += damageDiceNumber == 0 ? "" : $"Damage DiceNumber: {damageDiceNumber}\n";
		description += strength == 0 ? "" : $"Strength: {strength}\n";
		description += agility == 0 ? "" : $"Agility: {agility}\n";
		description += toughness == 0 ? "" : $"Toughness: {toughness}\n";
		description += intelligence == 0 ? "" : $"Intelligence: {intelligence}\n";
		description += AV == 0 ? "" : $"AV: {AV}\n";
		description += DV == 0 ? "" : $"DV: {DV}\n";
	}

	public void ReceivedFrom(JsonPickUp jsonPickUp)
	{
		part = jsonPickUp.part;
		damage = jsonPickUp.damage;
		damageDiceNumber = jsonPickUp.damageDiceNumber;
		strength = jsonPickUp.strength;
		agility = jsonPickUp.agility;
		toughness = jsonPickUp.toughness;
		intelligence = jsonPickUp.intelligence;
		AV = jsonPickUp.AV;
		DV = jsonPickUp.DV;
		isEquipped = jsonPickUp.isEquipped;
	}
}
