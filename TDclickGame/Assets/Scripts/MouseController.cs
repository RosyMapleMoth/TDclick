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
		//Setup gameState link
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		//Listen for initialization call
		gameState.boardMade.AddListener (InitializeButtonsStart);
		//Set the current update function.
		updateFunction = NormalUpdate;
	}

	// Update is called once per frame
	void Update ()
	{
		//Make sure updateFunction is set to something
		if (updateFunction == null) {
			updateFunction = NormalUpdate;
		}

		//Call the update
		updateFunction ();
	}

	private void NormalUpdate ()
	{
		//Cast a ray to fin out what the mouse is on
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction, Color.black, 2f);
		RaycastHit hitObject;

		//if the mouse is over a game object, hand that to gamestate.objectHovered
		if (Physics.Raycast (ray, out hitObject)) {
			gameState.objectHovered = hitObject.collider.gameObject;

			//if we just exited the build menu, throw out any mouse input.
			if (leavingmenu) {
				Debug.Log ("leaving menu");
				buildMenu = false;
				leavingmenu = false;
			} 
			//Otherwise, if the mouse was released, register a click.
			else if (Input.GetMouseButtonUp (0) && !buildMenu) {
				//Hand clicked object to gamestate.objectClicked
				gameState.objectClicked = hitObject.collider.gameObject;
				
				//if clicked on a town, move to the clicker
				if (gameState.objectClicked.tag == "End")
				{
					ChangeToClicker();
				}
				//otherwise, trigger the click event
				else
				{
					gameState.validClick.Invoke ();
				}
			} else {
				//if mouse wasnt clicked, clear gamestate.objectClicked
				gameState.objectClicked = null;
			}
		} else {
			//if mouse not hovering over something, clear gamestate.objectHovered
			gameState.objectHovered = null;
		}
	}

//Move from TD to Clicker
	public void ChangeToClicker()
	{
		//deactivate TD hud
		gameState.huds[0].gameObject.SetActive(false);
		//activate clicker hud
		gameState.huds[1].gameObject.SetActive(true);
		//deactivate TD camera
		gameState.Cameras[0].gameObject.SetActive(false);
		//activate clicker camera
		gameState.Cameras[1].gameObject.SetActive(true);

		//Set update function to cilckerUpdate
		updateFunction = ClickUpdate;
	}

	public void ChangeToTD()
	{
		gameState.huds[1].gameObject.SetActive(false);
		gameState.huds[0].gameObject.SetActive(true);
		gameState.Cameras[1].gameObject.SetActive(false);
		gameState.Cameras[0].gameObject.SetActive(true);

		updateFunction = NormalUpdate;
	}

	private void ClickUpdate()
	{

	}

	private void NoUpdate ()
	{
		updateFunction = NormalUpdate;
	}


	private void ClickerState()
	{
		if (Input.GetMouseButtonUp (0))
		{
//			gameState
		}
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



