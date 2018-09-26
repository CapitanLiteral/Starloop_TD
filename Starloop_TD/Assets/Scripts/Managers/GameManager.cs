using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	#region Private members

	private static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script
	private MapManager map = null;
	private Shop shop = null;

	PoolManager.PrefabType turretTobuild = PoolManager.PrefabType.TURRET_CANNON;

	[SerializeField]
	int life = 10;
	[SerializeField]
	int money = 200;

	[SerializeField]
	GameObject gameOverMenu;

	#endregion

	#region Public members
	[HideInInspector]
	public int wavesSurvived = 0;
	[HideInInspector]
	public bool GameIsOver;
	#endregion

	#region Getters&Setters
	public static GameManager Instance
	{
		get
		{
			return instance;
		}
	}
	public MapManager Map
	{
		get
		{
			return map;
		}
	}
	public int Life
	{
		get
		{
			return life;
		}

		set
		{
			life = value;
		}
	}
	public PoolManager.PrefabType TurretTobuild
	{
		get
		{
			return turretTobuild;
		}

		set
		{
			turretTobuild = value;
		}
	}
	public int Money
	{
		get
		{
			return money;
		}

		set
		{
			money = value;
		}
	}
	public Shop Shop
	{
		get
		{
			return shop;
		}

		set
		{
			shop = value;
		}
	}
	#endregion


	//Awake is always called before any Start functions
	void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (Instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
		map = GetComponent<MapManager>();
		shop = FindObjectOfType<Shop>();
		GameIsOver = false;
    }


    //Update is called every frame.
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (life <= 0)
		{
			GameIsOver = true;
			gameOverMenu.SetActive(true);
		}
    }

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}

}