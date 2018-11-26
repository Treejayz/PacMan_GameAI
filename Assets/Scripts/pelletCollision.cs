using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pelletCollision : MonoBehaviour {

    public GameObject clyde;
    public GameObject pinky;
    public GameObject blinky;
    public GameObject inky; 

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        clyde = GameObject.Find("Clyde(Clone)");
        pinky = GameObject.Find("Pinky(Clone)");
        inky = GameObject.Find("Inky(Clone)");
        blinky = GameObject.Find("Blinky(Clone)");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "pellet") {
			Destroy (collision.gameObject);
			gameManager.SendMessage("updateScore");
		}

        if (collision.tag == "powerpellet")
        {
            Destroy(collision.gameObject);
            gameManager.SendMessage("updateState");
            for(int i = 0; i < 5; i++)
            {
                gameManager.SendMessage("updateScore");
            }
            clyde.GetComponent<Animator>().SetBool("Running", true);
            pinky.GetComponent<Animator>().SetBool("Running", true);
            inky.GetComponent<Animator>().SetBool("Running", true);
            blinky.GetComponent<Animator>().SetBool("Running", true);

            //set ghosts to flee. 
        }
        /*
        if(collision.collider.gameObject.name == "Pellet(Clone)")
        {
            Destroy(collision.collider.gameObject);
            gameManager.SendMessage("updateScore");
        }*/
        if (collision.CompareTag ("ghost")) {
            gameManager.SendMessage("updateLives");

            if (gameManager.GetComponent<scoreManager>().powerPellet)
            {
                //turn ghost into eyes
                collision.GetComponent<Animator>().SetBool("Dead", true);

                //set state to path find back to start
            }
        }
    }


}
