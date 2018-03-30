﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireTower : Tower
{
    private float range;
    private float rate;
    private int damage;

    protected override void Start()
    {
        range = 1;
        rate = 1;
        damage = 1;
        base.Start();
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
        int cost = damage * damage * 10;
		if (gameState.GetGold () >= cost) {
			gameState.ChangeGold (-1 * cost);
			damage = damage + 1;
            rate = rate * .9f;
            range = range * 1.1f;
			Debug.Log ("Uppgraded Bonfire for " + cost.ToString() + ". New DPS: " + damage * 1/rate);
		} else {
			Debug.Log ("Not enough gold, needed " + damage * damage * 10);
		}
	}

	protected override float GetRate ()
	{
		return rate;
	}

    protected override float GetRange()
    {
        return range;
    }
}

