using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector3 currentTile;
    private SpriteRenderer renderer;

    public GameObject particles;

    private bool moving = false;

    private float lerpTime = 1f;
    private float currentLerpTime = 0;

    private Vector3 cube;

    private Game game;

    public enum TOOL {
        NONE,
        AXE,
        PICKAXE,
        WATER_PUMP
    }

    private TOOL currentTool = TOOL.NONE;

	void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();
        currentTile = transform.position;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
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
        if(!Game.isPaused()){
            if(Input.GetMouseButtonDown(0)){
                GameObject tileClicked = GetObjectClicked();
                if(tileClicked != null){
                    MapItem mapItem = tileClicked.GetComponent<MapItem>();
                    if(mapItem!=null){
                        if(!game.timerStarted) game.StartTimer();
                        //CreateParticles(mapItem.transform.position);
                        mapItem.Hurt(currentTool);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Alpha1)) setCurrentTool(TOOL.AXE);
            if(Input.GetKeyDown(KeyCode.Alpha2)) setCurrentTool(TOOL.PICKAXE);
            if(Input.GetKeyDown(KeyCode.Alpha3)) setCurrentTool(TOOL.WATER_PUMP);
        
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");

            Debug.Log(axisX + "   " + axisY);
            if(Mathf.Abs(axisX) > 0)
                Move((int)Mathf.Floor(axisX), 0);
            else if(Mathf.Abs(axisY) > 0)
                Move(0, (int)Mathf.Floor(axisY));
        }
    }

    private void CreateParticles(Vector3 position){
        Vector3 pos = new Vector3(position.x, position.y, -15);
        GameObject instance = Instantiate(particles, position, transform.rotation) as GameObject;
        Particles p = instance.GetComponent<Particles>();
        p.Init(Color.red, Particles.TYPE.SOFT);
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

    public bool IsInmune(){
        GameObject obj = GetObjectAtPosition(transform.position.x - 1, transform.position.y);
        MapItem item = obj.GetComponent<MapItem>();
        if(item != null){
            MapItem.TYPE type = item.GetType();
            return type == MapItem.TYPE.SOLID;
        }
        return false; 
    }

    public TOOL getCurrentTool(){
        return currentTool;
    }

    public void setCurrentTool(TOOL tool){
        Debug.Log("CURRENT TOOL: " + tool);
        this.currentTool = tool;
    }

    private GameObject GetObjectClicked(){
        Vector2 attack = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, attack);
        if(distance <= 1.5f){
            RaycastHit2D hit = Physics2D.Raycast(attack, Vector2.zero);
            return hit.collider != null ? hit.collider.gameObject : null;
        }
        return null;
    }

    private GameObject GetObjectAtPosition(float x, float y){
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
