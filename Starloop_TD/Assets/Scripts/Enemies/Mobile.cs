using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobile : MonoBehaviour
{
	[HideInInspector]
	List<Vector3> Path;

	[SerializeField]
	float speed = 10f;
	[SerializeField]
	float startHealth = 100;
	[SerializeField]
	int money = 10;
	
	float health;
	

	[Header("UI")]
	public Image healthBar;
	
	public Vector3 target;
	public int pathIndex = 0;

	public bool active = false;

	public Vector3 velocity = Vector3.zero;

	public float Speed
	{
		get
		{
			return speed;
		}

		set
		{
			speed = value;
		}
	}

	public float StartHealth
	{
		get
		{
			return startHealth;
		}

		set
		{
			startHealth = value;
		}
	}

	// Use this for initialization
	void Start ()
	{
		SetDefaultStats();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.Instance.GameIsOver)
		{
			if (Path != null)
			{
				Vector3 dir = target - transform.position;
				velocity = dir.normalized * speed;
				transform.Translate(velocity * Time.deltaTime, Space.World);

				if (Vector3.Distance(target, transform.position) <= 0.5f)
				{
					GetNextWaypoint();
				}
			}
			if (health <= 0)
			{
				active = false;
				SetDefaultStats();
				GameManager.Instance.Money += money;
			}

		}
	}

	void GetNextWaypoint()
	{
		if (pathIndex >= Path.Count - 1)
		{
			active = false;
			GameManager.Instance.Life--;
		}
		else
		{
			pathIndex++;
			target = Path[pathIndex];
		}

	}

	public void SetPath(List<Vector3> path)
	{
		Path = path;
		pathIndex = 0;

		if (!(pathIndex +1 >= Path.Count))
		{
			target = Path[pathIndex];
		}
		
	}

	public void TakeDamage(float amount)
	{
		health -= amount;
		healthBar.fillAmount = health / StartHealth;
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject go = other.gameObject;
		if (go.tag == "Bullet")
		{
			Bullet bullet = go.GetComponent<Bullet>();
			TakeDamage(bullet.damage);
			bullet.PoolBullet();
		}
	}

	void SetDefaultStats()
	{
		health = StartHealth;
		healthBar.fillAmount = health / StartHealth;
	}
}
