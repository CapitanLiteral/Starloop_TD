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


	// Use this for initialization
	void Start ()
	{
		Invoke("PoolBullet", lifeTime);
	}

	void Update()
	{
		gameObject.transform.Translate(direction.x * speed * Time.deltaTime, 
										0,
										direction.z * speed * Time.deltaTime);
	}

	public void PoolBullet()
	{
		PoolManager.Instance.PoolObject(gameObject);
	}

}
