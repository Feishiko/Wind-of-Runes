using Godot;
using System;

public partial class CharacterMenuItem : Node2D
{
	public string text;
	public Vector2 size;
	public override void _Ready()
	{
		GetNode<Label>("Label").Text = text;
	}

	public override void _Process(double delta)
	{
		size = GetNode<Label>("Label").Size;
	}
}
