using Godot;
using System;

public partial class Enemy : BaseObject
{
	public string name { get; set; } = RandomName.RandomCharacterName();
	public int hitPoint { get; set; }
	public int level { get; set; }
	public string species { get; set; }
	public string gender { get; set; } = RandomName.RandomGender();
	public int strength { get; set; }
	public int agility { get; set; }
	public int intelligence { get; set; }
	public int toughness { get; set; }
	public int AV { get; set; }
	public int DV { get; set; }
	public int nutrition { get; set; }

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public virtual void TurnPassed()
	{
		GD.Print("SayHI");
	}
}
