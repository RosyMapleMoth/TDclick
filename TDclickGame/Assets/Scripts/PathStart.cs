using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStart : MonoBehaviour
{
    private GameState gameState;
	public GameObject nextPath;
	public GameObject Enemy;
	private float counter;
	private int count;
	private int waveBreak;

	delegate void UpdateFunction ();

	UpdateFunction updateFunction;

	// Use this for initialization
	void Start ()
	{
        //gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        gameState = FindObjectOfType<GameState>();
        waveBreak = 4;
		updateFunction = NormalUpdate;
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateFunction ();
	}

	private void SpawnEnemy ()
	{
		GameObject enemy = Instantiate (Enemy, this.transform.position, Quaternion.identity);
        enemy.GetComponent<MonsterAI>().Death = new MonsterAI.GameObjectEvent();
        enemy.GetComponent<MonsterAI> ().ChangeHealth (gameState.GetWave() * 3);
        gameState.AddEnemy(enemy);
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
		}

	}

	private void WaveBreakUpdate ()
	{
        if (gameState.ActiveEnemies() == 0) {
            gameState.IncrementWave();
			updateFunction = NormalUpdate;
            Debug.Log("Enemy Health: " + gameState.GetWave() * 3);
		}
	}
}
