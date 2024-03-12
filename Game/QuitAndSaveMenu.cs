using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;


public class SaveGame
{
	public JsonPlayer player { get; set; }
	public int currentFloor { get; set; } = 0;
	public int maxFloor { get; set; } = -1;
	// LevelWidth is 40, LevelHeight is 40
	public JsonBaseObject[,,,] wholeLevels = new JsonBaseObject[40, 40, 4, 20];
	public bool isUp { get; set; } = false;
	public bool isWin { get; set; } = false;
	public bool isAnimation { get; set; } = false;
	public string playerName { get; set; } = "null";
	public string playerSpecies { get; set; } = "null";
	public string playerGender { get; set; } = "null";

	public void CopyPlayer(Player player)
	{
		this.player.hitPoint = player.hitPoint;
		this.player.maxHitPoint = player.maxHitPoint;
		this.player.level = player.level;
		this.player.name = player.name;
		this.player.gender = player.gender;
		this.player.species = player.species;
		this.player.strength = player.strength;
		this.player.agility = player.agility;
		this.player.intelligence = player.intelligence;
		this.player.toughness = player.toughness;
		this.player.AV = player.AV;
		this.player.DV = player.DV;
		this.player.hungryNess = player.hungryNess;
		this.player.maxHungryNess = player.maxHungryNess;
		this.player.time = player.time;
		this.player.exp = player.exp;
		this.player.weight = player.weight;
		this.player.maxWeight = player.weight;
		for (var iter = 0; iter < 200; iter++)
		{
			var jsonPickUp = this.player.inventory[iter];
			var pickUp = player.inventory[iter];
			if (pickUp != null)
			{
				jsonPickUp.name = pickUp.name;
				jsonPickUp.description = pickUp.description;
				jsonPickUp.weight = pickUp.weight;
				if (pickUp is Equipment equipment)
				{
					var jsonPickUpEquipment = jsonPickUp as JsonEquipment;
					jsonPickUpEquipment.part = equipment.part;
					jsonPickUpEquipment.damage = equipment.damage;
					jsonPickUpEquipment.damageDiceNumber = equipment.damageDiceNumber;
					jsonPickUpEquipment.strength = equipment.strength;
					jsonPickUpEquipment.agility = equipment.agility;
					jsonPickUpEquipment.intelligence = equipment.intelligence;
					jsonPickUpEquipment.toughness = equipment.toughness;
					jsonPickUpEquipment.AV = equipment.AV;
					jsonPickUpEquipment.DV = equipment.DV;
					jsonPickUpEquipment.isEquipped = equipment.isEquipped;
				}
				if (pickUp is Food food)
				{
					var jsonPickUpFood = jsonPickUp as JsonFood;
					jsonPickUpFood.nutrition = food.nutrition;
					if (food is Corpse)
					{
						jsonPickUpFood.type = "Corpse";
					}
					if (food is Bread)
					{
						jsonPickUpFood.type = "Bread";
					}
				}
				if (pickUp is Bullet bullet)
				{
					var jsonPickUpBullet = jsonPickUp as JsonBullet;
					jsonPickUpBullet.numbers = bullet.numbers;
				}
				if (pickUp is ShrinkGun shrinkGun)
				{
					var jsonPickUpShrinkGun = jsonPickUp as JsonShrinkGun;
					jsonPickUpShrinkGun.ammo = shrinkGun.ammo;
					jsonPickUpShrinkGun.maxAmmo = shrinkGun.maxAmmo;
				}
				if (pickUp is LaserGun laserGun)
				{

				}
				if (pickUp is Micro micro)
				{
					
				}
			}
		}
	}
}
public partial class QuitAndSaveMenu : Node2D
{
	[Export]
	private PackedScene packedSecondaryMenuItem;
	private string[] menu = new string[] { "Quit", "Cancel" };
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
				// var save = Godot.FileAccess.Open("user://savegame.json", Godot.FileAccess.ModeFlags.Write);
				// GD.Print(jsonString);
				// save.StoreString(jsonString);
				var saveGame = new SaveGame();
				controller.Init();
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
