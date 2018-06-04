using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

	private GameState gameState;
	private MouseController mouse;
	private Button[] menuButtons;
	public GameObject buttonTemplate;
	// Use this for initialization
	void Awake () {
		gameState = GameObject.FindObjectOfType<GameState>();
		mouse = GameObject.FindObjectOfType<MouseController>();
		menuButtons = new Button [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildMenu(GameObject [] numButtons)
	{
		numButtons.GetLength(0);
		if (!this.gameObject.activeSelf) 
		{
			this.gameObject.SetActive (true);
		}
		// clear all buttons
		foreach (Button button in menuButtons) 
		{
			GameObject.Destroy (button.gameObject);
		}
		// init button array
		menuButtons = new Button[numButtons.GetLength(0) + 1];
		// create/init buttons
		for (int i = 0; i < numButtons.GetLength(0); i++) 
		{
			GameObject tempButton = Instantiate (buttonTemplate);
			tempButton.transform.SetParent (this.transform.GetChild (0).GetChild (0).GetChild (0));
			tempButton.gameObject.GetComponentInChildren<Text> ().text = numButtons[i].name;
			menuButtons [i] = tempButton.GetComponent<Button> ();
		}

		// make exit button
		menuButtons [numButtons.GetLength(0)] = Instantiate (buttonTemplate).GetComponent<Button> ();
		menuButtons [numButtons.GetLength(0)].transform.SetParent (this.transform.GetChild (0).GetChild (0).GetChild (0));
		menuButtons [numButtons.GetLength(0)].gameObject.GetComponentInChildren<Text> ().text = "Exit Menu";
		menuButtons [numButtons.GetLength(0)].onClick.AddListener (RemoveListeners);


		this.gameObject.SetActive (false);
	}


	public void AddBlockListeners(Action<InitializeCube.CubeType> method)
	{
		mouse.openMenu();
		this.gameObject.SetActive(true);

		menuButtons [0].onClick.AddListener (() => method (InitializeCube.CubeType.Tower));
		menuButtons [1].onClick.AddListener (() => method (InitializeCube.CubeType.Town));
		menuButtons [2].onClick.AddListener (() => method (InitializeCube.CubeType.Road));
		menuButtons [3].onClick.AddListener (() => method (InitializeCube.CubeType.Forest));

		Debug.Log ("openeing menu");
	}

	public void AddTowerListeners(Action<GameObject> method, GameObject [] towers)
	{
		mouse.openMenu();
		this.gameObject.SetActive(true);

		for (int i = 0; i < towers.GetLength(0); i ++)
		{
			AddTowerListener(method, i, towers[i]);
		}
	}

	private void AddTowerListener(Action<GameObject> method, int i, GameObject tower)
	{
		menuButtons [i].onClick.AddListener (() => method (tower));
	}

	public void RemoveListeners()
	{
		mouse.closeMenu();
		this.gameObject.SetActive(false);

		foreach (Button button in menuButtons)
		{
			button.onClick.RemoveAllListeners();
		}
		menuButtons [menuButtons.GetLength(0) - 1].onClick.AddListener (RemoveListeners);
		this.gameObject.SetActive (false);
	}
}
