using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TYPE {
        WATER,
        SAND,
        GRASS,
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
                //Color color = new Color(1,1,1, Random.Range(0,10) * 0.1f);
                renderer.sprite = sand;
                //renderer.color = color;
                break;
            case TYPE.GRASS:
                Color color = new Color(0,1,0);
                renderer.sprite = sand;
                renderer.color = color;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
