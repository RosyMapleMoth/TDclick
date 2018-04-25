using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTower : Tower {

    private float rangeCelling;
    private float rangeFloor;
    private float rate;
    private int damage;
    private int baseCost = 20;
    private float bombRange;
    public GameObject bomBomb;


    // Use this for initialization
    protected override void Start ()
    {
        rangeCelling = 3f;
        rangeFloor = 1f;
        rate = 5f;
        bombRange = 1f;
        damage = 5;
        gameState = GameObject.FindObjectOfType<GameState>();

        base.Start();
	}

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}


    protected override void Upgrade()
    {
        gameState.ChangeGold(-1 * damage * damage * baseCost);
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
    }

    protected override void DealDamage()
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



}
