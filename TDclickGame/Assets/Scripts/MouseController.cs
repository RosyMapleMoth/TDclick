using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseController : MonoBehaviour
{

	private GameState gameState;
	private bool buildMenu = false;
	private bool leavingmenu = false;

	delegate void UpdateFunction ();

	private UpdateFunction updateFunction;

	// Use this for initialization
	void Start ()
	{
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();

		gameState.boardMade.AddListener (InitializeButtonsStart);

		updateFunction = NormalUpdate;
	}

	// Update is called once per frame
	void Update ()
	{
		if (updateFunction == null) {
			updateFunction = NormalUpdate;
		}
		updateFunction ();
	}

	private void NormalUpdate ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction, Color.black, 2f);
		RaycastHit hitObject;

		if (Physics.Raycast (ray, out hitObject)) {
			gameState.objectHovered = hitObject.collider.gameObject;


			if (leavingmenu) {
				Debug.Log ("leaving menu");
				buildMenu = false;
				leavingmenu = false;
			} else if (Input.GetMouseButtonUp (0) && !buildMenu) {

				gameState.objectClicked = hitObject.collider.gameObject;
				gameState.validClick.Invoke ();
			} else {
				gameState.objectClicked = null;
			}
		} else {
			gameState.objectHovered = null;
		}
	}

	private void NoUpdate ()
	{
		updateFunction = NormalUpdate;
	}

	private void InitializeButtonsStart ()
	{
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Initialize Cube");

		foreach (GameObject thisBlock in blocks) {
			thisBlock.GetComponent<InitializeCube> ().closeMenu.AddListener (closeMenu);

			thisBlock.GetComponent<InitializeCube> ().openMenu.AddListener (openMenu);
		}

		gameState.boardMade.RemoveListener (InitializeButtonsStart);
		gameState.boardMade.AddListener (InitializeButtons);
	}

	private void InitializeButtons ()
	{
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("TowerBase");

		foreach (GameObject thisBlock in blocks) {
			thisBlock.GetComponent<CreateTower> ().closeMenu.AddListener (closeMenu);

			thisBlock.GetComponent<CreateTower> ().openMenu.AddListener (openMenu);
		}
	}

	public void openMenu ()
	{
		buildMenu = true;
	}

	public void closeMenu ()
	{
		leavingmenu = true;
		updateFunction = NoUpdate;
	}

}



