using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	public bool Walkable;
	public Vector2 Position;
	public Tile parent;
	public bool HasTurret = false;

	public Tile(Vector2 position, bool walkable = true, Tile parent = null)
	{
		Position = position;
		Walkable = walkable;
		HasTurret = false;
		this.parent = parent;
	}
}

public class MapManager : MonoBehaviour {
	PoolManager pm;
	GameObject map;

	#region Map
	[SerializeField]
	Vector2 mapSize = new Vector2(10, 10);
	//Space between tiles
	[SerializeField]
	float tileOffset = 0.5f;
	Vector3 tileSize;

	//Logic map to use BFS
	public Tile[,] TileMap;

	#endregion

	#region Scenario
	[SerializeField]
	Vector2 spawnerPosition;
	[SerializeField]
	Vector2 crystalPosition;

	[SerializeField]
	GameObject spawnerPrefab;
	[SerializeField]
	GameObject crystalPrefab;

	[HideInInspector]
	public GameObject spawner;
	[HideInInspector]
	public GameObject crystal;

	#endregion
	
	void Awake()
	{
		map = new GameObject("Map");
		pm = GetComponent<PoolManager>();
		foreach (var prefab in pm.prefabs)
		{
			if (prefab.type == PoolManager.PrefabType.TILE)
			{
				tileSize = new Vector3(prefab.objectPrefab.transform.localScale.x, 
										prefab.objectPrefab.transform.localScale.y, 
										prefab.objectPrefab.transform.localScale.z);
			}
		}
		GenerateMap(mapSize);
	}
	void Start()
	{
		
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (crystalPosition.x > mapSize.x - 1)
			crystalPosition.x = mapSize.x - 1;
		if (crystalPosition.x < 0)
			crystalPosition.x = 0;
		if (crystalPosition.y > mapSize.y - 1)
			crystalPosition.y = mapSize.y - 1;
		if (crystalPosition.y < 0)
			crystalPosition.y = 0;

		if (spawnerPosition.x > mapSize.x - 1)
			spawnerPosition.x = mapSize.x - 1;
		if (spawnerPosition.x < 0)
			spawnerPosition.x = 0;
		if (spawnerPosition.y > mapSize.y - 1)
			spawnerPosition.y = mapSize.y - 1;
		if (spawnerPosition.y < 0)
			spawnerPosition.y = 0;
	}
#endif

	// Generates the map and sets Crystal and Spawner locations
	void GenerateMap(Vector2 size)
	{
		TileMap = new Tile[(int)mapSize.x, (int)mapSize.y];
		//Generating the grid
		for (int i = 0; i < mapSize.x; i++)
		{
			for (int j = 0; j < mapSize.y; j++)
			{
				GameObject go;
				go = pm.GetObjectByType(PoolManager.PrefabType.TILE);
				go.transform.position = new Vector3(((i - mapSize.x / 2) * (tileSize.x + tileOffset)) + (tileSize.x / 2  + tileOffset / 2), 
														0.0f, 
														(j - mapSize.y / 2) * (tileSize.z + tileOffset) + (tileSize.z / 2 + tileOffset / 2));
				go.transform.parent = map.transform;
				TileMap[i, j] = new Tile(new Vector2(i, j));
			}
		}

		//Setting crystal and spawner
		spawner = Instantiate(spawnerPrefab) as GameObject;
		crystal = Instantiate(crystalPrefab) as GameObject;

		spawner.transform.parent = map.transform;		
		crystal.transform.parent = map.transform;

		spawner.transform.localPosition = MapToWorldPosition(spawnerPosition);
		crystal.transform.localPosition = MapToWorldPosition(crystalPosition);

	}

	//Converts map coords to wolrd coords with origin as reference
	public Vector3 MapToWorldPosition(Vector2 tile)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (tileOffset * (mapSize.x-1)),
											(mapSize.y * tileSize.z) + (tileOffset * (mapSize.y - 1)));
		return new Vector3((-worldMapSize.x / 2)+(tileSize.x + tileOffset) * tile.x + tileSize.x / 2, 
							0,
							(worldMapSize.y / 2) - (tileSize.z + tileOffset) * tile.y - tileSize.z / 2);
	}
	//Converts world coords to map coords with origin as reference
	public Vector2 WorldToMapPosition(Vector3 worldPosition)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (tileOffset * (mapSize.x - 1)),
											(mapSize.y * tileSize.z) + (tileOffset * (mapSize.y - 1)));
		/*
		//I could't find a truncate method so I cast to int
		int x = (int)(worldPosition.x + (worldMapSize.x / 2)) / 2;
		int y = (int)Mathf.Abs((worldPosition.z - (worldMapSize.y / 2)) / 2);
		//Control about positions that are away from map. In this case I prefer to get an edge tile before to return some null value or error message;
		x = (int)Mathf.Clamp(x, 0, mapSize.x-1);
		y = (int)Mathf.Clamp(y, 0, mapSize.y-1);
		return new Vector2(x, y);
		*/

		float px = (worldPosition.x + (worldMapSize.x / 2)) / worldMapSize.x;
		float py = 1 - (worldPosition.z + (worldMapSize.y / 2)) / worldMapSize.y;

		px = Mathf.Clamp01(px);
		py = Mathf.Clamp01(py);

		int x = (int)(mapSize.x * px);
		int y = (int)(mapSize.y * py);

		return new Vector2(x, y);

	}

	private void OnDrawGizmosSelected()
	{
		if (TileMap != null)
		{
			foreach (var item in TileMap)
			{
				if (item.Walkable)
				{
					Gizmos.color = new Color(0, 1, 0, 0.5f);
					Gizmos.DrawCube(MapToWorldPosition(item.Position), Vector3.one * 2.1f);
				}
				else
				{
					Gizmos.color = new Color(1, 0, 0, 0.5f);
					Gizmos.DrawCube(MapToWorldPosition(item.Position), Vector3.one * 2.1f);
				}
			}

		}
	}

	public Vector2 GetWorldSize()
	{
		return new Vector2(mapSize.x * tileSize.x + (mapSize.x - 1) * tileOffset, mapSize.y * tileSize.z + (mapSize.y - 1) * tileOffset);
	}

}

