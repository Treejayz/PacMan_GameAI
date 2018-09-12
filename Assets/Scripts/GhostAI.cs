using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {




	Movement move;
	public bool[] dirs = new bool[4];
	private bool[] prevDirs = new bool[4];
	public float releaseTime = 0f;
	public float waitTime = 0f;
	private const float ogWaitTime = 1f;
	public int range = 0;


	public bool chooseDirection = true;
	public int[] choices ;
	public int choice;
	public enum State{
		waiting,
		leaving,
		active,
		fleeing
	}

	public State _state = State.waiting;

	// Use this for initialization
	void Start () {
		move = GetComponent<Movement> ();

	}
	
	// Update is called once per frame
	void Update () {
		switch (_state) {
		case(State.waiting):
			if (releaseTime <= 0f) {
				chooseDirection = true;
				_state = State.leaving;
			}
			releaseTime -= Time.deltaTime;
			break;


		case(State.leaving):
			if (transform.position.x < 13.49f || transform.position.x > 13.51) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (13.5f, transform.position.y, transform.position.z), 3f * Time.deltaTime);
			} else if (transform.position.y < -11.01f || transform.position.y > -10.99f) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, -11f, transform.position.z), 3f * Time.deltaTime);
			} else {
				_state = State.active;
			}
			break;

		case(State.active):
			if (waitTime > 0f) {
				waitTime -= Time.deltaTime;
			}
			for (int i = 0; i < dirs.Length; i++) {
				dirs [i] = move.checkDirectionClear (num2vec (i));
				if ((dirs [i] != prevDirs [i] && waitTime <= 0f) || move._dir == Movement.Direction.still) {
					chooseDirection = true;
				}
				prevDirs [i] = dirs [i];
			}
			if (chooseDirection) {
				choices = new int[4];
				range = 0;
				for (int i = 0; i < dirs.Length; i++) {
					if (dirs [i]) {
						//print (i + " is " + dirs [i]);
						choices[range] = i;
						range++;
					}
				}
				//print (Mathf.FloorToInt (Random.Range (0, range)));
				choice = choices[Mathf.FloorToInt(Random.Range (0, range))];
				if (choice == 0) {
					move._dir = Movement.Direction.up;
				}else if(choice == 1){
					move._dir = Movement.Direction.right;
				}else if(choice == 2){
					move._dir = Movement.Direction.down;
				}else{
					move._dir = Movement.Direction.left;
				}
				chooseDirection = false;
				waitTime = ogWaitTime;
			}




			break;

		}
	}
	Vector2 num2vec(int n){
		if (n == 0) {
			return new Vector2 (0, 1);
		} else if (n == 1) {
			return new Vector2(1,0);
		} else if (n == 2) {
			return new Vector2(0,-1);
		} else {
			return new Vector2(-1,0);
		}
	}

	bool compairDirections(bool[] n, bool[] p){
		for(int i = 0; i < n.Length; i++){
			if (n [i] != p [i]) {
				return false;
			}
		}
		return true;
	}
}
