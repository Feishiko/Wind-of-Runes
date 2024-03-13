using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
public class SaveGame
{
	public JsonPlayer player { get; set; } = new JsonPlayer();
	public int currentFloor { get; set; } = 0;
	public int maxFloor { get; set; } = -1;
	// LevelWidth is 40, LevelHeight is 40
	// public JsonBaseObject[,,,] wholeLevels = new JsonBaseObject[40, 40, 4, 20];
	// Opened Door -> + | Closed Door -> / | Ground -> . | Wall -> #
	public char[] terrian { get; set; } = new char[40 * 40 * 20];
	public bool[] isMemory { get; set; } = new bool[40 * 40 * 20];
	public bool isAnimation { get; set; } = false;
	public string playerName { get; set; } = "null";
	public string playerSpecies { get; set; } = "null";
	public string playerGender { get; set; } = "null";
	public JsonDropItems[] jsonDropItems { get; set; } = new JsonDropItems[40 * 40 * 20];
	public JsonEnemy[] jsonEnemies { get; set; } = new JsonEnemy[40 * 40 * 20];

	public void CopiedFrom(Player player, Controller controller, Game game)
	{
		GD.Print("this.player: " + this.player);
		this.player.CopiedFrom(player);
		currentFloor = controller.currentFloor;
		maxFloor = controller.maxFloor;
		isAnimation = controller.isAnimation;
		playerName = controller.playerName;
		playerSpecies = controller.playerSpecies;
		playerGender = controller.playerGender;

		// Terrian
		for (var floor = 0; floor < 20; floor++)
		{
			for (var y = 0; y < 40; y++)
			{
				for (var x = 0; x < 40; x++)
				{
					if (controller.wholeLevels[x, y, 0, floor] is Wall wall)
					{
						terrian[x + y * 40 + floor * 40 * 40] = '#';
						isMemory[x + y * 40 + floor * 40 * 40] = wall.isMemorized;
					}
					if (controller.wholeLevels[x, y, 0, floor] is Ground ground)
					{
						terrian[x + y * 40 + floor * 40 * 40] = '.';
						isMemory[x + y * 40 + floor * 40 * 40] = ground.isMemorized;
					}
					if (controller.wholeLevels[x, y, 1, floor] is Door door)
					{
						terrian[x + y * 40 + floor * 40 * 40] = door.isOpen ? '/' : '+';
					}
					if (controller.wholeLevels[x, y, 1, floor] is Upstair)
					{
						terrian[x + y * 40 + floor * 40 * 40] = '<';
					}
					if (controller.wholeLevels[x, y, 1, floor] is Downstair)
					{
						terrian[x + y * 40 + floor * 40 * 40] = '>';
					}
					// Drop Items
					if (controller.wholeLevels[x, y, 2, floor] is DropItems dropItems)
					{
						for (var iter = 0; iter < 200; iter++)
						{
							if (dropItems.dropItems[iter] != null)
							{
								dropItems.dropItems[iter].name = dropItems.dropItems[iter].name;
								dropItems.dropItems[iter].description = dropItems.dropItems[iter].description;
								dropItems.dropItems[iter].name = dropItems.dropItems[iter].name;
								if (dropItems.dropItems[iter] is Food food)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonFood)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonFood();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonFood).CopiedFrom(food);
								}
								if (dropItems.dropItems[iter] is Equipment equipment)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonEquipment)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonEquipment();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonEquipment).CopiedFrom(equipment);
								}
								if (dropItems.dropItems[iter] is Micro micro)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonMicro)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonMicro();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonMicro).CopiedFrom(micro);
								}
								if (dropItems.dropItems[iter] is ShrinkGun shrinkGun)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonShrinkGun)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonShrinkGun();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonShrinkGun).CopiedFrom(shrinkGun);
								}
								if (dropItems.dropItems[iter] is LaserGun laserGun)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonLaserGun)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonLaserGun();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonLaserGun).CopiedFrom(laserGun);
								}
								if (dropItems.dropItems[iter] is Bullet bullet)
								{
									if (jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] is not JsonBullet)
									{
										jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] = new JsonBullet();
									}
									(jsonDropItems[x + y * 40 + floor * 40 * 40].dropItems[iter] as JsonBullet).CopiedFrom(bullet);
								}
							}
						}
					}
					// Enemies
					if (controller.wholeLevels[x, y, 3, floor] is Enemy enemy)
					{
						if (jsonEnemies[x + y * 40 + floor * 40 * 40] == null)
						{
							jsonEnemies[x + y * 40 + floor * 40 * 40] = new JsonEnemy();
						}
						jsonEnemies[x + y * 40 + floor * 40 * 40].CopiedFrom(enemy);
					}
				}
			}
		}
	}
}
public partial class QuitAndSaveMenu : Node2D
{
	[Export]
	private PackedScene packedSecondaryMenuItem;
	private string[] menu = new string[] { "Save and Quit", "Cancel" };
	private SecondaryMenuItem[] secondaryMenuItems = new SecondaryMenuItem[2];
	private ColorRect select;
	private int currentMenu = 0;
	private GameShell gameShell;
	private Controller controller;
	public override void _Ready()
	{
		for (var iter = 0; iter < 2; iter++)
		{
			secondaryMenuItems[iter] = packedSecondaryMenuItem.Instantiate<SecondaryMenuItem>();
			secondaryMenuItems[iter].text = menu[iter];
			secondaryMenuItems[iter].Position = new Vector2(150, 150 - 25 + 50 * iter);
			AddChild(secondaryMenuItems[iter]);
		}
		select = GetNode<ColorRect>("Select");
		gameShell = GetParent<GameShell>();
		controller = Controller.GetInstance();
	}

	public override void _Process(double delta)
	{
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, secondaryMenuItems[currentMenu].Position.X - secondaryMenuItems[currentMenu].size.X / 2, .2f * delta * 120),
		(float)Mathf.Lerp(select.Position.Y, secondaryMenuItems[currentMenu].Position.Y - secondaryMenuItems[currentMenu].size.Y / 2 - 2.5, .2f * delta * 120));
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, secondaryMenuItems[currentMenu].size.X, .2f * delta * 120),
		(float)Mathf.Lerp(select.Size.Y, secondaryMenuItems[currentMenu].size.Y, .2f * delta * 120));
		if (Input.IsActionJustPressed("Up") && currentMenu > 0)
		{
			currentMenu -= 1;
		}
		if (Input.IsActionJustPressed("Down") && currentMenu < 1)
		{
			currentMenu += 1;
		}
		if (Input.IsActionJustPressed("Confirm"))
		{
			if (currentMenu == 0)
			{
				// Save and Quit
				// controller.player = gameShell.game.player;
				// var saveGame = new SaveGame();
				// saveGame.player = controller.player;
				// string jsonString = JsonSerializer.Serialize(saveGame);
				// string jsonString = JsonSerializer.Serialize<Controller>(controller);
				// GD.Print(jsonString);
				gameShell.game.MapLevel();
				gameShell.game.player.Record();
				var saveGame = new SaveGame();
				saveGame.CopiedFrom(gameShell.game.player, controller, gameShell.game);
				string jsonString = JsonSerializer.Serialize(saveGame);
				var savePath = ProjectSettings.GlobalizePath("user://savegame.json");
				File.WriteAllText(savePath, jsonString);
				// var save = Godot.FileAccess.Open("user://savegame.json", Godot.FileAccess.ModeFlags.Write);
				// save.StoreString(jsonString);
				// GD.Print(jsonString);
				// controller.Init();
				GetTree().ChangeSceneToFile("res://Start/Start.tscn");
			}
			if (currentMenu == 1)
			{
				// Cancel
				gameShell.isQuitAndSave = false;
			}
		}
	}
}
