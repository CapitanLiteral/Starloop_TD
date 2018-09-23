using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage = 10;
	[SerializeField]
	float speed = 1;
	[SerializeField]
	float lifeTime = 0.5f;

	public Vector3 direction = Vector3.zero;

	PoolManager pool;

	// Use this for initialization
	void Start ()
	{
		pool = FindObjectOfType<PoolManager>();
		StartCoroutine(PoolBullet());
	}

	void Update()
	{
		gameObject.transform.Translate(direction.x * speed * Time.deltaTime, 
										0,
										direction.z * speed * Time.deltaTime);
	}

	IEnumerator	PoolBullet()
	{
		yield return new WaitForSeconds(lifeTime);
		pool.PoolObject(gameObject);
	}
}
