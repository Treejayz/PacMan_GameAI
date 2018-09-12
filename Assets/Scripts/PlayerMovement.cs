﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Movement move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") > 0) {
			//move.move (new Vector2(0,1));

			if (move.checkDirectionClear (new Vector2(0,1))) {
				move._dir = Movement.Direction.up;
			} else {
				move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Horizontal") > 0) {
			//move.move (new Vector2(1,0));

			if (move.checkDirectionClear (new Vector2(1,0))) {
				move._dir = Movement.Direction.right;
			} else {
				move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Vertical") < 0) {
			//move.move (new Vector2(0,-1));

			if (move.checkDirectionClear (new Vector2(0,-1))) {
				move._dir = Movement.Direction.down;
			} else {
				move._dir = Movement.Direction.still;
			}
		} else if (Input.GetAxisRaw ("Horizontal") < 0) {
			//move.move (new Vector2(-1,0));

			if (move.checkDirectionClear (new Vector2(-1,0))) {
				move._dir = Movement.Direction.left;
			} else {
				move._dir = Movement.Direction.still;
			}
		} else {
			//move._dir = Movement.Direction.still;
		}


	}
}