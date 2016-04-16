using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {

    public enum TYPE {
        TREE,
        WATER,
        ROCK,
    }

    [Header("Tile Types")]
    public Sprite tree;
    public Sprite rock;
    public Sprite water;

    private int maxHp;
    private int currentHp;

    private TYPE type;

    private SpriteRenderer renderer;

    public void Init(TYPE type){
        renderer = GetComponent<SpriteRenderer>(); 

        switch(type){
            case TYPE.TREE:
                maxHp = 13;
                renderer.sprite = tree;
                break;
            case TYPE.WATER:
                maxHp = 20;
                renderer.sprite = rock;
                break;
            case TYPE.ROCK:
                maxHp = 26;
                renderer.sprite = water;
                break;
        }

        currentHp = maxHp;
    }

    public void Hurt(){
        currentHp--;
        if(currentHp <= 0){
            Die();
        }
    }

    private void Die(){
        Destroy(transform.gameObject);
    }

    // Update is called once per frame
    void Update () {

    }
}
