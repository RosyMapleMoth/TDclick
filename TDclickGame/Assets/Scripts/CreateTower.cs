using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CreateTower : MonoBehaviour {


    public GameObject buttonTemplate;
    public GameObject[] blockPrefabs;
	public UnityEvent openMenu;
	public UnityEvent closeMenu;
    public Canvas towerCreationMenu;
    private GameState gameState;
    public GameObject heldTower;
    private bool instantiated;
    public Button[] towerSelectionButtons;
    public GameObject[] Towers;

	public class intEvent : UnityEvent<int>
	{}


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
            if (!gameState.IsBoardInitialized())
            {
                OpenBlocks();
            }
        else if (!instantiated)
        {
            OpenMenu();
        }
    	}
	}

    public bool GetTower( out GameObject tower)
    {
        if (instantiated)
        {
			tower = heldTower;
            return true;

        }
        tower = null;
        return false;
    }

	public void MakeTower(GameObject towertoInst)
    {
		if (gameState.GetGold() >= towertoInst.GetComponent<Tower>().GetBaseCost())
        {
			heldTower = Instantiate(towertoInst, this.transform.position + Vector3.up, Quaternion.identity);
			gameState.ChangeGold(towertoInst.GetComponent<Tower>().GetBaseCost()*(-1));

            gameState.GetComponent<GameState>().validClick.RemoveListener(ClickedOn);
            instantiated = true;

			CloseMenu ();
        }
    }

    private void MakeBlock(GameObject blockToInst)
    {

    }

	public void OpenMenu()
	{
		openMenu.Invoke ();
		towerCreationMenu.gameObject.SetActive(true);
        towerSelectionButtons[0].onClick.AddListener(() => MakeTower(Towers[0]));
        towerSelectionButtons[1].onClick.AddListener(() => MakeTower(Towers[1]));
        towerSelectionButtons[2].onClick.AddListener(CloseMenu);

        Debug.Log("openeing menu");

	}

	public void CloseMenu()
	{
		closeMenu.Invoke ();
		towerCreationMenu.gameObject.SetActive(false);
		towerSelectionButtons [0].onClick.RemoveAllListeners();
		towerSelectionButtons [1].onClick.RemoveAllListeners();
        towerSelectionButtons [2].onClick.RemoveAllListeners();
        Debug.Log("closing menu");
	}





    private void OpenBlocks()
    {
        openMenu.Invoke();
        towerCreationMenu.gameObject.SetActive(true);

        towerSelectionButtons[0].onClick.AddListener(() => MakeBlock(blockPrefabs[0]));
        towerSelectionButtons[1].onClick.AddListener(() => MakeBlock(blockPrefabs[1]));
        towerSelectionButtons[2].onClick.AddListener(() => MakeBlock(blockPrefabs[2]));
        towerSelectionButtons[3].onClick.AddListener(() => MakeBlock(blockPrefabs[3]));
        towerSelectionButtons[4].onClick.AddListener(CloseBlocks);

        Debug.Log("openeing menu");
    }

    private void CloseBlocks()
    {
        closeMenu.Invoke();
        towerCreationMenu.gameObject.SetActive(false);
        towerSelectionButtons[0].onClick.RemoveAllListeners();
        towerSelectionButtons[1].onClick.RemoveAllListeners();
        towerSelectionButtons[2].onClick.RemoveAllListeners();
        towerSelectionButtons[3].onClick.RemoveAllListeners();
        towerSelectionButtons[4].onClick.RemoveAllListeners();

        Debug.Log("closing menu");
    }



    private void setupMenu(int numButtons)
    {
        // clear all buttons
        foreach (Button button in towerSelectionButtons)
        {
            Destroy(button);
        }

        for (int i =0 ; i < numButtons; i++)
        {
            GameObject tempButton = Instantiate(buttonTemplate);
            tempButton.transform.SetParent(towerCreationMenu.transform.GetChild(0).GetChild(0).GetChild(0));
            towerSelectionButtons[i] = tempButton.GetComponent<Button>();
        }

    }


    
}
