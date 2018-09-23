using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

	public Camera MainCamera;
	// Use this for initialization
	void Start ()
	{
		if (MainCamera == null)
		{
			MainCamera = Camera.current;
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (MainCamera != null)
		{
			transform.LookAt(MainCamera.transform);			
		}
		else
		{
			MainCamera = Camera.current;
		}
		
	}
}
