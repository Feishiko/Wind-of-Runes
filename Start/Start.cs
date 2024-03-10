using Godot;
using System;

public partial class Start : Node2D
{
	[Export]
	private PackedScene packedMenu;
	private Label label;
	private double timer = 0;
	private bool alpha = true;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	public override void _Process(double delta)
	{
		timer += delta * 120;
		if (timer >= 240)
		{
			alpha = false;
		}
		if (timer >= 400)
		{
			GetTree().ChangeSceneToPacked(packedMenu);
		}
		if (timer >= 60)
		{
			label.Modulate = new Color(1, 1, 1, (float)Mathf.Lerp(label.Modulate.A, alpha ? 1 : 0, .1f * 120 * delta));
		}
	}
}
