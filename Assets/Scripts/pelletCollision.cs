using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pelletCollision : MonoBehaviour {

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.name == "Pellet(Clone)")
        {
            Destroy(collision.collider.gameObject);
            gameManager.SendMessage("updateScore");
        }
    }


}
