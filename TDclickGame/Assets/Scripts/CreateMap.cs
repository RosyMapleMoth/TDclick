using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{

	public GameObject buttonTemplate;
	public GameObject initTile;
	private const int MAPSIZE = 5;
	public GameState gameState;
	public Button[] towerbuttons;
	public Canvas menu;
	public GameObject[] towers;

	private GameObject[,] map;


	// Use this for initialization
	void Start ()
	{
		setupMenu (5);

		towerbuttons [0].gameObject.GetComponentInChildren<Text> ().text = "Tower Block";
		towerbuttons [1].gameObject.GetComponentInChildren<Text> ().text = "Town Block";
		towerbuttons [2].gameObject.GetComponentInChildren<Text> ().text = "Road Block";
		towerbuttons [3].gameObject.GetComponentInChildren<Text> ().text = "Forest Block";
		towerbuttons [4].gameObject.GetComponentInChildren<Text> ().text = "Exit Menu";

		gameState.startButton.onClick.AddListener (MapCreated);

		CreateMapSkel ();

		gameState.boardMade.Invoke ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	public void CreateMapSkel ()
	{
		map = new GameObject[MAPSIZE, MAPSIZE];
		GameObject temp;
		for (int c = 0; c < MAPSIZE; c++) {
			for (int r = 0; r < MAPSIZE; r++) {
				temp = Instantiate (initTile, new Vector3 (r, 0f, c), Quaternion.identity, transform);
				temp.GetComponent<InitializeCube> ().cubeSelectionButtons = towerbuttons;
				temp.GetComponent<InitializeCube> ().cubeCreationMenu = menu;

				map [c, r] = temp;
			}
		}
	}

	private void MapCreated ()
	{
		if (InitializeCube.TryForRoad (this)) {
			gameState.startButton.onClick.RemoveListener (MapCreated);
			setupMenu (3);

			towerbuttons [0].gameObject.GetComponentInChildren<Text> ().text = "Bonfire Tower";
			towerbuttons [1].gameObject.GetComponentInChildren<Text> ().text = "Arrow Tower";
			towerbuttons [2].gameObject.GetComponentInChildren<Text> ().text = "Exit Menu";

			GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Initialize Cube");

			foreach (GameObject thisBlock in blocks) {
				thisBlock.GetComponent<InitializeCube> ().InitMap ();
			}

			gameState.boardMade.Invoke ();
			gameState.MapDone ();
		} else {
			Debug.Log ("Invalid Road");
		}
	}

	private void setupMenu (int numButtons)
	{
		if (!menu.gameObject.activeSelf) {
			menu.gameObject.SetActive (true);
		}
		// clear all buttons
		foreach (Button button in towerbuttons) {
			GameObject.Destroy (button.gameObject);
		}
		towerbuttons = new Button[numButtons];
		for (int i = 0; i < numButtons; i++) {
			GameObject tempButton = Instantiate (buttonTemplate);
			tempButton.transform.SetParent (menu.transform.GetChild (0).GetChild (0).GetChild (0));
			towerbuttons [i] = tempButton.GetComponent<Button> ();
		}

		menu.gameObject.SetActive (false);
	}

	public bool GetTile (int column, int row, out GameObject tile)
	{
		if (column < MAPSIZE && row < MAPSIZE && column > -1 && row > -1) {
			tile = map [column, row];
			return true;
		}

		tile = null;
		return false;
	}
}
