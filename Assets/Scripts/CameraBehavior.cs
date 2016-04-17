﻿using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Max(0, Camera.main.orthographicSize - zoom);

        if(player == null){
            player = GameObject.Find("Player");
            if(player == null) return;
        }
        Vector3 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
	}
}
