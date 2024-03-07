using Godot;
using System;

public partial class DamageNumber : Node2D
{
	private double damageTimer = 0;
	public int damage = 0;
	public Vector2 endPos;
	public override void _Ready()
	{
		GetNode<Label>("Label").Text = $"-{damage}";
		endPos = new Vector2(Position.X, Position.Y - 10);
	}

	public override void _Process(double delta)
	{
		Position = new Vector2((float)Mathf.Lerp(Position.X, endPos.X, .2f * 120 * delta),
		(float)Mathf.Lerp(Position.Y, endPos.Y, .2f * 120 * delta));
		damageTimer += delta * 120;
		if (damageTimer > 60)
		{
			QueueFree();
		}
	}
}
