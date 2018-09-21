using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
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

	List<MobMove> activeEnemies;

	// Use this for initialization
	void Start ()
	{
		Pool = FindObjectOfType<PoolManager>();
		Pathfinder = FindObjectOfType<BFS>();
		Map = FindObjectOfType<MapManager>();
		Crystal = Map.crystal;

		activeEnemies = new List<MobMove>();

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
		foreach (var item in activeEnemies)
		{
			if (!item.active)
			{
				Pool.PoolObject(item.gameObject);
				activeEnemies.Remove(item);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (Path != null)
		{
			foreach (var item in Path)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawCube(item, Vector3.one);
			}
		}
	}

	IEnumerator SpawnWave()
	{
		for (int i = 0; i < Waves; ++i)
		{
			for (int j = 0; j < 5; j++)
			{
				MobMove mob = SpawnEnemyNormal();
				activeEnemies.Add(mob);
				mob.active = true;

				yield return new WaitForSeconds(0.5f);
			}
			yield return new WaitForSeconds(5f);
		}
	}

	MobMove SpawnEnemyNormal()
	{
		GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_NORMAL);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		MobMove ret = tmp.GetComponent<MobMove>();
		ret.SetPath(Path);

		return ret;
	}

	public void RecalculatePath()
	{
		Path = Pathfinder.FindPath(transform.position, Crystal.transform.position);
		foreach (var item in activeEnemies)
		{
			List<Vector3> tempPath = Pathfinder.FindPath(item.transform.position, Crystal.transform.position);
			Debug.Log(tempPath);
			item.SetPath(tempPath);
		}
	}
}

