using Godot;
using System;
using System.IO;
using System.Text.Json;

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
				// string jsonString = JsonSerializer.Serialize(controller);
				// var save = Godot.FileAccess.Open("user://savegame.json", Godot.FileAccess.ModeFlags.Write);
				// GD.Print(jsonString);
				// save.StoreString(jsonString);
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
