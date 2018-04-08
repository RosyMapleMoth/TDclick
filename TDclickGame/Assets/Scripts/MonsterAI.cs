using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{

	public int Health;
	private int Speed;
	private GameObject nextPath;
	public GameObject startPath;
	public Material damageMat;
	public Material normalMat;

	public class GameObjectEvent : UnityEvent<GameObject>
	{

	}

	public GameObjectEvent Death;
	public float distance;
	private GameState gameState;

	private float damageTimer;
	private bool alive;

	// Use this for initialization
	void Start ()
	{
		Speed = 1;
		if (startPath == null) {
			startPath = GameObject.FindGameObjectWithTag ("Start");
		}
		nextPath = startPath;
		damageTimer = .2f;
		distance = 0;
		gameState = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		Health = gameState.GetWave () * 3;
		alive = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Health > 0) {
			Vector3 direction = nextPath.transform.position - this.transform.position;

			float movementThisFrame = Speed * Time.deltaTime;
			distance += movementThisFrame;

			if (direction.magnitude <= movementThisFrame) {
				try {
					nextPath = nextPath.GetComponent<Path> ().nextPath;
				} catch {
					gameState.LoseLife ();
					Death.Invoke (this.gameObject);
					GameObject.Destroy (this.gameObject);
				}
			} else {
				transform.Translate (direction.normalized * movementThisFrame, Space.World);
				Quaternion targetRotation = Quaternion.LookRotation (direction);
				this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * Speed);
			}
		}

		if (damageTimer > 0 && alive) {
			gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (Color.black, Color.red, damageTimer);

			damageTimer -= Time.deltaTime;

			if (damageTimer < 0) {
				gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.black;
				damageTimer = 0;
			}
		} else if (damageTimer > 0 && !alive) {
			gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (Color.clear, Color.red, damageTimer);

			damageTimer -= Time.deltaTime;

			if (damageTimer < 0) {
				gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.clear;
				damageTimer = 0;
				GameObject.Destroy (gameObject);
			}
		}     
	}

	public void ChangeHealth (int change)
	{
		if (Health > 0) {
			Health += change;
			if (change < 0) {
				damageTimer = 1;

				if (Health < 1) {
					gameState.ChangeGold (gameState.GetWave ());
					gameState.IncreaseScore (gameState.GetWave ());
					Death.Invoke (this.gameObject);
					alive = false;
				}
			}
		} else {
			Debug.Log ("Damaged dead Enemy");
			Death.Invoke (this.gameObject);
			alive = false;
		}
	}

	public bool isAlive ()
	{
		return alive;
	}

	public float GetDistance ()
	{
		return distance;
	}
}
