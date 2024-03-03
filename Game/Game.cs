using Godot;
using System;

public partial class Game : Node2D
{
	// Export all objects
	[Export]
	private PackedScene packedPlayer;
	[Export]
	private PackedScene packedWall;
	[Export]
	private PackedScene packedUpstair;
	[Export]
	private PackedScene packedDownstair;
	[Export]
	private PackedScene packedGround;
	[Export]
	private PackedScene packedDoor;
	[Export]
	private PackedScene packedRat;
	public Player player;
	const int levelWidth = 40;
	const int levelHeight = 40;
	// Level Generator
	public BaseObject[,,] level = new BaseObject[levelWidth, levelHeight, 3];
	private int[,] roomSpace = new int[levelWidth * levelHeight, 2];
	// Level Info
	public int floor = 1;
	// tmp
	private Font font = new Godot.SystemFont();
	public override void _Ready()
	{
		player = packedPlayer.Instantiate<Player>();

		// Level Generate
		level = TerrianGenerate(10, 3, 3, 5, 5);
		Position += new Vector2(40, 40);
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.R))
		{
			GetTree().ReloadCurrentScene();
		}
		// Sprite Display
		foreach (var item in level)
		{
			if (item != null)
			{
				item.Position = new Vector2(Mathf.Lerp(item.Position.X, item.gridX * 16, .2f * (float)delta * 120),
				Mathf.Lerp(item.Position.Y, item.gridY * 16, .2f * (float)delta * 120));
				// Visible
				item.isVisible = true;
				for (var iter = 0; iter < 40; iter++)
				{
					if (level[(int)Math.Round(Mathf.Lerp(player.gridX, item.gridX, iter / 40f)),
					(int)Math.Round(Mathf.Lerp(player.gridY, item.gridY, iter / 40f)), 0] is Wall wall)
					{
						if (wall != level[item.gridX, item.gridY, 0])
						{
							item.isVisible = false;
							break;
						}
					}

					if (level[(int)Math.Round(Mathf.Lerp(player.gridX, item.gridX, iter / 40f)),
					(int)Math.Round(Mathf.Lerp(player.gridY, item.gridY, iter / 40f)), 1] is Door door)
					{
						if (door != level[item.gridX, item.gridY, 1])
						{
							if (!door.isOpen)
							{
								item.isVisible = false;
								break;
							}
						}
					}
				}
				if (item.isVisible)
				{
					item.isMemorized = true;
					item.Visible = true;
					item.Modulate = Colors.White;
				}
				else
				{
					if (item.isMemorized)
					{
						item.Modulate = Colors.DarkGray;
					}
					else
					{
						// item.Visible = false;
					}
				}
			}
		}
	}

    public override void _Draw()
    {
        DrawStringOutline(font, new Vector2(10, 600),
		$"{player.name} {player.species} HitPoint: {player.hitPoint} Level: {player.level} Gender: {player.gender}", HorizontalAlignment.Left, -1, 16, 1);
		DrawStringOutline(font, new Vector2(10, 620),
		$"Str: {player.strength} Agi: {player.agility} Int: {player.intelligence} Tou: {player.toughness} AV: {player.AV} DV: {player.DV}", HorizontalAlignment.Left, -1, 16, 1);
    }

    public BaseObject[,,] TerrianGenerate(int roomIter, int minWidth, int minHeight, int maxWidth, int maxHeight, int roomType = 0)
	{
		var level = new BaseObject[levelWidth, levelHeight, 3];

		// Bottom
		// Add Wall
		for (var height = 0; height < levelHeight; height++)
		{
			for (var width = 0; width < levelWidth; width++)
			{
				level[width, height, 0] = packedWall.Instantiate<Wall>();
				level[width, height, 0].gridX = width;
				level[width, height, 0].gridY = height;
			}
		}
		// Add room
		var previousRoomX = -1;
		var previousRoomY = -1;
		for (var iter = 0; iter < roomIter; iter++)
		{
			var random = new Random();
			var randomPosX = random.Next(0, levelWidth);
			var randomPosY = random.Next(0, levelHeight);
			var randomSizeX = random.Next(minWidth, maxWidth + 1);
			var randomSizeY = random.Next(minHeight, maxHeight + 1);
			var Even = (int val) => val % 2 == 0 ? val : val + 1;
			var currentRoomX = randomPosX + Even(randomSizeX) / 2;
			var currentRoomY = randomPosY + Even(randomSizeY) / 2;
			// GD.Print($"randomPosX: {randomPosX}, randomPosY: {randomPosY}, randomSizeX: {randomSizeX}, randomSizeY: {randomSizeY}");
			var validRoom = true;
			if (randomSizeX + randomPosX > levelWidth - 1 || randomSizeY + randomPosY > levelHeight - 1)
			{
				continue;
			}
			if (randomPosX < 1 || randomPosY < 1)
			{
				continue;
			}
			for (var i = 0; i < 2; i++)
			{
				for (var yPos = randomPosY; yPos < randomSizeY + randomPosY; yPos++)
				{
					for (var xPos = randomPosX; xPos < randomSizeX + randomPosX; xPos++)
					{
						// Try Generate to find wheather this room is valid in a first time
						if (i == 0 && validRoom)
						{
							validRoom = level[xPos, yPos, 0] is Wall;
						}
						if (i == 1 && validRoom)
						{
							// level[xPos, yPos, 0].Free();
							level[xPos, yPos, 0] = packedGround.Instantiate<Ground>();
							level[xPos, yPos, 0].gridX = xPos;
							level[xPos, yPos, 0].gridY = yPos;

							// Debug generate Rats
							var someRandom = new Random();
							if (someRandom.Next(100) < 2)
							{
								level[xPos, yPos, 2] = packedRat.Instantiate<Rat>();
								level[xPos, yPos, 2].gridX = xPos;
								level[xPos, yPos, 2].gridY = yPos;
							}

							for (var space = 0; space < levelWidth * levelHeight; space++)
							{
								if (roomSpace[space, 0] == 0)
								{
									GD.Print("some");
									roomSpace[xPos, 0] = xPos;
									roomSpace[yPos, 1] = yPos;
									break;
								}
							}
						}
					}
				}
			}
			// Add Passage
			var IsInRoom = (int x, int y) =>
			{
				for (var space = 0; space < levelWidth * levelHeight; space++)
				{
					if (x == roomSpace[space, 0] && y == roomSpace[space, 1])
					{
						GD.Print("true");
						return true;
					}
				}
				return false;
			};
			if (previousRoomX != -1)
			{
				// lengthX > 0 -> previous is on the right of the current
				// lengthY > 0 -> previous is on the bottom of the current
				var lengthX = previousRoomX - currentRoomX;
				var lengthY = previousRoomY - currentRoomY;
				var isDoor = false;
				var DoorCreate = () =>
				{
					isDoor = true;
					return this.packedDoor.Instantiate<Door>();
				};
				for (var horizontal = 0; horizontal <= Math.Abs(lengthX); horizontal++)
				{
					// // GD.Print(isDoor);
					// // GD.Print(level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] is Wall);
					// level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] =
					// // !IsInRoom(currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY) &&
					// level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] is Wall ?
					// isDoor ? this.packedGround.Instantiate<Ground>() : DoorCreate()
					// : level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0];
					if (level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] is Wall)
					{
						if (isDoor)
						{
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] = this.packedGround.Instantiate<Ground>();
						}
						else
						{
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1] = DoorCreate();
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1].gridX = currentRoomX + horizontal * Math.Sign(lengthX);
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1].gridY = currentRoomY;
						}
					}
					// isDoor = (isDoor == false) && (level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] is Door);
					level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0].gridX = currentRoomX + horizontal * Math.Sign(lengthX);
					level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0].gridY = currentRoomY;
					if (level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1] is Door)
					{
						if (level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1] is not Downstair ||
						level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1] is not Upstair)
						{
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1] = this.packedDoor.Instantiate<Door>();
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1].gridX = currentRoomX + horizontal * Math.Sign(lengthX);
							level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 1].gridY = currentRoomY;
						}
						level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0] = this.packedGround.Instantiate<Ground>();
						level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0].gridX = currentRoomX + horizontal * Math.Sign(lengthX);
						level[currentRoomX + horizontal * Math.Sign(lengthX), currentRoomY, 0].gridY = currentRoomY;
					}
				}
				isDoor = false;
				lengthY *= -1;
				for (var vertical = 0; vertical <= Math.Abs(lengthY); vertical++)
				{
					if (level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0] is Wall)
					{
						if (isDoor)
						{
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0] = this.packedGround.Instantiate<Ground>();
						}
						else
						{
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1] = DoorCreate();
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1].gridX = previousRoomX;
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1].gridY = previousRoomY + vertical * Math.Sign(lengthY);
						}
					}
					// isDoor = isDoor == false && level[currentRoomX, currentRoomY + vertical * Math.Sign(lengthY), 0] is Door;
					level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0].gridX = previousRoomX;
					level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0].gridY = previousRoomY + vertical * Math.Sign(lengthY);

					if (level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1] is Door)
					{
						if (level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1] is not Downstair ||
						level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1] is not Upstair)
						{
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1] = this.packedDoor.Instantiate<Door>();
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1].gridX = previousRoomX;
							level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 1].gridY = previousRoomY + vertical * Math.Sign(lengthY);
						}
						level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0] = this.packedGround.Instantiate<Ground>();
						level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0].gridX = previousRoomX;
						level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0].gridY = previousRoomY + vertical * Math.Sign(lengthY);

					}

					// Create another door
					// if (isDoor)
					// {
					// 	if (isInRoom == false && IsInRoom(previousRoomX, previousRoomY + vertical * Math.Sign(lengthY)))
					// 	{
					// 		isInRoom = true;
					// 		level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY) - Math.Sign(lengthY), 0] =
					// 		this.packedDoor.Instantiate<Door>();
					// 		level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY) - Math.Sign(lengthY), 0].Position =
					// 		new Vector2(previousRoomX * 16, (previousRoomY + vertical * Math.Sign(lengthY) - Math.Sign(lengthY)) * 16);
					// 	}
					// 	if (isInRoom == true && !IsInRoom(previousRoomX, previousRoomY + vertical * Math.Sign(lengthY)))
					// 	{
					// 		isInRoom = false;
					// 		level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0] =
					// 		this.packedDoor.Instantiate<Door>();
					// 		level[previousRoomX, previousRoomY + vertical * Math.Sign(lengthY), 0].Position =
					// 		new Vector2(previousRoomX * 16, (previousRoomY + vertical * Math.Sign(lengthY)) * 16);
					// 	}
					// }
				}
			}
			// Add Stairs and Player
			if (previousRoomX == -1)
			{
				level[currentRoomX, currentRoomY, 1] = packedDownstair.Instantiate<Downstair>();
				level[currentRoomX, currentRoomY, 1].gridX = currentRoomX;
				level[currentRoomX, currentRoomY, 1].gridY = currentRoomY;
				level[currentRoomX, currentRoomY, 2] = packedPlayer.Instantiate<Player>();
				level[currentRoomX, currentRoomY, 2].gridX = currentRoomX;
				level[currentRoomX, currentRoomY, 2].gridY = currentRoomY;
				if (level[currentRoomX, currentRoomY, 2] is Player player)
				{
					this.player = player;
				}
			}
			// if (!isUpstair && previousRoomX != -1)
			// {
			// 	level[currentRoomX, currentRoomY, 1] = packedUpstair.Instantiate<Upstair>();
			// 	level[currentRoomX, currentRoomY, 1].Position = new Vector2(currentRoomX * 16, currentRoomY * 16);
			// 	isUpstair = true;
			// }

			previousRoomX = randomPosX + Even(randomSizeX) / 2;
			previousRoomY = randomPosY + Even(randomSizeY) / 2;
		}
		// Add Upstair
		level[previousRoomX, previousRoomY, 1] = packedUpstair.Instantiate<Upstair>();
		level[previousRoomX, previousRoomY, 1].gridX = previousRoomX;
		level[previousRoomX, previousRoomY, 1].gridY = previousRoomY;


		// Add Child
		foreach (var item in level)
		{
			if (item != null)
			{
				AddChild(item);
			}
		}

		return level;
	}

	public int LevelWidthGet()
	{
		return levelWidth;
	}

	public int LevelHeightGet()
	{
		return levelHeight;
	}

	public void TurnPassed()
	{
		// Add function one by one
		foreach (var item in level)
		{
			if (item != null)
			{
				if (item is Rat rat)
				{
					rat.TurnPassed();
				}
			}
		}
	}
}
