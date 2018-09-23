using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
	protected Transform _XForm_Camera;
	protected Transform _XForm_Parent;

	protected Vector3 LocalRotation;
	protected float CameraDistance = 10f;

	[SerializeField]
	float MouseSensitivity = 4f;
	[SerializeField]
	float ScrollSensitivity = 2f;
	[SerializeField]
	float OrbitDampening = 10f;
	[SerializeField]
	float ScrollDampening = 6f;
	[SerializeField]
	bool CameraDisabled = true;

	bool firstButtonPressed = false;
	float timeOfFirstButton = 0f;
	bool reset = false;

	// Use this for initialization
	void Start ()
	{
		_XForm_Camera = transform;
		_XForm_Parent = transform.parent;
	}	
	
	void LateUpdate ()
	{
		if (Input.GetKeyDown(KeyCode.LeftAlt))
			CameraDisabled = !CameraDisabled;

		if (Input.GetKeyDown(KeyCode.A) && firstButtonPressed)
		{
			if (Time.time - timeOfFirstButton < 0.5f)
			{
				Debug.Log("DoubleClicked");
			}
			else
			{
				Debug.Log("Too late");
			}

			reset = true;
		}

		if (Input.GetKeyDown(KeyCode.A) && !firstButtonPressed)
		{
			firstButtonPressed = true;
			timeOfFirstButton = Time.time;
		}

		if (reset)
		{
			firstButtonPressed = false;
			reset = false;
		}

		if (!CameraDisabled)
		{
			if (Input.GetAxis("Mouse X") != 0 ||Input.GetAxis("Mouse Y") != 0)
			{
				LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
				LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

				//Clamp rotation to horizon and not flip at the top
				LocalRotation.y = Mathf.Clamp(LocalRotation.y, 0f, 90f);


			}

			//Scrolling
			if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

				//Scroll slowing when you are closer to the map;
				ScrollAmount *= (CameraDistance * 0.3f);

				CameraDistance += ScrollAmount * -1f;

				CameraDistance = Mathf.Clamp(CameraDistance, 1.5f, 100f);
			}
		}

		//Camera transformations;
		Quaternion QT = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
		_XForm_Parent.rotation = Quaternion.Lerp(_XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

		if (_XForm_Camera.localPosition.z != CameraDistance * -1f)
		{
			_XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_XForm_Camera.localPosition.z, CameraDistance * -1f, Time.deltaTime * ScrollDampening));
		}

	}

	void DefaultCameraPosition()
	{
		Quaternion QT = Quaternion.Euler(0, 45f, 0);
		_XForm_Parent.rotation = QT;
		_XForm_Camera.localPosition = new Vector3(0f, 0f, -10f);

	}
}
