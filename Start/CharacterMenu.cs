using Godot;
using System;

public partial class CharacterMenu : Node2D
{
	[Export]
	private PackedScene packedCharacterMenuItem;
	private int state = 0; // 0 -> Name, 1 -> Species, 2 -> Gender
	private Label name;
	private Controller controller;
	private CharacterMenuItem[] speciesItems = new CharacterMenuItem[5];
	private CharacterMenuItem[] genderItems = new CharacterMenuItem[2];
	private string[] species = new string[] { "Human", "Kobold", "Avian", "Avali", "Robot" };
	private string[] gender = new string[] { "Male", "Female" };
	private Label speciesLabel;
	private ColorRect select;
	private int currentSpecies = 0;
	private int currentGender = 0;
	private Menu menu;
	public override void _Ready()
	{
		name = GetNode<Label>("InputName");
		speciesLabel = GetNode<Label>("Species");
		select = GetNode<ColorRect>("Select");
		menu = GetParent<Menu>();
		controller = Controller.GetInstance();
	}

	public override void _Process(double delta)
	{
		if (state == 0)
		{
			select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, name.Position.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Position.Y, name.Position.Y + 2, .2f * delta * 120));
			select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, name.Size.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Size.Y, name.Size.Y - 2, .2f * delta * 120));
		}
		if (state == 1)
		{
			select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, speciesItems[currentSpecies].Position.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Position.Y, speciesItems[currentSpecies].Position.Y + 2, .2f * delta * 120));
			select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, speciesItems[currentSpecies].size.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Size.Y, speciesItems[currentSpecies].size.Y - 2, .2f * delta * 120));
			if (Input.IsActionJustPressed("Up") && currentSpecies > 0)
			{
				currentSpecies -= 1;
			}
			if (Input.IsActionJustPressed("Down") && currentSpecies < 4)
			{
				currentSpecies += 1;
			}
		}
		if (state == 2)
		{
			select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, genderItems[currentGender].Position.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Position.Y, genderItems[currentGender].Position.Y + 2, .2f * delta * 120));
			select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, genderItems[currentGender].size.X, .2f * delta * 120),
			(float)Mathf.Lerp(select.Size.Y, genderItems[currentGender].size.Y - 2, .2f * delta * 120));
			if (Input.IsActionJustPressed("Up") && currentGender > 0)
			{
				currentGender -= 1;
			}
			if (Input.IsActionJustPressed("Down") && currentGender < 1)
			{
				currentGender += 1;
			}
		}
		if (Input.IsActionJustPressed("Confirm"))
		{
			if (state == 0)
			{
				state = 1;
				controller.playerName = name.Text.Substring(1, name.Text.Length - 1);
				for (var iter = 0; iter < 5; iter++)
				{
					speciesItems[iter] = packedCharacterMenuItem.Instantiate<CharacterMenuItem>();
					speciesItems[iter].text = species[iter];
					speciesItems[iter].Position = new Vector2(5, 90 + iter * 25);
					AddChild(speciesItems[iter]);
				}
			}
			else if (state == 1)
			{
				controller.playerSpecies = species[currentSpecies];
				state = 2;
				for (var iter = 0; iter < 2; iter++)
				{
					genderItems[iter] = packedCharacterMenuItem.Instantiate<CharacterMenuItem>();
					genderItems[iter].text = gender[iter];
					genderItems[iter].Position = new Vector2(5, 245 + iter * 25);
					AddChild(genderItems[iter]);
				}
			}
			else if (state == 2)
			{
				controller.playerGender = gender[currentGender];
				state = 3;
				menu.isStartGame = true;
			}
		}
		if (Input.IsActionJustPressed("Cancel"))
		{
			if (state == 2)
			{
				state = 1;
				for (var iter = 0; iter < 2; iter++)
				{
					genderItems[iter].QueueFree();
				}
			}
			else if (state == 1)
			{
				state = 0;
				for (var iter = 0; iter < 5; iter++)
				{
					speciesItems[iter].QueueFree();
				}
			}
			else if (state == 0)
			{
				menu.isCharacterMenu = false;
			}
		}
		GetNode<Label>("Gender").Visible = state >= 2;
		GetNode<ColorRect>("GenderBack").Visible = state >= 2;
		GetNode<ColorRect>("SpeciesBack").Visible = state >= 1;
		speciesLabel.Visible = state >= 1;
		GetNode<Label>("SpeciesDes").Visible = state >= 1;
		GetNode<ColorRect>("SpeciesDesBack").Visible = state >= 1;

		switch(species[currentSpecies])
		{
			case "Human":{
				GetNode<Label>("SpeciesDes").Text = "Humans is a good learner, they learn more stuffs than other species at once.\nBut they are not good at eating something stuffs they don't want to eat.";
			}break;
			case "Kobold":{
				GetNode<Label>("SpeciesDes").Text = "Kobold stolen a laser gun from avali's spaceship, and turns to this tower try to climb it.\nWhat is their goal? We don't know.";
			}break;
			case "Avian":{
				GetNode<Label>("SpeciesDes").Text = "Avians is a nature shooter, they always bring a pistol with themselves.\nAlso like eating worms.";
			}break;
			case "Avali":{
				GetNode<Label>("SpeciesDes").Text = "Avalis is going to conquer your planet, but before this they will conquer this tower.\nThey bring a shrink gun with themselves, what will they do...";
			}break;
			case "Robot":{
				GetNode<Label>("SpeciesDes").Text = "Robots are curious creature, they learn anything without eating food.\nBut their battery are still hungry.";
			}break;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsPressed() && state == 0)
			{
				var key = eventKey.AsTextKeyLabel();
				if (key.Length == 1 && name.Text.Length < 9)
				{
					name.Text += key;
				}
				if (key == "Space" && name.Text.Length < 9)
				{
					name.Text += " ";
				}
				if (key == "Backspace")
				{
					name.Text = name.Text.Substring(0, Mathf.Max(name.Text.Length - 1, 1));
				}
			}
		}
	}
}
