using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{

	[SerializeField]
	float damage = 20;
	[SerializeField]
	float radius = 3;
	[SerializeField]
	float rotationSpeed = 10;
	//Shoot bullets per second
	[SerializeField]
	float fireRate = 10;
	GameObject target;
	[SerializeField]
	Transform firePoint;
	LineRenderer laser;

	[SerializeField]
	Transform partToRotate;

	float counter = 0;

	void Start()
	{
		InvokeRepeating("GetTarget", 0, 0.1f);
		laser = GetComponent<LineRenderer>();
	}

	void Update()
	{
		counter += Time.deltaTime;
		if (target == null)
		{
			if (laser.enabled)
				laser.enabled = false;
			return;
		}

		Mobile mob = target.transform.parent.GetComponent<Mobile>();

		Vector3 dir = target.transform.position - transform.position;
		
		//Rotate turret
		//Quaternion lookRotation = Quaternion.LookRotation(dir);
		//Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
		//partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
		//partToRotate.rotation = lookRotation;


		if (counter >= 1 / fireRate)
		{
			if (target != null)
				mob.TakeDamage(damage);
			counter = 0;
		}
		Laser();
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

		if (nearestEnemy != null)
		{
			target = nearestEnemy;
		}
		else
		{
			target = null;
		}

	}

	void Laser()
	{
		if (!laser.enabled)
			laser.enabled = true;

		laser.SetPosition(0, firePoint.position);
		laser.SetPosition(1, target.transform.position);
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
