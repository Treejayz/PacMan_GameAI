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
		clyde = GameObject.Find("Clyde(Clone)") ? GameObject.Find("Clyde(Clone)") : GameObject.Find("Clyde 1(Clone)");
		pinky = GameObject.Find("Pinky(Clone)") ? GameObject.Find("Pinky(Clone)"): GameObject.Find("Pinky 1(Clone)");
		inky = GameObject.Find("Inky(Clone)") ? GameObject.Find("Inky(Clone)"): GameObject.Find("Inky 1(Clone)");
		blinky = GameObject.Find("Blinky(Clone)") ? GameObject.Find("Blinky(Clone)"): GameObject.Find("Blinky 1(Clone)");
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
			clyde.GetComponent<GhostAI>().fleeing = true;
			pinky.GetComponent<GhostAI>().fleeing = true;
			inky.GetComponent<GhostAI>().fleeing = true;
			blinky.GetComponent<GhostAI>().fleeing = true;

			clyde.GetComponent<GhostAI>().chooseDirection = true;
			pinky.GetComponent<GhostAI>().chooseDirection = true;
			inky.GetComponent<GhostAI>().chooseDirection = true;
			blinky.GetComponent<GhostAI>().chooseDirection = true;

			if (!clyde.GetComponent<GhostAI> ().dead) {
				clyde.GetComponent<Movement> ().MSpeed = 3f;
			}
			if (!pinky.GetComponent<GhostAI> ().dead) {
				pinky.GetComponent<Movement> ().MSpeed = 3f;
			}
			if (!inky.GetComponent<GhostAI> ().dead) {
				inky.GetComponent<Movement> ().MSpeed = 3f;
			}
			if (!blinky.GetComponent<GhostAI> ().dead) {
				blinky.GetComponent<Movement> ().MSpeed = 3f;
			}
        }
        

        if (collision.CompareTag ("ghost")) {
			gameManager.GetComponent<scoreManager>().updateLives(collision);

			if (gameManager.GetComponent<scoreManager>().powerPellet && collision.GetComponent<GhostAI>().fleeing)
            {
                collision.GetComponent<Animator>().SetBool("Dead", true);
				collision.GetComponent<GhostAI> ().dead = true;
				collision.GetComponent<GhostAI> ().fleeing = false;
				collision.GetComponent<Movement> ().MSpeed = 7.5f;
				collision.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
                //set state to path find back to start
            }
        }
    }


}
