using Godot;
using System;

public partial class Upgrade : Node2D
{
	[Export]
	private PackedScene packedUpgradeItem;
	private UpgradeItem[] upgradeItems = new UpgradeItem[4];
	private string[] playerAttribute = new string[] { "Strength", "Agility", "Toughness", "Intelligence" };
	private int selectItem = 0;
	private ColorRect select;
	private GameShell gameShell;
	public override void _Ready()
	{
		select = GetNode<ColorRect>("Select");
		gameShell = GetParent<GameShell>();
		for (var iter = 0; iter < 4; iter++)
		{
			upgradeItems[iter] = packedUpgradeItem.Instantiate<UpgradeItem>();
			upgradeItems[iter].Position = new Vector2(150, 75 + iter * 50);
			upgradeItems[iter].text = playerAttribute[iter];
			AddChild(upgradeItems[iter]);
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Down") && selectItem < 3)
		{
			selectItem++;
		}
		if (Input.IsActionJustPressed("Up") && selectItem > 0)
		{
			selectItem--;
		}
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, upgradeItems[selectItem].size.X, .2f * delta * 120),
		(float)Mathf.Lerp(select.Size.Y, upgradeItems[selectItem].size.Y, .2f * delta * 120));
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, upgradeItems[selectItem].Position.X - select.Size.X / 2, .2f * delta * 120),
		(float)Mathf.Lerp(select.Position.Y, upgradeItems[selectItem].Position.Y - select.Size.Y / 2, .2f * delta * 120));
		if (Input.IsActionJustPressed("Confirm"))
		{
			var player = gameShell.game.player;
			player.exp = player.exp - player.level * 20;
			player.level += 1;
			switch (upgradeItems[selectItem].text)
			{
				case "Strength": player.strength++; break;
				case "Agility": player.agility++; break;
				case "Intelligence": player.intelligence++; break;
				case "Toughness": player.toughness++; break;
			}
			if (player.species == "Human")
			{
				switch (upgradeItems[selectItem].text)
				{
					case "Strength": player.strength++; break;
					case "Agility": player.agility++; break;
					case "Intelligence": player.intelligence++; break;
					case "Toughness": player.toughness++; break;
				}
			}
		}
	}
}
