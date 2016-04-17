using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {

    public enum TYPE {
        TREE,
        WATER,
        ROCK,
        SOLID,
        TREASURE
    }

    [Header("Tile Types")]
    public Sprite tree;
    public Sprite rock;
    public Sprite water;
    public Sprite solid;
    public Sprite treasure;

    private int maxHp;
    private int currentHp;

    private TYPE type;

    private bool flying = false;
    private float flyingMaxTime = 1f;
    private float currentFlyingTime = 0;

    private SpriteRenderer renderer;
    private Rigidbody2D body;

    private Transform lastTransform;

    public void Init(TYPE type){
        this.type = type;
        renderer = GetComponent<SpriteRenderer>(); 
        body = GetComponent<Rigidbody2D>();

        switch(type){
            case TYPE.TREE:
                maxHp = 6;
                renderer.sprite = tree;
                break;
            case TYPE.WATER:
                maxHp = 10;
                renderer.sprite = water;
                break;
            case TYPE.ROCK:
                maxHp = 13;
                renderer.sprite = rock;
                break;
            case TYPE.SOLID:
                maxHp = 999999;
                renderer.sprite = solid;
                break;
            case TYPE.TREASURE:
                maxHp = 999999;
                renderer.sprite = treasure;
                break;
        }

        currentHp = maxHp;
    }

    public TYPE GetType(){
        return type;
    }

    public void Hurt(Player.TOOL tool){
        if(type == TYPE.TREASURE){
            Debug.Log("WIN");
        }else{
            if(isValidTool(tool)){
                renderer.color = new Color(1.0f, 0.5f, 0.5f);
                currentHp--;
                if(currentHp <= 0){
                    Die();
                }
            }
        }
    }

    public void StartFlying(bool shouldFly){
        if(true){
            lastTransform = transform;
            flying = true;
            body.AddForceAtPosition(new Vector2(8, 8), transform.position);
            body.AddTorque(0.5f);
        }else{
            
        }

    }

    private bool isValidTool(Player.TOOL tool){
        return (tool == Player.TOOL.AXE && type == TYPE.TREE) || (tool == Player.TOOL.PICKAXE && type == TYPE.ROCK) || (tool == Player.TOOL.WATER_PUMP && type == TYPE.WATER);
    }

    private void Die(){
        Destroy(transform.gameObject);
    }

    void Update () {
        if(flying){
            if(currentFlyingTime >= flyingMaxTime){
                Destroy(body);
                transform.position = lastTransform.position;
                body = gameObject.AddComponent<Rigidbody2D>();

                flying = false;
                currentFlyingTime = 0;
            }else{
                currentFlyingTime += Time.deltaTime;
            }
        }
    }
}
