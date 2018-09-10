using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour {

	[SerializeField]
	Vector2 mapSize = new Vector2(10,10);
	[SerializeField]
	float offset = 0.5f;

	PoolManager pm;

	Vector3 tileSize;

	GameObject map;
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
		
	}
	void Start()
	{
		GenerateMap(mapSize);
	}

	void GenerateMap(Vector2 size)
	{
		for (int i = 0; i < mapSize.x; i++)
		{
			for (int j = 0; j < mapSize.y; j++)
			{
				GameObject go;
				go = pm.GetObjectByType(PoolManager.PrefabType.TILE);
				go.transform.position = new Vector3(((i - mapSize.x / 2) * (tileSize.x + offset)) + (tileSize.x / 2  + offset / 2), 
														0.0f, 
														(j - mapSize.y / 2) * (tileSize.z + offset) + (tileSize.z / 2 + offset / 2));
				go.transform.parent = map.transform;
			}
		}
	}
	//Converts map coords to wolrd coords
	Vector3 MapToWorldPosition(Vector2 tile)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (offset * (mapSize.x-1)),
											(mapSize.x * tileSize.z) + (offset * (mapSize.x - 1)));
		return new Vector3((-worldMapSize.x / 2)+(tileSize.x + offset) * tile.x + tileSize.x / 2, 
							0,
							(worldMapSize.y / 2) - (tileSize.z + offset) * tile.y - tileSize.z / 2);
	}
	//Converts world coords to map coords
	Vector2 WorldToMapPosition(Vector3 worldPosition)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (offset * (mapSize.x - 1)),
											(mapSize.x * tileSize.z) + (offset * (mapSize.x - 1)));

		//I could't find a truncate method so I cast to int
		int x = (int)(worldPosition.x + (worldMapSize.x / 2)) / 2;
		int y = (int)Math.Abs((worldPosition.z - (worldMapSize.y / 2)) / 2);
		//Control about positions that are away from map. In this case I prefer to get an edge tile before to return some null value or error message;
		x = (int)Mathf.Clamp(x, 0, mapSize.x-1);
		y = (int)Mathf.Clamp(y, 0, mapSize.y-1);
		return new Vector2(x, y);
	}

}
