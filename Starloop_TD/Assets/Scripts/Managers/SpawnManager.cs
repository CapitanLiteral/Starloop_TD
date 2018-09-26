using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	BFS Pathfinder;

	GameObject Crystal;
	List<Vector3> Path;

	[SerializeField]
	GameObject normalEnemy;

	[SerializeField]
	int Waves = 5;

	[SerializeField]
	float distance = 1.5f;

	[SerializeField]
	float waveStep;
	[SerializeField]
	float waveEnemies  = 5;
	[SerializeField]
	float timeBetweenWaves = 5;

	List<Mobile> activeEnemies;

	GameObject containerObject;

	private void Awake()
	{
		containerObject = new GameObject("Enemies");
	}

	// Use this for initialization
	void Start()
	{
		Pathfinder = FindObjectOfType<BFS>();
		Crystal = GameManager.Instance.Map.crystal;

		activeEnemies = new List<Mobile>();

		Path = Pathfinder.FindPath(transform.position, Crystal.transform.position);

		/*GameObject tmp = Pool.GetObjectByType(PoolManager.PrefabType.ENEMY_NORMAL);
		tmp.transform.parent = transform;
		tmp.transform.position = transform.position;

		tmp.GetComponent<MobMove>().SetPath(Path);*/

		StartCoroutine(SpawnWave());

	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < activeEnemies.Count; ++i)
		{
			if (!activeEnemies[i].active)
			{
				PoolManager.Instance.PoolObject(activeEnemies[i].gameObject);
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
		
		int waveType = (int)Mathf.Round(Random.Range(1, 4));
		float timeBetweenMobs = 0f;

		for (int i = 0; i < Waves; ++i)
		{
			//yield return new WaitForSeconds(5f);
			if (waveType == 1)
			{
				for (int j = 0; j < waveEnemies; j++)
				{
					Mobile mob = SpawnEnemy(PoolManager.PrefabType.ENEMY_NORMAL);
					activeEnemies.Add(mob);
					mob.active = true;
					timeBetweenMobs =  distance / mob.Speed;
					yield return new WaitForSeconds(timeBetweenMobs);
				}
			}
			else if (waveType == 2)
			{
				for (int j = 0; j < waveEnemies; j++)
				{
					Mobile mob = SpawnEnemy(PoolManager.PrefabType.ENEMY_FAST);
					activeEnemies.Add(mob);
					mob.active = true;
					timeBetweenMobs = distance / mob.Speed;
					yield return new WaitForSeconds(timeBetweenMobs);
				}
			}
			else if (waveType == 3)
			{
				for (int j = 0; j < waveEnemies; j++)
				{
					Mobile mob = SpawnEnemy(PoolManager.PrefabType.ENEMY_HEAVY);
					activeEnemies.Add(mob);
					mob.active = true;
					timeBetweenMobs = distance / mob.Speed;
					yield return new WaitForSeconds(timeBetweenMobs);
				}
			}
			waveType = (int)Mathf.Round(Random.Range(1, 4));
			waveEnemies = waveEnemies + waveStep;
			GameManager.Instance.wavesSurvived++;
			yield return new WaitForSeconds(timeBetweenWaves);
			if (GameManager.Instance.GameIsOver)
			{
				break;
			}
		}
	}

	Mobile SpawnEnemy(PoolManager.PrefabType enemyType)
	{
		GameObject tmp;
		tmp = PoolManager.Instance.GetObjectByType(enemyType);
		tmp.transform.parent = containerObject.transform;
		tmp.transform.position = transform.position;

		Mobile ret = tmp.GetComponent<Mobile>();
		ret.SetPath(Path);

		return ret;
	}



	public bool RecalculatePath()
	{
		bool availablePath = true;
		List<List<Vector3>> activePaths = new List<List<Vector3>>();

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

