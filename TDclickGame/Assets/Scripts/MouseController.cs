using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseController : MonoBehaviour
{

	private GameState gameState;

	// Use this for initialization
	void Start ()
	{
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Mouse Clicked");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.black, 2f);
			RaycastHit hitObject;
			if (Physics.Raycast (ray, out hitObject)) {
				Debug.Log ("Object Found = " + hitObject.collider.gameObject.ToString ());
				gameState.objectClicked = hitObject.collider.gameObject.gameObject;
				gameState.validClick.Invoke ();
			}
		} else {
			gameState.objectClicked = null;
		}
	}
}
