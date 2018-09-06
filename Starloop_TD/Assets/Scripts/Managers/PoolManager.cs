using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Singleton class
public class PoolManager : MonoBehaviour
{
	// A struct to store info about wich prefabs will load and how many instances of each one will be at the beggining of the game.
	[Serializable]
	public struct Prefab
	{
		public PrefabType type;
		public int amount;
		public GameObject objectPrefab;
	}

	//Type of accepted prefabs
	public enum PrefabType
	{
		TILE,
		ENEMY_NORMAL,
		ENEMY_FAST,
		ENEMY_HEAVY,
		TURRET_LASER,
		TURRET_CANNON
	}

	public static PoolManager instance = null;

	//Array that stores prefabs to be loaded on awake
	public Prefab[] prefabs;

	// Stores the available pooled objects
	public Dictionary<PrefabType, List<GameObject>> pooledObjects;

	GameObject containerObject;

	[SerializeField]
	Transform latticeTransform;

	void Awake()
	{
		instance = this;
		Initialize();
	}

	void Initialize()
	{
		containerObject = new GameObject("PoolManager");

		pooledObjects = new Dictionary<PrefabType, List<GameObject>>();

		for (int i = 0; i < prefabs.Length; i++)
		{
			Prefab prefab = prefabs[i];
			pooledObjects[prefab.type] = new List<GameObject>();

			for (int n = 0; n < prefab.amount; n++)
			{
				GameObject newObj = Instantiate(prefab.objectPrefab) as GameObject;
				newObj.name = prefab.objectPrefab.name;
				PoolObject(newObj);
			}
		}
	}

	// Pools the object specified.  Will not be pooled if there is no prefab of that type.
	public void PoolObject(GameObject obj)
	{
		for (int i = 0; i < prefabs.Length; i++)
		{
			if (prefabs[i].objectPrefab.name == obj.name)
			{
				obj.SetActive(false);
				obj.transform.parent = containerObject.transform;
				pooledObjects[prefabs[i].type].Add(obj);
				return;
			}
		}
	}
	{
	}
}
