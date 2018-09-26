using System;
using System.Collections.Generic;
using UnityEngine;



public class Shop : MonoBehaviour
{
	[Serializable]
	public struct shopItem
	{
		public PoolManager.PrefabType type;
		public int cost;
	}


	public shopItem[] items;

	public void SelectCannon()
	{
		GameManager.Instance.TurretTobuild = PoolManager.PrefabType.TURRET_CANNON;
	}

	public void SelectLaser()
	{
		GameManager.Instance.TurretTobuild = PoolManager.PrefabType.TURRET_LASER;
	}
	
	public shopItem GetItem(PoolManager.PrefabType type)
	{
		foreach (var item in items)
		{
			if (item.type == type)
			{
				return item;
			}
		}

		Debug.LogWarning("Item not found in items list");
		return items[0];
	}
}
