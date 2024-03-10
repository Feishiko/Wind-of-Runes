using Godot;
using System;

public partial class Ending : Node2D
{
	private Controller controller;
	private Label label;
	private double timer;
	public override void _Ready()
	{
		controller = Controller.GetInstance();
		label = GetNode<Label>("Label");
		label.Text += controller.isWin ? $"{controller.player.name} the {controller.player.species}({controller.player.gender}) conquered the tower\n" : "You are dead\n";
		label.Text += $"Your strength is {controller.player.strength}\n";
		label.Text += $"Your agility is {controller.player.agility}\n";
		label.Text += $"Your toughness is {controller.player.toughness}\n";
		label.Text += $"Your intelligence is {controller.player.intelligence}\n";
		label.Text += $"Your AV is {controller.player.AV}\n";
		label.Text += $"Your DV is {controller.player.DV}\n";
		label.Text += $"Your time cost is {controller.player.time} turns\n";
		label.Text += $"Your arrived at {controller.currentFloor} floors\n";
		label.Text += controller.isWin ? $"Thanks for your playing!\n" : "";
		label.Text += "Press [ESC] to quit\n";
	}

	public override void _Process(double delta)
	{
		timer += delta * 120;
		if (timer > 10)
		{
			label.VisibleCharacters += 1;
			timer = 0;
		}
		if (Input.IsActionJustPressed("Cancel"))
		{
			GetTree().Quit();
		}
	}
}
