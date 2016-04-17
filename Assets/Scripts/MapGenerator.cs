using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

    private int [,] shapeMap;
    private int [,] mapItemsMap;

    private GameObject mapObject;

    private GameObject game;
    private Game gameInstance;
    private GameObject player;

    public void NewMap(GameObject game){
        this.game = game;
        gameInstance = game.GetComponent<Game>();
        mapObject = new GameObject("Map");
        if(player != null) GameObject.Destroy(player);

        GenerateShapeMap();
        GenerateMapItemsMap();
        CalCulateStartPosition();
        CalCulateFinishPosition();

        PresentMap();
    }

    void GenerateShapeMap(){
        shapeMap = new int[gameInstance.width, gameInstance.height];
        RandomFillShapeMap();

        for(int i = 0; i< 8; i++){
            SmoothShapeMap();
        }

    }

    void GenerateMapItemsMap(){
        mapItemsMap = new int[gameInstance.width, gameInstance.height];
        RandomFillmapItemsMap();
    }

    void RandomFillShapeMap(){
        if(gameInstance.useRandomSeed){
            gameInstance.seed = (Time.time + UnityEngine.Random.Range(0, 100)).ToString();
        }

        System.Random random = new System.Random(gameInstance.seed.GetHashCode());
        for(int x = 0; x < gameInstance.width; x++){
            for(int y = 0; y < gameInstance.height; y++){
                if(x <= gameInstance.borderWidth || x >= gameInstance.width - gameInstance.borderWidth || y <= gameInstance.borderWidth || y >= gameInstance.height - gameInstance.borderWidth){
                    shapeMap[x, y] = 1;
                }else{
                    shapeMap[x, y] = random.Next(0, 100) < gameInstance.randomFillPercent ? 1 : 0;
                }
            }   
        }
    }

    void RandomFillmapItemsMap(){
        if(gameInstance.useRandomSeed){
            gameInstance.seed = (Time.time + UnityEngine.Random.Range(0, 100)).ToString();
        }

        System.Random random = new System.Random(gameInstance.seed.GetHashCode());
        for(int x = 0; x < gameInstance.width; x++){
            for(int y = 0; y < gameInstance.height; y++){
                if(shapeMap[x, y] == 0 && !isWaterNeighbour(x, y)){
                    int r = random.Next(0,100);
                    int solid = 10;
                    int tree = 40;
                    int water = 10;
                    int rock = 40;

                    Debug.Log(r);

                    if(r < solid) shapeMap[x, y] = 7;
                    else if(r > solid && r < solid + tree) shapeMap[x, y] = 4; 
                    else if(r > solid + tree && r < solid + tree + water) shapeMap[x, y] = 6; 
                    else if(r > solid + tree + water && r < solid + tree + water + rock) shapeMap[x, y] = 5;

                    //shapeMap[x, y] = random.Next(4, 8);
                }
            }   
        }
    }

    bool isWaterNeighbour(int tX, int tY){
        for(int nX = tX - 1; nX <= tX + 1; nX++){
            for(int nY = tY -1; nY <= tY + 1; nY++){
                if(nX >= 0 && nX < gameInstance.width && nY >= 0 && nY < gameInstance.height){
                    if(nX != tX || nY != tY){
                        if(shapeMap[nX, nY] == 1)return true;
                    }
                }
            }
        }
        return false;
    }

    void SmoothShapeMap(){
        for(int x = 0; x < gameInstance.width; x++){
            for(int y = 0; y < gameInstance.height; y++){
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
                if(nX >= 0 && nX < gameInstance.width && nY >= 0 && nY < gameInstance.height){
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

    void CalCulateStartPosition(){
        for(int xx = 0; xx < gameInstance.width; xx++){
            for(int yy = 0; yy < gameInstance.height; yy++){
                if(shapeMap[xx, yy] == 0){
                    Vector3 pos = new Vector3(-gameInstance.width/2 + xx, -gameInstance.height/2 + yy, 0);
                    player = Game.Instantiate(gameInstance.player, pos, game.transform.rotation) as GameObject;
                    player.name = "Player";
                    return;
                }
            }
        }
    }

    void CalCulateFinishPosition(){
        for(int xx = gameInstance.width-1; xx > 0; xx--){
            for(int yy = 0; yy < gameInstance.height; yy++){
                if(shapeMap[xx, yy] == 0){
                    shapeMap[xx, yy] = 8;
                    return;
                }
            }
        }
    }

    Tile CreateTileAtPosition(int x, int y){
        Vector3 pos = new Vector3(-gameInstance.width/2 + x, -gameInstance.height/2 + y, 0);
        GameObject instance = Game.Instantiate(gameInstance.tile, pos, game.transform.rotation) as GameObject;
        instance.transform.SetParent(mapObject.transform);

        return instance.GetComponent<Tile>();
    }

    MapItem CreateMapItemAtPosition(int x, int y){
        Vector3 pos = new Vector3(-gameInstance.width/2 + x, -gameInstance.height/2 + y, -0.5f);
        GameObject instance = GameObject.Instantiate(gameInstance.mapItem, pos, game.transform.rotation) as GameObject;
        instance.transform.SetParent(mapObject.transform);

        return instance.GetComponent<MapItem>();
    }

    void PresentMap(){
        if(shapeMap != null){
            for(int x = 0; x < gameInstance.width; x++){
                for(int y = 0; y < gameInstance.height; y++){
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

                    if(value == 7){
                        MapItem mapItemInstance = CreateMapItemAtPosition(x, y);
                        tileInstance.Init(Tile.TYPE.SAND);
                        mapItemInstance.Init(MapItem.TYPE.SOLID);
                    }

                    if(value == 8){
                        MapItem mapItemInstance = CreateMapItemAtPosition(x, y);
                        tileInstance.Init(Tile.TYPE.SAND);
                        mapItemInstance.Init(MapItem.TYPE.TREASURE);
                    }
                }   
            }
        }
    }

}