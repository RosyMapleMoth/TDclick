using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireTower : Tower
{
	private float range;
	private float rate;
	private int damage;
	private int baseCost = 10;
	private float bonfireTimer;
    private Color thisColor;

	protected override void Start ()
	{
		range = 1;
		rate = 2f;
		damage = 1;
		bonfireTimer = 0f;
        thisColor = base.RangeObject.GetComponent<MeshRenderer>().sharedMaterial.color;
        base.Start ();
	}

	protected override void Update ()
	{
		if (bonfireTimer > 0) {

			MeshRenderer mesh = base.RangeObject.GetComponent<MeshRenderer> ();

			Color color = Color.Lerp (thisColor, Color.red, bonfireTimer);
			color.a = .3f;
			mesh.material.color = color;

			bonfireTimer -= Time.deltaTime * 3f;

			if (bonfireTimer < 0) {
				color = thisColor;
				mesh.material.color = color;

				bonfireTimer = 0;
			}
		}

		base.Update ();
	}

	protected override void DealDamage ()
	{
		foreach (var enemy in GetEnemies ()) {
			if (enemy != null && enemy.GetComponentInParent<MonsterAI> ().isAlive ()) {
				enemy.GetComponentInParent<MonsterAI> ().ChangeHealth (-1 * damage);
				bonfireTimer = 1f;
			}
		}
	}

	protected override void Upgrade ()
	{
		int cost = damage * damage * baseCost;
		if (gameState.GetGold () >= cost) {
			gameState.ChangeGold (-1 * cost);
			damage = damage + 1;

			rate = rate - .01f;
			if (rate < .5f) {
				rate = .5f;
			}
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

    public override int GetUpgradeCost()
    {
        return damage * damage * baseCost;
    }
}

