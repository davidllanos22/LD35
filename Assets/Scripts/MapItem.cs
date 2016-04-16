using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {

    public enum TYPE {
        TREE,
        ROCK,
    }

    [Header("Tile Types")]
    public Sprite tree;
    public Sprite rock;

    private int maxHp = 2;
    private int currentHp = 2;

    private TYPE type;

    private SpriteRenderer renderer;

    public void Init(TYPE type){
        renderer = GetComponent<SpriteRenderer>(); 

        switch(type){
            case TYPE.TREE:
                renderer.sprite = tree;
                break;
            case TYPE.ROCK:
                renderer.sprite = rock;
                renderer.color = Color.grey;
                break;
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
