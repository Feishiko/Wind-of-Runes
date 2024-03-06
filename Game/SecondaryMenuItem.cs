using Godot;
using System;

public partial class SecondaryMenuItem : Node2D
{
	public string text = null;
	public Vector2 size;
	public override void _Ready()
	{
		GetNode<Label>("Label").Text = text;
		size = GetNode<Label>("Label").Size;
	}

	public override void _Process(double delta)
	{
	}
}
