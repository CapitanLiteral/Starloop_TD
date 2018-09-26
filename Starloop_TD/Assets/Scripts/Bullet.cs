using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[HideInInspector]
	public float damage = 10;
	[SerializeField]
	float speed = 1;

	public Vector3 direction = Vector3.zero;

	public static int bulletsActive = 0;

	public float fspeed;
	// Use this for initialization
	void OnEnable ()
	{
		
	}
	private void OnDisable()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;

	}

	void Update()
	{
		/*gameObject.transform.Translate(direction.x * speed * Time.deltaTime, 
										0,
										direction.z * speed * Time.deltaTime);*/
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		fspeed = rigidbody.velocity.magnitude;
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

	public void Fire(Vector3 direction, float _damage)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(direction * speed);
		damage = _damage;
	}

}
