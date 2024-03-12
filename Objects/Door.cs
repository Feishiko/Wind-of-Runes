using Godot;
using System;
public class JsonDoor : JsonBaseObject
{
	public bool isOpen { get; set; }
}
public partial class Door : BaseObject
{
	[Export]
	public Texture2D closedDoor;
	[Export]
	public Texture2D openedDoor;
	public bool isOpen = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GetChild<Sprite2D>(0).Texture = isOpen ? openedDoor : closedDoor;
	}

	public void DoorCopy(Door door)
	{
		isOpen = door.isOpen;
	}
}
