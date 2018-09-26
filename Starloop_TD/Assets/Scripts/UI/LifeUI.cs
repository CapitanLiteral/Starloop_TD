using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
	public Text lifeText;

	// Update is called once per frame
	void Update()
	{
		lifeText.text = GameManager.Instance.Life + " Lives";
	}
}
