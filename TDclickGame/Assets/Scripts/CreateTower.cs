using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTower : MonoBehaviour {

    private GameState gameState;
    public GameObject bonfireTower;
    private bool instantiated;

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
                MakeTower();
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

    private void MakeTower()
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
