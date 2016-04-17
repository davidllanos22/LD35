using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    GameObject player;
    bool followPlayer = true;

    public float shake = 0;
    private float shakeAmount = 0.1f;
    private float decreaseFactor = 1.0f;

    float lastZoom = 5;
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


        if (shake > 0) {
            Vector2 shakeVector = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(shakeVector.x, shakeVector.y, transform.position.z);
            shake -= Time.deltaTime * decreaseFactor;
            lastZoom = Camera.main.orthographicSize;
            Camera.main.orthographicSize = 12;
        } else {
            shake = 0.0f;
            Game.setPaused(false);
            //Camera.main.orthographicSize = lastZoom;
            if(followPlayer){
                Vector3 playerPosition = player.transform.position;
                transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
            }
        }
	}
}
