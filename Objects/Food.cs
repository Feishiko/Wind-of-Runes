using Godot;
using System;

public class JsonFood : JsonPickUp
{
	public void CopiedFrom(Food food)
	{
		name = food.name;
		description = food.description;
		weight = food.weight;
		nutrition = food.nutrition;
		if (food is Corpse)
		{
			type = "Corpse";
		}
		if (food is Bread)
		{
			type = "Bread";
		}
		pickUpType = "Food";
	}
}

public partial class Food : PickUp
{
	public int nutrition { get; set; }
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void ReceivedFrom(JsonPickUp jsonPickUp)
	{
		nutrition = jsonPickUp.nutrition;
	}
}
