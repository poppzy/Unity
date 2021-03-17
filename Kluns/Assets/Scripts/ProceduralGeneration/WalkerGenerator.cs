using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerGenerator : TileMapGeneratorTemplate
{
	public int steps = 1000;
	Vector3Int walkerPos;

	public override void InitStartPos()
	{
		base.InitStartPos();
		walkerPos = startPos;
	}

	public override int[,] GenerateTilePositions(int[,] oldMap)
	{
		int[,] newMap = oldMap;
		int x = walkerPos.x;
		int y = walkerPos.y;

		for (int i = 0; i < steps; i++)
		{
			var dir = Random.Range(0, 4);

			switch (dir)
			{
				case 0:
					x++;
					break;
				case 1:
					x--;
					break;
				case 2:
					y++;
					break;
				default:
					y--;
					break;
			}
			x = Mathf.Clamp(x, 0, width - 1);
			y = Mathf.Clamp(y, 0, height - 1);

			newMap[x, y] = 1;
		}

		return newMap;
	}
}
