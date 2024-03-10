using Godot;
using System;

public partial class Menu : Node2D
{
	[Export]
	private PackedScene packedStartMenuItem;
	[Export]
	private PackedScene packedGame;
	[Export]
	private PackedScene packedCharacterMenu;
	private ColorRect colorRect;
	private bool black = false;
	private string[] menuNames = new string[] { "Continue(Unsupported)", "New Game", "Credit", "Quit" };
	private StartMenuItem[] startMenuItems = new StartMenuItem[4];
	private ColorRect select;
	private int selectIndex = 1;
	private Sprite2D back;
	private Vector2 skewPos;
	private Vector2 afterSkewPos;
	private double skewTimer = 0;
	private Controller controller;
	public bool isCharacterMenu = false;
	private CharacterMenu characterMenu;
	private bool isCredit = false;
	public bool isStartGame = false;
	private double startGameTimer = 0;
	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		select = GetNode<ColorRect>("Select");
		back = GetNode<Sprite2D>("Back");
		skewPos = back.Position;
		controller = Controller.GetInstance();
		controller.isAnimation = true;
		GetNode<Label>("Words").Text = RandomName.RandomIntro();
		for (var iter = 0; iter < 4; iter++)
		{
			startMenuItems[iter] = packedStartMenuItem.Instantiate<StartMenuItem>();
			startMenuItems[iter].text = menuNames[iter];
			startMenuItems[iter].Position = new Vector2(200, 160 + iter * 30);
			AddChild(startMenuItems[iter]);
		}
	}

	public override void _Process(double delta)
	{
		colorRect.Modulate = new Color(1, 1, 1, (float)Mathf.Lerp(colorRect.Modulate.A, black ? 1f : 0f, .01f * delta * 120));
		select.Position = new Vector2((float)Mathf.Lerp(select.Position.X, startMenuItems[selectIndex].Position.X - startMenuItems[selectIndex].size.X / 2, .2f * delta * 120),
		(float)Mathf.Lerp(select.Position.Y, startMenuItems[selectIndex].Position.Y - startMenuItems[selectIndex].size.Y / 2, .2f * delta * 120));
		select.Size = new Vector2((float)Mathf.Lerp(select.Size.X, startMenuItems[selectIndex].size.X, .2f * delta * 120),
		(float)Mathf.Lerp(select.Size.Y, startMenuItems[selectIndex].size.Y, .2f * delta * 120));
		if (!isCharacterMenu && !isCredit && !isStartGame)
		{
			// Select
			if (selectIndex > 0 && Input.IsActionJustPressed("Up"))
			{
				selectIndex -= 1;
			}
			if (selectIndex < 3 && Input.IsActionJustPressed("Down"))
			{
				selectIndex += 1;
			}
			// Confirm
			if (Input.IsActionJustPressed("Confirm"))
			{
				switch (startMenuItems[selectIndex].text)
				{
					case "Continue":; break;
					case "New Game":
						{
							isCharacterMenu = true;
						}; break;
					case "Credit":
						{
							isCredit = true;
						}; break;
					case "Quit":
						{
							GetTree().Quit();
						}; break;
				}
			}
		}


		// Skew
		skewTimer += delta * 120;
		if (skewTimer >= 200)
		{
			var random = new Random();
			afterSkewPos = new Vector2(random.Next(-4, 4), random.Next(-4, 4));
			skewTimer = 0;
		}
		back.Position = new Vector2((float)Mathf.Lerp(back.Position.X, skewPos.X + afterSkewPos.X, .01f * delta * 120),
		(float)Mathf.Lerp(back.Position.Y, skewPos.Y + afterSkewPos.Y, .01f * delta * 120));



		// Character Menu
		if (isCharacterMenu)
		{
			if (characterMenu == null)
			{
				characterMenu = packedCharacterMenu.Instantiate<CharacterMenu>();
				AddChild(characterMenu);
			}
		}
		else
		{
			if (characterMenu != null)
			{
				characterMenu.QueueFree();
				characterMenu = null;
			}
		}

		for (var iter = 0; iter < 4; iter++)
		{
			startMenuItems[iter].Visible = !isCharacterMenu;
		}
		select.Visible = !isCharacterMenu;
		GetNode<Label>("Label").Visible = !isCharacterMenu;
		GetNode<ColorRect>("ColorRect2").Visible = !isCharacterMenu;

		// Credit
		GetNode<Label>("Credit").Visible = isCredit;
		GetNode<ColorRect>("CreditBack").Visible = isCredit;

		if (Input.IsActionJustPressed("Cancel"))
		{
			isCredit = false;
		}

		black = isStartGame;

		// Start Game
		startGameTimer += isStartGame ? delta * 120 : 0;

		if (isStartGame)
		{
			GetNode<AudioStreamPlayer>("Music").Stop();
		}

		if (startGameTimer >= 420)
		{
			GetNode<Label>("Words").Modulate = new Color(1, 1, 1, startGameTimer < 700 ? 1 : 0);
		}

		if (startGameTimer >= 820)
		{
			GetTree().ChangeSceneToPacked(packedGame);
		}
	}
}
