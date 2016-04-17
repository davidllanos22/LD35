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
    public GameObject player;

    MapGenerator mapGenerator;

    public bool timerStarted = false;

    private int waveSeconds = 30;
    private int shapeshiftSeconds = 60;

    private int startSeconds;
    private int currentSeconds;
    private int nextWaveSeconds;
    private int nextShapeshiftSeconds;

	void Start () {
        mapGenerator = new MapGenerator();
        mapGenerator.NewMap(gameObject);
	}
	
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            GameObject.Destroy(GameObject.Find("Map"));
            mapGenerator.NewMap(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.P)) StartTimer();

        if(timerStarted){
            currentSeconds = (int)Time.time - startSeconds;
            Debug.Log(currentSeconds);

            if(currentSeconds == nextWaveSeconds){
                Debug.Log("HERE COMES THE WAVE!");
            }

            if(currentSeconds == nextShapeshiftSeconds){
                Debug.Log("HERE COMES THE SHAPESHIFT!");
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
}
