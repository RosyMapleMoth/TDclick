using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPoint : MonoBehaviour {

    float timer;

	// Use this for initialization
	void Start () {
        timer = 7;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
	}
}
