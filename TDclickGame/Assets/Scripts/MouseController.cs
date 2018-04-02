using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseController : MonoBehaviour
{

	private GameState gameState;
	private bool buildMenu = false;
	private bool leavingmenu = false;

	// Use this for initialization
	void Start ()
	{
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("TowerBase");


		foreach (GameObject thisBlock in blocks)
		{
			thisBlock.GetComponent<CreateTower>().closeMenu.AddListener(() => (leavingmenu = true));

			thisBlock.GetComponent<CreateTower>().openMenu.AddListener(() => (buildMenu = true));
		}

	}

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.black, 2f);
        RaycastHit hitObject;

		if (Physics.Raycast (ray, out hitObject)) {
			gameState.objectHovered = hitObject.collider.gameObject;


			if (leavingmenu) {
				Debug.Log("leaving menu");
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



	public void openMenu()
	{
		buildMenu = true;
	}

	public void closeMenu()
	{
		buildMenu = false;
	}

}



