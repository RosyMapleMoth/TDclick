using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class Tower : MonoBehaviour
{

	private List<GameObject> enemiesInRange;
	private float timer;
	protected GameState gameState;
	public int damage;

	// Use this for initialization
	void Start ()
	{
		enemiesInRange = new List<GameObject> ();
		timer = 0;
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		damage = 1;
		gameState.validClick.AddListener (ClickedOn);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		if (timer > 1) {
			timer = 0;
			DealDamage ();
		}

	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			enemiesInRange.Add (other.gameObject);
			other.gameObject.GetComponentInParent<MonsterAI> ().Death.AddListener (EnemyDeath);
		}
	}

	private void OnTriggerExit (Collider other)
	{
		enemiesInRange.Remove (other.gameObject);
	}

	private void EnemyDeath (GameObject enemy)
	{
		enemiesInRange.Remove (enemy);
	}

	public float GetTimer ()
	{
		return timer;
	}

	public void AddTimer ()
	{
		timer += Time.deltaTime;
	}

	public void SetTimerZero ()
	{
		timer = 0;
	}

	public List<GameObject> GetEnemies ()
	{
		return enemiesInRange;
	}

	public void ClickedOn ()
	{
		if (gameState.objectClicked != null && gameState.objectClicked == this.gameObject) {
			Upgrade ();
		}
	}

	protected abstract void Upgrade ();

	protected abstract void DealDamage ();
}
