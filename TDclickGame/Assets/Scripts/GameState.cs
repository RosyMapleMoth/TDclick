using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameState : MonoBehaviour {

    private int gold;
    public Text goldCount;
    private int score;
    public Text scoreCount;
    private int lives;
    public Text livesCount;
    public GameObject objectClicked;
    public UnityEvent validClick;
    public Text gameOver;

	// Use this for initialization
	void Start () {
        gold = 0;
        validClick = new UnityEvent();
        score = 0;
        lives = 10;
	}
	
	// Update is called once per frame
	void Update ()
    {
        goldCount.text = "Gold: " + gold.ToString();
        scoreCount.text = "Score: " + score.ToString();
        livesCount.text = "Lives: " + lives.ToString();


        if (lives < 1)
        {
            gameOver.text = "Game Over!\nYour Score: " + score.ToString();
        }
    }

    public void ChangeGold (int change)
    {
        gold += change;
    }

    public int GetGold ()
    {
        return gold;
    }

    public void LoseLife ()
    {
        lives -= 1;
    }

    public void IncreaseScore (int extraScore)
    {
        score += extraScore;
    }
}
