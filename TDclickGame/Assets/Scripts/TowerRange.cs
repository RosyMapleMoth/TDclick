using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRange : MonoBehaviour {
    public class GameObjectEvent : UnityEvent<GameObject>
    {

    }

    public GameObjectEvent monsterEntered;
    public GameObjectEvent monsterExited;


    // Use this for initialization
    void Start () {

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            monsterEntered.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        monsterExited.Invoke(other.gameObject);
    }
}
