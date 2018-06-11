using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CreateTower : MonoBehaviour
{
	public UnityEvent openMenu;
	public UnityEvent closeMenu;
    public Menu menu;
    private GameState gameState;
    public GameObject heldTower;
    private bool instantiated;
    public Button[] towerSelectionButtons;
    public GameObject[] towers;

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

    /// <summary>
    /// ClickedOn is called when the mouse is clicked.
    /// First, it checks if the object clicked was it's
    /// gameobject, and then if it was it calls
    /// menu's AddTowerListeners function.
    /// </summary>
    private void ClickedOn()
    {
        if (gameState.objectClicked == gameObject)
        {
            if (!instantiated)
            {
                menu.AddTowerListeners(MakeTower, towers);
            }
        }
	}

    /// <summary>
    /// If a tower has been created on this tower block,
    /// returns true and that tower, otherwise returns false and null
    /// </summary>
    /// <param name="tower"></param>
    /// <returns></returns>
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

    /// <summary>
    /// If there is enough gold to build the tower, creates the tower
    /// and removes the gold to pay for the tower.
    /// Then removes this funcction from the listeners
    /// on validClick, marks itself as instantiated and
    /// tells menu to remove the listeners.
    /// </summary>
    /// <param name="Tower To Instantiate"></param>
	public void MakeTower(GameObject towertoInst)
    {
		if (gameState.GetGold() >= towertoInst.GetComponent<Tower>().GetBaseCost())
        {
			heldTower = Instantiate(towertoInst, this.transform.position + Vector3.up, Quaternion.identity);
			gameState.ChangeGold(towertoInst.GetComponent<Tower>().GetBaseCost()*(-1));

            gameState.GetComponent<GameState>().validClick.RemoveListener(ClickedOn);
            instantiated = true;

			menu.RemoveListeners ();
        }
    }

	// public void OpenMenu()
	// {
	// 	openMenu.Invoke ();
	// 	menu.gameObject.SetActive(true);
    //     towerSelectionButtons[0].onClick.AddListener(() => MakeTower(towers[0]));
    //     towerSelectionButtons[1].onClick.AddListener(() => MakeTower(towers[1]));
    //     towerSelectionButtons[2].onClick.AddListener(() => MakeTower(towers[2]));
    //     towerSelectionButtons[3].onClick.AddListener(() => MakeTower(towers[3]));
    //     towerSelectionButtons[4].onClick.AddListener(CloseMenu);

    //     Debug.Log("openeing menu");

	// }

	// public void CloseMenu()
	// {
	// 	closeMenu.Invoke ();
	// 	menu.gameObject.SetActive(false);
	// 	towerSelectionButtons [0].onClick.RemoveAllListeners();
	// 	towerSelectionButtons [1].onClick.RemoveAllListeners();
    //     towerSelectionButtons [2].onClick.RemoveAllListeners();
    //     towerSelectionButtons [3].onClick.RemoveAllListeners();
    //     towerSelectionButtons [4].onClick.RemoveAllListeners();
    //     Debug.Log("closing menu");
	// }

}
