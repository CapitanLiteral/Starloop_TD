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
		//Vector2 pos = MapToWorldPosition(new Vector2(0, 0));
		GameObject cube;// = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//cube.transform.position = new Vector3(pos.x, 0, pos.y);

		/*cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = MapToWorldPosition(new Vector2(0, 0));
		
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = MapToWorldPosition(new Vector2(3, 0));

		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = MapToWorldPosition(new Vector2(3, 3));

		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = MapToWorldPosition(new Vector2(0, 3));

		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = MapToWorldPosition(new Vector2(2, 2));*/

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
				Debug.Log("i: " + i + " j: " + j);
				Debug.Log("Tile position: " + go.transform.position);
				go.transform.parent = map.transform;
			}
		}
	}
	
	Vector3 MapToWorldPosition(Vector2 tile)
	{
		Vector2 worldMapSize = new Vector2((mapSize.x * tileSize.x) + (offset * (mapSize.x-1)),
											(mapSize.x * tileSize.z) + (offset * (mapSize.x - 1)));
		Debug.Log("World Size:" + worldMapSize);
		return new Vector3((-worldMapSize.x / 2)+(tileSize.x + offset) * tile.x + tileSize.x / 2, 
							0,
							(worldMapSize.y / 2) - (tileSize.z + offset) * tile.y - tileSize.z / 2);
	}
	Vector2 WorldToMapPosition(Vector2 worldPosition)
	{
		return new Vector2(0, 0);
	}

}
