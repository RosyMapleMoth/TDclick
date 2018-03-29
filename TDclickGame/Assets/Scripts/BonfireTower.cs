using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireTower : Tower
{


	protected override void DealDamage ()
	{
		foreach (var enemy in GetEnemies ()) {
			if (enemy != null) {
				enemy.GetComponentInParent<MonsterAI> ().ChangeHealth (-damage);
			}
		}
	}

	protected override void Upgrade ()
	{
		Debug.Log ("Clicked on Bonfire");
		if (gameState.GetGold () >= damage * damage * 10) {
			gameState.ChangeGold (-1 * damage * damage * 10);
			damage = damage + 1;
			Debug.Log ("Uppgraded Bonfire");
		} else {
			Debug.Log ("Not enough gold, needed " + damage * damage * 10);
		}
	}

	protected override float InitRate ()
	{
		return 1f;
	}
}
