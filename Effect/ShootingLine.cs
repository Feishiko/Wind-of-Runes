using Godot;
using System;

public partial class ShootingLine : Node2D
{
	private double timer = 0;
	private Line2D line2D;
	public Vector2 originPos;
	public Vector2 targetPos;
	public override void _Ready()
	{
		line2D = GetNode<Line2D>("Line2D");
		line2D.AddPoint(originPos);
		line2D.AddPoint(targetPos);
	}

	public override void _Process(double delta)
	{
		// if (timer < 30)
		// {
		// 	line2D.Width = (float)Mathf.Lerp(line2D.Width, 2, .2f * delta * 120);
		// }
		// else
		// {
		line2D.Width = (float)Mathf.Lerp(line2D.Width, 0, .2f * delta * 120);
		// }

		timer += delta * 120;
		if (timer >= 120)
		{
			QueueFree();
		}
	}
}
