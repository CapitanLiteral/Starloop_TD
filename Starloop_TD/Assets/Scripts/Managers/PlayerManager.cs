using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	PoolManager Pool;
	MapManager Map;
	SpawnManager Spawner;

	// Use this for initialization
	void Start ()
	{
		Pool = FindObjectOfType<PoolManager>();
		Map = FindObjectOfType<MapManager>();
		Spawner = FindObjectOfType<SpawnManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				SpawnTurret(hit.point, PoolManager.PrefabType.TURRET_CANNON);
			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				SpawnTurret(hit.point, PoolManager.PrefabType.TURRET_LASER);
			}
		}
	}


	GameObject SpawnTurret(Vector3 position, PoolManager.PrefabType type)
	{
		Vector2 map = Map.WorldToMapPosition(position);
		if (!Map.TileMap[(int)map.x, (int)map.y].HasTurret && (type == PoolManager.PrefabType.TURRET_CANNON 
																|| type == PoolManager.PrefabType.TURRET_LASER))
		{
			Map.TileMap[(int)map.x, (int)map.y].HasTurret = true;
			Map.TileMap[(int)map.x, (int)map.y].Walkable = false;
			if (Spawner.RecalculatePath())
			{
				GameObject tmp = Pool.GetObjectByType(type);
				tmp.transform.parent = transform;
				Vector3 world = Map.MapToWorldPosition(map);
				tmp.transform.position = world;
				

				return tmp;
			}
			else
			{
				Map.TileMap[(int)map.x, (int)map.y].HasTurret = false;
				Map.TileMap[(int)map.x, (int)map.y].Walkable = true;
			}
		}

		return null;
	}

}
