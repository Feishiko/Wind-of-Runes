using Godot;
using System;

public partial class SecondaryMenu : Node2D
{
	[Export]
	private PackedScene packedSecondaryMenuItem;
	[Export]
	private PackedScene packedEquipmentPanel;
	public PickUp selectItem;
	public SecondaryMenuItem[] secondaryMenuItems = new SecondaryMenuItem[6];
	public int selectMenuItem = 0;
	public int maxMenuItems = 0;
	private ColorRect select;
	public Inventory inventory;
	private EquipmentPanel equipmentPanel;
	public bool isEquipmentPanel = false;
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
			if (equipment.part == "Hand" && equipment != inventory.gameShell.game.player.hand)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "Head" && equipment != inventory.gameShell.game.player.head)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "Foot" && equipment != inventory.gameShell.game.player.foot)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "Body" && equipment != inventory.gameShell.game.player.body)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "Weapon" && equipment != inventory.gameShell.game.player.weapon)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "RangeWeapon" && equipment != inventory.gameShell.game.player.rangeWeapon)
			{
				TryAddMenuItem("Equip");
			}
			if (equipment.part == "Ammo" && equipment != inventory.gameShell.game.player.ammo)
			{
				TryAddMenuItem("Equip");
			}
			if (!equipment.isEquipped)
			{
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
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, secondaryMenuItems[selectMenuItem].size.X, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Size.Y, secondaryMenuItems[selectMenuItem].size.Y, delta * 120 * .2f));
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, secondaryMenuItems[selectMenuItem].Position.X - secondaryMenuItems[selectMenuItem].size.X / 2, delta * 120 * .2f),
		(float)Mathf.Lerp(select.Position.Y, secondaryMenuItems[selectMenuItem].Position.Y - secondaryMenuItems[selectMenuItem].size.Y / 2 - 2.5, delta * 120 * .2f));
		if (!isEquipmentPanel)
		{
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
					case "Eat":
						{
							inventory.Eat();
						}; break;
					case "Equip":
						{
							if (selectItem is Equipment equipment)
							{
								equipmentPanel = packedEquipmentPanel.Instantiate<EquipmentPanel>();
								isEquipmentPanel = true;
								equipmentPanel.equipmentPart = equipment.part;
								equipmentPanel.newOne = equipment;
								equipmentPanel.Position = new Vector2(-150, -150);
								AddChild(equipmentPanel);
							}
						}; break;
					case "Take off":
						{
							if (selectItem is Equipment equipment)
							{
								var player = inventory.gameShell.game.player;
								equipment.isEquipped = false;
								if (equipment.part == "Hand")
								{
									inventory.gameShell.game.player.hand = null;
								}
								if (equipment.part == "Head")
								{
									inventory.gameShell.game.player.head = null;
								}
								if (equipment.part == "Body")
								{
									inventory.gameShell.game.player.body = null;
								}
								if (equipment.part == "Foot")
								{
									inventory.gameShell.game.player.foot = null;
								}
								if (equipment.part == "Weapon")
								{
									inventory.gameShell.game.player.weapon = null;
								}
								if (equipment.part == "RangeWeapon")
								{
									inventory.gameShell.game.player.rangeWeapon = null;
								}
								if (equipment.part == "Ammo")
								{
									inventory.gameShell.game.player.ammo = null;
								}
								player.strength -= equipment.strength;
								player.agility -= equipment.agility;
								player.intelligence -= equipment.intelligence;
								player.toughness -= equipment.toughness;
								player.AV -= equipment.AV;
								player.DV -= equipment.DV;
							}
							Cancel();
						}; break;
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

		if (!isEquipmentPanel && equipmentPanel != null)
		{
			equipmentPanel.QueueFree();
			isEquipmentPanel = false;
			equipmentPanel = null;
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
