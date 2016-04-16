using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TYPE {
        TILE_WATER,
        TILE_SAND,
        TILE_TREE,
        TILE_ROCK,
        TILE_SOLID
    }

    public TYPE type = TYPE.TILE_WATER;

	// Use this for initialization
	void Start () {
	
	}

    void Init(){
            
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
