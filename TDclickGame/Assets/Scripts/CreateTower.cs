using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTower : MonoBehaviour {

    public Canvas towerCreationMenu;
    private GameState gameState;
    public GameObject bonfireTower;
    private bool instantiated;
    public Button[] towerSelectionButtons;
    public GameObject[] Towers;



	// Use this for initialization
	void Start () {
        gameState = GameObject.FindObjectOfType<GameState>();
        gameState.validClick.AddListener(ClickedOn);
        instantiated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ClickedOn()
    {
        if (gameState.objectClicked == gameObject)
        {
            if (!instantiated)
            {
                towerCreationMenu.gameObject.SetActive(true);
                for(int i = 0; i < towerSelectionButtons.Length; i++)
                {
                    //towerSelectionButtons[i].onClick.AddListener()
                }

                //MakeTower();
            }
        }
    }

    public bool GetTower( out GameObject tower)
    {
        if (instantiated)
        {
            tower = bonfireTower;
            return true;
        }
        tower = null;
        return false;
    }

    private void MakeTower(int index)
    {
        if (gameState.GetGold() >= 10)
        {
            bonfireTower = Instantiate(bonfireTower, this.transform.position + Vector3.up, Quaternion.identity);
            gameState.ChangeGold(-10);

            gameState.GetComponent<GameState>().validClick.RemoveListener(ClickedOn);
            instantiated = true;
        }
    }
}
