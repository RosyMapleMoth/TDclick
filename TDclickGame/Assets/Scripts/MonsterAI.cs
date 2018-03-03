using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    private int Health;
    private int Speed;
    private GameObject nextPath;
    public GameObject startPath;

	// Use this for initialization
	void Start () {
        Health = 10;
        Speed = 1;
        if (startPath == null)
        {
            startPath = GameObject.FindGameObjectWithTag("Start");
        }
        nextPath = startPath;
	}

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = nextPath.transform.position - this.transform.position;

        float movementThisFrame = Speed * Time.deltaTime;

        if (direction.magnitude <= movementThisFrame)
        {
            try
            {
                nextPath = nextPath.GetComponent<Path>().nextPath;
            }
            catch
            {
                Debug.Log("We hit the end of the path!");
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(direction.normalized * movementThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * Speed);
        }

        if (Health < 1)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void changeHealth (int change)
    {
        Health += change;
    }
}
