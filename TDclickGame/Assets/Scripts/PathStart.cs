using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStart : MonoBehaviour
{
	private GameState gameState;
	public GameObject Enemy;
	private float counter;
	private int count;

	delegate void UpdateFunction ();

	UpdateFunction updateFunction;

	// Use this for initialization
	void Start ()
	{
		//gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		gameState = FindObjectOfType<GameState> ();
		updateFunction = BlankUpdate;
		gameState.newWave.AddListener (GameStart);
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateFunction ();
	}

	private void SpawnEnemy ()
	{
		GameObject enemy = Instantiate (Enemy, this.transform.position, Quaternion.identity);
		enemy.GetComponent<MonsterAI> ().Death = new MonsterAI.GameObjectEvent ();
		enemy.GetComponent<MonsterAI> ().startPath = gameObject;
		gameState.AddEnemy (enemy);
	}

	private void NormalUpdate ()
	{
		counter += Time.deltaTime;

		if (counter > 1) {
			counter -= 1;
			SpawnEnemy ();
			count += 1;
		}

		if (/*count % 10 == 0*/ count > 9) {
			count = 0;
			updateFunction = WaveBreakUpdate;
			gameState.newWave.AddListener (NewWave);
			gameState.waveOver.Invoke ();
		}

	}

	private void WaveBreakUpdate ()
	{


	}

	private void NewWave ()
	{
		updateFunction = NormalUpdate;
	}

	private void GameStart ()
	{
		updateFunction = NormalUpdate;
		gameState.newWave.RemoveListener (GameStart);
	}

	private void BlankUpdate ()
	{
	}
}
