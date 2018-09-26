using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{

	Vector2 MaxDistance;
	[SerializeField]
	float PanningSensitivity = 4f;

	private void Start()
	{
		if (GameManager.Instance.Map != null)
		{
			MaxDistance = GameManager.Instance.Map.GetWorldSize() / 2;
		}
		
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if (!GameManager.Instance.GameIsOver)
		{
			float xAxisValue = Input.GetAxis("Horizontal") * PanningSensitivity * Time.deltaTime;
			float zAxisValue = Input.GetAxis("Vertical") * PanningSensitivity * Time.deltaTime;

			transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));

			Vector3 pos = transform.position;

			pos.x = Mathf.Clamp(pos.x, -MaxDistance.x, MaxDistance.x);
			pos.z = Mathf.Clamp(pos.z, -MaxDistance.y, MaxDistance.y);

			transform.position = pos;

		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, new Vector3(1f, 1f, 1f));
	}
}
