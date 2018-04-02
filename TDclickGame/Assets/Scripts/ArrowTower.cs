using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
<<<<<<< Updated upstream
	private float range;
	private float rate;
	private int damage;
	private int baseCost;
=======
    private float range;
    private float rate;
	private int damage;
	private int baseCost = 5;
>>>>>>> Stashed changes

	protected override void Start ()
	{
		range = 2;
		rate = 2;
		damage = 1;
		baseCost = 1;
		base.Start ();
	}

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
			leader.GetComponentInParent<MonsterAI> ().ChangeHealth (-1 * damage);
		}
	}


	protected override void Upgrade ()
	{
		Debug.Log ("Clicked on Arrow");
		if (gameState.GetGold () >= damage * damage) {
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

<<<<<<< Updated upstream
	public override int GetDamage ()
	{
		return damage;
	}
=======
    public override int GetDamage()
    {
        return damage;
    }
>>>>>>> Stashed changes

	public override int GetBaseCost ()
	{
		return baseCost;
	}
}
