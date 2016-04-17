using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

    public int [,] shapeMap;

    private GameObject mapObject;

    private GameObject game;
    private Game gameInstance;
    public GameObject player;

    public void NewMap(GameObject game){
        this.game = game;
        gameInstance = game.GetComponent<Game>();
        mapObject = new GameObject("Map");
        if(player != null) GameObject.Destroy(player);

        GenerateShapeMap();

        CalculateInitialPositions();

        PresentMap();
    }

    void GenerateShapeMap(){
        shapeMap = new int[gameInstance.width, gameInstance.height];
        RandomFillShapeMap();

        for(int i = 0; i< 8; i++){
            SmoothShapeMap();
        }

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
                if(shapeMap[x, y] == 0 && !isLeftBeachBitch(x, y)){

					int rand = random.Next(0, 101);

					if (x < gameInstance.width / 3) {
						if (rand <= 15) {
							shapeMap[x, y] = 7;
						} else if (rand <= 70) {
							shapeMap[x, y] = 4;
						} else if (rand <= 80) {
							shapeMap[x, y] = 6;
						} else { // rand <= 10
							shapeMap[x, y] = 5;
						}
					} else if ( x < (gameInstance.width / 3) * 2) {
						if (rand <= 10) {
							shapeMap[x, y] = 7;
						} else if (rand <= 40) {
							shapeMap[x, y] = 4;
						} else if (rand <= 55) {
							shapeMap[x, y] = 6;
						} else { // rand <= 10
							shapeMap[x, y] = 5;
						}
					} else {
						if (rand <= 5) {
							shapeMap[x, y] = 7;
						} else if (rand <= 15) {
							shapeMap[x, y] = 4;
						} else if (rand <= 35) {
							shapeMap[x, y] = 6;
						} else { // rand <= 10
							shapeMap[x, y] = 5;
						}
					}
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

	bool isLeftBeachBitch(int tX, int tY){
		return shapeMap [Math.Max(tX - 1, 0), tY] != 0;
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
        
	void CalculateInitialPositions() {
		System.Random rnd = new System.Random();
		bool asc = rnd.Next(0, 2) == 0;
		CalculateStartPosition(asc);
		CalculateFinishPosition(asc);
	}

	void CalculateStartPosition(bool asc) {
		double prop = (double)gameInstance.height/ (double)gameInstance.width;
		double x = 0.0;
		double y = 0.0;
		int yf = 0;
		while (x < gameInstance.width && y < gameInstance.height) {
			if (asc) {
				if (shapeMap [(int)x, (int)y] == 0) {
					Vector3 pos = new Vector3 (-gameInstance.width / 2 + (int)x, -gameInstance.height / 2 + (int)y, 0);
					player = Game.Instantiate (gameInstance.player, pos, game.transform.rotation) as GameObject;
					player.name = "Player";
					return;
				}
			} else {
				yf = gameInstance.height - (int)y -1;
				if (shapeMap [(int)x, yf] == 0) {
					Vector3 pos = new Vector3 (-gameInstance.width / 2 + (int)x, -gameInstance.height / 2 + yf, 0);
					player = Game.Instantiate (gameInstance.player, pos, game.transform.rotation) as GameObject;
					player.name = "Player";
					return;
				}
			}
			x += 1;
			y += prop;
		}
	}

	void CalculateFinishPosition(bool asc) {

		double prop = (double)gameInstance.height/(double)gameInstance.width;
		double x = gameInstance.width-1;
		double y = gameInstance.height-1;
		int yf = 0;
        while (x > 0 && y > 0) {
			if (asc) {
				if (shapeMap [gameInstance.width - (int)x, gameInstance.height - (int)y] == 0) {
					shapeMap [(int)x, (int)y] = 8;
					return;
				}
			} else {
				yf = gameInstance.height - (int)y;
				if (shapeMap [gameInstance.width - (int)x, gameInstance.height - yf] == 0) {
					shapeMap [(int)x, yf] = 8;
					return;
				}
			}
			x -= 1;
			y -= prop;
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