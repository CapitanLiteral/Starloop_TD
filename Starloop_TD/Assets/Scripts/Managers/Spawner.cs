using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	BFS Pathfinder;

	MapManager Map;
	PoolManager Pool;

	GameObject Crystal;
	List<Vector3> Path;

	[SerializeField]
	GameObject normalEnemy;

	[SerializeField]
	int Waves = 5;

	// Use this for initialization
	void Start ()
	{
		Pool = FindObjectOfType<PoolManager>();
		Pathfinder = FindObjectOfType<BFS>();
		Map = FindObjectOfType<MapManager>();
		Crystal = Map.crystal;

		Path = Pathfinder.FindPath(transform.position, Crystal.transform.position);

		/*GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_NORMAL);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		tmp.GetComponent<MobMove>().SetPath(Path);*/

		StartCoroutine(SpawnWave());
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnDrawGizmos()
	{
		/*if (Path != null)
		{
			foreach (var item in Path)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawCube(item, Vector3.one);
			}
		}*/
	}

	IEnumerator SpawnWave()
	{
		for (int i = 0; i < Waves; ++i)
		{
			for (int j = 0; j < 5; j++)
			{
				SpawnEnemyNormal();
				yield return new WaitForSeconds(0.5f);
			}
			yield return new WaitForSeconds(5f);
		}
	}

	GameObject SpawnEnemyNormal()
	{
		GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_NORMAL);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		tmp.GetComponent<MobMove>().SetPath(Path);

		return tmp;
	}
}
