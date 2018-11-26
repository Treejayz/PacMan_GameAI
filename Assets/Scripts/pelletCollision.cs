using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pelletCollision : MonoBehaviour {

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
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
                //set state to path find back to start
            }
		}
    }


}
