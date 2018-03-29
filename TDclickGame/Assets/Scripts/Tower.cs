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
	private float fireRate;
    public GameObject RangeObject;

	// Use this for initialization
	void Start ()
	{
		enemiesInRange = new List<GameObject> ();
		timer = 0;
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		damage = 1;
		gameState.validClick.AddListener (ClickedOn);
		fireRate = InitRate ();

        GameObject rangeObject = Instantiate(RangeObject);
        rangeObject.transform.parent = this.transform;
        rangeObject.transform.SetPositionAndRotation(new Vector3(0, -.1f, 0) + gameObject.transform.position, Quaternion.identity);

        TowerRange towerRange = rangeObject.GetComponent<TowerRange>();
        towerRange.monsterEntered = new TowerRange.GameObjectEvent();
        towerRange.monsterExited = new TowerRange.GameObjectEvent();

        towerRange.monsterEntered.AddListener(DetectedEnemy);
        towerRange.monsterExited.AddListener(EnemyLeft);
        SetRange(rangeObject);
	}
	
	// Update is called once per frame
	void Update ()
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
		if (gameState.objectClicked != null && gameState.objectClicked == this.gameObject) {
			Upgrade ();
		}
	}

	protected abstract void Upgrade ();

	protected abstract void DealDamage ();

	protected abstract float InitRate ();

    protected abstract void SetRange(GameObject rangeObject);
}
