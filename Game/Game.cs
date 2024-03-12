using Godot;
using System;
using System.Linq;

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
	[Export]
	private PackedScene packedCorpse;
	[Export]
	private PackedScene packedDropItems;
	[Export]
	private PackedScene packedDamageNumber;
	[Export]
	private PackedScene packedWorm;
	[Export]
	private PackedScene packedRogue;
	[Export]
	private PackedScene packedSlime;
	[Export]
	private PackedScene packedAvian;
	[Export]
	private PackedScene packedMushroom;
	[Export]
	private PackedScene packedMicro;
	[Export]
	private PackedScene packedBulletMan;
	[Export]
	private PackedScene packedKobold;
	[Export]
	private PackedScene packedStoneMan;
	public Player player;
	const int levelWidth = 40;
	const int levelHeight = 40;
	// Level Generator
	// 0->Ground/Wall 1->Wall/Stairs 2->Dropitems 3->Creatures
	public BaseObject[,,] level = new BaseObject[levelWidth, levelHeight, 4];
	private int[,] roomSpace = new int[levelWidth * levelHeight, 2];
	public Upstair upstair;
	private Downstair downstair;
	// Level Info
	public int floor = 1;
	// tmp
	private Font font = new Godot.SystemFont();
	private Enemy[] enemyList = new Enemy[200];
	private Controller controller;
	public GameShell gameShell;
	public override void _Ready()
	{
		// Controller
		controller = Controller.GetInstance();
		// GameShell
		gameShell = GetNode<GameShell>("../../../.");
		// GD.Print("currentFloor:" + controller.currentFloor);
		floor = controller.currentFloor;

		// Level Generate

		// level = TerrianGenerate(10, 3, 3, 5, 5);
		if (controller.maxFloor < controller.currentFloor)
		{
			controller.maxFloor = controller.currentFloor;
			if (controller.player == null)
			{
				level = InitTerrianGenerate();
				gameShell.AddLog($"{player.name} enters the tower, press ? to check help document");
			}
			else
			{
				if (controller.currentFloor <= 5)
				{
					level = TerrianGenerate(12, 3, 3, 5, 5);
				}
				if (controller.currentFloor > 5 && controller.currentFloor <= 10)
				{
					level = TerrianGenerate(20, 5, 5, 10, 10);
				}
				if (controller.currentFloor > 10 && controller.currentFloor <= 15)
				{
					level = TerrianGenerate(20, 2, 2, 2, 2);
				}
				if (controller.currentFloor >= 16)
				{
					level = EndTerrianGenerate();
				}
				level[downstair.gridX, downstair.gridY, 3] = packedPlayer.Instantiate<Player>();
				(level[downstair.gridX, downstair.gridY, 3] as Player).PlayerCopy(controller.player);
				level[downstair.gridX, downstair.gridY, 3].gridX = downstair.gridX;
				level[downstair.gridX, downstair.gridY, 3].gridY = downstair.gridY;
				AddChild(level[downstair.gridX, downstair.gridY, 3]);
				player = level[downstair.gridX, downstair.gridY, 3] as Player;
				gameShell.AddLog($"Welcome to the floor of {controller.currentFloor}");
			}
		}
		else
		{
			GD.Print("Load level");
			for (var layer = 0; layer < 4; layer++)
			{
				for (var height = 0; height < levelHeight; height++)
				{
					for (var width = 0; width < levelWidth; width++)
					{
						// level[width, height, layer] = controller.wholeLevels[width, height, layer, controller.currentFloor];
						var item = controller.wholeLevels[width, height, layer, controller.currentFloor];
						if (item is Wall wall)
						{
							level[width, height, layer] = packedWall.Instantiate<Wall>();
							level[width, height, layer].Copy(wall);
						}
						if (item is Door door)
						{
							level[width, height, layer] = packedDoor.Instantiate<Door>();
							level[width, height, layer].Copy(door);
							(level[width, height, layer] as Door).DoorCopy(door);
						}
						if (item is Enemy enemy)
						{
							if (enemy is Rat)
							{
								level[width, height, layer] = packedRat.Instantiate<Rat>();
							}
							if (enemy is Worm)
							{
								level[width, height, layer] = packedWorm.Instantiate<Worm>();
							}
							if (enemy is Rogue)
							{
								level[width, height, layer] = packedRogue.Instantiate<Rogue>();
							}
							if (enemy is Slime)
							{
								level[width, height, layer] = packedSlime.Instantiate<Slime>();
							}
							if (enemy is Mushroom)
							{
								level[width, height, layer] = packedMushroom.Instantiate<Mushroom>();
							}
							if (enemy is Avian)
							{
								level[width, height, layer] = packedAvian.Instantiate<Avian>();
							}
							if (enemy is BulletMan)
							{
								level[width, height, layer] = packedBulletMan.Instantiate<BulletMan>();
							}
							if (enemy is Kobold)
							{
								level[width, height, layer] = packedKobold.Instantiate<Kobold>();
							}
							if (enemy is StoneMan)
							{
								level[width, height, layer] = packedStoneMan.Instantiate<StoneMan>();
							}
							level[width, height, layer].Copy(enemy);
							(level[width, height, layer] as Enemy).EnemyCopy(enemy);
							AddEnemy(level[width, height, layer] as Enemy);
						}
						if (item is DropItems dropItems)
						{
							level[width, height, layer] = packedDropItems.Instantiate<DropItems>();
							level[width, height, layer].Copy(dropItems);
							(level[width, height, layer] as DropItems).DropItemsCopy(dropItems);
						}
						if (item is Ground ground)
						{
							level[width, height, layer] = packedGround.Instantiate<Ground>();
							level[width, height, layer].Copy(ground);
						}
						if (item is Upstair upstair)
						{
							level[width, height, layer] = packedUpstair.Instantiate<Upstair>();
							this.upstair = level[width, height, layer] as Upstair;
							this.upstair.Copy(upstair);
						}
						if (item is Downstair downstair)
						{
							level[width, height, layer] = packedDownstair.Instantiate<Downstair>();
							this.downstair = level[width, height, layer] as Downstair;
							this.downstair.Copy(downstair);
						}
					}
				}
			}

			// Add Child
			foreach (var item in level)
			{
				if (item != null)
				{
					AddChild(item);
				}
			}

			if (controller.isUp)
			{
				level[downstair.gridX, downstair.gridY, 3] = packedPlayer.Instantiate<Player>();
				(level[downstair.gridX, downstair.gridY, 3] as Player).PlayerCopy(controller.player);
				level[downstair.gridX, downstair.gridY, 3].gridX = downstair.gridX;
				level[downstair.gridX, downstair.gridY, 3].gridY = downstair.gridY;
				AddChild(level[downstair.gridX, downstair.gridY, 3]);
				player = level[downstair.gridX, downstair.gridY, 3] as Player;
			}
			else
			{
				level[upstair.gridX, upstair.gridY, 3] = packedPlayer.Instantiate<Player>();
				(level[upstair.gridX, upstair.gridY, 3] as Player).PlayerCopy(controller.player);
				level[upstair.gridX, upstair.gridY, 3].gridX = upstair.gridX;
				level[upstair.gridX, upstair.gridY, 3].gridY = upstair.gridY;
				AddChild(level[upstair.gridX, upstair.gridY, 3]);
				player = level[upstair.gridX, upstair.gridY, 3] as Player;
			}
		}


		// if (player == null)
		// {
		// 	GetTree().ReloadCurrentScene();
		// }

		Position += new Vector2(40, 40);

		// Music Player
		if (controller.currentFloor >= 1 && controller.currentFloor <= 5)
		{
			gameShell.musicPlayer.Play("WindUnderneath");
		}
		if (controller.currentFloor >= 6 && controller.currentFloor <= 10)
		{
			gameShell.musicPlayer.Play("Forest");
		}
		if (controller.currentFloor >= 11 && controller.currentFloor <= 15)
		{
			gameShell.musicPlayer.Play("StrangeCrystal");
		}
		if (controller.currentFloor == 16)
		{
			gameShell.musicPlayer.Play("SharpShard");
		}
	}

	public override void _Process(double delta)
	{
		// Sprite Display
		foreach (var item in level)
		{
			if (item != null)
			{
				// if (item is Player) {GD.Print("item is player");}
				item.Position = new Vector2(Mathf.Lerp(item.Position.X, item.gridX * 16, .2f * (float)delta * 120),
				Mathf.Lerp(item.Position.Y, item.gridY * 16, .2f * (float)delta * 120));
				// if (item is Player) {GD.Print("item is player and error");}
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
				// Around
				for (var y = -1; y <= 1; y++)
				{
					for (var x = -1; x <= 1; x++)
					{
						if (level[player.gridX + x, player.gridY + y, 0] == item)
						{
							item.isVisible = true;
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
						item.Modulate = Colors.DimGray;
					}
					else
					{
						item.Visible = false;
					}
				}
			}
		}
	}

	public override void _Draw()
	{
		// DrawStringOutline(font, new Vector2(10, 600),
		// $"{player.name} {player.species} HitPoint: {player.hitPoint} Level: {player.level} Gender: {player.gender}", HorizontalAlignment.Left, -1, 16, 1);
		// DrawStringOutline(font, new Vector2(10, 620),
		// $"Str: {player.strength} Agi: {player.agility} Int: {player.intelligence} Tou: {player.toughness} AV: {player.AV} DV: {player.DV}", HorizontalAlignment.Left, -1, 16, 1);
	}

	public BaseObject[,,] InitTerrianGenerate()
	{
		var level = new BaseObject[levelWidth, levelHeight, 4];

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

		for (var height = 5; height < 10; height++)
		{
			for (var width = 5; width < 10; width++)
			{
				level[width, height, 0] = packedGround.Instantiate<Ground>();
				level[width, height, 0].gridX = width;
				level[width, height, 0].gridY = height;
			}
		}

		// Player
		level[7, 8, 3] = packedPlayer.Instantiate<Player>();
		level[7, 8, 3].gridX = 7;
		level[7, 8, 3].gridY = 8;
		player = level[7, 8, 3] as Player;
		GD.Print(controller);
		controller.player = player;
		GD.Print("player:" + player);

		// UpStair
		level[7, 6, 1] = packedUpstair.Instantiate<Upstair>();
		level[7, 6, 1].gridX = 7;
		level[7, 6, 1].gridY = 6;
		upstair = level[7, 6, 1] as Upstair;

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

	public BaseObject[,,] EndTerrianGenerate()
	{
		var level = new BaseObject[levelWidth, levelHeight, 4];

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

		for (var height = 5; height < 30; height++)
		{
			for (var width = 5; width < 10; width++)
			{
				level[width, height, 0] = packedGround.Instantiate<Ground>();
				level[width, height, 0].gridX = width;
				level[width, height, 0].gridY = height;
			}
		}

		for (var width = 5; width < 10; width++)
		{
			level[width, 10, 0] = packedWall.Instantiate<Wall>();
			level[width, 10, 0].gridX = width;
			level[width, 10, 0].gridY = 10;
		}
		// Door
		level[7, 10, 0] = packedGround.Instantiate<Ground>();
		level[7, 10, 0].gridX = 7;
		level[7, 10, 0].gridY = 10;
		level[7, 10, 1] = packedDoor.Instantiate<Door>();
		level[7, 10, 1].gridX = 7;
		level[7, 10, 1].gridY = 10;

		// Enemies
		for (var height = 11; height < 15; height++)
		{
			for (var width = 5; width < 10; width++)
			{
				var random = new Random();
				var randomNumber = random.Next(9);
				switch (randomNumber)
				{
					case 0: level[width, height, 3] = packedRat.Instantiate<Rat>(); break;
					case 1: level[width, height, 3] = packedWorm.Instantiate<Worm>(); break;
					case 2: level[width, height, 3] = packedRogue.Instantiate<Rogue>(); break;
					case 3: level[width, height, 3] = packedMushroom.Instantiate<Mushroom>(); break;
					case 4: level[width, height, 3] = packedSlime.Instantiate<Slime>(); break;
					case 5: level[width, height, 3] = packedAvian.Instantiate<Avian>(); break;
					case 6: level[width, height, 3] = packedBulletMan.Instantiate<BulletMan>(); break;
					case 7: level[width, height, 3] = packedStoneMan.Instantiate<StoneMan>(); break;
					case 8: level[width, height, 3] = packedKobold.Instantiate<Kobold>(); break;
				}
				AddEnemy(level[width, height, 3] as Enemy);
				level[width, height, 3].gridX = width;
				level[width, height, 3].gridY = height;
			}
		}

		// Downstair
		level[7, 6, 1] = packedDownstair.Instantiate<Downstair>();
		level[7, 6, 1].gridX = 7;
		level[7, 6, 1].gridY = 6;
		downstair = level[7, 6, 1] as Downstair;

		// Upstair
		level[7, 28, 1] = packedUpstair.Instantiate<Upstair>();
		level[7, 28, 1].gridX = 7;
		level[7, 28, 1].gridY = 28;
		upstair = level[7, 28, 1] as Upstair;

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

	public BaseObject[,,] TerrianGenerate(int roomIter, int minWidth, int minHeight, int maxWidth, int maxHeight, int roomType = 0)
	{
		var level = new BaseObject[levelWidth, levelHeight, 4];

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

							// Generate Enemies
							var someRandom = new Random();
							var randomNumber = someRandom.Next(100);
							if (controller.currentFloor > 10 && controller.currentFloor <= 15)
							{
								randomNumber = someRandom.Next(30);
							}
							if (randomNumber < 2)
							{
								if (controller.currentFloor <= 5)
								{
									level[xPos, yPos, 3] = packedRat.Instantiate<Rat>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 5 && controller.currentFloor <= 10)
								{
									level[xPos, yPos, 3] = packedSlime.Instantiate<Slime>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 10 && controller.currentFloor <= 15)
								{
									level[xPos, yPos, 3] = packedBulletMan.Instantiate<BulletMan>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
							}
							if (randomNumber >= 2 && randomNumber < 5)
							{
								if (controller.currentFloor <= 5)
								{
									level[xPos, yPos, 3] = packedWorm.Instantiate<Worm>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 5 && controller.currentFloor <= 10)
								{
									level[xPos, yPos, 3] = packedAvian.Instantiate<Avian>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 10 && controller.currentFloor <= 15)
								{
									level[xPos, yPos, 3] = packedKobold.Instantiate<Kobold>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
							}
							if (randomNumber >= 6 && randomNumber < 8)
							{
								if (controller.currentFloor <= 5)
								{
									level[xPos, yPos, 3] = packedRogue.Instantiate<Rogue>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 5 && controller.currentFloor <= 10)
								{
									level[xPos, yPos, 3] = packedMushroom.Instantiate<Mushroom>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
								if (controller.currentFloor > 10 && controller.currentFloor <= 15)
								{
									level[xPos, yPos, 3] = packedStoneMan.Instantiate<StoneMan>();
									AddEnemy(level[xPos, yPos, 3] as Enemy);
									level[xPos, yPos, 3].gridX = xPos;
									level[xPos, yPos, 3].gridY = yPos;
								}
							}

							for (var space = 0; space < levelWidth * levelHeight; space++)
							{
								if (roomSpace[space, 0] == 0)
								{
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
				}
			}
			// Add Stairs and Player
			if (previousRoomX == -1)
			{
				level[currentRoomX, currentRoomY, 1] = packedDownstair.Instantiate<Downstair>();
				level[currentRoomX, currentRoomY, 1].gridX = currentRoomX;
				level[currentRoomX, currentRoomY, 1].gridY = currentRoomY;
				downstair = level[currentRoomX, currentRoomY, 1] as Downstair;
				GD.Print("Create a downstair");
				if (level[currentRoomX, currentRoomY, 3] is Enemy enemy)
				{
					for (var i = 0; i < 200; i++)
					{
						if (enemyList[i] == enemy)
						{
							enemyList[i] = null;
							level[currentRoomX, currentRoomY, 3] = null;
						}
					}
				}
				// level[currentRoomX, currentRoomY, 3] = packedPlayer.Instantiate<Player>();
				// level[currentRoomX, currentRoomY, 3].gridX = currentRoomX;
				// level[currentRoomX, currentRoomY, 3].gridY = currentRoomY;
				// if (level[currentRoomX, currentRoomY, 3] is Player player)
				// {
				// 	this.player = player;
				// }
			}

			previousRoomX = randomPosX + Even(randomSizeX) / 2;
			previousRoomY = randomPosY + Even(randomSizeY) / 2;
		}
		// Add Upstair
		level[previousRoomX, previousRoomY, 1] = packedUpstair.Instantiate<Upstair>();
		level[previousRoomX, previousRoomY, 1].gridX = previousRoomX;
		level[previousRoomX, previousRoomY, 1].gridY = previousRoomY;
		upstair = level[previousRoomX, previousRoomY, 1] as Upstair;


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

	public void AddEnemy(Enemy enemy)
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (enemyList[iter] == null)
			{
				enemyList[iter] = enemy;
				break;
			}
		}
	}

	public void RemoveEnemy(Enemy enemy)
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (enemyList[iter] == enemy)
			{
				enemyList[iter] = null;
				level[enemy.gridX, enemy.gridY, 3] = null;
				if (enemy.isShrink)
				{
					var micro = packedMicro.Instantiate<Micro>();
					micro.name = $"Micro {enemy.species}[{enemy.rune}]";
					micro.nutrition = enemy.nutrition * 2;
					micro.rune = enemy.rune;
					micro.weight = 0;
					micro.icon = enemy.icon;
					DropItem(micro, enemy.gridX, enemy.gridY);
					gameShell.AddLog($"{enemy.species}[{enemy.rune}] has been shrinked");
				}
				else
				{
					var corpse = packedCorpse.Instantiate<Corpse>();
					corpse.name = $"{enemy.species}'s Corpse";
					corpse.nutrition = enemy.nutrition;
					corpse.weight = enemy.weight;
					DropItem(corpse, enemy.gridX, enemy.gridY);
					player.GetRune(enemy);
					gameShell.AddLog($"{enemy.species}[{enemy.rune}] has been destroyed");
				}
				for (var i = 0; i < 100; i++)
				{
					if (enemy.inventory[i] != null)
					{
						DropItem(enemy.inventory[i], enemy.gridX, enemy.gridY);
					}
				}
				enemy.QueueFree();
			}
		}
	}

	public void TurnPassed()
	{
		player.time++;
		if (player.hungryNess > 0)
		{
			player.hungryNess--;
		}
		if (player.time % 10 == 1)
		{
			player.Heal(1);
		}
		if (player.hungryNess <= 0)
		{
			player.hitPoint -= 1;
		}
		for (var iter = 0; iter < 200; iter++)
		{
			if (enemyList[iter] != null)
			{
				enemyList[iter].TurnPassed();
			}
		}
		// ShrinkGun Charge
		if (player.rangeWeapon is ShrinkGun shrinkGun)
		{
			if ((player.time % Mathf.Max(120 - player.intelligence, 2)) == 1)
			{
				shrinkGun.ammo = Mathf.Min(shrinkGun.maxAmmo, shrinkGun.ammo + 1);
			}
		}
		// LaserGun Charge
		if (player.rangeWeapon is LaserGun laserGun)
		{
			if ((player.time % Mathf.Max(50 - player.intelligence, 2)) == 1)
			{
				laserGun.ammo = Mathf.Min(laserGun.maxAmmo, laserGun.ammo + 1);
			}
		}
	}

	public void DropItem(PickUp item, int x, int y)
	{
		if (level[x, y, 2] is not DropItems)
		{
			NewInstance<DropItems>(packedDropItems, x, y, 2);
		}
		for (var i = 0; i < 200; i++)
		{
			if ((level[x, y, 2] as DropItems).dropItems[i] == null)
			{
				(level[x, y, 2] as DropItems).dropItems[i] = item;
				return;
			}
		}
	}

	public void NewInstance<T>(PackedScene packedScene, int x, int y, int layer) where T : BaseObject
	{
		level[x, y, layer] = packedScene.Instantiate<T>();
		level[x, y, layer].gridX = x;
		level[x, y, layer].gridY = y;
		level[x, y, layer].Position = new Vector2(x * 16, y * 16);
		AddChild(level[x, y, layer]);
	}

	public void MapLevel()
	{
		for (var layer = 0; layer < 4; layer++)
		{
			for (var height = 0; height < levelHeight; height++)
			{
				for (var width = 0; width < levelWidth; width++)
				{
					if (level[width, height, layer] is not Player)
					{
						controller.wholeLevels[width, height, layer, controller.currentFloor] = level[width, height, layer];
					}
				}
			}
		}
	}

	public void DamageNumber(int x, int y, int damage)
	{
		var damageNumber = packedDamageNumber.Instantiate<DamageNumber>();
		damageNumber.Position = new Vector2(x * 16, y * 16);
		damageNumber.damage = damage;
		AddChild(damageNumber);
	}
}
