using Godot;
using System;

public class JsonMicro : JsonFood
{
	public string rune { get; set; }
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
}
