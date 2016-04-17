using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public int width, height;
    [Range(0,100)]
    public int randomFillPercent;
    public string seed;
    public bool useRandomSeed;
    public int borderWidth = 10;

    public GameObject tile;
    public GameObject mapItem;
    public GameObject player;
    public GameObject enemy;
    public Text timeText;

    private CameraBehavior camera;

    MapGenerator mapGenerator;

    [HideInInspector]
    public bool timerStarted = false;

    private int waveSeconds = 5;
    private int shapeshiftSeconds = 10;

    private int startSeconds;
    private int currentSeconds;
    private int nextWaveSeconds;
    private int nextShapeshiftSeconds;

    private GameObject wave;

    [HideInInspector]
    public static bool paused = false;


	void Start () {
        mapGenerator = new MapGenerator();
        mapGenerator.NewMap(gameObject);
        GameObject cam = GameObject.Find("Main Camera");
        camera = cam.GetComponent<CameraBehavior>();
	}
	
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            GameObject.Destroy(GameObject.Find("Map"));
            mapGenerator.NewMap(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.P)) StartTimer();

        if(timerStarted){
            currentSeconds = (int)Time.time - startSeconds;
            timeText.text = "Time: " + currentSeconds;

            if(currentSeconds == nextWaveSeconds){
                createWave();
                nextWaveSeconds = currentSeconds + waveSeconds;
            }

            if(currentSeconds == nextShapeshiftSeconds){
                Debug.Log("HERE COMES THE SHAPESHIFT!");
                createShapeShift();
                nextShapeshiftSeconds = currentSeconds + shapeshiftSeconds;
            }
        }

    }

    public void StartTimer(){
        startSeconds = (int)Time.time;
        currentSeconds = 0;
        nextWaveSeconds = currentSeconds + waveSeconds;
        nextShapeshiftSeconds = currentSeconds + shapeshiftSeconds;
        timerStarted = true;
    }

    private void createWave(){
        wave = new GameObject("Wave");
        for(int yy = 0; yy < 5; yy++){
            int x = (int)(mapGenerator.player.transform.position.x - 5);
            int y = (int)(yy - 2 + mapGenerator.player.transform.position.y);
            Vector3 pos = new Vector3(x, y, 0);
            GameObject instance = Game.Instantiate(enemy, pos, transform.rotation) as GameObject;
            Enemy e = instance.GetComponent<Enemy>();
            instance.transform.SetParent(wave.transform);
            e.SetPosition((int)pos.x, (int)pos.y);
        }
    }

    public static bool isPaused(){
        return paused;
    }

    public static void setPaused(bool p){
        paused = p;
    }

    private void createShapeShift(){
        Game.setPaused(true);
        camera.shake = 2.0f;
    }
}
