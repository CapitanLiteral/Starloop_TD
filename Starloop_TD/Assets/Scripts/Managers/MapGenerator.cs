using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour {

	[SerializeField]
	Vector2 mapSize = new Vector2(10,10);
	[SerializeField]
	float offset = 0.5f;

	GameObject map;
	void Awake()
	{
		map = new GameObject("Map");
	}
	void Start()
	{
		GenerateMap(mapSize);
	}

	void GenerateMap(Vector2 size)
	{
		PoolManager pm = this.GetComponent<PoolManager>();
		

		for (int i = 0; i < mapSize.x; i++)
		{
			for (int j = 0; j < mapSize.y; j++)
			{
				GameObject go = pm.GetObjectByType(PoolManager.PrefabType.TILE);
				Vector3 tileSize = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
				go.transform.position = new Vector3((i - mapSize.x / 2) * (tileSize.x + offset), 0.0f, (j - mapSize.y / 2) * (tileSize.z + offset));
				go.transform.parent = map.transform;
			}
		}

		

	}
	
}
