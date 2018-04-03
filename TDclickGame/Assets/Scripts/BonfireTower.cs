using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireTower : Tower
{
	private float range;
	private float rate;
	private int damage;
	private int baseCost = 10;

	protected override void Start ()
	{
		range = 1;
		rate = .5f;
		damage = 1;
		base.Start ();
	}

	protected override void DealDamage ()
	{
		foreach (var enemy in GetEnemies ()) {
			if (enemy != null) {
				enemy.GetComponentInParent<MonsterAI> ().ChangeHealth (-1 * damage);
			}
		}
	}

	protected override void Upgrade ()
	{
		Debug.Log ("Clicked on Bonfire");
		int cost = damage * damage * baseCost;
		if (gameState.GetGold () >= cost) {
			gameState.ChangeGold (-1 * cost);
			damage = damage + 1;
			Debug.Log ("Uppgraded Bonfire for " + cost.ToString () + ". New DPS: " + damage * 1 / rate);
		} else {
			Debug.Log ("Not enough gold, needed " + damage * damage * 10);
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

