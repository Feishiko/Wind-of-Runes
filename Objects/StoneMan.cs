using Godot;
using System;

public partial class StoneMan : Enemy
{
	private Game game;
	public override void _Ready()
	{
		game = GetParent<Game>();
		level = game.floor;
		hitPoint = level * 8;
		species = "StoneMan";
		strength = level + 4;
		agility = level;
		intelligence = level;
		toughness = level;
		weight = 200;
		AV = level + 10;
		DV = 0;
		RuneSprite(rune);
	}

	public override void _Process(double delta)
	{
		if (nutrition == 0 && game.player != null)
		{
			switch (game.player.species)
			{
				case "Human": nutrition = 0; break;
				case "Kobold": nutrition = 0; break;
				case "Avian": nutrition = 0; break;
				case "Avali": nutrition = 0; break;
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
					var damage = random.Next(1, strength + 1);
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
