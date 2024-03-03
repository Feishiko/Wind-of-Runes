using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public static class Behavior
{
	public static Vector2I BFS(int startPosX, int startPosY, int endPosX, int endPosY, Game game)
	{
		int[,] paths = new int[game.LevelWidthGet(), game.LevelHeightGet()];
		Stack<Vector2I> stackPos = new Stack<Vector2I>();
		Vector2I currentPos = new Vector2I();
		paths[endPosX, endPosY] = 10; // Start Diffuse
		stackPos.Push(new Vector2I(endPosX, endPosY));
		var Avoid = (Vector2I pos) =>
		{
			// Wall
			if (game.level[pos.X, pos.Y, 0] is Wall)
			{
				return false;
			}

			// Closed Door
			if (game.level[pos.X, pos.Y, 1] is Door door)
			{
				return door.isOpen;
			}

			// Enemy
			// if (game.level[pos.X, pos.Y, 2] is Enemy)
			// {
			// 	return false;
			// }

			// Player
			// if (game.level[pos.X, pos.Y, 2] is Player)
			// {
			// 	return false;
			// }

			return true;
		};
		// 8-dir Diffuse
		for (int i = 0; i < 100; i++)
		{
			if (stackPos.TryPop(out currentPos))
			{
				var CheckAndSet = (int posX, int posY) =>
				{
					if (paths[posX, posY] == 0 && Avoid(new Vector2I(posX, posY)))
					{
						paths[posX, posY] = paths[currentPos.X, currentPos.Y] + 1;
						stackPos.Push(new Vector2I(posX, posY));
					}
				};
				// Left Right Up Down
				CheckAndSet(currentPos.X - 1, currentPos.Y);
				CheckAndSet(currentPos.X + 1, currentPos.Y);
				CheckAndSet(currentPos.X, currentPos.Y - 1);
				CheckAndSet(currentPos.X, currentPos.Y + 1);
				// Corner
				CheckAndSet(currentPos.X - 1, currentPos.Y - 1);
				CheckAndSet(currentPos.X + 1, currentPos.Y - 1);
				CheckAndSet(currentPos.X - 1, currentPos.Y + 1);
				CheckAndSet(currentPos.X + 1, currentPos.Y + 1);
			}
			if (paths[startPosX, startPosY] != 0)
			{
				break;
			}
		}
		// Find the road by check the number
		for (var i = -1; i <= 1; i++)
		{
			for (var j = -1; j <= 1; j++)
			{
				if (paths[startPosX + i, startPosY + j] == paths[startPosX, startPosY] - 1)
				{
					return new Vector2I(startPosX + i, startPosY + j);
				}
			}
		}
		return new Vector2I(startPosX, startPosY);
	}
}