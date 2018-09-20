using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFSnode
{
	public bool walkable;
	public Vector2Int position;
	public BFSnode parent;

	public BFSnode(Vector2Int _position, bool _walkable = true, BFSnode _parent = null)
	{
		position = _position;
		walkable = _walkable;
		parent = _parent;
	}
}

public class BFS
{
	public Vector2Int Start;
	public Vector2Int End;

	public Queue<BFSnode> OpenList;
	public List<BFSnode> ExploredNodes { get; private set; }

	public Vector2Int Size;
	public BFSnode[,] map;

	public BFS(Vector2Int size)
	{
		Size = size;
		map = new BFSnode[Size.x, Size.y];
		OpenList = new Queue<BFSnode>();
		ExploredNodes = new List<BFSnode>();

		for (int i = 0; i < Size.x; i++)
		{
			for (int j = 0; j < Size.y; j++)
			{
				map[i, j] = new BFSnode(new Vector2Int(i, j));
			}
		}
	}


	//Finds a path to a node, if there is no path returns null
	public List<BFSnode> FindPath(Vector2Int start, Vector2Int end)
	{
		float time = Time.realtimeSinceStartup;
		Start = start;
		End = end;

		BFSnode startNode = map[Start.x, Start.y];
		BFSnode endNode = map[End.x, End.y];

		List<BFSnode> ret = new List<BFSnode>();

		if (Start != End)
		{
			BFSnode actualNode = startNode;
			OpenList.Enqueue(startNode);
			while ((actualNode.position != endNode.position) && OpenList.Any())
			{
				actualNode = OpenList.Dequeue();
				//Add neighbors to the open list
				List<BFSnode> neighbors = GetNeighborNodes(actualNode);
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
			Vector2Int dir = ret[1].position - ret[0].position;
			for (int i = 0; i < ret.Count; i++)
			{
				if (ret[i].position != end)
				{

					Vector2Int tmpDir = ret[i + 1].position - ret[i].position;
					if (tmpDir == dir)
					{
						//don't keep the node
						ret.RemoveAt(i--);
					}
					dir = tmpDir;
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

	List<BFSnode> GetNeighborNodes(BFSnode center)
	{
		List<BFSnode> neighbors = new List<BFSnode>();

		if (!(center.position.x - 1 < 0))
		{
			BFSnode left = map[center.position.x - 1, center.position.y];
			if (!(OpenList.Contains(left) ||
			      ExploredNodes.Contains(left)))
			{
				if (left.walkable)
					neighbors.Add(left);
			}
		}
		if (!(center.position.x + 1 >= Size.x))
		{
			BFSnode right = map[center.position.x + 1, center.position.y];
			if (!(OpenList.Contains(right) ||
			      ExploredNodes.Contains(right)))
			{
				if (right.walkable)
					neighbors.Add(right);
			}
		}
		if (!(center.position.y - 1 < 0))
		{
			BFSnode top = map[center.position.x, center.position.y - 1];
			if (!(OpenList.Contains(top) ||
			      ExploredNodes.Contains(top)))
			{
				if (top.walkable)
					neighbors.Add(top);
			}
		}
		if (!(center.position.y + 1 >= Size.y))
		{
			BFSnode bottom = map[center.position.x, center.position.y + 1];
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
