using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFSnode 
{
	public bool walkable;
	public Vector2 position;
	public BFSnode parent;

	public BFSnode(Vector2 _position, bool _walkable = true, BFSnode _parent = null)
	{
		position = _position;
		walkable = _walkable;
		parent = _parent;
	}
}

public class BFS : MonoBehaviour
{
	public Queue<BFSnode> OpenList;
	public List<BFSnode> ExploredNodes { get; private set; }

	//Finds a path to a node, if there is no path returns null
	public List<BFSnode> FindPath(Vector2 start, Vector2 end, BFSnode[,] map)
	{
		OpenList = new Queue<BFSnode>();
		ExploredNodes = new List<BFSnode>();
		float time = Time.realtimeSinceStartup;

		BFSnode startNode = map[(int)start.x, (int)start.y];
		BFSnode endNode = map[(int)end.x, (int)end.y];

		List<BFSnode> ret = new List<BFSnode>();

		if (start != end)
		{
			BFSnode actualNode = startNode;
			OpenList.Enqueue(startNode);
			while ((actualNode.position != endNode.position) && OpenList.Any())
			{
				actualNode = OpenList.Dequeue();
				//Add neighbors to the open list
				List<BFSnode> neighbors = GetNeighborNodes(actualNode, map);
				foreach (var neighbor in neighbors)
				{
					neighbor.parent = actualNode;
					OpenList.Enqueue(neighbor);
				}

				ExploredNodes.Add(actualNode);
			} 

			//Filling path
			while (actualNode.position != startNode.position)
			{
				ret.Add(actualNode);
				BFSnode tmpNode = actualNode;
				actualNode = actualNode.parent;
				//Resetting parent status for the next path finding
				tmpNode.parent = null;
			}
			ret.Reverse();

			// Delete useless nodes
			if (ret.Count > 1)
			{
				Vector2 dir = ret[1].position - ret[0].position;
				for (int i = 0; i < ret.Count; i++)
				{
					if (ret[i].position != end)
					{

						Vector2 tmpDir = ret[i + 1].position - ret[i].position;
						if (tmpDir == dir)
						{
							//don't keep the node
							ret.RemoveAt(i--);
						}
						dir = tmpDir;
					}
				}
			}

			if (ret[ret.Count - 1].position != end)
			{
				ret = null;
			}
		}

		time = (Time.realtimeSinceStartup - time) * 1000.0f;
		Debug.Log("Path found in " + time + "ms");
		return ret;
	}

	List<BFSnode> GetNeighborNodes(BFSnode center, BFSnode[,] map)
	{
		List<BFSnode> neighbors = new List<BFSnode>();
		
		if (!(center.position.x - 1 < 0))
		{
			BFSnode left = map[(int)center.position.x - 1, (int)center.position.y];
			if (!(OpenList.Contains(left) ||
			      ExploredNodes.Contains(left)))
			{
				if (left.walkable)
					neighbors.Add(left);
			}
		}
		if (!(center.position.x + 1 >= map.GetLength(0)))
		{
			BFSnode right = map[(int)center.position.x + 1, (int)center.position.y];
			if (!(OpenList.Contains(right) ||
			      ExploredNodes.Contains(right)))
			{
				if (right.walkable)
					neighbors.Add(right);
			}
		}
		if (!(center.position.y - 1 < 0))
		{
			BFSnode top = map[(int)center.position.x, (int)center.position.y - 1];
			if (!(OpenList.Contains(top) ||
			      ExploredNodes.Contains(top)))
			{
				if (top.walkable)
					neighbors.Add(top);
			}
		}
		if (!(center.position.y + 1 >= map.GetLength(1)))
		{
			BFSnode bottom = map[(int)center.position.x, (int)center.position.y + 1];
			if (!(OpenList.Contains(bottom) ||
			      ExploredNodes.Contains(bottom)))
			{
				if(bottom.walkable)
					neighbors.Add(bottom);
			}
		}

		return neighbors;
	}

}
