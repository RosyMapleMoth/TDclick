using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{

	private Gold gold;
	public Text goldCount;
	private int score;
	public Text scoreCount;
	private int lives;
	public Text livesCount;
	public GameObject objectClicked;
	public GameObject objectHovered;
	public UnityEvent validClick;
	public UnityEvent newWave;
	public UnityEvent waveOver;
	public UnityEvent boardMade;
	public Text gameOver;
	public Button startButton;
	private bool boardInitialized;
	private bool finishedWave;

	private List<GameObject> activeEnemies;
	private int wave;
	public Text waveCount;
	public Text mouseStats;

	// Use this for initialization
	void Start ()
	{
        gold = new Gold();

        gold.setAmount(10);
		//validClick = new UnityEvent(); //This isnt needed because apparently UnityEvents are automatically created.
		//This was activating AFTER the bonfire and BEFORE the arrow tower, so only the arrow tower was listening.
		score = 0;
		lives = 10;
		wave = 0;

		boardInitialized = false;

		waveOver.AddListener (WaveFinished);
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateText ();

		if (finishedWave) {
			CheckWave ();
		}
	}

	private void CheckWave ()
	{
		if (activeEnemies.Count == 0) {
			IncrementWave ();
		}
	}

	private void UpdateText ()
	{
		goldCount.text = "Gold: " + gold.getIntAmount().ToString ();
		scoreCount.text = "Score: " + score.ToString ();
		livesCount.text = "Lives: " + lives.ToString ();
		waveCount.text = "Wave: " + wave.ToString ();

		if (lives < 1) {
			gameOver.text = "Game Over!\nYour Score: " + score.ToString ();
		}

		if (objectHovered != null) {
			if (objectHovered.CompareTag ("Enemy")) {
				mouseStats.text = "Enemy Health: " + objectHovered.GetComponentInParent<MonsterAI> ().Health + " / " + wave * 3;
			} else if (objectHovered.CompareTag ("Tower")) {
				Tower tower = objectHovered.GetComponent<Tower> ();
				float dps = (float)tower.GetDamage () / tower.GetRate ();
				mouseStats.text = "Damage: " + tower.GetDamage () +
				".\nFire Rate: " + tower.GetRate ().ToString () +
				".\nDPS: " + dps.ToString () +
				".\n\nNext Upgrade: " + tower.GetDamage () * tower.GetDamage () * tower.GetBaseCost () + " gold.";
			} else if (objectHovered.CompareTag ("Start")) {
				mouseStats.text = "Monster Forest.";
			} else if (objectHovered.CompareTag ("End")) {
				mouseStats.text = "The City.";
			} else if (objectHovered.CompareTag ("Road")) {
				mouseStats.text = "The Road.";
			} else if (objectHovered.CompareTag ("TowerBase")) {
				GameObject towerObject;
				if (objectHovered.GetComponent<CreateTower> ().GetTower (out towerObject)) {
					Tower tower = towerObject.GetComponent<Tower> ();
					float dps = (float)tower.GetDamage () / tower.GetRate ();
					mouseStats.text = "Damage: " + tower.GetDamage () +
					".\nFire Rate: " + tower.GetRate ().ToString () +
					".\nDPS: " + dps.ToString () +
					".\n\nNext Upgrade: " + tower.GetDamage () * tower.GetDamage () * tower.GetBaseCost () + " gold.";
				} else {
					mouseStats.text = "Open Field.";
				}
			} else {
				mouseStats.text = "";
			}
		} else {
			mouseStats.text = "";
		}
	}

	public void ChangeGold (int change)
	{
		gold.addAmmount(change);
	}

	public int GetGold ()
	{
		return gold.getIntAmount();
	}

	public void LoseLife ()
	{
		lives -= 1;
	}

	public void IncreaseScore (int extraScore)
	{
		score += extraScore;
	}

	private void WaveFinished ()
	{
		finishedWave = true;
	}

	public void IncrementWave ()
	{
		wave += 1;
		activeEnemies = new List<GameObject> ();
		finishedWave = false;
		newWave.Invoke ();
	}

	public int GetWave ()
	{
		return wave;
	}

	public int ActiveEnemies ()
	{
		return activeEnemies.Count;
	}

	public void AddEnemy (GameObject enemy)
	{
		activeEnemies.Add (enemy);
		enemy.GetComponent<MonsterAI> ().Death.AddListener (EnemyDeath);
	}

	private void EnemyDeath (GameObject enemy)
	{
		activeEnemies.Remove (enemy);
	}

	private void StartGame ()
	{
		if (wave == 0) {
			IncrementWave ();
			//startButton.interactable = false;
			startButton.gameObject.SetActive (false);
		}
	}

	public void MapDone ()
	{
		startButton.onClick.AddListener (StartGame);
		startButton.GetComponentInChildren<Text> ().text = "Start";

		boardInitialized = true;
	}

	public bool IsBoardInitialized ()
	{
		return boardInitialized;
	}
}
