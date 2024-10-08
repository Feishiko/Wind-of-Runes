using Godot;
using System;
using System.Security.Cryptography;
public class JsonDropItems : JsonBaseObject
{
	public JsonPickUp[] dropItems { get; set; } = new JsonPickUp[200];
}
public partial class DropItems : BaseObject
{
	private Game game;
	public PickUp[] dropItems = new PickUp[200];
	private Texture2D icon;
	public int itemNumber = 0;
	public override void _Ready()
	{
		game = GetParent<Game>();
	}

	public override void _Process(double delta)
	{
		itemNumber = 0;
		for (var iter = 0; iter < 200; iter++)
		{
			if (dropItems[iter] != null)
			{
				itemNumber++;
			}
		}
		if (itemNumber == 0)
		{
			game.level[gridX, gridY, 2] = null;
			QueueFree();
		}
		for (var iter = 0; iter < 200; iter++)
		{
			if (dropItems[iter] != null)
			{
				icon = dropItems[iter].icon;
				if (dropItems[iter] is Micro)
				{
					Scale = new Vector2(.4f, .4f);
				}
				else
				{
					Scale = Vector2.One;
				}
				break;
			}
		}
		GetNode<Sprite2D>("Sprite2D").Texture = icon;
	}

	public bool IsSingleItem()
	{
		var itemNumber = 0;
		for (var iter = 0; iter < 200; iter++)
		{
			if (dropItems[iter] != null)
			{
				itemNumber++;
			}
		}
		return itemNumber == 1;
	}

	public PickUp GetSingleItem()
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (dropItems[iter] != null)
			{
				return dropItems[iter];
			}
		}
		return null;
	}

	public void DeleteItem(PickUp pickUp)
	{
		for (var iter = 0; iter < 200; iter++)
		{
			if (dropItems[iter] == pickUp)
			{
				dropItems[iter] = null;
				GD.Print("Item deleted!");
			}
		}
	}

	public void DropItemsCopy(DropItems dropItems)
	{
		this.dropItems = dropItems.dropItems;
		icon = dropItems.icon;
		itemNumber = dropItems.itemNumber;
	}
}
