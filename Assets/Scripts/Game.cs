using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public int width, height;
    [Range(0,100)]
    public int randomFillPercent;
    public string seed;
    public bool useRandomSeed;
    public int borderWidth = 10;
    public GameObject tile;
    public GameObject mapItem;

    MapGenerator mapGenerator;


	// Use this for initialization
	void Start () {
        mapGenerator = new MapGenerator();
        mapGenerator.NewMap(gameObject);
	}
	
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            GameObject.Destroy(GameObject.Find("Map"));
            mapGenerator.NewMap(gameObject);
        }
    }
}
