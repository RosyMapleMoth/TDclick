using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour {

    public int Health;
    private int Speed;
    private GameObject nextPath;
    public GameObject startPath;
    public Material damageMat;
    public Material normalMat;
    public UnityEvent Death;
    public float distance;
    private GameState gameState;

    private float damageTimer;

	// Use this for initialization
	void Start () {
        Health = 5;
        Speed = 1;
        if (startPath == null)
        {
            startPath = GameObject.FindGameObjectWithTag("Start");
        }
        nextPath = startPath;
        damageTimer = .2f;
        distance = 0;
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Health > 0)
        {
            Vector3 direction = nextPath.transform.position - this.transform.position;

            float movementThisFrame = Speed * Time.deltaTime;
            distance += movementThisFrame;

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
        }

        if (damageTimer < .2f)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > .2f)
            {
                gameObject.GetComponentInChildren<MeshRenderer>().material = normalMat;

                if (Health < 1)
                {
                    gameState.ChangeGold(1);
                    Death.Invoke();
                    GameObject.Destroy(this.gameObject);
                }
            }
        }      
    }

    public void ChangeHealth (int change)
    {
        Health += change;
        if(change < 0)
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = damageMat;
            damageTimer = 0;
        }
    }

    public float GetDistance()
    {
        return distance;
    }
}
