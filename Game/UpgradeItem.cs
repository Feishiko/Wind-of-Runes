using Godot;
using System;

public partial class UpgradeItem : Node2D
{
	public string text;
	public Vector2 size;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GetNode<Label>("Label").Text = text;
		size = GetNode<Label>("Label").Size;
	}
}
