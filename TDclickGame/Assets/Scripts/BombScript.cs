using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public float explosionRange;
    public GameObject target;
    private float timer;
    private Vector3 start;
    private Vector3 end;
    public GameObject range;
    private bool fuseLit;
    private List<GameObject> enemiesInRange;
    public int damage;


    // Use this for initialization
    void Start () {
        fuseLit = false;
        timer = 0f;
        end = target.transform.position;
        start = transform.position;
        enemiesInRange = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!fuseLit)
        {
            transform.position = Vector3.Lerp(start, end, timer);
            timer = timer + Time.deltaTime;

            if (Mathf.Abs(Vector3.Distance(transform.position, end)) <= .2f)
            {
                LightFuse();
            }
        }
        else
        {
            Explode();
        }
	}

    private void Explode()
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            MonsterAI monster = enemy.GetComponentInParent<MonsterAI>();

            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            enemyDistance -= explosionRange;
            enemyDistance /= explosionRange;

            int enemyDamage = (int)Mathf.Ceil(enemyDistance * (float)damage);


            monster.ChangeHealth(enemyDamage);
        }

        GameObject.Destroy(gameObject);
    }

    private void LightFuse()
    {
        fuseLit = true;

        range = Instantiate(this.range);
        range.transform.parent = transform;
        range.transform.SetPositionAndRotation(new Vector3(0, -.1f, 0) + gameObject.transform.position, Quaternion.identity);

        TowerRange towerRange = range.GetComponent<TowerRange>();
        towerRange.monsterEntered = new TowerRange.GameObjectEvent();
        towerRange.monsterExited = new TowerRange.GameObjectEvent();

        towerRange.monsterEntered.AddListener(DetectedEnemy);
        towerRange.monsterExited.AddListener(EnemyLeft);

        float towerRangeValue = explosionRange * 2 + 1;
        range.transform.localScale = new Vector3(towerRangeValue, 1, towerRangeValue);
    }

    private void DetectedEnemy(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other);
            other.GetComponentInParent<MonsterAI>().Death.AddListener(EnemyLeft);
        }
    }

    private void EnemyLeft(GameObject other)
    {
        enemiesInRange.Remove(other);
    }
}
