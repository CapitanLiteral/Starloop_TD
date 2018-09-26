using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage = 10;
	[SerializeField]
	float speed = 1;

	public Vector3 direction = Vector3.zero;

	public static int bulletsActive = 0;
	// Use this for initialization
	void OnEnable ()
	{
		//Invoke("PoolBullet", lifeTime);
		bulletsActive++;
	}
	private void OnDisable()
	{
		bulletsActive--;
	}

	void Update()
	{
		gameObject.transform.Translate(direction.x * speed * Time.deltaTime, 
										0,
										direction.z * speed * Time.deltaTime);

		Vector2 worldSize = GameManager.Instance.Map.GetWorldSize();

		if (gameObject.transform.position.x > worldSize.x)
		{
			PoolBullet();
		}
		else if (gameObject.transform.position.x < -worldSize.x)
		{
			PoolBullet();
		}
		if (gameObject.transform.position.z > worldSize.y)
		{
			PoolBullet();
		}
		else if (gameObject.transform.position.z < -worldSize.y)
		{
			PoolBullet();
		}
	}

	public void PoolBullet()
	{
		PoolManager.Instance.PoolObject(gameObject);
		
	}

}
