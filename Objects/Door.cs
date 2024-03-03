using Godot;
using System;

public partial class Door : BaseObject
{
	public bool isOpen = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GetChild<Label>(0).Text = isOpen?"/":"+";
	}
}
