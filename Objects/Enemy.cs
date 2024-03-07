using Godot;
using System;

public partial class Enemy : BaseObject
{
	[Export]
	private Texture2D textureGearRune;
	[Export]
	private Texture2D textureLeafRune;
	[Export]
	private Texture2D textureFireRune;
	[Export]
	private Texture2D textureWaterRune;
	[Export]
	private Texture2D textureElectricRune;
	public string name { get; set; } = RandomName.RandomCharacterName();
	public int hitPoint { get; set; }
	public int level { get; set; }
	public string species { get; set; }
	public string gender { get; set; } = RandomName.RandomGender();
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public int nutrition { get; set; }
	public int weight { get; set; }
	public BaseObject[] inventory = new BaseObject[100];
	public Equipment head { get; set; }
	public Equipment hand { get; set; }
	public Equipment body { get; set; }
	public Equipment foot { get; set; }
	public Equipment weapon { get; set; }
	public Equipment rangeWeapon { get; set; }
	public Equipment ammo { get; set; }
	public string rune { get; set; } = RandomName.RandomRune();

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public virtual void TurnPassed()
	{
	}

	public void EnemyCopy(Enemy enemy)
	{
		// TODO
		name = enemy.name;
		hitPoint = enemy.hitPoint;
		level = enemy.level;
		species = enemy.species;
		gender = enemy.gender;
		strength = enemy.strength;
		agility = enemy.agility;
		intelligence = enemy.intelligence;
		toughness = enemy.toughness;
		AV = enemy.AV;
		DV = enemy.DV;
		nutrition = enemy.nutrition;
		weight = enemy.weight;
		inventory = enemy.inventory;
		head = enemy.head;
		hand = enemy.hand;
		body = enemy.body;
		foot = enemy.foot;
		weapon = enemy.weapon;
		rangeWeapon = enemy.rangeWeapon;
		ammo = enemy.ammo;
		rune = enemy.rune;
	}

	public void RuneSprite(string rune)
	{
		switch (rune)
		{
			case "Fire": GetNode<Sprite2D>("Rune").Texture = textureFireRune; break;
			case "Water": GetNode<Sprite2D>("Rune").Texture = textureWaterRune; break;
			case "Leaf": GetNode<Sprite2D>("Rune").Texture = textureLeafRune; break;
			case "Gear": GetNode<Sprite2D>("Rune").Texture = textureGearRune; break;
			case "Electric": GetNode<Sprite2D>("Rune").Texture = textureElectricRune; break;
		}
	}
}
