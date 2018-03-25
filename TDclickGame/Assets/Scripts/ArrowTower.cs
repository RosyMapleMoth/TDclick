using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower {
	
	// Update is called once per frame
	void Update () {

        AddTimer();

        if (GetTimer() > 2)
        {
            GameObject leader = null;
            float leaderDistance = 0;
            SetTimerZero();

            foreach (var enemy in GetEnemies())
            {
                if (enemy != null && enemy.GetComponentInParent<MonsterAI>().GetDistance() > leaderDistance)
                {
                    leader = enemy;
                    leaderDistance = leader.GetComponentInParent<MonsterAI>().GetDistance();
                }
            }

            if (leader != null)
            {
                leader.GetComponentInParent<MonsterAI>().ChangeHealth(-2);
            }
        }
    }

    private void ClickedOn ()
    {
        Debug.Log("Clicked on Arrow");
        if (gameState.objectClicked == this.gameObject && gameState.GetGold() >= damage * damage)
        {
            gameState.ChangeGold(-1 * damage * damage);
            damage = damage + 1;
            Debug.Log("Upgraded Arrow");
        }
    }
}
