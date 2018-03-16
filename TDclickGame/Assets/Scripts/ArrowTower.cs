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

        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = new Ray(Input.mousePosition, Vector3.down);
            RaycastHit hitObject;
            if (Physics.Raycast(ray, out hitObject))
            {
                if (hitObject.transform == transform.parent.transform)
                {
                    if (gameState.GetGold() > damage)
                    {
                        damage += 1;
                        gameState.ChangeGold(-damage);
                    }
                }
            }
        }
    }
}
