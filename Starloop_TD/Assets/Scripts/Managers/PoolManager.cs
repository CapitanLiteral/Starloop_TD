using System;
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

	//Getters and Setters

	public GameObject GetObjectByType(PrefabType objectType, bool onlyPooled = false)
	{
		return GetObjectByType(objectType.ToString(), onlyPooled);
	}

	public GameObject GetObjectByType(string objectType, bool onlyPooled = false)
	{
		bool typeExist = false;

		//If type does not exist we do not iterate the main list
		string[] GameObjectTypes = System.Enum.GetNames(typeof(PrefabType));
		foreach (string type in GameObjectTypes)
		{
			if (type == objectType)
			{
				typeExist = true;
			}
		}
		//If type exists we proceed to find an object.
		if (typeExist)
		{
			PrefabType type = (PrefabType)Enum.Parse(typeof(PrefabType), objectType);
			List<GameObject> objects = pooledObjects[type];
			if (objects.Count > 0)
			{
				GameObject pooledObject = objects[0];
				objects.RemoveAt(0);
				pooledObject.SetActive(true);

				return pooledObject;
			}
			else if(!onlyPooled)
			{
				for (int i = 0; i < prefabs.Length; i++)
				{
					if (prefabs[i].type.ToString() == objectType)
					{
						GameObject pooledObject = Instantiate(prefabs[i].objectPrefab) as GameObject;
						return pooledObject;
					}					
				}				
			}
			else
			{
				Debug.LogWarning("There is no pooled object of type " + objectType + "(check 'onlyPooled' parameter)", this); // idk if I should use 'this' as context...
			}
			
			
		}
		else
		{
			Debug.LogError("There is no object of type " + objectType + " in the pool", this); // idk if I should use 'this' as context...
		}


		return null;
	}
}