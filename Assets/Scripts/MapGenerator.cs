using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

    public int width, height;
    [Range(0,100)]
    public int randomFillPercent;
    public string seed;
    public bool useRandomSeed;
    public int borderWidth = 5;
    public GameObject tile;

    //===========================

    private int [,] map;
    private GameObject mapObject;

    void Start(){
        GenerateMap();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            if(map!= null){
                Destroy(GameObject.Find("Map"));
            }
            GenerateMap();
        }
    }

    void GenerateMap(){
        map = new int[width, height];
        mapObject = new GameObject("Map");
        RandomFillMap();

        for(int i = 0; i< 5; i++){
            SmoothMap();
        }

        PresentMap();
    }

    void RandomFillMap(){
        if(useRandomSeed){
            seed = Time.time.ToString();
        }

        System.Random random = new System.Random(seed.GetHashCode());
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                if(x <= borderWidth || x >= width - borderWidth || y <= borderWidth || y >= height - borderWidth){
                    map[x, y] = 1;
                }else{
                    map[x, y] = random.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }   
        }
    }

    void SmoothMap(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                int numTiles = GetNeighbourTiles(x, y);
                if(numTiles > 4){
                    map[x, y] = 1;
                }else if(numTiles < 4){
                    map[x, y] = 0;
                }
            }   
        }
    }

    int GetNeighbourTiles(int tX, int tY){
        int wallCount = 0;
        for(int nX = tX - 1; nX <= tX + 1; nX++){
            for(int nY = tY -1; nY <= tY + 1; nY++){
                if(nX >= 0 && nX < width && nY >= 0 && nY < height){
                    if(nX != tX || nY != tY){
                        wallCount += map[nX, nY];
                    }
                }else{
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void OnDrawGizmos(){
        /*if(map != null){
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    Gizmos.color = map[x, y] == 1 ? Color.blue: Color.yellow;
                    Vector3 pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }   
            }
        }*/
    }

    void PresentMap(){
        if(map != null){
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    Color color = map[x, y] == 1 ? Color.blue: Color.yellow;
                    Vector3 pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f, 0);
                    GameObject instance = Instantiate(tile, pos, transform.rotation) as GameObject;
                    instance.transform.SetParent(mapObject.transform);
                    instance.GetComponent<SpriteRenderer>().color = color;
                }   
            }
        }
    }

}
