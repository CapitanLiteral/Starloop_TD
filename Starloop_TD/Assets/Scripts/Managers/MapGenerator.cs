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
	
}
