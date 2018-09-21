using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	PoolManager Pool;
	MapManager Map;

	// Use this for initialization
	void Start ()
	{
		Pool = FindObjectOfType<PoolManager>();
		Map = FindObjectOfType<MapManager>();
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
				SpawnCannonTurret(hit.point);
			}
		}	
	}


	GameObject SpawnCannonTurret(Vector3 position)
	{		
		Vector2 map = Map.WorldToMapPosition(position);
		if (Map.TileMap[(int)map.x, (int)map.y].Walkable)
		{
			GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.TURRET_CANNON);
			tmp.transform.parent = transform;
			Vector3 world = Map.MapToWorldPosition(map);
			tmp.transform.position = world;
			Map.TileMap[(int)map.x, (int)map.y].Walkable = false;
			return tmp;
		}

		return null;
	}

}
