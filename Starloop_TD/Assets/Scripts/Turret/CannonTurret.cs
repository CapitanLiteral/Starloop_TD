using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : MonoBehaviour
{

	[SerializeField]
	float damage = 20;
	[SerializeField]
	float radius = 3;
	[SerializeField]
	float rotationSpeed = 10;
	//Shoot bullets per second
	[SerializeField]
	float fireRate = 4;
	GameObject target;
	[SerializeField]
	Transform bulletOut;

	[SerializeField]
	Transform partToRotate;

	float counter = 0;

	PoolManager pool;

	void Start()
	{
		InvokeRepeating("GetTarget", 0, 0.2f);
		pool = FindObjectOfType<PoolManager>();
	}

	void Update()
	{
		counter += Time.deltaTime;
		if (target == null)
			return;


		Vector3 dir = target.transform.position - transform.position;

		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);



		if (counter >= 1/fireRate)
		{
			if (target != null)
				SpawnBullet();
			counter = 0;
		}


	}

	void GetTarget()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (var item in hitColliders)
		{
			if (item.gameObject.tag == "Enemy")
			{
				float distanceToEnemy = Vector3.Distance(transform.position, item.transform.position);
				if (distanceToEnemy < shortestDistance)
				{
					shortestDistance = distanceToEnemy;
					nearestEnemy = item.gameObject;
				}
			}
		}

		if (nearestEnemy != null && nearestEnemy != target)
		{
			target = nearestEnemy;
		}
		else
		{
			target = null;
		}

	}

	void SpawnBullet()
	{
		GameObject bulletObject = pool.GetObjectByType(PoolManager.PrefabType.BULLET);
		Bullet bullet = bulletObject.GetComponent<Bullet>();
		bulletObject.transform.position = bulletOut.position;
		Vector3 direction = target.transform.position - transform.position;

		bullet.direction = direction;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
		if (target != null)
		{
			Gizmos.DrawSphere(target.transform.position, 1.5f);
		}
	}
}
