using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseController : MonoBehaviour
{

	private GameState gameState;
	private bool leavingmenu;

	delegate void UpdateFunction ();

	private UpdateFunction updateFunction;

	// Use this for initialization
	void Start ()
	{
		leavingmenu = false;
		//Setup gameState link
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();

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

			//If the mouse was released, register a click.
			if (Input.GetMouseButtonUp (0)) {
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
		if (leavingmenu)
		{
			updateFunction = TowerDefenseUpdate;
			leavingmenu = false;
		}
	}


	private void ClickerState()
	{
		if (Input.GetMouseButtonUp (0))
		{
//			gameState
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
		updateFunction = NoUpdate;
		leavingmenu = false;
	}

	public void closeMenu ()
	{
		leavingmenu = true;
	}

}



