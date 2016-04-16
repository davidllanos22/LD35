using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector3 currentTile;
    private Vector3 targetTile;
    private SpriteRenderer renderer;

    private float lerpTime = 1f;
    private float currentLerpTime = 0;

	// Use this for initialization
	void Start () {
        currentTile = new Vector3(-5, 0, -1);
        targetTile = currentTile;
        renderer = transform.GetComponent<SpriteRenderer>();
        transform.position = targetTile;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        

        currentLerpTime += 0.01f;
        if(currentLerpTime > lerpTime){
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;

        transform.position = Vector3.Lerp(transform.position, targetTile, perc);
	}

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            GameObject tileClicked = GetTileClicked();
            tileClicked.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if(Input.GetKeyDown(KeyCode.W)) SetPosition(0, -1);
        if(Input.GetKeyDown(KeyCode.A)) SetPosition(-1, 0);
        if(Input.GetKeyDown(KeyCode.S)) SetPosition(0, 1);
        if(Input.GetKeyDown(KeyCode.D)) SetPosition(1, 0);
    }

    private void SetPosition(int offX, int offY){
        targetTile += new Vector3(offX, -offY, 0);
        currentLerpTime = 0;
    }

    private GameObject GetTileClicked(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
