using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private Vector3 currentTile;
    private SpriteRenderer renderer;
    private Game game;
    private Rigidbody2D body;

    private bool moving = false;

    private float lerpTime = 2f;
    private float currentLerpTime = 0;

	// Use this for initialization
	void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();
        currentTile = transform.position;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate () {
        currentLerpTime += 0.08f;
        if(currentLerpTime > lerpTime){
            currentLerpTime = lerpTime;
            if(moving) moving = false;
        }

        float perc = currentLerpTime / lerpTime;

        body.AddForce(-currentTile);
        //body.MovePosition(Vector3.Lerp(transform.position, currentTile, perc));
        //transform.position = Vector3.Lerp(transform.position, currentTile, perc);
    }
	
	// Update is called once per frame
	void Update () {
        Move(1, 0);
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
                MapItem item = obj.GetComponent<MapItem>();

                bool isSolid = false;

                if(item != null){
                    MapItem.TYPE type = item.GetType();
                    item.StartFlying(type != MapItem.TYPE.SOLID);
                }

                currentTile += new Vector3(offX, offY, 0);
                currentLerpTime = 0;
            }
        }
    }

    private GameObject GetObjectAtPosition(float x, float y){
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
