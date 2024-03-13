using Godot;
using System;

public class JsonMicro : JsonFood
{
	public void CopiedFrom(Micro micro)
	{
		rune = micro.rune;
		pickUpType = "Micro";
	}
}

public partial class Micro : Food
{
	public string rune;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void MicroReceivedFrom(JsonPickUp jsonPickUp)
	{
		nutrition = jsonPickUp.nutrition;
		rune = jsonPickUp.rune;
	}
}
