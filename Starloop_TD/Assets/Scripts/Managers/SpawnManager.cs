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

	List<Mobile> activeEnemies;

	GameObject containerObject;

	private void Awake()
	{
		containerObject = new GameObject("Enemies");
	}

	// Use this for initialization
	void Start ()
	{
		Pool = FindObjectOfType<PoolManager>();
		Pathfinder = FindObjectOfType<BFS>();
		Map = FindObjectOfType<MapManager>();
		Crystal = Map.crystal;

		activeEnemies = new List<Mobile>();

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
		for(int i = 0; i < activeEnemies.Count; ++i)
		{
			if (!activeEnemies[i].active)
			{
				Pool.PoolObject(activeEnemies[i].gameObject);
				activeEnemies.RemoveAt(i);
			}
		}
	}

	private void OnDrawGizmos()
	{
		//if (Path != null)
		//{
		//	foreach (var item in Path)
		//	{
		//		Gizmos.color = Color.red;
		//		Gizmos.DrawCube(item, new Vector3(0.5f, 4, 0.5f));
		//	}
		//}
	}

	IEnumerator SpawnWave()
	{
		//yield return new WaitForSeconds(5f);
		for (int i = 0; i < Waves; ++i)
		{
			if (i < 2)
			{
				for (int j = 0; j < 5; j++)
				{
					Mobile mob = SpawnEnemyNormal();
					activeEnemies.Add(mob);
					mob.active = true;

					yield return new WaitForSeconds(0.5f);
				}

			}
			else if(i < 4)
			{
				for (int j = 0; j < 5; j++)
				{
					Mobile mob = SpawnEnemySwift();
					activeEnemies.Add(mob);
					mob.active = true;

					yield return new WaitForSeconds(0.5f);
				}

			}
			else if(i < 6)
			{
				for (int j = 0; j < 5; j++)
				{
					Mobile mob = SpawnEnemyHeavy();
					activeEnemies.Add(mob);
					mob.active = true;

					yield return new WaitForSeconds(0.5f);
				}
			}
			yield return new WaitForSeconds(5f);
		}
	}

	Mobile SpawnEnemyNormal()
	{
		GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_NORMAL);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		Mobile ret = tmp.GetComponent<Mobile>();
		ret.SetPath(Path);

		return ret;
	}
	Mobile SpawnEnemySwift()
	{
		GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_FAST);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		Mobile ret = tmp.GetComponent<Mobile>();
		ret.SetPath(Path);

		return ret;
	}
	Mobile SpawnEnemyHeavy()
	{
		GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_HEAVY);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		Mobile ret = tmp.GetComponent<Mobile>();
		ret.SetPath(Path);

		return ret;
	}


	public bool RecalculatePath()
	{
		bool availablePath = true;		
		List <List <Vector3>> activePaths = new List<List<Vector3>>();

		List<Vector3> pathToCheck = Pathfinder.FindPath(transform.position, Crystal.transform.position); ;

		if (pathToCheck == null)
		{
			availablePath = false;
		}
		else
		{
			Path = pathToCheck;
			foreach (var item in activeEnemies)
			{
				List<Vector3> tempPath = Pathfinder.FindPath(item.transform.position, Crystal.transform.position);
				if (tempPath == null)
				{
					availablePath = false;
					break;
				}
				else
				{
					activePaths.Add(tempPath);
				}
			}

			if (availablePath)
			{
				for (int i = 0; i < activeEnemies.Count; ++i)
				{
					activeEnemies[i].SetPath(activePaths[i]);
				}
			}

		}

		return availablePath;
	}
}

