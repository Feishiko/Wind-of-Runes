using Godot;
using System;

public partial class GameShell : Node2D
{
	[Export]
	private PackedScene packedInventory;
	[Export]
	private PackedScene packedLookingGround;
	[Export]
	private PackedScene packedRune;
	[Export]
	private PackedScene packedUpgrade;
	[Export]
	private PackedScene packedFireMode;
	[Export]
	private PackedScene packedLog;
	private Inventory inventory;
	private LookingGround lookingGround;
	private Upgrade upgrade;
	private Rune[] runes = new Rune[5];
	public Game game;
	private Controller controller;
	private FireMode fireMode;
	public string[] logs = new string[4];
	public Log[] labelLogs = new Log[4];
	public override void _Ready()
	{
		// Controller
		controller = Controller.GetInstance();
		game = GetNode<Game>("SubViewportContainer/SubViewport/Game");

		// Runes
		for (var iter = 0; iter < 5; iter++)
		{
			runes[iter] = packedRune.Instantiate<Rune>();
			runes[iter].Position = new Vector2(310 + iter * 20, 110);
			AddChild(runes[iter]);
		}

		for (var iter = 0; iter < 4; iter++)
		{
			labelLogs[iter] = packedLog.Instantiate<Log>();
			labelLogs[iter].text = logs[iter] != null ? logs[iter] : "";
			labelLogs[iter].Position = new Vector2(310, 220 + iter * 20);
			AddChild(labelLogs[iter]);
		}
	}

	public override void _Process(double delta)
	{
		GetNode<Label>("Name").Text = game.player.name;
		GetNode<Label>("Hitpoint").Text = "HP: " + game.player.hitPoint.ToString() + "/" + game.player.maxHitPoint.ToString();
		GetNode<Label>("Species").Text = game.player.species + $"({game.player.gender})";
		GetNode<Label>("Level").Text = "Level: " + game.player.level.ToString();
		GetNode<Label>("Hungry").Text = "Food: " + game.player.hungryNess.ToString() + "/" + game.player.maxHungryNess.ToString();
		GetNode<Label>("Str").Text = "Strength: " + game.player.strength.ToString();
		GetNode<Label>("Agi").Text = "Agility: " + game.player.agility.ToString();
		GetNode<Label>("Int").Text = "Intelligence: " + game.player.intelligence.ToString();
		GetNode<Label>("Tou").Text = "Toughness: " + game.player.toughness.ToString();
		GetNode<Label>("AV").Text = "AV: " + game.player.AV.ToString();
		GetNode<Label>("DV").Text = "DV: " + game.player.DV.ToString();
		GetNode<Label>("Time").Text = "Time: " + game.player.time.ToString();
		GetNode<Label>("Exp").Text = "Exp: " + game.player.exp.ToString() + "/" + (game.player.level * 20).ToString();
		GetNode<Label>("Floor").Text = "Floor: " + game.floor.ToString();

		// Open Inventory
		if (game.player.isBagOpen)
		{
			if (inventory == null)
			{
				inventory = packedInventory.Instantiate<Inventory>();
				// Weight Info
				inventory.playerWeight = game.player.weight;
				inventory.playerMaxWeight = game.player.maxWeight;
				// Add Items
				for (var page = 0; page < 10; page++)
				{
					for (var iter = 0; iter < 12; iter++)
					{
						if (game.player.inventory[iter + page * 12] != null)
						{
							inventory.pickUps[iter, page] = game.player.inventory[iter + page * 12];
						}
					}
					if (inventory.pickUps[0, page] != null)
					{
						inventory.maxPage = page;
					}
				}
				AddChild(inventory);
			}
		}
		else
		{
			if (inventory != null)
			{
				inventory.QueueFree();
				inventory = null;
			}
		}

		// Open Looking Ground
		if (game.player.isLookingGround)
		{
			if (lookingGround == null)
			{
				lookingGround = packedLookingGround.Instantiate<LookingGround>();
				// Add Items
				for (var page = 0; page < 10; page++)
				{
					for (var iter = 0; iter < 12; iter++)
					{
						if ((game.level[game.player.gridX, game.player.gridY, 2] as DropItems).dropItems[iter + page * 12] != null)
						{
							lookingGround.pickUps[iter, page] = (game.level[game.player.gridX, game.player.gridY, 2] as DropItems).dropItems[iter + page * 12];
							lookingGround.pickUps[iter, page].selected = false;
						}
					}
					if (lookingGround.pickUps[0, page] != null)
					{
						lookingGround.maxPage = page;
					}
				}
				AddChild(lookingGround);
			}
		}
		else
		{
			if (lookingGround != null)
			{
				lookingGround.QueueFree();
				lookingGround = null;
			}
		}

		// Runes
		for (var iter = 0; iter < 5; iter++)
		{
			runes[iter].rune = game.player.runes[iter];
		}

		// When Upgrade
		if (game.player.isUpgrade)
		{
			if (upgrade == null)
			{
				upgrade = packedUpgrade.Instantiate<Upgrade>();
				AddChild(upgrade);
			}
		}
		else
		{
			if (upgrade != null)
			{
				upgrade.QueueFree();
				upgrade = null;
			}
		}

		// Fire Mode
		if (game.player.isFire)
		{
			if (fireMode == null)
			{
				fireMode = packedFireMode.Instantiate<FireMode>();
				AddChild(fireMode);
			}
		}
		else
		{
			if (fireMode != null)
			{
				fireMode.QueueFree();
				fireMode = null;
			}
		}

		// Logs
		for (var iter = 0; iter < 4; iter++)
		{
			labelLogs[iter].text = $">{logs[iter]}";
		}
	}

	public void AddLog(string text)
	{
		// for (var iter = 0; iter < 4; iter++)
		// {
		// 	if (logs[iter] == null)
		// 	{
		// 		logs[iter] = text;
		// 		return;
		// 	}
		// }
		// Full
		for (var iter = 0; iter < 3; iter++)
		{
			logs[iter] = logs[iter + 1];
		}
		logs[3] = text;
	}
}
