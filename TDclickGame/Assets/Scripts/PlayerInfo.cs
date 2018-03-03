using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public int Money;

    void OnGUI()
    {
        GUI.Box(new Rect(30, 30, 300, 40), Money.ToString());

    }



    void Update ()
    {
		
	}





}
