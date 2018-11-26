using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {

    public GameObject[] liveSprite;
    public GameObject inky;
    public GameObject clyde;
    public GameObject pinky;
    public GameObject blinky;
    private Text scoreText;
    private Text highText; 
    private int score;
    private int highscore;
    private int newlife;
    private int lives;
    private bool extra = false;
    public bool powerPellet = false;
    private int ghostCount = 0;
    private float timer = 9f;
	// Use this for initialization
	void Start ()
    {
        score = 0;
        newlife = 0;
        lives = 4;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = ("Game"+'\n' + "Score" + '\n' + string.Format("{0:0\n0\n0\n0}", score));
        
        highText = GameObject.Find("HighScore").GetComponent<Text>();
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");

        }
        else
        {
            highscore = 0;
        }
        highText.text = "High"+"\n"+"Score" + '\n' + string.Format("{0:0\n0\n0\n0}", highscore);


        inky = GameObject.Find("Inky(Clone)");
        blinky = GameObject.Find("Blinky(Clone)");
        pinky = GameObject.Find("Pinky(Clone)");
        clyde = GameObject.Find("Clyde(Clone)");
    }

    private void Update()
    {
        if (powerPellet)
        {
            if(timer <= 0f)
            {
                powerPellet = false;
                timer = 9f;
                ghostCount = 0;
                //set all ghosts back to pursue.
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
  
    }
    public void updateScore()
    {
        score += 1;
        newlife += 1;
		scoreText.text = "Game"+'\n' + "Score" + '\n' + string.Format("{0:0\n0\n0\n0}", score);
        if(newlife > 10000 && !extra)
        {
            newlife = 0;
            lives += 1;
            extra = true;
            liveSprite[lives].SetActive(true);
        }
		if(score > highscore)
		{
			highscore = score;
			PlayerPrefs.SetInt("highscore", score);
			PlayerPrefs.Save();
		}
		highText.text = "High"+"\n"+"Score" + '\n' + string.Format("{0:0\n0\n0\n0}", highscore);


    }

    public void updateLives()
    {
        if (!powerPellet)
        {
           
            liveSprite[lives].SetActive(false);
            lives -= 1;
            if (lives == -1)
            {
                //game over motherfucker.
            }
        }
        else
        {
            if(ghostCount == 0)
            {
                for(int i = 0; i < 100; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }

            else if(ghostCount == 1)
            {
                for (int i = 0; i < 200; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }

            else if(ghostCount == 2)
            {
                for (int i = 0; i < 400; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }
            else if(ghostCount == 3)
            {
                for (int i = 0; i < 800; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }
        }
       
    }

    public void updateState()
    {
        powerPellet = !powerPellet;
    }

    private void OnDestroy()
    {
        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
    }


}
