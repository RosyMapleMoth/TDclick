using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower {

    private float range;
    private float rate;
    private int damage;
    private int level;
    private int baseCost = 10;

    protected override void Start()
    {
        range = 1;
        rate = 0f;
        damage = 0;
        level = 1;
        base.Start();

        TowerRange towerRange = base.RangeObject.GetComponent<TowerRange>();
        towerRange.monsterEntered.AddListener(DetectedEnemy);
        towerRange.monsterExited.AddListener(EnemyLeft);
    }

    // Update is called once per frame
    protected override void Update () {
		
	}

    public override float GetRate()
    {
        return rate;
    }

    public override float GetRange()
    {
        return range;
    }

    public override int GetDamage()
    {
        return damage;
    }

    public override int GetBaseCost()
    {
        return baseCost;
    }

    public override int GetUpgradeCost()
    {
        return level * level * baseCost;
    }

    protected override void Upgrade()
    {
        if (base.gameState.GetGold() >= GetUpgradeCost())
        {
            base.gameState.ChangeGold(-1 * GetUpgradeCost());
            range += .1f;
            level += 1;
        }
    }

    protected override void DealDamage()
    {

    }

    private void DetectedEnemy(GameObject enemy)
    {
        enemy.GetComponentInParent<MonsterAI>().MultiplySpeed(.5f);
    }

    private void EnemyLeft(GameObject enemy)
    {
        enemy.GetComponentInParent<MonsterAI>().MultiplySpeed(2f);
    }
}
