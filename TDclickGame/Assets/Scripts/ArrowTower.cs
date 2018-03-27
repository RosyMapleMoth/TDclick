using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
	
	protected override void DealDamage ()
	{
		float leaderDistance = 0;
		GameObject leader = null;

		foreach (var enemy in GetEnemies()) {
			if (enemy != null && enemy.GetComponentInParent<MonsterAI> ().GetDistance () > leaderDistance) {
				leader = enemy;
				leaderDistance = leader.GetComponentInParent<MonsterAI> ().GetDistance ();
			}
		}

		if (leader != null) {
			leader.GetComponentInParent<MonsterAI> ().ChangeHealth (-2);
		}
	}


	protected override void Upgrade ()
	{
		Debug.Log ("Clicked on Arrow");
		if (gameState.GetGold () >= damage * damage) {
			gameState.ChangeGold (-1 * damage * damage);
			damage = damage + 1;
			Debug.Log ("Upgraded Arrow");
		} else {
			Debug.Log ("Not Enough gold, needed " + damage * damage);
		}
	}
}
