using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour {

    private List<GameObject> enemiesInRange;
    private float timer;
    protected GameState gameState;
    protected int damage;

	// Use this for initialization
	void Start () {
        enemiesInRange = new List<GameObject>();
        timer = 0;
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        damage = 1;
        gameState.validClick.AddListener(ClickedOn);
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

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            Debug.Log("thing added" + other.ToString());
            other.gameObject.GetComponentInParent<MonsterAI>().Death.AddListener(EnemyDeath);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject);
    }

    private void EnemyDeath (GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
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

    private void ClickedOn ()
    {
        Debug.Log("Clicked on Bonfire");
        if (gameState.objectClicked != null && gameState.objectClicked == this.gameObject && gameState.GetGold() >= damage * damage * 10)
        {
            gameState.ChangeGold(-1 * damage * damage * 10);
            damage = damage + 1;
            Debug.Log("Uppgraded Bonfire");
        }
    }
}
