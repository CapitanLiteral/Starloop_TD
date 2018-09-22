using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class BFS : MonoBehaviour
{
	public Queue<Tile> OpenList;
	public List<Tile> ExploredNodes { get; private set; }

	[SerializeField]
	MapManager Map;

	//Finds a path to a node, if there is no path returns null
	public List<Vector3> FindPath(Vector3 start, Vector3 end)
	{
		float time = Time.realtimeSinceStartup;

		OpenList = new Queue<Tile>();
		ExploredNodes = new List<Tile>();		

		Vector2 startMapPos = Map.WorldToMapPosition(start);
		Vector2 endMapPos = Map.WorldToMapPosition(end);

		Tile startNode = Map.TileMap[(int)startMapPos.x, (int)startMapPos.y];
		Tile endNode = Map.TileMap[(int)endMapPos.x, (int)endMapPos.y];

		List<Vector3> ret = new List<Vector3>();

		//Exploring nodes
		if (start != end)
		{
			Tile actualNode = startNode;
			OpenList.Enqueue(startNode);
			while ((actualNode.Position != endNode.Position) && OpenList.Any())
			{
				actualNode = OpenList.Dequeue();
				//Add neighbors to the open list
				List<Tile> neighbors = GetNeighborNodes(actualNode);
				foreach (var neighbor in neighbors)
				{
					neighbor.parent = actualNode;
					OpenList.Enqueue(neighbor);
				}

				ExploredNodes.Add(actualNode);
			} 

			//Filling path
			while (actualNode.Position != startNode.Position)
			{
				ret.Add(Map.MapToWorldPosition(actualNode.Position));
				Tile tmpNode = actualNode;
				actualNode = actualNode.parent;
				//Resetting parent status for the next path finding
				tmpNode.parent = null;
			}
			ret.Reverse();

			if (ret.Count > 0 && ret[ret.Count - 1] != end)
			{
				ret = null;
			}
			else
			{
				// Delete useless nodes
				if (ret.Count > 1)
				{
					Vector3 dir = Vector3.zero;// ret[1] - ret[0];
					for (int i = 0; i < ret.Count; i++)
					{
						if (ret[i] != end)
						{							
							Vector3 tmpDir = ret[i + 1] - ret[i];
							
							if (tmpDir == dir)
							{
								//don't keep the node
								ret.RemoveAt(i--);
							}
							dir = tmpDir;
						}
					}
				}

			}			
		}

		time = (Time.realtimeSinceStartup - time) * 1000.0f;
		//Debug.Log("Path found in " + time + "ms");
		return ret;
	}

	List<Tile> GetNeighborNodes(Tile center)
	{
		List<Tile> neighbors = new List<Tile>();
		
		if (!(center.Position.x - 1 < 0))
		{
			Tile left = Map.TileMap[(int)center.Position.x - 1, (int)center.Position.y];
			if (!(OpenList.Contains(left) ||
			      ExploredNodes.Contains(left)))
			{
				if (left.Walkable)
					neighbors.Add(left);
			}
		}
		if (!(center.Position.x + 1 >= Map.TileMap.GetLength(0)))
		{
			Tile right = Map.TileMap[(int)center.Position.x + 1, (int)center.Position.y];
			if (!(OpenList.Contains(right) ||
			      ExploredNodes.Contains(right)))
			{
				if (right.Walkable)
					neighbors.Add(right);
			}
		}
		if (!(center.Position.y - 1 < 0))
		{
			Tile top = Map.TileMap[(int)center.Position.x, (int)center.Position.y - 1];
			if (!(OpenList.Contains(top) ||
			      ExploredNodes.Contains(top)))
			{
				if (top.Walkable)
					neighbors.Add(top);
			}
		}
		if (!(center.Position.y + 1 >= Map.TileMap.GetLength(1)))
		{
			Tile bottom = Map.TileMap[(int)center.Position.x, (int)center.Position.y + 1];
			if (!(OpenList.Contains(bottom) ||
			      ExploredNodes.Contains(bottom)))
			{
				if(bottom.Walkable)
					neighbors.Add(bottom);
			}
		}

		return neighbors;
	}

}
