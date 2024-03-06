using Godot;
using System;

public partial class SecondaryMenu : Node2D
{
	[Export]
	private PackedScene packedSecondaryMenuItem;
	public PickUp selectItem;
	public SecondaryMenuItem[] secondaryMenuItems = new SecondaryMenuItem[6];
	public int selectMenuItem = 0;
	public int maxMenuItems = 0;
	private ColorRect select;
	private Inventory inventory;
	public override void _Ready()
	{
		select = GetNode<ColorRect>("Select");
		inventory = GetParent<Inventory>();
		if (selectItem is Food)
		{
			TryAddMenuItem("Eat");
		}
		if (selectItem is Equipment equipment)
		{
			if (!equipment.isEquipped)
			{
				TryAddMenuItem("Equip");
				TryAddMenuItem("Drop");
			}
			else
			{
				TryAddMenuItem("Take off");
			}
		}
		else
		{
			TryAddMenuItem("Drop");
		}
		TryAddMenuItem("Cancel");

		// Position Changes
		var Even = (int num) => num % 2 == 0 ? num : num + 1;
		for (var iter = 0; iter < 6; iter++)
		{
			if (secondaryMenuItems[iter] != null)
			{
				secondaryMenuItems[iter].Position = new Vector2(0, -20 * (maxMenuItems - 1) / 2 + iter * 20);
			}
		}
	}

	public override void _Process(double delta)
	{
		// select.Position = secondaryMenuItems[selectMenuItem].Position - secondaryMenuItems[selectMenuItem].size / 2 - new Vector2(0, 2);
		// select.Size = secondaryMenuItems[selectMenuItem].size;
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, secondaryMenuItems[selectMenuItem].size.X, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Size.Y, secondaryMenuItems[selectMenuItem].size.Y, delta * 120 * .2f));
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, secondaryMenuItems[selectMenuItem].Position.X - secondaryMenuItems[selectMenuItem].size.X / 2, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Position.Y, secondaryMenuItems[selectMenuItem].Position.Y - secondaryMenuItems[selectMenuItem].size.Y / 2 - 2.5, delta * 120 * .2f));
		if (Input.IsActionJustPressed("Up") && selectMenuItem > 0)
		{
			selectMenuItem -= 1;
		}
		if (Input.IsActionJustPressed("Down") && selectMenuItem < maxMenuItems - 1)
		{
			selectMenuItem += 1;
		}
		if (Input.IsActionJustPressed("Confirm"))
		{
			switch (secondaryMenuItems[selectMenuItem].text)
			{
				case "Eat":; break;
				case "Equip":; break;
				case "Take off":; break;
				case "Drop":
					{
						inventory.Drop();
						Cancel();
					}; break;
				case "Cancel":
					{
						Cancel();
					}; break;
			}
		}
	}

	public void TryAddMenuItem(string menuItem)
	{
		for (var iter = 0; iter < 6; iter++)
		{
			if (secondaryMenuItems[iter] == null)
			{
				secondaryMenuItems[iter] = packedSecondaryMenuItem.Instantiate<SecondaryMenuItem>();
				secondaryMenuItems[iter].text = menuItem;
				AddChild(secondaryMenuItems[iter]);
				maxMenuItems++;
				return;
			}
		}
	}

	public void Cancel()
	{
		inventory.isSecondaryMenu = false;
		inventory.secondaryMenu = null;
		QueueFree();
	}
}
