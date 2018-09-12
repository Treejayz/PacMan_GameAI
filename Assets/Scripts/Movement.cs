﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public enum Direction{
		still,
		up,
		down,
		left,
		right

	}
	public TextAsset inputMap;
	public string[] Map;
	public Direction _dir = Direction.still;

	public float MSpeed = 2f;

	private Vector2 direc= new Vector2(0f,0f);
	private static Vector2 up = new Vector2(0f,1f);
	private static Vector2 down = new Vector2(0f,-1f);
	private static Vector2 right = new Vector2(1f,0f);
	private static Vector2 left = new Vector2(-1f,0f);

	// Use this for initialization
	void Start () {
		string text = inputMap.text;
		string[] lines = text.Split('\n');
		Map = lines;
	}
	
	// Update is called once per frame
	void Update () {

		switch (_dir) {
		case Direction.still:

			break;
		case Direction.down:
			direc = down;
			break;
		case Direction.up:
			direc = up;
			break;
		case Direction.left:
			direc = left;
			break;
		case Direction.right:
			direc = right;
			break;

		}

		move (direc);

	}

	public void move(Vector2 direc){
		if (checkDirectionClear (direc)) {
			if (direc.x != 0) {
				transform.position = new Vector3 (transform.position.x, Mathf.Round (transform.position.y), transform.position.z);
			}
			if (direc.y != 0) {
				transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, transform.position.z);
			}
			transform.position = new Vector3 (transform.position.x + direc.x * MSpeed * Time.deltaTime, transform.position.y + direc.y * MSpeed * Time.deltaTime, transform.position.z);
	
		} else {
			_dir = Direction.still;
		}
	}

	public bool checkDirectionClear(Vector2 direction){
		int y =-1 * Mathf.RoundToInt( transform.position.y);
		int x = Mathf.RoundToInt (transform.position.x);



		if (direction.x == 0 && direction.y == 1) {
			y =-1 * Mathf.FloorToInt( transform.position.y);
			if(Map[y-1][x] == '-'|| Map[y-1][x]  == '#'){
				return false;
			}
		}else if(direction.x == 1 && direction.y == 0){
			if (x == Map [0].Length - 1) {
				transform.position = new Vector3 (1, transform.position.y, transform.position.z);
			}

			x = Mathf.FloorToInt (transform.position.x);
			if(Map[y][x+1] == '-' || Map[y][x+1] == '#'){
				return false;
			}
		}else if(direction.x == 0 && direction.y == -1){
			y =-1 * Mathf.CeilToInt( transform.position.y);
			if(Map[y+1][x] == '-'|| Map[y+1][x] == '#'){
				return false;
			}
		}else if(direction.x == -1 && direction.y == 0){
			if (x == 0) {
				transform.position = new Vector3 (Map [0].Length - 2, transform.position.y, transform.position.z);
			}

			x = Mathf.CeilToInt (transform.position.x);
			if(Map[y][x-1] == '-'|| Map[y][x-1] == '#'){
				return false;
			}
		}
		return true;
	}
}