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
	GameObject target;

	[SerializeField]
	Transform partToRotate;

	void Start()
	{
		InvokeRepeating("GetTarget", 0, 0.2f);
	}

	// Update is called once per frame
	void Update ()
	{		
		if (target == null)
			return;

		Vector3 dir = target.transform.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
		//Vector3 rotation = lookRotation.eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
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

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
		Gizmos.DrawSphere(target.transform.position, 1.5f);
	}
}
