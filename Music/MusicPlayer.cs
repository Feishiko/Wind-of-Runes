using Godot;
using System;

public partial class MusicPlayer : Node2D
{
	private AudioStreamPlayer beneathTheTower;
	private AudioStreamPlayer windUnderneath;
	private AudioStreamPlayer forest;
	private AudioStreamPlayer strangeCrystal;
	private AudioStreamPlayer sharpShard;
	private AudioStreamPlayer currentPlaying;
	private double delta;
	public override void _Ready()
	{
		beneathTheTower = GetNode<AudioStreamPlayer>("BeneathTheTower");
		windUnderneath = GetNode<AudioStreamPlayer>("WindUnderneath");
		forest = GetNode<AudioStreamPlayer>("Forest");
		strangeCrystal = GetNode<AudioStreamPlayer>("StrangeCrystal");
		sharpShard = GetNode<AudioStreamPlayer>("SharpShard");
	}

	public override void _Process(double delta)
	{
		this.delta = delta;
	}

	public void Play(string name)
	{
		var playing = beneathTheTower;
		switch (name)
		{
			case "BeneathTheTower":
				{
					playing = beneathTheTower;
				}
				break;
			case "WindUnderneath":
				{
					playing = windUnderneath;
				}
				break;
			case "Forest":
				{
					playing = forest;
				}
				break;
			case "StrangeCrystal":
				{
					playing = strangeCrystal;
				}
				break;
			case "SharpShard":
				{
					playing = sharpShard;
				}
				break;
			case "Null":
				{
					if (currentPlaying != null)
					{
						currentPlaying.Stop();
						currentPlaying = null;
					}
					return;
				}
		}
		if (playing == currentPlaying)
		{
			return;
		}
		playing.Play();
		if (currentPlaying != null)
		{
			currentPlaying.Stop();
		}
		currentPlaying = playing;
	}
}
