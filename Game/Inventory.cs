using Godot;
using System;
using System.Security;

public partial class Inventory : Node2D
{
	[Export]
	private PackedScene packedInventoryItem;
	[Export]
	private PackedScene packedSecondaryMenu;
	private Label page;
	private Label weight;
	private Select select;
	private Vector2 size = new Vector2(0, 0);
	public InventoryItem[] inventoryItems = new InventoryItem[12];
	public PickUp[,] pickUps = new PickUp[12, 10];
	public int currentPage = 0;
	public int currentItem = 0;
	public int maxPage = 0;
	public int playerWeight = 0;
	public int playerMaxWeight = 0;
	public bool isSecondaryMenu = false;
	public SecondaryMenu secondaryMenu;
	public GameShell gameShell;
	public override void _Ready()
	{
		page = GetNode<Label>("Page");
		page.Position = new Vector2(300 - 9 - page.Size.X, 9);
		page.Text = $"Page: {currentPage + 1}/{maxPage + 1}";
		weight = GetNode<Label>("Weight");
		weight.Text = $"Weight: {playerWeight}/{playerMaxWeight}";
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
		// QueueRedraw();
		page.Position = new Vector2(300 - 9 - page.Size.X, 9);
		page.Text = $"Page: {currentPage + 1}/{maxPage + 1}";
		for (var iter = 0; iter < 12; iter++)
		{
			inventoryItems[iter].Text = "-";
			if (pickUps[iter, currentPage] != null)
			{
				inventoryItems[iter].Text = pickUps[iter, currentPage].name;
			}
		}

		if (!isSecondaryMenu)
		{
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
			select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, inventoryItems[currentItem].Size.X + 3, delta * 120 * .2f),
			(float)Mathf.Lerp(select.Size.Y, inventoryItems[currentItem].Size.Y - 3, delta * 120 * .2f));
			select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, inventoryItems[currentItem].Position.X - 3, delta * 120 * .2f),
			(float)Mathf.Lerp(select.Position.Y, inventoryItems[currentItem].Position.Y, delta * 120 * .2f));

			// SecondaryMenu
			if (Input.IsActionJustPressed("Confirm") && pickUps[currentItem, currentPage] != null)
			{
				isSecondaryMenu = true;
				secondaryMenu = packedSecondaryMenu.Instantiate<SecondaryMenu>();
				secondaryMenu.selectItem = pickUps[currentItem, currentPage];
				secondaryMenu.Position = new Vector2(150, 150);
				AddChild(secondaryMenu);
			}
		}
	}

	public void Drop()
	{
		gameShell.game.player.DeleteItem(secondaryMenu.selectItem);
		gameShell.game.DropItem(secondaryMenu.selectItem, gameShell.game.player.gridX, gameShell.game.player.gridY);
		gameShell.game.player.isBagOpen = false;
	}

	public void Eat()
	{
		gameShell.game.player.DeleteItem(secondaryMenu.selectItem);
		// gameShell.game.DropItem(secondaryMenu.selectItem, gameShell.game.player.gridX, gameShell.game.player.gridY);
		gameShell.game.player.hungryNess += (secondaryMenu.selectItem as Food).nutrition;
		gameShell.game.player.hungryNess = Math.Min(gameShell.game.player.hungryNess, gameShell.game.player.maxHungryNess);
		gameShell.game.player.isBagOpen = false;
	}
}
