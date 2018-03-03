using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStart : MonoBehaviour {

    public GameObject nextPath;
    public GameObject Enemy;
    private float counter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;

        if (counter > 1)
        {
            counter -= 1;
            SpawnEnemy();
        }
	}

    private void SpawnEnemy()
    {
        Instantiate(Enemy, this.transform.position, Quaternion.identity);
    }
}
