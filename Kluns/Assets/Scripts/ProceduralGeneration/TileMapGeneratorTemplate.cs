using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGeneratorTemplate : MonoBehaviour
{
	public Vector3Int startSize = new Vector3Int(10, 10, 0);
	public RuleTile tile;

	internal Tilemap map;
	internal int[,] terrainMap;
	internal int width;
	internal int height;
	internal Vector3Int startPos;

	private void Start()
	{
		map = GetComponent<Tilemap>();
		Generate();
	}

	public void Generate()
	{
		ClearMap(false);
		width = startSize.x;
		height = startSize.y;

		//Set up our terrain map
		if (terrainMap == null)
		{
			terrainMap = new int[width, height];
			InitStartPos();
		}

		//Call our generation function
		terrainMap = GenerateTilePositions(terrainMap);

		//take our result and update the actual tiles.
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (terrainMap[x, y] == 1)
					map.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), tile);
			}
		}
	}

	public virtual void InitStartPos()
	{
		//Generate starting state
		startPos = new Vector3Int(Random.Range(0, width), Random.Range(0, height), 0);

		startPos = ClampVector(startPos, new Vector2Int(1, 1));

		//Surround the start pos with tiles.
		terrainMap[startPos.x, startPos.y] = 1;
		terrainMap[startPos.x + 1, startPos.y] = 1;
		terrainMap[startPos.x - 1, startPos.y] = 1;
		terrainMap[startPos.x, startPos.y + 1] = 1;
		terrainMap[startPos.x, startPos.y - 1] = 1;
		terrainMap[startPos.x + 1, startPos.y + 1] = 1;
		terrainMap[startPos.x - 1, startPos.y + 1] = 1;
		terrainMap[startPos.x + 1, startPos.y - 1] = 1;
		terrainMap[startPos.x - 1, startPos.y - 1] = 1;

		//Get the world position of our start tile position.
		GameObject.FindGameObjectWithTag("Player").transform.position = map.CellToWorld(new Vector3Int(-startPos.x + width / 2, -startPos.y + height / 2, 0));
	}

	//Makes sure we are not to close to the edge of our matrix
	internal Vector3Int ClampVector(Vector3Int value, Vector2Int limit = new Vector2Int())
	{
		return new Vector3Int(Mathf.Clamp(value.x, limit.x, width - limit.x - 1),
			Mathf.Clamp(value.y, limit.y, height - limit.y - 1), 0);
	}

	public virtual int[,] GenerateTilePositions(int[,] oldMap)
	{
		int[,] newMap = oldMap;

		//TODO: Generate the world

		return newMap;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
			Generate();

		if (Input.GetKeyDown(KeyCode.H))
			ClearMap(true);
	}

	public void ClearMap(bool complete)
	{
		//Clears tilemap
		map.ClearAllTiles();
		if (complete)
		{
			//Clears our internal terrainMap
			terrainMap = null;
		}
	}
}
