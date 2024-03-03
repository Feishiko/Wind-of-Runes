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
	}

	public void TurnPassed()
	{
		var newPos = Behavior.BFS(gridX, gridY, game.player.gridX, game.player.gridY, game);
		game.level[gridX, gridY, 2] = null;
		gridX = newPos.X;
		gridY = newPos.Y;
		game.level[gridX, gridY, 2] = this;
	}
}
