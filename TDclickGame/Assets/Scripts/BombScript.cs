using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public float explosionRange;
    public GameObject target;
    private float timer;
    private float rangeTimer;
    private Vector3 start;
    private Vector3 end;
    public GameObject range;
    private bool fuseLit;
    public List<GameObject> enemiesInRange;
    public int damage;
    private bool WaitForMe = false;

    delegate void UpdateFunction();

    private UpdateFunction updateFunction;

    // Use this for initialization
    void Start () {
        fuseLit = false;
        timer = 0f;
        end = target.transform.position;
        start = transform.position;
        rangeTimer = 0f;
        updateFunction = CheckIfLit;
        enemiesInRange = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        updateFunction();
	}

    private void CheckIfLit()
    {
        if (!fuseLit)
        {
            transform.position = Vector3.Lerp(start, end, timer);
            timer = timer + Time.deltaTime;

           // transform.GetChild(0).up = 

            if (Mathf.Abs(Vector3.Distance(transform.position, end)) <= .2f)
            {
                LightFuse();
                updateFunction = WaitUpdate;
            }
        }
    }

    private void WaitUpdate()
    {
        updateFunction = ExplodeUpdate;
    }

    private void ExplodeUpdate()
    {
        Explode();

        GameObject.Destroy(gameObject.transform.GetChild(0).gameObject);

        updateFunction = AftermathUpdate;
    }

    private void AftermathUpdate()
    {
        if (rangeTimer > 0)
        {

            MeshRenderer mesh = range.GetComponent<MeshRenderer>();

            Color color = Color.Lerp(Color.clear, Color.red, rangeTimer);
            color.a = .3f;
            mesh.material.color = color;

            rangeTimer -= Time.deltaTime;

            if (rangeTimer < 0)
            {
                color = Color.clear;
                color.a = .3f;
                mesh.material.color = color;

                rangeTimer = 0f;

                GameObject.Destroy(range);
                GameObject.Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                MonsterAI monster = enemy.GetComponentInParent<MonsterAI>();

                float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
                enemyDistance -= explosionRange;
                enemyDistance /= explosionRange;

                int enemyDamage = (int)Mathf.Ceil(enemyDistance * (float)damage);


                if (enemyDistance > 0)
                {
                    enemyDamage = 0;
                }

                monster.ChangeHealth(enemyDamage);
            }
        }

        rangeTimer = 1f;
    }

    private void LightFuse()
    {
        fuseLit = true;

        range = Instantiate(this.range);
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
