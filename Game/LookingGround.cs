using Godot;
using System;

public partial class LookingGround : Node2D
{
	[Export]
	private PackedScene packedInventoryItem;
	private Label page;
	private Select select;
	private Vector2 size = new Vector2(0, 0);
	public InventoryItem[] inventoryItems = new InventoryItem[12];
	public PickUp[,] pickUps = new PickUp[12, 10];
	public int currentPage = 0;
	public int currentItem = 0;
	public int maxPage = 0;
	public GameShell gameShell;
	public PickUp[] selectedItems = new PickUp[120];
	public override void _Ready()
	{
		page = GetNode<Label>("Page");
		page.Position = new Vector2(300 - 9 - page.Size.X, 9);
		page.Text = $"Page: {currentPage + 1}/{maxPage + 1}";
		select = GetNode<Select>("Select");
		gameShell = GetParent<GameShell>();
		for (var iter = 0; iter < 12; iter++)
		{
			inventoryItems[iter] = packedInventoryItem.Instantiate<InventoryItem>();
			inventoryItems[iter].Position = new Vector2(30, 40 + iter * 20);
			AddChild(inventoryItems[iter]);
		}
	}

	public override void _Process(double delta)
	{
		page.Position = new Vector2(300 - 9 - page.Size.X, 9);
		page.Text = $"Page: {currentPage + 1}/{maxPage + 1}";
		for (var iter = 0; iter < 12; iter++)
		{
			inventoryItems[iter].Text = "-";
			if (pickUps[iter, currentPage] != null)
			{
				if (pickUps[iter, currentPage] is Bullet bullet)
				{
					inventoryItems[iter].Text = pickUps[iter, currentPage].selected ? $"[{pickUps[iter, currentPage].name}({pickUps[iter, currentPage].weight}w)x{bullet.numbers}]" : pickUps[iter, currentPage].name + $"({pickUps[iter, currentPage].weight}w)x{bullet.numbers}";
				}
				else
				{
					inventoryItems[iter].Text = pickUps[iter, currentPage].selected ? $"[{pickUps[iter, currentPage].name}({pickUps[iter, currentPage].weight}w)]" : pickUps[iter, currentPage].name + $"({pickUps[iter, currentPage].weight}w)";
				}
				if (pickUps[iter, currentPage] is ShrinkGun shrinkGun)
				{
					inventoryItems[iter].Text = pickUps[iter, currentPage].selected ? "[" + shrinkGun.name + $"({shrinkGun.weight}w)[{shrinkGun.ammo}/{shrinkGun.maxAmmo}]]" : shrinkGun.name + $"({shrinkGun.weight}w)[{shrinkGun.ammo}/{shrinkGun.maxAmmo}]";
				}
				if (pickUps[iter, currentPage] is LaserGun laserGun)
				{
					inventoryItems[iter].Text = pickUps[iter, currentPage].selected ? "[" + laserGun.name + $"({laserGun.weight}w)[{laserGun.ammo}/{laserGun.maxAmmo}]]" : laserGun.name + $"({laserGun.weight}w)[{laserGun.ammo}/{laserGun.maxAmmo}]";
				}
			}
		}

		// Select
		if (Input.IsActionJustPressed("Up") && currentItem > 0)
		{
			currentItem -= 1;
		}
		if (Input.IsActionJustPressed("Down") && currentItem < 11)
		{
			currentItem += 1;
		}

		if (Input.IsActionJustPressed("Left") && currentPage > 0)
		{
			currentPage -= 1;
		}

		if (Input.IsActionJustPressed("Right") && currentPage < maxPage)
		{
			currentPage += 1;
		}

		if (Input.IsActionJustPressed("Confirm"))
		{
			pickUps[currentItem, currentPage].selected = !pickUps[currentItem, currentPage].selected;
		}

		if (Input.IsActionJustPressed("Submit"))
		{
			foreach (var item in pickUps)
			{
				if (item != null)
				{
					if (item.selected && gameShell.game.player.Pickable(item))
					{
						gameShell.game.player.Pick(item);
						(gameShell.game.level[gameShell.game.player.gridX, gameShell.game.player.gridY, 2] as DropItems).DeleteItem(item);
					}
					if (!gameShell.game.player.Pickable(item))
					{
						gameShell.AddLog("Too heavy! You can't pick up more items!");
					}
				}
			}
			gameShell.game.player.isLookingGround = false;
			gameShell.game.TurnPassed();
		}

		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, inventoryItems[currentItem].Size.X + 3, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Size.Y, inventoryItems[currentItem].Size.Y - 3, delta * 120 * .2f));
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, inventoryItems[currentItem].Position.X - 3, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Position.Y, inventoryItems[currentItem].Position.Y, delta * 120 * .2f));
	}
}
