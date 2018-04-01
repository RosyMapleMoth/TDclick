using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour {

    float timer;

    int money;
    int wave;
    int damage;
    float fireRate;

    public GameObject graphPoint;
    public Text text;

	// Use this for initialization
	void Start () {
        money = 0;
        wave = 0;
        damage = 1;
        fireRate = 1;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer -= 1;
            DoWave();
        }
    }

    void DoWave()
    {
        wave += 1;
        money += wave * 10;
        float dps = damage / fireRate;
        float enemyHealth = wave * 3;
        float hitsToKill = Mathf.Ceil(enemyHealth / damage);
        float secondsToKill = enemyHealth / dps;
        Debug.Log("Wave " + wave + 
            ".\nEnemy Health: " + enemyHealth.ToString() + 
            ".\nDamage: " + damage +
            ".\nFire Rate: " + fireRate.ToString() +
            ".\nDPS: " + dps.ToString() + 
            ".\nHits To Kill: " + hitsToKill.ToString() + 
            ".\nSeconds to kill: "+ secondsToKill.ToString() + ".");
        text.text = "Wave " + wave +
            ".\nEnemy Health: " + enemyHealth.ToString() +
            ".\nDamage: " + damage +
            ".\nFire Rate: " + fireRate.ToString() +
            ".\nDPS: " + dps.ToString() +
            ".\nHits To Kill: " + hitsToKill.ToString() +
            ".\nSeconds to kill: " + secondsToKill.ToString() + ".";
        int upgradeCost = damage * damage * 10;
        if (upgradeCost <= money)
        {
            money -= upgradeCost;
            damage += 1;
            fireRate *= .9f;
        }
        Vector3 graphPosition = new Vector3((float)wave % 10f / 2f, 0, secondsToKill);
        Instantiate(graphPoint, graphPosition, Quaternion.identity);
    }
}
