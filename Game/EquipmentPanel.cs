using Godot;
using System;

public partial class EquipmentPanel : Node2D
{
	[Export]
	private PackedScene packedEquipmentPanelItem;
	private EquipmentPanelItem[] equipmentPanelItems = new EquipmentPanelItem[2];
	private int selectedItem = 1;
	public string equipmentPart;
	public Equipment oldOne;
	public Equipment newOne;
	private SecondaryMenu secondaryMenu;
	private ColorRect select;
	public override void _Ready()
	{
		secondaryMenu = GetParent<SecondaryMenu>();
		select = GetNode<ColorRect>("Select");
		var player = secondaryMenu.inventory.gameShell.game.player;
		switch (equipmentPart)
		{
			case "Hand": oldOne = player.hand; break;
			case "Foot": oldOne = player.foot; break;
			case "Body": oldOne = player.body; break;
			case "Head": oldOne = player.head; break;
			case "Weapon": oldOne = player.weapon; break;
			case "RangeWeapon": oldOne = player.rangeWeapon; break;
			case "Ammo": oldOne = player.ammo; break;
		}

		for (var iter = 0; iter < 2; iter++)
		{
			equipmentPanelItems[iter] = packedEquipmentPanelItem.Instantiate<EquipmentPanelItem>();
			equipmentPanelItems[iter].Position = new Vector2(25 + iter * 150, 50);
		}
		equipmentPanelItems[0].text = oldOne == null ? "Empty" : oldOne.description;
		equipmentPanelItems[1].text = newOne.description;
		AddChild(equipmentPanelItems[0]);
		AddChild(equipmentPanelItems[1]);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Up") || Input.IsActionJustPressed("Left"))
		{
			if (selectedItem > 0)
			{
				selectedItem -= 1;
			}
		}

		if (Input.IsActionJustPressed("Down") || Input.IsActionJustPressed("Right"))
		{
			if (selectedItem < 1)
			{
				selectedItem += 1;
			}
		}
		var equipment = selectedItem == 0 ? oldOne : newOne;
		if (Input.IsActionJustPressed("Confirm") && equipment != null)
		{
			var player = secondaryMenu.inventory.gameShell.game.player;
			var otherEquipment = selectedItem == 1 ? oldOne : newOne;
			switch (equipmentPart)
			{
				case "Hand": player.hand = equipment; break;
				case "Foot": player.foot = equipment; break;
				case "Body": player.body = equipment; break;
				case "Head": player.head = equipment; break;
				case "Weapon": player.weapon = equipment; break;
				case "RangeWeapon": player.rangeWeapon = equipment; break;
				case "Ammo": player.ammo = equipment; break;
			}
			if (equipment != null)
			{
				equipment.isEquipped = true;
				player.strength += equipment.strength;
				player.agility += equipment.agility;
				player.intelligence += equipment.intelligence;
				player.toughness += equipment.toughness;
				player.AV += equipment.AV;
				player.DV += equipment.DV;
			}
			if (otherEquipment != null)
			{
				otherEquipment.isEquipped = false;
				player.strength -= otherEquipment.strength;
				player.agility -= otherEquipment.agility;
				player.intelligence -= otherEquipment.intelligence;
				player.toughness -= otherEquipment.toughness;
				player.AV -= otherEquipment.AV;
				player.DV -= otherEquipment.DV;
			}
			secondaryMenu.isEquipmentPanel = false;
			secondaryMenu.Cancel();
		}

		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, equipmentPanelItems[selectedItem].Position.X, .2f * delta * 120),
		(float)Mathf.Lerp(select.Position.Y, equipmentPanelItems[selectedItem].Position.Y, .2f * delta * 120));
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, equipmentPanelItems[selectedItem].size.X, .2f * delta * 120),
		(float)Mathf.Lerp(select.Size.Y, equipmentPanelItems[selectedItem].size.Y, .2f * delta * 120));
	}
}
