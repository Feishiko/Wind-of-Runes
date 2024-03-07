using Godot;
using System;
using System.IO;

public partial class Player : BaseObject
{
	[Export]
	private Texture2D textureHumanMale;
	[Export]
	private Texture2D textureHumanFemale;
	[Export]
	private Texture2D textureKoboldMale;
	[Export]
	private Texture2D textureKoboldFemale;
	[Export]
	private Texture2D textureAvianMale;
	[Export]
	private Texture2D textureAvianFemale;
	[Export]
	private Texture2D textureAvaliMale;
	[Export]
	private Texture2D textureAvaliFemale;
	[Export]
	private Texture2D textureRobotMale;
	[Export]
	private Texture2D textureRobotFemale;
	private Game game;
	public int hitPoint = 20;
	public int maxHitPoint = 20;
	public int level = 1;
	public string name = RandomName.RandomCharacterName(); // Can custom
	public string gender = RandomName.RandomGender(); // Male or Female
	public string species = RandomName.RandomSpecies(); // Human, Kobold, Avian, Avali, Robot
	public int strength = 5; // Decide the melee weapon's damage and resistence
	public int agility = 5; // Decide the range weapon's damage and resistence
	public int intelligence = 5; // Decide magic's damage and resistence and max health
	public int toughness = 5; // Decide how strong you are(how heavy stuff you can take)
	public int AV = 0; // Armor Value
	public int DV = 0; // Dodge Value
	public int hungryNess = 500; // Turns(if robot, instead of hungry, it will have a large number of turns)
	public int maxHungryNess = 500; // Turns(if robot, instead of hungry, it will have a large number of turns)
	public int time = 0; // How many turns has passed
	public int exp = 0;
	public int weight = 0;
	public int maxWeight = 500; // Str*10 + Tou*20?
	public PickUp[] inventory = new PickUp[200];
	public bool isBagOpen = false;
	public bool isLookingGround = false;
	public bool isMultiItems = false; // Pick up items when there are multi-items
	public Equipment head { get; set; }
	public Equipment hand { get; set; }
	public Equipment body { get; set; }
	public Equipment foot { get; set; }
	public Equipment weapon { get; set; }
	public Equipment rangeWeapon { get; set; }
	public Equipment ammo { get; set; }
	public string[] runes = new string[5];
	private Controller controller;
	public override void _Ready()
	{
		// Controller
		controller = Controller.GetInstance();

		game = GetParent<Game>();
		var playerTexture = textureHumanMale;
		species = "Robot";
		if (species == "Human")
		{
			if (gender == "Male")
			{
				playerTexture = textureHumanMale;
			}
			else
			{
				playerTexture = textureHumanFemale;
			}
		}
		if (species == "Kobold")
		{
			if (gender == "Male")
			{
				playerTexture = textureKoboldMale;
			}
			else
			{
				playerTexture = textureKoboldFemale;
			}
		}
		if (species == "Avian")
		{
			if (gender == "Male")
			{
				playerTexture = textureAvianMale;
			}
			else
			{
				playerTexture = textureAvianFemale;
			}
		}
		if (species == "Avali")
		{
			if (gender == "Male")
			{
				playerTexture = textureAvaliMale;
			}
			else
			{
				playerTexture = textureAvaliFemale;
			}
		}
		if (species == "Robot")
		{
			if (controller.maxFloor <= 0)
			{
				hungryNess = 5000;
				maxHungryNess = 5000;
			}
			if (gender == "Male")
			{
				playerTexture = textureRobotMale;
			}
			else
			{
				playerTexture = textureRobotFemale;
			}
		}
		GetChild<Sprite2D>(0).Texture = playerTexture;
	}

	public override void _Process(double delta)
	{
		// If bag is not open
		if (!isBagOpen && !isLookingGround)
		{
			if (Input.IsActionJustPressed("Left"))
			{
				Movement(Vector2.Left);
			}
			if (Input.IsActionJustPressed("Down"))
			{
				Movement(Vector2.Down);
			}
			if (Input.IsActionJustPressed("Up"))
			{
				Movement(Vector2.Up);
			}
			if (Input.IsActionJustPressed("Right"))
			{
				Movement(Vector2.Right);
			}
			if (Input.IsActionJustPressed("UpLeft"))
			{
				Movement(new Vector2(-1, -1));
			}
			if (Input.IsActionJustPressed("UpRight"))
			{
				Movement(new Vector2(1, -1));
			}
			if (Input.IsActionJustPressed("DownLeft"))
			{
				Movement(new Vector2(-1, 1));
			}
			if (Input.IsActionJustPressed("DownRight"))
			{
				Movement(new Vector2(1, 1));
			}

			if (Input.IsActionJustPressed("Close"))
			{
				for (var i = -1; i <= 1; i++)
				{
					for (var j = -1; j <= 1; j++)
					{
						if (i != 0 || j != 0)
						{
							if (game.level[gridX + i, gridY + j, 1] is Door door)
							{
								door.isOpen = false;
								game.TurnPassed();
							}
						}
					}
				}
			}
			// Pick up items
			if (Input.IsActionJustPressed("Pick"))
			{
				if (game.level[gridX, gridY, 2] is DropItems dropItems)
				{
					if (dropItems.IsSingleItem())
					{
						if (Pickable(dropItems.GetSingleItem()))
						{
							Pick(dropItems.GetSingleItem());
							dropItems.DeleteItem(dropItems.GetSingleItem());
							game.TurnPassed();
						}
					}
					else
					{
						isLookingGround = true;
					}
				}
			}

			// Go Upstair
			if (Input.IsActionJustPressed("Upstair") && game.level[gridX, gridY, 1] is Upstair)
			{
				GoUpStair();
			}

			// Go Downstair
			if (Input.IsActionJustPressed("Downstair") && game.level[gridX, gridY, 1] is Downstair)
			{
				GoDownStair();
			}
		}

		// Open or Close Bag
		if (Input.IsActionJustPressed("Inventory") && !isLookingGround)
		{
			isBagOpen = !isBagOpen;
		}

		// Close Inventory and LookingGround
		if (Input.IsActionJustPressed("Cancel"))
		{
			isLookingGround = false;
			isBagOpen = false;
		}

		if (exp >= level * 20)
		{
			exp = exp - level * 20;
			level += 1;
		}
	}

	public void Movement(Vector2 dir)
	{
		game.TurnPassed();
		// Is Door
		if (game.level[gridX + (int)dir.X, gridY + (int)dir.Y, 1] is Door door)
		{
			if (!door.isOpen)
			{
				door.isOpen = true;
				return;
			}
		}
		// Attack
		if (game.level[gridX + (int)dir.X, gridY + (int)dir.Y, 3] is Enemy enemy)
		{
			var random = new Random();
			var damage = random.Next(1, strength + 1);
			enemy.hitPoint -= damage; // 1d(str)
			Position = new Vector2(enemy.gridX * 16, enemy.gridY * 16);
			game.DamageNumber(enemy.gridX, enemy.gridY, damage);
			return;
		}
		// Is Ground
		if (game.level[gridX + (int)dir.X, gridY + (int)dir.Y, 0] is Ground)
		{
			game.level[gridX, gridY, 3] = null;
			gridX += (int)dir.X;
			gridY += (int)dir.Y;
			game.level[gridX, gridY, 3] = this;
		}
	}

	public void Pick(PickUp pickUp)
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (inventory[iter] == null)
			{
				inventory[iter] = pickUp;
				weight += pickUp.weight;
				return;
			}
		}
	}

	public bool Pickable(PickUp pickUp)
	{
		return weight + pickUp.weight <= maxWeight;
	}

	public void DeleteItem(PickUp pickUp)
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (inventory[iter] == pickUp)
			{
				weight -= pickUp.weight;
				inventory[iter] = null;
			}
		}
	}

	public void GoUpStair()
	{
		game.MapLevel();
		controller.currentFloor += 1;
		controller.player = this;
		controller.isUp = true;
		GetTree().ReloadCurrentScene();
	}

	public void GoDownStair()
	{
		game.MapLevel();
		controller.currentFloor -= 1;
		controller.player = this;
		controller.isUp = false;
		GetTree().ReloadCurrentScene();
	}

	public void GetRune(Enemy enemy)
	{
		for (var iter = 0; iter < 5; iter++)
		{
			if (runes[iter] == null)
			{
				runes[iter] = enemy.rune;
				if (runes[4] != null)
				{
					var fire = 0;
					var water = 0;
					var gear = 0;
					var leaf = 0;
					var electric = 0;
					for (var i = 0; i < 5; i++)
					{
						fire += runes[i] == "Fire" ? 1 : 0;
						water += runes[i] == "Water" ? 1 : 0;
						gear += runes[i] == "Gear" ? 1 : 0;
						leaf += runes[i] == "Leaf" ? 1 : 0;
						electric += runes[i] == "Electric" ? 1 : 0;
						runes[i] = null;
					}
					exp += Math.Max(Math.Max(Math.Max(Math.Max(fire, water), gear), leaf), electric) * 20;
				}
				return;
			}
		}
	}

	public void PlayerCopy(Player player)
	{
		name = player.name;
		species = player.species;
		gender = player.gender;
		hitPoint = player.hitPoint;
		maxHitPoint = player.maxHitPoint;
		level = player.level;
		strength = player.strength;
		agility = player.agility;
		intelligence = player.intelligence;
		toughness = player.toughness;
		AV = player.AV;
		DV = player.DV;
		hungryNess = player.hungryNess;
		maxHungryNess = player.maxHungryNess;
		time = player.time;
		exp = player.exp;
		weight = player.weight;
		maxWeight = player.maxWeight;
		inventory = player.inventory;
		head = player.head;
		hand = player.hand;
		body = player.body;
		foot = player.foot;
		weapon = player.weapon;
		rangeWeapon = player.rangeWeapon;
		ammo = player.ammo;
		runes = player.runes;
	}
}
