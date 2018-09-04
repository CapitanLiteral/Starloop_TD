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
		public int nInstances;
		public GameObject prefab;
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



	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
