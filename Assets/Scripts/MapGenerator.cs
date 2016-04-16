using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

    public int width, height;
    [Range(0,100)]
    public int randomFillPercent;
    public string seed;
    public bool useRandomSeed;
    public int borderWidth = 10;
    public GameObject tile;

    //===========================

    private int [,] map;
    private GameObject mapObject;

    void Start(){
        GenerateMap();
    }

    void Update(){
        if(Input.GetMouseButtonDown(1)){
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

        for(int i = 0; i< 8; i++){
            SmoothMap();
        }

        CalCulateStartingPosition();

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

    void CalCulateStartingPosition(){
        float startPercent = 0.3f;
        bool assigned = false;
        System.Random random = new System.Random();
        int startX = (int)(width * startPercent);
        int startY = (int)(height * startPercent);
        int x = random.Next(0, startX);
        int y = random.Next(0, startY);
        Debug.Log("X: " + x);
        Debug.Log("Y: " + y);
        /*do {
            int x = random.Next(0, (int)(width * startPercent));
            int y = random.Next(0, (int)(height * startPercent));

            if(map[x, y] == 0){
                assigned = true;
                map[x, y] = 2;
            }
        } while (!assigned);*/
    }

    void PresentMap(){
        if(map != null){
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    Color color = Color.white;

                        
                    Vector3 pos = new Vector3(-width/2 + x, -height/2 + y, 0);
                    GameObject instance = Instantiate(tile, pos, transform.rotation) as GameObject;
                    instance.transform.SetParent(mapObject.transform);

                    Tile instanceTile = instance.GetComponent<Tile>();


                    /*int value = map[x, y]; if(value == 0) ColorUtility.TryParseHtmlString("#bd9d72", out color);
                    if(value == 1) ColorUtility.TryParseHtmlString("#29b6f6", out color);
                    if(value == 2) ColorUtility.TryParseHtmlString("#FF0000", out color);*/

                    int value = map[x, y]; 
                    if(value == 0) instanceTile.Init(Tile.TYPE.SAND);
                    if(value == 1) instanceTile.Init(Tile.TYPE.WATER);
                    if(value == 2) instanceTile.Init(Tile.TYPE.SOLID);
                }   
            }
        }
    }

}
