using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    private List<GameObject> enemiesInRange;
    private float timer;
    protected GameState gameState;
    protected int damage;

	// Use this for initialization
	void Start () {
        enemiesInRange = new List<GameObject>();
        timer = 0;
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.GetComponentInParent<MonsterAI>().ChangeHealth(-damage);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = new Ray(Input.mousePosition, Vector3.down);
            RaycastHit hitObject;
            if (Physics.Raycast(ray, out hitObject))
            {
                if (hitObject.transform == transform.parent.transform)
                {
                    if (gameState.GetGold() > damage)
                    {
                        damage += 1;
                        gameState.ChangeGold(-damage);
                    }
                }
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log("thing added");
           // other.GetComponent<MonsterAI>().Death.AddListener(EnemyDeath(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject);
    }

    public void EnemyDeath(GameObject enemy)
    {

    }

    public float GetTimer()
    {
        return timer;
    }

    public void AddTimer()
    {
        timer += Time.deltaTime;
    }

    public void SetTimerZero()
    {
        timer = 0;
    }

    public List<GameObject> GetEnemies()
    {
        return enemiesInRange;
    }
}
