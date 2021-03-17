using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : TileMapGeneratorTemplate
{
	public int numberOfRooms = 3;

	public int roomMinHeight = 3;
	public int roomMaxHeight = 5;
	public int roomMinWidth = 3;
	public int roomMaxWidth = 10;

	public int corridorWidth = 3;

	int[,] newMap;

	public override void InitStartPos()
	{
		base.InitStartPos();
		startPos = ClampVector(startPos, new Vector2Int(roomMaxWidth / 2, roomMaxHeight / 2));	
	}

	public override int[,] GenerateTilePositions(int[,] oldMap)
	{
		newMap = oldMap;

		int newRoomWidth = Random.Range(roomMinWidth, roomMaxWidth);
		int newRoomHeight = Random.Range(roomMinHeight, roomMaxHeight);

		//Start room
		GenerateRoom(startPos, newRoomWidth, newRoomHeight);

		int count;
		do
		{
			int tries = 0;

			for (int i = 0; i < numberOfRooms; i++)
			{
				var oldPos = startPos;

				//Create hallway to new point.
				if (Random.Range(0f, 1f) > 0.5f)
				{
					do
					{
						startPos.x += (Random.Range(0, 2) * 2 - 1) * Random.Range(newRoomWidth + 1, newRoomWidth * 2);

						startPos = new Vector3Int(Mathf.Clamp(startPos.x, roomMaxWidth / 2, width - roomMaxWidth / 2),
						Mathf.Clamp(startPos.y, roomMaxHeight / 2, height - roomMaxHeight / 2), 0);

						tries++;
					}
					while (newMap[startPos.x, startPos.y] == 1 && tries < 100);

					GenerateRoom((oldPos + startPos) / 2, Mathf.Abs(Mathf.Abs(startPos.x) - Mathf.Abs(oldPos.x)), corridorWidth);
				}
				else
				{
					do
					{
						startPos.y += (Random.Range(0, 2) * 2 - 1) * Random.Range(newRoomHeight + 1, newRoomHeight * 2);

						startPos = new Vector3Int(Mathf.Clamp(startPos.x, roomMaxWidth / 2, width - roomMaxWidth / 2),
						Mathf.Clamp(startPos.y, roomMaxHeight / 2, height - roomMaxHeight / 2), 0);

						tries++;
					}
					while (newMap[startPos.x, startPos.y] == 1 && tries < 100);

					GenerateRoom((oldPos + startPos) / 2, corridorWidth, Mathf.Abs(Mathf.Abs(startPos.y) - Mathf.Abs(oldPos.y)));
				}

				GenerateRoom(startPos, Random.Range(roomMinWidth, roomMaxWidth), Random.Range(roomMinHeight, roomMaxHeight));
			}

			count = 0;
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (newMap[x, y] == 1)
					{
						count++;
					}
				}
			}
			if (tries >= 100)
			{
				count = 1500;
			}
		}
		while (count < 1500);

		return newMap;
	}

	private void GenerateRoom(Vector3Int startPos, int roomWidth, int roomHeight)
	{
		int x = startPos.x;
		int y = startPos.y;

		var noiseOffset = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));

		//Debug.Log("low: " + Mathf.CeilToInt(-roomWidth / 2) + " hi: " + Mathf.CeilToInt((float)roomWidth / 2));

		for (int i = Mathf.FloorToInt(-roomWidth/2); i < Mathf.CeilToInt((float)roomWidth /2); i++)
		{
			float val = Mathf.PerlinNoise(i * 0.05f + noiseOffset.x, i * 0.05f + noiseOffset.y);
			int xOffset = Mathf.RoundToInt(0 + (val - 0) * (10 - 0) / (1 - 0)) -4;

			for (int j = Mathf.FloorToInt(-roomHeight/2); j < Mathf.CeilToInt((float)roomHeight / 2); j++)
			{
				var newPos = new Vector3Int(x + i, y + j - xOffset, 0);
				newPos = ClampVector(newPos);

				newMap[newPos.x, newPos.y] = 1;
			}
		}
	}
}
