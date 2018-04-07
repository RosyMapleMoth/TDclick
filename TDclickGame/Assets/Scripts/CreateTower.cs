using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CreateTower : MonoBehaviour
{
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
            if (!instantiated)
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

}
