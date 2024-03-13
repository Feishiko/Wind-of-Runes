using Godot;
using System;

public class JsonEnemy : JsonBaseObject
{
	public string name { get; set; }
	public int hitPoint { get; set; }
	public int level { get; set; }
	public string species { get; set; }
	public string gender { get; set; }
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public int nutrition { get; set; }
	public int weight { get; set; }
	public JsonPickUp[] inventory { get; set; } = new JsonPickUp[100];
	public string rune { get; set; }

	public void CopiedFrom(Enemy enemy)
	{
		name = enemy.name;
		hitPoint = enemy.hitPoint;
		level = enemy.level;
		species = enemy.species;
		gender = enemy.gender;
		strength = enemy.strength;
		agility = enemy.agility;
		toughness = enemy.toughness;
		intelligence = enemy.intelligence;
		AV = enemy.AV;
		DV = enemy.DV;
		nutrition = enemy.nutrition;
		weight = enemy.weight;
		rune = enemy.rune;
		gridX = enemy.gridX;
		gridY = enemy.gridY;
		for (var iter = 0; iter < 10; iter++)
		{
			if (enemy.inventory[iter] != null)
			{
				if (inventory[iter] == null)
				{
					inventory[iter] = new JsonPickUp();
				}
				inventory[iter].name = enemy.inventory[iter].name;
				inventory[iter].description = enemy.inventory[iter].description;
				inventory[iter].weight = enemy.inventory[iter].weight;
				if (enemy.inventory[iter] is Food food)
				{
					if (inventory[iter] is not JsonFood)
					{
						inventory[iter] = new JsonFood();
					}
					(inventory[iter] as JsonFood).CopiedFrom(food);
				}
				if (enemy.inventory[iter] is Equipment equipment)
				{
					if (inventory[iter] is not JsonEquipment)
					{
						inventory[iter] = new JsonEquipment();
					}
					(inventory[iter] as JsonEquipment).CopiedFrom(equipment);
				}
				if (enemy.inventory[iter] is Bullet bullet)
				{
					if (inventory[iter] is not JsonBullet)
					{
						inventory[iter] = new JsonBullet();
					}
					(inventory[iter] as JsonBullet).CopiedFrom(bullet);
				}
				if (enemy.inventory[iter] is LaserGun laserGun)
				{
					if (inventory[iter] is not JsonLaserGun)
					{
						inventory[iter] = new JsonLaserGun();
					}
					(inventory[iter] as JsonLaserGun).CopiedFrom(laserGun);
				}
			}
		}
	}
}

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
	[Export]
	public PackedScene packedBullet;
	[Export]
	public PackedScene packedBoot;
	[Export]
	public PackedScene packedSword;
	[Export]
	public PackedScene packedClothes;
	[Export]
	public PackedScene packedHelmet;
	[Export]
	public PackedScene packedPistol;
	[Export]
	public PackedScene packedGlove;
	[Export]
	public Texture2D icon;
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
	public bool isShrink { get; set; }
	public PickUp[] inventory = new PickUp[100];
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

	public void Pick(PickUp pickUp)
	{
		for (var iter = 0; iter < 100; iter++)
		{
			if (inventory[iter] == null)
			{
				inventory[iter] = pickUp;
				break;
			}
		}
	}
}
