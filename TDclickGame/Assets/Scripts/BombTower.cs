using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTower : Tower {

    private float rangeCelling;
    private float rangeFloor;
    private float rate;
    private int damage;
    private int level;
    private int baseCost = 20;
    private float bombRange;
    public GameObject bomBomb;
    private bool fireWhenReady;


    // Use this for initialization
    protected override void Start ()
    {
        rangeCelling = 3f;
        rangeFloor = 1f;
        rate = 5f;
        bombRange = 1f;
        damage = 5;
        level = 1;
        gameState = GameObject.FindObjectOfType<GameState>();
        fireWhenReady = false;

        base.Start();
	}

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        if (fireWhenReady)
        {
            if (DealActualDamage())
            {
                base.SetTimerZero();
                fireWhenReady = false;
            }
        }
	}


    protected override void Upgrade()
    {
        if (gameState.GetGold() >= level * level * baseCost)
        {
            gameState.ChangeGold(-1 * level * level * baseCost);
            rate -= .01f;
            if (rate < 3f)
            {
                rate = 3f;
            }
            bombRange += .01f;
            if (bombRange > 1.5f)
            {
                bombRange = 1.5f;
            }
            damage = damage + 5;
            level += 1;
        }
    }

    protected override void DealDamage()
    {
        fireWhenReady = true;
    }

    private bool DealActualDamage()
    {
        float leaderHealth = 0;
        GameObject leader = null;

        foreach (var enemy in GetEnemies())
        {
            if (enemy != null)
            {
                MonsterAI enemyAI = enemy.GetComponentInParent<MonsterAI>();
                if (enemyAI.isAlive() && enemyAI.Health > leaderHealth && Mathf.Abs(Vector3.Distance(transform.position, enemy.transform.position)) > rangeFloor)
                {
                    leader = enemy;
                    leaderHealth = enemyAI.Health;
                }
            }
        }


        //launch bomb at enemy
        if (leader != null)
        {
            GameObject GO = Instantiate(bomBomb, transform.position, Quaternion.identity);
            BombScript BS = GO.GetComponent<BombScript>();
            BS.target = leader;
            BS.explosionRange = bombRange;
            BS.damage = damage;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override float GetRate()
    {
        return rate;
    }

    public override float GetRange()
    {
        return rangeCelling;
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

}
