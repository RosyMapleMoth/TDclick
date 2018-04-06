using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    public GameObject buttonTemplate;
    public GameObject[] tiles;
    private const int MAPSIZE = 5;
    public GameState gameState;
    public Button[] towerbuttons;
    public Canvas menu;


    // Use this for initialization
    void Start () {
        CreateMapSkel();

        setupMenu(5);

        towerbuttons[0].gameObject.GetComponentInChildren<Text>().text = "Tower Block";
        towerbuttons[1].gameObject.GetComponentInChildren<Text>().text = "Town Block";
        towerbuttons[2].gameObject.GetComponentInChildren<Text>().text = "Forest Block";
        towerbuttons[3].gameObject.GetComponentInChildren<Text>().text = "Road Block";
        towerbuttons[4].gameObject.GetComponentInChildren<Text>().text = "close menu";

        gameState.startButton.onClick.AddListener(MapCreated);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void CreateMapSkel()
    {
        GameObject temp;
        for (int c = 0; c < MAPSIZE; c++)
        {
            for (int r = 0; r < MAPSIZE; r++)
            {
                temp = Instantiate(tiles[0], new Vector3(r, 0f, c), Quaternion.identity);
                temp.GetComponent<CreateTower>().towerSelectionButtons = towerbuttons;
                temp.GetComponent<CreateTower>().towerCreationMenu = menu;
            }
        }
    }

    private void MapCreated()
    {
        //Check If Path Valid

        gameState.startButton.onClick.RemoveListener(MapCreated);
        setupMenu(3);
        gameState.MapDone();
    }

    private void setupMenu(int numButtons)
    {
        // clear all buttons
        foreach (Button button in towerbuttons)
        {
            Destroy(button);
        }
        towerbuttons = new Button[numButtons];
        for (int i = 0; i < numButtons; i++)
        {
            GameObject tempButton = Instantiate(buttonTemplate);
            tempButton.transform.SetParent(menu.transform.GetChild(0).GetChild(0).GetChild(0));
            towerbuttons[i] = tempButton.GetComponent<Button>();
        }

    }
}
