using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathStart : MonoBehaviour
{

	public GameObject nextPath;
	public GameObject Enemy;
	private float counter;
	private int count;
	private int waveBreak;
	private int wave;

	delegate void UpdateFunction ();

	UpdateFunction updateFunction;

	// Use this for initialization
	void Start ()
	{
		wave = 0;
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
		enemy.GetComponent<MonsterAI> ().ChangeHealth (wave * 3);
	}

	private void NormalUpdate ()
	{
		counter += Time.deltaTime;

		if (counter > 1) {
			counter -= 1;
			SpawnEnemy ();
			count += 1;
		}

		if (count % 10 == 0) {
			updateFunction = WaveBreakUpdate;
		}

	}

	private void WaveBreakUpdate ()
	{
		counter += Time.deltaTime;

		if (counter > waveBreak) {
			wave += 1;
			updateFunction = NormalUpdate;
		}
	}
}
