using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {




	Movement move;
    public Vector3 startPos;
	public bool[] dirs = new bool[4];
	private bool[] prevDirs = new bool[4];
	public float releaseTime = 0f;
	private float releaseTimeReset = 0f;
	public float waitTime = 0f;
	private const float ogWaitTime = .1f;
	public int range = 0;

	public bool dead = false;
	public bool fleeing = false;

	//Default: base value of likelyhood of choice for each path
	public float D = 1f;
	//Available: Zero or one based on whether a path is available
	int A = 0;
	//Value: negative 1 or 1 based on direction of pacman
	int V = 1;
	//Fleeing: negative if fleeing
	int F = 1;
	//Priority: calculated preference based on distance of target in one direction weighted by the distance in others (dist/total)
	float P = 0f;
	public float distx = 0f;
	 public float disty = 0f;
	float total = 0f;
	//percent chance of each coice. order is: up < right < 0 < down < left for random choice
	public float[] directions = new float[4];
	//float up = 0f;
	//float right = 0f;
	//float down = 0f;
	//float left = 0f;

	//remember previous choice and make inverse illegal!
	public int[] prevChoices = new int[4]{1,1,1,1};

	public GameObject target;


	GameObject gate;
	GameObject pacMan;

	public bool chooseDirection = true;
	public int[] choices ;
	public float choice;
	public enum State{
		waiting,
		entering,
		leaving,
		active,
		fleeing
	}

	public State _state = State.waiting;

    // Use this for initialization
    private void Awake()
    {
        startPos = this.gameObject.transform.position;
    }
    void Start () {
		move = GetComponent<Movement> ();
		gate = GameObject.Find("Gate(Clone)");
		pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
		releaseTimeReset = releaseTime;

	}

	public void restart(){
		releaseTime = releaseTimeReset;
		transform.position = startPos;
		_state = State.waiting;
	}
	
	// Update is called once per frame
	void Update () {
		switch (_state) {
		case(State.waiting):
			move._dir = Movement.Direction.still;
			if (releaseTime <= 0f) {
				chooseDirection = true;
				gameObject.GetComponent<Animator>().SetBool("Dead", false);
				gameObject.GetComponent<Animator>().SetBool("Running", false);
				gameObject.GetComponent<Animator>().SetInteger ("Direction", 0);
				gameObject.GetComponent<Movement> ().MSpeed = 5f;

				_state = State.leaving;
			}
			gameObject.GetComponent<Animator>().SetBool("Dead", false);
			gameObject.GetComponent<Animator>().SetBool("Running", false);
			gameObject.GetComponent<Animator>().SetInteger ("Direction", 0);
			gameObject.GetComponent<Movement> ().MSpeed = 5f;
			releaseTime -= Time.deltaTime;
			break;


		case(State.leaving):
			gameObject.GetComponent<CircleCollider2D> ().enabled = true;
			gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
			gameObject.GetComponent<Animator> ().SetBool ("Running", false);
			gameObject.GetComponent<Movement> ().MSpeed = 5f;
			fleeing = false;
			dead = false;
			if (transform.position.x < 13.49f || transform.position.x > 13.51) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (13.5f, transform.position.y, transform.position.z), 3f * Time.deltaTime);
			} else if (transform.position.y < -11.01f || transform.position.y > -10.99f) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, -11f, transform.position.z), 3f * Time.deltaTime);
			} else {
				_state = State.active;
			}
			break;

		case(State.active):
			if (dead) {
				target = gate;
				distx = target.transform.position.x - transform.position.x;
				if (dead) {

					disty = target.transform.position.y + 1f - transform.position.y;
				} else {
					disty = target.transform.position.y - transform.position.y;
				}
				total = Mathf.Abs (distx) + Mathf.Abs (disty);
				if (dead) {
					if (disty < 1f && disty >= -.09f && (Mathf.Abs (distx) < 4f)) {
						_state = State.entering;
					}
				}

			} else {
				target = pacMan;
			}
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
				choices = new int[4]{-1,-1,-1,-1};
				range = 0;
				for (int i = 0; i < dirs.Length; i++) {
					if (dirs [i]) {
						//print (i + " is " + dirs [i]);
						choices[range] = i;
						range++;
					}
				}
				//calculate the distance between target and us
				distx = target.transform.position.x - transform.position.x;
				if (dead) {
					
					disty = target.transform.position.y + 1f - transform.position.y;
				} else {
					disty = target.transform.position.y - transform.position.y;
				}
				total = Mathf.Abs (distx) + Mathf.Abs (disty);
				if(dead){
					if (disty < 1f && disty >= -.09f && (Mathf.Abs(distx) < 4f) ) {
						_state = State.entering;
					}


				}
				//set F value based on if we're fleeing or not
				if (fleeing && !dead) {
					F = -1;
				} else {
					F = 1;
				}

				//add decision and randomness to choices
				//Likelyhood = A * (D*V + -1*P*F)
				for (int i = 0; i < 4; i++) {

					//check if x or y direction
					//if 0 or 2, we're on y axis
					if (i % 2 == 0) {
						P = disty / total;
					} else {//otherwise we're on the x axis (1[right] and 3[left])
						P = distx / total;
					}

					//check if path is available
					if (System.Array.IndexOf (choices, i) != -1) {
						A = 1;
					} else {
						A = 0;
					}
					//set V to the sign of i - 2, such that V is neg for up and right, and pos for down and left
					if (i != 2) {
						V = Mathf.Abs (i - 2) / (i - 2);
					} else {
						V = 1;
					}
					//Likelyhood = A * (D*V + -1*P*F)
					//directions[i] = A * (D*V + -1*P*F);
					if (!dead) {
						if (D == 0f) {
							directions [i] = Mathf.Abs(prevChoices [i] * A * (Mathf.Ceil( -1 * P * V)+ .000001f));
						} else {
							directions [i] = Mathf.Abs (prevChoices [i] * A * (D * V + -1 * P * F));
						}

					} else {
						
						directions [i] = Mathf.Abs(prevChoices [i] * A * (Mathf.Ceil( -1 * P * V)+ .000001f));
					}
				}

				//print (Mathf.FloorToInt (Random.Range (0, range)));

				for (int i = 1; i < 4; i++) {
					directions [i] += directions [i - 1];
				}
				choice = Random.Range (0.0000001f , directions [3] );
				//Debug.Log (choice);

				if (choice < directions[0]) {
					move._dir = Movement.Direction.up;
					prevChoices = new int[4]{1,1,0,1};
				}else if(choice < directions[1]){
					move._dir = Movement.Direction.right;
					prevChoices = new int[4]{1,1,1,0};
				}else if(choice < directions[2]){
					move._dir = Movement.Direction.down;
					prevChoices = new int[4]{0,1,1,1};
					//print ("I just chose to go down");
				}else{
					move._dir = Movement.Direction.left;
					prevChoices = new int[4]{1,0,1,1};
				}
				/*
				choice = 0f;
				if (choice == 0f) {
					choice = Random.Range ((directions [0] + directions [1]) , (directions [2]  + directions [3]) );
					//Debug.Log (choice);
				}
				if (choice == 0f) {
					choice = Random.Range ((directions [0] + directions [1]) , (directions [2]  + directions [3]) );
					//Debug.Log (choice);
				}
				if (choice == 0f) {
					choice = Random.Range ((directions [0] + directions [1]) , (directions [2]  + directions [3]) );
					//Debug.Log (choice);
				}
				if (choice == 0f) {
					choice = Random.Range ((directions [0] + directions [1]) , (directions [2]  + directions [3]) );
					//Debug.Log (choice);
				}
				if (choice == 0f) {
					choice = Random.Range ((directions [0] + directions [1]) , (directions [2]  + directions [3]) );
					//Debug.Log (choice);
				}
				if (choice < directions[1]) {
					move._dir = Movement.Direction.up;
					prevChoices = new int[4]{1,1,0,1};
				}else if(choice < 0f){
					move._dir = Movement.Direction.right;
					prevChoices = new int[4]{1,1,1,0};
				}else if(choice <= directions[2]){
					move._dir = Movement.Direction.down;
					prevChoices = new int[4]{0,1,1,1};
					//print ("I just chose to go down");
				}else{
					move._dir = Movement.Direction.left;
					prevChoices = new int[4]{1,0,1,1};
				}*/
				chooseDirection = false;
				waitTime = ogWaitTime;
			}




			break;
		case State.entering:
			move._dir = Movement.Direction.still;

			if (transform.position.x < 13.48f || transform.position.x > 13.52) {
				//print ("GOING LEFT OR RIGHT");
				transform.position = Vector3.Lerp (transform.position, new Vector3 (13.5f, transform.position.y, transform.position.z), 3f * Time.deltaTime);
			} else if (transform.position.y > -13.99f || transform.position.y < -14.01f) {
				gameObject.GetComponent<Animator>().SetInteger ("Direction", 2);
				transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, -14f, transform.position.z), 3f * Time.deltaTime);
			} else {
				fleeing = false;
				dead = false;
				gameObject.GetComponent<Animator>().SetBool("Running", true);
				_state = State.waiting;
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
