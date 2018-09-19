using System.Collections.Generic;
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

	public List<BFSnode> FindPath(Vector2Int start, Vector2Int end)
	{
		Start = start;
		End = end;

		BFSnode startNode = map[Start.x, Start.y];
		BFSnode endNode = map[End.x, End.y];

		List<BFSnode> ret = new List<BFSnode>();

		if (Start != End)
		{
			BFSnode actualNode = startNode;
			//while (actualNode.position != endNode.position)
			while (actualNode.position != endNode.position)
			{
				//Add neighbours to the open list
				List<BFSnode> neighbours = GetNeighbourNodes(actualNode);
				foreach (var neighbour in neighbours)
				{
					neighbour.parent = actualNode;
					OpenList.Enqueue(neighbour);
				}

				ExploredNodes.Add(actualNode);

				actualNode = OpenList.Dequeue();
			} 

			while (actualNode.position != startNode.position)
			{
				ret.Add(actualNode);
				actualNode = actualNode.parent;
			}
			ret.Reverse();
		}

		return ret;
	}

	List<BFSnode> GetNeighbourNodes(BFSnode center)
	{
		List<BFSnode> neighbours = new List<BFSnode>();

		if (!(center.position.x - 1 < 0))
		{
			BFSnode left = map[center.position.x - 1, center.position.y];
			if (!(OpenList.Contains(left) ||
			      ExploredNodes.Contains(left)))
			{
				if (left.walkable)
					neighbours.Add(left);
			}
		}
		if (!(center.position.x + 1 >= Size.x))
		{
			BFSnode right = map[center.position.x + 1, center.position.y];
			if (!(OpenList.Contains(right) ||
			      ExploredNodes.Contains(right)))
			{
				if (right.walkable)
					neighbours.Add(right);
			}
		}
		if (!(center.position.y - 1 < 0))
		{
			BFSnode top = map[center.position.x, center.position.y - 1];
			if (!(OpenList.Contains(top) ||
			      ExploredNodes.Contains(top)))
			{
				if (top.walkable)
					neighbours.Add(top);
			}
		}
		if (!(center.position.y + 1 >= Size.y))
		{
			BFSnode bottom = map[center.position.x, center.position.y + 1];
			if (!(OpenList.Contains(bottom) ||
			      ExploredNodes.Contains(bottom)))
			{
				if(bottom.walkable)
					neighbours.Add(bottom);
			}
		}

		return neighbours;
	}

}
