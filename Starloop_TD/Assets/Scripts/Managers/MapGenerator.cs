using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour {
	PoolManager pm;
	GameObject map;

	#region Map
	[SerializeField]
	Vector2 mapSize = new Vector2(10, 10);
	//Space between tiles
	[SerializeField]
	float tileOffset = 0.5f;

	Vector3 tileSize;
	#endregion

	#region Scenario
	[SerializeField]
	Vector2Int spawnerPosition;
	[SerializeField]
	Vector2Int crystalPosition;

	[SerializeField]
	GameObject spawnerPrefab;
	[SerializeField]
	GameObject crystalPrefab;

	GameObject spawner;
	GameObject crystal;

	#endregion


	BFS bfs;
	private List<BFSnode> path;
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
		//asdasdasdasdasdasdasd
		bfs = new BFS(new Vector2Int(4, 4));

		Vector2Int start = new Vector2Int(0, 0);
		Vector2Int end = new Vector2Int(3, 3);

		bfs.map[1, 0].walkable = false;
		bfs.map[1, 1].walkable = false;
		bfs.map[1, 2].walkable = false;
		bfs.map[1, 3].walkable = false;

		path = bfs.FindPath(start, end);

		int i = 0;
		foreach (var bfSnode in path)
		{
			Debug.Log("I: " + i + " -> " + bfSnode.position);
			i++;
		}
		//asdasdasdasdasdasdasd
	}
	void Start()
	{
		GenerateMap(mapSize);
	}
	// Generates the map and sets Crystal and Spawner locations
	void GenerateMap(Vector2 size)
	{
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

	//Converts map coords to wolrd coords
	Vector3 MapToWorldPosition(Vector2 tile)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (tileOffset * (mapSize.x-1)),
											(mapSize.x * tileSize.z) + (tileOffset * (mapSize.x - 1)));
		return new Vector3((-worldMapSize.x / 2)+(tileSize.x + tileOffset) * tile.x + tileSize.x / 2, 
							0,
							(worldMapSize.y / 2) - (tileSize.z + tileOffset) * tile.y - tileSize.z / 2);
	}
	//Converts world coords to map coords
	Vector2 WorldToMapPosition(Vector3 worldPosition)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (tileOffset * (mapSize.x - 1)),
											(mapSize.x * tileSize.z) + (tileOffset * (mapSize.x - 1)));

		//I could't find a truncate method so I cast to int
		int x = (int)(worldPosition.x + (worldMapSize.x / 2)) / 2;
		int y = (int)Mathf.Abs((worldPosition.z - (worldMapSize.y / 2)) / 2);
		//Control about positions that are away from map. In this case I prefer to get an edge tile before to return some null value or error message;
		x = (int)Mathf.Clamp(x, 0, mapSize.x-1);
		y = (int)Mathf.Clamp(y, 0, mapSize.y-1);
		return new Vector2(x, y);
	}



	void OnDrawGizmos()
	{
		// Draw a semitransparent blue cube at the transforms position
		if (path != null)
			foreach (var bfSnode in path)
			{
				Gizmos.color = new Color(1, 0, 0, 0.5f);
				Gizmos.DrawCube(MapToWorldPosition(bfSnode.position), new Vector3(1, 1, 1));
			}
	}


}
