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
		updateFunction = TowerDefenseUpdate;
	}

	// Update is called once per frame
	void Update ()
	{
		//Make sure updateFunction is set to something
		if (updateFunction == null) {
			updateFunction = TowerDefenseUpdate;
		}

		//Call the update
		updateFunction ();
	}

    //What needs to be updated while in Tower Defense
	private void TowerDefenseUpdate ()
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

    //Move from Clicker to TD
	public void ChangeToTD()
	{
        //deactivate clicker hud
		gameState.huds[1].gameObject.SetActive(false);
        //activate TD hud
		gameState.huds[0].gameObject.SetActive(true);
        //deactivate clicker camera
		gameState.Cameras[1].gameObject.SetActive(false);
        //activate TD camera
		gameState.Cameras[0].gameObject.SetActive(true);

        //Set update function to TowerDefenseUpdate
		updateFunction = TowerDefenseUpdate;
	}

    //what needs to be updated while in Clicker
	private void ClickUpdate()
	{

	}

    //Used for eating one frame of inputs
	private void NoUpdate ()
	{
		updateFunction = TowerDefenseUpdate;
	}


	private void ClickerState()
	{
		if (Input.GetMouseButtonUp (0))
		{
//			gameState
		}
	}

    /// <summary>
    /// This function runs when the boardMade unity event is invoked for the first time by CreateMap,
    /// after the initialization blocks have been created as the game first starts. 
    /// </summary>
	private void InitializeButtonsStart ()
	{
        //gets an array of the blocks in the game
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Initialize Cube");
        
        //for each block, add listeners to the close and openMenu unity events.
		foreach (GameObject thisBlock in blocks) {
			thisBlock.GetComponent<InitializeCube> ().closeMenu.AddListener (closeMenu);

			thisBlock.GetComponent<InitializeCube> ().openMenu.AddListener (openMenu);
		}

        //removes this function from the listeners of boardMade, and adds its sister function in its place, InitializeButtons()
		gameState.boardMade.RemoveListener (InitializeButtonsStart);
		gameState.boardMade.AddListener (InitializeButtons);
	}


    /// <summary>
    /// This function runs once the board has been set up, with a legal path from a forest to a town,
    /// and the player presses the initialize board button.
    /// </summary>
	private void InitializeButtons ()
	{
        //get all towerBase blocks
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("TowerBase");

        //And add listeners to their unity events.
		foreach (GameObject thisBlock in blocks) {
			thisBlock.GetComponent<CreateTower> ().closeMenu.AddListener (closeMenu);

			thisBlock.GetComponent<CreateTower> ().openMenu.AddListener (openMenu);
		}
	}

    /// <summary>
    /// Q : what does this do and how does it work?
    /// A : all it dose is set buildmenu to true; 
	///     However the program uses the bool to insure that you can't click on tiles(opening new menus) while the menu is open.
	/// Q : Also can it be simplified?
	/// A : Probably not in our current system.
    /// </summary>
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



