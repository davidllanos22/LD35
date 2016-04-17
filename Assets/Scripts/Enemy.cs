using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Vector3 currentTile;
    private SpriteRenderer renderer;
    private Game game;
    private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();
        //currentTile = transform.position;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
	}
        
	void Update () {
        if(currentTile != null)Move(1, 0);
        if(transform.position.x > game.width) Destroy(gameObject);
	}

    public void SetPosition(int x, int y){
        currentTile = new Vector3(x, y, -1);
        transform.position = new Vector3(x, y, -1);
    }

    private void MoveOld(int offX, int offY){
        Vector3 targetTile = transform.position + new Vector3(offX, offY, 0);
        GameObject obj = GetObjectAtPosition(targetTile.x, targetTile.y);
        if(obj != null){
            MapItem item = obj.GetComponent<MapItem>();
            Player player = obj.GetComponent<Player>();

            if(player != null){
                Debug.Log("PLAYER IS NOT NULLLLL");
                bool isDead = !player.IsInmune();
                if(isDead) Application.LoadLevel("Game Over");
            }else{
                if(item != null){
                    MapItem.TYPE type = item.GetType();
                    item.StartFlying(type != MapItem.TYPE.SOLID);
                }
            }

            currentTile += new Vector3(offX * 0.5f, offY * 0.5f, 0);
        }
        body.AddForce(currentTile * 0.2f);

    }

    private void Move(int offX, int offY){
        Vector3 targetTile = transform.position + new Vector3(offX, offY, 0);
        GameObject obj = GetObjectAtPosition(targetTile.x, targetTile.y);
        GameObject objPlayer = IsCollidingWithPlayer(targetTile.x, targetTile.y);
        if(objPlayer != null){
            Player player = objPlayer.GetComponent<Player>();

            if(player != null){
                Debug.Log("PLAYER IS NOT NULLLLL");
                bool isDead = !player.IsInmune();
                if(isDead) Application.LoadLevel("Game Over");
            }
        }

        if(obj != null){
            MapItem item = obj.GetComponent<MapItem>();
            if(item != null){
                MapItem.TYPE type = item.GetType();
                item.StartFlying(type != MapItem.TYPE.SOLID);
            }

            currentTile += new Vector3(offX * 0.5f, offY * 0.5f, 0);
        }
        body.AddForce(currentTile * 0.2f);

    }

    private GameObject GetObjectAtPosition(float x, float y){
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    private GameObject IsCollidingWithPlayer(float x, float y){
        RaycastHit2D [] hit = Physics2D.RaycastAll(new Vector2(x, y), Vector2.zero);
        foreach (var item in hit) {
            if(item.collider.gameObject.name == "Player") return item.collider.gameObject;
        }
        return null;
    }
}
