using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
	[HideInInspector]
	List<Vector3> Path;

	MapManager Map;
	PoolManager Pool;

	[SerializeField]
	float speed = 10f;

	public Vector3 target;
	public int pathIndex = 0;

	public bool active = false;

	// Use this for initialization
	void Start ()
	{
		Map = FindObjectOfType<MapManager>();
		Pool = FindObjectOfType<PoolManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Path != null)
		{
			Vector3 dir = target - transform.position;
			transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

			if (Vector3.Distance(target, transform.position) <= 0.2f)
			{
				GetNextWaypoint();
			}
		}
	}

	void GetNextWaypoint()
	{
		if (pathIndex >= Path.Count - 1)
		{
			active = false;
		}
		else
		{
			pathIndex++;
			target = Path[pathIndex];
		}

	}

	public void SetPath(List<Vector3> path)
	{
		Path = path;
		pathIndex = 0;

		if (!(pathIndex +1 >= Path.Count))
		{
			target = Path[pathIndex];
		}
		
	}
}
