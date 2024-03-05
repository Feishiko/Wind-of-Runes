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
		var playerTexture = textureHumanMale;
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
			enemy.hitPoint -= random.Next(1, strength + 1); // 1d(str)
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
}
