using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    GameObject player;
    bool followPlayer = true;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	void Update () {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Max(1, Camera.main.orthographicSize - zoom);

        if(player == null){
            player = GameObject.Find("Player");
            if(player == null) return;
        }

        if(followPlayer){
            Vector3 playerPosition = player.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        }
	}
}
