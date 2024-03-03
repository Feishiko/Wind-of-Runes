using Godot;
using System;
using System.IO;

public partial class Player : BaseObject
{
	private Game game;
	public int hitPoint = 20;
	public int level = 1;
	public string name = RandomName.RandomCharacterName(); // Can custom
	public string gender = RandomName.RandomGender(); // Male or Female
	public string species = RandomName.RandomSpecies(); // Human, Kobold, Avian, Avali, Robot
	public int strength = 5; // Decide the melee weapon's damage and resistence
	public int agility = 5; // Decide the range weapon's damage and resistence
	public int intelligence = 5; // Decide magic's damage and resistence
	public int toughness = 5; // Decide how strong you are
	public int AV = 0; // Armor Value
	public int DV = 0; // Dodge Value
	public int hungryNess = 500; // Turns(if robot, instead of hungry, it will have a large number of turns)
	public override void _Ready()
	{
		game = GetParent<Game>();
	}

	public override void _Process(double delta)
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
		// Is Ground
		if (game.level[gridX + (int)dir.X, gridY + (int)dir.Y, 0] is Ground)
		{
			game.level[gridX, gridY, 2] = null;
			gridX += (int)dir.X;
			gridY += (int)dir.Y;
			game.level[gridX, gridY, 2] = this;
		}
	}
}
