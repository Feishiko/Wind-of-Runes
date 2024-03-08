using Godot;
using System;

public partial class Log : Node2D
{
	public string text;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GetNode<Label>("Label").Text = text;
	}
}
