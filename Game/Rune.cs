using Godot;
using System;

public partial class Rune : Node2D
{
	[Export]
	private Texture2D textureEmptyRune;
	[Export]
	private Texture2D textureGearRune;
	[Export]
	private Texture2D textureLeafRune;
	[Export]
	private Texture2D textureFireRune;
	[Export]
	private Texture2D textureWaterRune;
	[Export]
	private Texture2D textureElectricRune;
	public String rune = null; // Null Water Fire Electric Leaf Gear
	private Sprite2D sprite;
	private Texture2D previousTexture;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		previousTexture = null;
	}

	public override void _Process(double delta)
	{
		Scale = new Vector2((float)Mathf.Lerp(Scale.X, 1, .2f * delta * 120), (float)Mathf.Lerp(Scale.Y, 1, .2f * delta * 120));
		switch (rune)
		{
			case "Water": sprite.Texture = textureWaterRune; break;
			case "Fire": sprite.Texture = textureFireRune; break;
			case "Electric": sprite.Texture = textureElectricRune; break;
			case "Leaf": sprite.Texture = textureLeafRune; break;
			case "Gear": sprite.Texture = textureGearRune; break;
			default: sprite.Texture = textureEmptyRune; break;
		}
		if (previousTexture != sprite.Texture)
		{
			previousTexture = sprite.Texture;
			Scale = Vector2.Zero;
		}
	}
}
