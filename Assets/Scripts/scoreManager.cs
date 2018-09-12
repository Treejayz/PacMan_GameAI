using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {

    private Text scoreText;
    private Text highText; 
    private int score;
    private int highscore; 
	// Use this for initialization
	void Start ()
    {
        score = 0;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = ("Game"+'\n' + "Score" + '\n' + string.Format("{0:0\n0\n0\n0\n0}", score));
        
        highText = GameObject.Find("HighScore").GetComponent<Text>();
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
        }
        else
        {
            highscore = 0;
        }
        highText.text = "High"+"\n"+"Score" + '\n' + string.Format("{0:0\n0\n0\n0\n0}", highscore);
    }
	
	public void updateScore()
    {
        score += 1;
        scoreText.text = "Score" + '\n' + string.Format("{0:0\n0\n0\n0\n0}", score);


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
