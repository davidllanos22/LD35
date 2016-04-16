using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TYPE {
        WATER,
        SAND,
        TREE,
        ROCK,
        SOLID
    }

    [Header("Tile Types")]
    public Sprite water;
    public Sprite sand;

    private TYPE type;

    private SpriteRenderer renderer;

    public void Init(TYPE type){
        renderer = GetComponent<SpriteRenderer>(); 

        switch(type){
            case TYPE.WATER:
                renderer.sprite = water;
                break;
            case TYPE.SAND:
                renderer.sprite = sand;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
