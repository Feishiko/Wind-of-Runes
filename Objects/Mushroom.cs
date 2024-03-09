using Godot;
using System;

public partial class Mushroom : Enemy
{
private Game game;
	public override void _Ready()
	{
		game = GetParent<Game>();
		level = game.floor;
		hitPoint = level * 3;
		species = "Mushroom";
		strength = level;
		agility = level;
		intelligence = level;
		toughness = level;
		weight = 100;
		AV = 0;
		DV = 20;
		RuneSprite(rune);
		var random = new Random();
		if (random.Next(15) == 1)
		{
			var boot = packedBoot.Instantiate<Boot>();
			boot.Init();
			Pick(boot);
		}
		if (random.Next(15) == 1)
		{
			var clothes = packedClothes.Instantiate<Clothes>();
			clothes.Init();
			Pick(clothes);
		}
		if (random.Next(15) == 1)
		{
			var sword = packedSword.Instantiate<Sword>();
			sword.Init();
			Pick(sword);
		}
		if (random.Next(15) == 1)
		{
			var glove = packedGlove.Instantiate<Glove>();
			glove.Init();
			Pick(glove);
		}
		if (random.Next(15) == 1)
		{
			var pistol = packedPistol.Instantiate<Pistol>();
			pistol.Init();
			Pick(pistol);
		}
		if (random.Next(15) == 1)
		{
			var helmet = packedHelmet.Instantiate<Helmet>();
			helmet.Init();
			Pick(helmet);
		}
	}

	public override void _Process(double delta)
	{
		if (nutrition == 0 && game.player != null)
		{
			switch (game.player.species)
			{
				case "Human": nutrition = 80; break;
				case "Kobold": nutrition = 50; break;
				case "Avian": nutrition = 50; break;
				case "Avali": nutrition = 50; break;
				case "Robot": nutrition = 0; break;
			}
		}
		Visible = isVisible;
		if (hitPoint <= 0)
		{
			game.RemoveEnemy(this);
		}
	}

	public override void TurnPassed()
	{
		for (var y = -1; y <= 1; y++)
		{
			for (var x = -1; x <= 1; x++)
			{
				if (game.level[gridX + x, gridY + y, 3] is Player player)
				{
					var random = new Random();
					var damage = random.Next(1, strength);
					damage = Mathf.Max(1, damage - player.AV);
					if (player.DV > random.Next(30))
					{
						damage = 0;
					}
					player.hitPoint -= damage;
					Position = new Vector2(player.gridX * 16, player.gridY * 16);
					game.DamageNumber(player.gridX, player.gridY, damage);
					return;
				}
			}
		}
		var newPos = Behavior.BFS(gridX, gridY, game.player.gridX, game.player.gridY, game);
		game.level[gridX, gridY, 3] = null;
		gridX = newPos.X;
		gridY = newPos.Y;
		game.level[gridX, gridY, 3] = this;
	}
}
