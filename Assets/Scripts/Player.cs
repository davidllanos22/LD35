using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector3 currentTile;
    private Vector3 targetTile;
    private SpriteRenderer renderer;

    private bool moving = false;

    private float lerpTime = 1f;
    private float currentLerpTime = 0;

	// Use this for initialization
	void Start () {
        currentTile = new Vector3(-5, 0, -1);
        targetTile = currentTile;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.position = targetTile;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        currentLerpTime += 0.08f;
        if(currentLerpTime > lerpTime){
            currentLerpTime = lerpTime;
            if(moving) moving = false;
        }

        float perc = currentLerpTime / lerpTime;

        transform.position = Vector3.Lerp(transform.position, targetTile, perc);
	}

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            GameObject tileClicked = GetTileClicked();
            tileClicked.GetComponent<SpriteRenderer>().color = Color.red;
        }

        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        Move((int)Mathf.Floor(axisX), (int)Mathf.Floor(axisY));

        /*if(axisY < 0) Move(0, -1);
        if(Input.GetKeyDown(KeyCode.A)) Move(-1, 0);
        if(Input.GetKeyDown(KeyCode.S)) Move(0, 1);
        if(Input.GetKeyDown(KeyCode.D)) Move(1, 0);*/
    }

    private void SetPosition(int x, int y){
        transform.position = new Vector3(x, y, -1);
    }

    private void Move(int offX, int offY){
        if(!moving){
            moving = true;

            if(offX>0){
                renderer.flipX = true;
            }else if(offX < 0){
                renderer.flipX = false;
            }
            targetTile += new Vector3(offX, offY, 0);
            currentLerpTime = 0;
        }
    }

    private GameObject GetTileClicked(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
