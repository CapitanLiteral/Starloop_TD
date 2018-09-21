using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	BFS Pathfinder;
	MapManager Map;

	GameObject Crystal;

	List<Vector3> Path;

	// Use this for initialization
	void Start ()
	{
		Pathfinder = FindObjectOfType<BFS>();
		Map = FindObjectOfType<MapManager>();
		Crystal = Map.crystal;
		List<BFSnode> BFSPath = Pathfinder.FindPath(Map.WorldToMapPosition(transform.position), 
									Map.WorldToMapPosition(Crystal.transform.position), 
									Map.nodeMap);
		foreach (var item in BFSPath)
		{
			Path.Add(item.position);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
