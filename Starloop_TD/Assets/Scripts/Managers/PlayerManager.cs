using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	SpawnManager Spawner;

	GameObject ContainerObject;

	private void Awake()
	{
		ContainerObject = new GameObject("Turrets");
	}

	// Use this for initialization
	void Start ()
	{
		Spawner = FindObjectOfType<SpawnManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground")))
			{
				GameObject turret = SpawnTurret(hit.point, PoolManager.PrefabType.TURRET_CANNON);
				if (turret != null)
					turret.transform.parent = ContainerObject.transform;



			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				GameObject turret = SpawnTurret(hit.point, PoolManager.PrefabType.TURRET_LASER);
				if (turret != null)
					turret.transform.parent = ContainerObject.transform;
			}
		}
	}


	GameObject SpawnTurret(Vector3 position, PoolManager.PrefabType type)
	{
		MapManager map = GameManager.Instance.Map;
		Vector2 mapPosition = map.WorldToMapPosition(position);
		if (!map.TileMap[(int)mapPosition.x, (int)mapPosition.y].HasTurret && (type == PoolManager.PrefabType.TURRET_CANNON 
																|| type == PoolManager.PrefabType.TURRET_LASER))
		{
			map.TileMap[(int)mapPosition.x, (int)mapPosition.y].HasTurret = true;
			map.TileMap[(int)mapPosition.x, (int)mapPosition.y].Walkable = false;
			if (Spawner.RecalculatePath())
			{
				GameObject tmp = PoolManager.Instance.GetObjectByType(type);
				tmp.transform.parent = transform;
				Vector3 world = map.MapToWorldPosition(mapPosition);
				tmp.transform.position = world;
				

				return tmp;
			}
			else
			{
				map.TileMap[(int)mapPosition.x, (int)mapPosition.y].HasTurret = false;
				map.TileMap[(int)mapPosition.x, (int)mapPosition.y].Walkable = true;
			}
		}

		return null;
	}

}
