using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{

	public int Health;
	private float speed;
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
        //set the starting speed
		speed = 1f;
        //if the starting path is not set, find one for it.
		if (startPath == null) {
			startPath = GameObject.FindGameObjectWithTag ("Start");
            Debug.Log("Monster did not have set starting path.");
		}

        //Set the initial starting variables
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
            //create a vector pointing from the current location to the destination
			Vector3 direction = nextPath.transform.position - this.transform.position;

            //figure out how far we are moving this frame
			float movementThisFrame = speed * Time.deltaTime;
            //add this frames movement to the total distance
			distance += movementThisFrame;

            //if the amount we would move this frame is less than the distance to
            //the next path block, try to get the path block after that.
			if (direction.magnitude <= movementThisFrame) {
				try {
					nextPath = nextPath.GetComponent<Path> ().nextPath;
				} catch {
                    //If there are no more path blocks, we are at a city, lose a life and kill the enemy.
					gameState.LoseLife ();
					Death.Invoke (this.gameObject);
					GameObject.Destroy (this.gameObject);
				}
			} else {
                //slide the enemy in the direction of the path block, with movementThisFrame as distance
				transform.Translate (direction.normalized * movementThisFrame, Space.World);
                //rotates the enemy to face the direction they are moving.
				Quaternion targetRotation = Quaternion.LookRotation (direction);
				this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * speed);
			}
		}

		if (damageTimer > 0 && alive) {
            //if damaged, set material color to a lerp between red and black using the time since last damaged.
			gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (Color.black, Color.red, damageTimer);
            //decrease the damage timer.
			damageTimer -= Time.deltaTime;

			if (damageTimer < 0) {
                //if the damage timer is now below zero, reset the material to fully black and the timer to 0
				gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.black;
				damageTimer = 0;
			}
		} else if (damageTimer > 0 && !alive) {
            //if the damage timer is still decreasing but enemy is dead, lerp between red and clear instead //NOT WORKING //TODO
            gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.Lerp (Color.clear, Color.red, damageTimer);

			damageTimer -= Time.deltaTime;

			if (damageTimer < 0) {
                //if damage timer now below zero, set the color to fully clear and then destroy the gameobject
				gameObject.GetComponentInChildren<MeshRenderer> ().material.color = Color.clear;
				damageTimer = 0;
				GameObject.Destroy (gameObject);
			}
		}     
	}

    /// <summary>
    /// Change Health adds 'Health Change' to the current health,
    /// activates the damage timer if it subtracted health,
    /// and does initial death preparation if the enemy dies.
    /// </summary>
    /// <param name="Health Change"></param>
	public void ChangeHealth (int change)
	{
		if (Health > 0) {
            //if enemy not dead yet, change their health by the entered amount
			Health += change;
			if (change < 0) {
                //if health is being negatively changed, activate the damage timer
				damageTimer = 1;

				if (Health < 1) {
                    //if enemy is now dead, increase the player's gold and score, invoke Dealth, and set the bool alive to false
                    //in preparation for the removal of the enemy from the game.
					gameState.ChangeGold (gameState.GetWave ());
					gameState.IncreaseScore (gameState.GetWave ());
					Death.Invoke (this.gameObject);
					alive = false;
				}
			}
		} else {
            //TODO: this shouldnt happen but it does. Need to figure out why and then fix it.
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

    /// <summary>
    /// MultiplySpeed takes a float and multiplies the current speed by that value
    /// </summary>
    /// <param name="Speed Multiplier"></param>
    public void MultiplySpeed(float newSpeed)
    {
        speed = newSpeed * speed;
    }

    /// <summary>
    /// Reset speed to original value.
    /// TODO: have original value stored so that there can be different base speeds.
    /// </summary>
    public void ResetSpeed()
    {
        speed = 1f;
    }
}
