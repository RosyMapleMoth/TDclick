using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{

	private float range;
	private float rate;
	private int damage;
	private int baseCost = 1;
	private float arrowTimer;
	private GameObject line;

	protected override void Start ()
	{
		range = 2;
		rate = 2;
		damage = 1;
		arrowTimer = 0;
		base.Start ();
	}

	protected override void Update ()
	{
		if (arrowTimer > 0) {
			arrowTimer -= Time.deltaTime;

			if (arrowTimer <= 0) {
				GameObject.Destroy (line);
			}
		}

		base.Update ();
	}

	protected override void DealDamage ()
	{
		float leaderDistance = 0;
		GameObject leader = null;

		foreach (var enemy in GetEnemies()) {
			if (enemy != null) {
				MonsterAI enemyAI = enemy.GetComponentInParent<MonsterAI> ();
				if (enemyAI.isAlive () && enemyAI.GetDistance () > leaderDistance) {
					leader = enemy;
					leaderDistance = enemyAI.GetDistance ();
				}
			}
		}

		if (leader != null) {
			leader.GetComponentInParent<MonsterAI> ().ChangeHealth (-1 * damage);

			if (line != null) {
				GameObject.Destroy (line);
			}

			line = new GameObject ();
			LineRenderer render = line.AddComponent<LineRenderer> ();

			render.SetPosition (0, transform.position);
			render.SetPosition (1, leader.transform.position);
			render.endWidth = .05f;
			render.startWidth = .05f;
			render.material.color = Color.black;

			arrowTimer = .1f;
		}
	}


	protected override void Upgrade ()
	{
		Debug.Log ("Clicked on Arrow");
		if (gameState.GetGold () >= damage * damage * baseCost) {
			gameState.ChangeGold (-1 * damage * damage);
			damage = damage + 1;
			rate = rate * .9f;
			range = range * 1.1f;
			Debug.Log ("Upgraded Arrow");
		} else {
			Debug.Log ("Not Enough gold, needed " + damage * damage);
		}
	}

	public override float GetRate ()
	{
		return rate;
	}

	public override float GetRange ()
	{
		return range;
	}

	public override int GetDamage ()
	{
		return damage;
	}

	public override int GetBaseCost ()
	{
		return baseCost;
	}
}
