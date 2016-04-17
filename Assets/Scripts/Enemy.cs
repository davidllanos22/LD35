using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private Vector3 currentTile;
    private SpriteRenderer renderer;
    private Game game;
    private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();
        currentTile = transform.position;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
	}
        
	void Update () {
        Move(1, 0);
	}

    private void SetPosition(int x, int y){
        transform.position = new Vector3(x, y, -1);
    }

    private void Move(int offX, int offY){
        Vector3 targetTile = transform.position + new Vector3(offX, offY, 0);
        GameObject obj = GetObjectAtPosition(targetTile.x, targetTile.y);
        if(obj != null){
            MapItem item = obj.GetComponent<MapItem>();
            Player player = obj.GetComponent<Player>();

            if(player != null){
                    
            }else{
                bool isSolid = false;
                if(item != null){
                    MapItem.TYPE type = item.GetType();
                    item.StartFlying(type != MapItem.TYPE.SOLID);
                }
            }

            currentTile += new Vector3(offX * 0.5f, offY * 0.5f, 0);
        }
        body.AddForce(currentTile * 0.2f);

    }

    private GameObject GetObjectAtPosition(float x, float y){
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
