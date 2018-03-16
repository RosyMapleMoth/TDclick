using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    private int gold;
    public Text goldcount;

	// Use this for initialization
	void Start () {
        gold = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        goldcount.text = gold.ToString();
    }

    public void ChangeGold (int change)
    {
        gold += change;
    }

    public int GetGold ()
    {
        return gold;
    }
}
