using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector3 currentTile;
    private SpriteRenderer renderer;

    private bool moving = false;

    private float lerpTime = 1f;
    private float currentLerpTime = 0;

    private Vector3 cube;

	// Use this for initialization
	void Start () {
        currentTile = new Vector3(-7, 0, -1);
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.position = currentTile;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        currentLerpTime += 0.08f;
        if(currentLerpTime > lerpTime){
            currentLerpTime = lerpTime;
            if(moving) moving = false;
        }

        float perc = currentLerpTime / lerpTime;

        transform.position = Vector3.Lerp(transform.position, currentTile, perc);
	}

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            GameObject tileClicked = GetObjectClicked();
            MapItem mapItem = tileClicked.GetComponent<MapItem>();
            if(mapItem!=null){
                tileClicked.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f);
                mapItem.Hurt();
            }
        }

        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        Move((int)Mathf.Floor(axisX), (int)Mathf.Floor(axisY));
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
            //TODO: obtener el tile y el item en la posición paraa comprobar si está libre
            Vector3 targetTile = transform.position + new Vector3(offX, offY, 0);
            GameObject obj = GetObjectAtPosition(targetTile.x, targetTile.y);
            if(obj != null){
                Tile tile = obj.GetComponent<Tile>();

                bool isSolid = true;

                if(tile != null){
                    Tile.TYPE type = tile.GetType();

                    isSolid = type == Tile.TYPE.WATER;
                }

                if(!isSolid){
                    currentTile += new Vector3(offX, offY, 0);
                    currentLerpTime = 0;
                }
            }
        }
    }

    private GameObject GetObjectClicked(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    private GameObject GetObjectAtPosition(float x, float y){
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
