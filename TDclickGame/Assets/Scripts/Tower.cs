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
	private float fireRate;
	public GameObject RangeObject;

	// Use this for initialization
	protected virtual void Start ()
	{
		enemiesInRange = new List<GameObject> ();
		timer = 0;
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();

		gameState.validClick.AddListener (ClickedOn);
		gameState.newWave.AddListener (NewWave);
		fireRate = GetRate ();

		RangeObject = Instantiate (this.RangeObject);
		RangeObject.transform.parent = this.transform;
		RangeObject.transform.SetPositionAndRotation (new Vector3 (0, -2.4f, 0) + gameObject.transform.position, Quaternion.identity);

		TowerRange towerRange = RangeObject.GetComponent<TowerRange> ();
		towerRange.monsterEntered = new TowerRange.GameObjectEvent ();
		towerRange.monsterExited = new TowerRange.GameObjectEvent ();

		towerRange.monsterEntered.AddListener (DetectedEnemy);
		towerRange.monsterExited.AddListener (EnemyLeft);

		SetRange ();
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		timer += Time.deltaTime;
		if (timer > fireRate) {
			timer = 0;
			DealDamage ();
		}

	}

	private void DetectedEnemy (GameObject other)
	{
		if (other.CompareTag ("Enemy")) {
			enemiesInRange.Add (other);
			other.GetComponentInParent<MonsterAI> ().Death.AddListener (EnemyDeath);
		}
	}

	private void EnemyLeft (GameObject other)
	{
		enemiesInRange.Remove (other);
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
		if (gameState.objectClicked != null) {
			GameObject tower;
			if (gameState.objectClicked == this.gameObject) {
				Upgrade ();
				SetRange ();
				fireRate = GetRate ();
			} else if (gameState.objectClicked.CompareTag ("TowerBase") && gameState.objectClicked.GetComponent<CreateTower> ().GetTower (out tower)) {
				if (tower == this.gameObject) {
					Upgrade ();
					SetRange ();
					fireRate = GetRate ();
				}
			}
		}
	}

	private void SetRange ()
	{
		float range = GetRange ();
		float towerRangeValue = range * 2 + 1;
		RangeObject.transform.localScale = new Vector3 (towerRangeValue, 1, towerRangeValue);
	}

	private void NewWave ()
	{
		enemiesInRange = new List<GameObject> ();
	}

	protected abstract void Upgrade ();

	protected abstract void DealDamage ();

	public abstract float GetRate ();

	public abstract float GetRange ();

	public abstract int GetDamage ();

	public abstract int GetBaseCost ();

    public abstract int GetUpgradeCost();
}
