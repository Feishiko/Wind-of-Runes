using Godot;
using System;

public partial class Rat : Enemy
{
	private Game game;
	public override void _Ready()
	{
		game = GetParent<Game>();
		level = game.floor;
		hitPoint = level * 5;
		species = "Rat";
		strength = level;
		agility = level;
		intelligence = level;
		toughness = level;
		weight = 50;
		AV = 0;
		DV = 0;
		switch (game.player.species)
		{
			case "Human": nutrition = -10; break;
			case "Kobold": nutrition = 20; break;
			case "Avian": nutrition = 50; break;
			case "Avali": nutrition = 50; break;
			case "Robot": nutrition = 0; break;
		}
	}

	public override void _Process(double delta)
	{
		Visible = isVisible;
		if (hitPoint <= 0)
		{
			game.RemoveEnemy(this);
		}
	}

	public override void TurnPassed()
	{
		GD.Print($"{game}");
		GD.Print($"{game.player}");
		var newPos = Behavior.BFS(gridX, gridY, game.player.gridX, game.player.gridY, game);
		game.level[gridX, gridY, 3] = null;
		gridX = newPos.X;
		gridY = newPos.Y;
		game.level[gridX, gridY, 3] = this;
	}
}
