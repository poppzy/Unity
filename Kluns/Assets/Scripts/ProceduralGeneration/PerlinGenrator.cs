using UnityEngine;

public class PerlinGenrator : TileMapGeneratorTemplate
{
	public override int[,] GenerateTilePositions(int[,] oldMap)
	{
		int[,] newMap = oldMap;

		float zoom = 0.2f; // play with this to zoom into the noise field

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector2 pos = zoom * (new Vector2(x, y));
				float noise = Mathf.PerlinNoise(pos.x, pos.y);

				if (noise > 0.5f)
					newMap[x, y] = 0;
				else
					newMap[x, y] = 1;
			}
		}

		return newMap;
	}
}
