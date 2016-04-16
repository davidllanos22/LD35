using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector3 currentTile;
    private Vector3 targetTile;
    private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        currentTile = new Vector3(-5, 0, -1);
        targetTile = currentTile;
        renderer = transform.GetComponent<SpriteRenderer>();
        transform.position = currentTile;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)){
            GetTileClicked();
        }
        if(Input.GetKeyDown(KeyCode.W)) SetPosition(0, -1);
        if(Input.GetKeyDown(KeyCode.A)) SetPosition(-1, 0);
        if(Input.GetKeyDown(KeyCode.S)) SetPosition(0, 1);
        if(Input.GetKeyDown(KeyCode.D)) SetPosition(1, 0);


        transform.position = Vector3.Lerp(transform.position, targetTile, Time.deltaTime * 10f);
	}

    private void SetPosition(int offX, int offY){
        targetTile += new Vector3(offX, -offY, 0); 
    }

    private GameObject GetTileClicked(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.collider != null){
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = Mathf.Floor(pos.x);
            float y = Mathf.Floor(pos.y);
            //Debug.Log(pos);
        }
        return null;
    }
}
