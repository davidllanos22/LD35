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
    public GameObject mapItem;

    //===========================

    private int [,] shapeMap;
    private int [,] mapItemsMap;

    private GameObject mapObject;

    void Start(){
        mapObject = new GameObject("Map");
        GenerateShapeMap();
        GenerateMapItemsMap();

        PresentMap();
    }

    void Update(){
        if(Input.GetMouseButtonDown(1)){
            if(shapeMap!= null){
                Destroy(GameObject.Find("Map"));
            }
            GenerateShapeMap();
            GenerateMapItemsMap();

            PresentMap();
        }
    }

    void GenerateShapeMap(){
        shapeMap = new int[width, height];
        RandomFillShapeMap();

        for(int i = 0; i< 8; i++){
            SmoothShapeMap();
        }

        CalCulateStartingPosition();
    }

    void GenerateMapItemsMap(){
        mapItemsMap = new int[width, height];
        RandomFillmapItemsMap();
    }

    void RandomFillShapeMap(){
        if(useRandomSeed){
            seed = (Time.time + UnityEngine.Random.Range(0, 100)).ToString();
        }

        System.Random random = new System.Random(seed.GetHashCode());
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                if(x <= borderWidth || x >= width - borderWidth || y <= borderWidth || y >= height - borderWidth){
                    shapeMap[x, y] = 1;
                }else{
                    shapeMap[x, y] = random.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }   
        }
    }

    void RandomFillmapItemsMap(){
        if(useRandomSeed){
            seed = (Time.time + UnityEngine.Random.Range(0, 100)).ToString();
        }

        System.Random random = new System.Random(seed.GetHashCode());
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                if(shapeMap[x, y] == 0){
                    shapeMap[x, y] = random.Next(4, 7);
                }
            }   
        }
    }

    void SmoothShapeMap(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                int numTiles = GetNeighbourTiles(shapeMap, x, y);
                if(numTiles > 4){
                    shapeMap[x, y] = 1;
                }else if(numTiles < 4){
                    shapeMap[x, y] = 0;
                }
            }   
        }
    }

    int GetNeighbourTiles(int [,] map, int tX, int tY){
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

    Tile CreateTileAtPosition(int x, int y){
        Vector3 pos = new Vector3(-width/2 + x, -height/2 + y, 0);
        GameObject instance = Instantiate(tile, pos, transform.rotation) as GameObject;
        instance.transform.SetParent(mapObject.transform);

        return instance.GetComponent<Tile>();
    }

    MapItem CreateMapItemAtPosition(int x, int y){
        Vector3 pos = new Vector3(-width/2 + x, -height/2 + y, -0.5f);
        GameObject instance = Instantiate(mapItem, pos, transform.rotation) as GameObject;
        instance.transform.SetParent(mapObject.transform);

        return instance.GetComponent<MapItem>();
    }

    void PresentMap(){
        if(shapeMap != null){
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    Tile tileInstance = CreateTileAtPosition(x, y);

                    int value = shapeMap[x, y]; 
                    if(value == 0) tileInstance.Init(Tile.TYPE.SAND);
                    if(value == 1) tileInstance.Init(Tile.TYPE.WATER);
                    if(value == 2){
                        //TODO: set player start position
                    }

                    if(value == 3){
                        //TODO: set player finish position
                    }

                    if(value == 4){
                        MapItem mapItemInstance = CreateMapItemAtPosition(x, y);
                        tileInstance.Init(Tile.TYPE.SAND);
                        mapItemInstance.Init(MapItem.TYPE.TREE);
                    }

                    if(value == 5){
                        MapItem mapItemInstance = CreateMapItemAtPosition(x, y);
                        tileInstance.Init(Tile.TYPE.SAND);
                        mapItemInstance.Init(MapItem.TYPE.ROCK);
                    }

                    if(value == 6){
                        MapItem mapItemInstance = CreateMapItemAtPosition(x, y);
                        tileInstance.Init(Tile.TYPE.SAND);
                        mapItemInstance.Init(MapItem.TYPE.WATER);
                    }
                }   
            }
        }
    }

}
