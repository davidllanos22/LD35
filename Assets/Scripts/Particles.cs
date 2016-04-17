using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {

    public enum TYPE{
        SOFT,
        STRONG
    }

    private TYPE type;
    private Color color;
    private ParticleSystem system;

    private float maxLife = 1;
    private float currentLife = 0;

    public void Init(Color color, TYPE type){
        this.color = color;
        this.type = type;
        system = GetComponent<ParticleSystem>();
        system.startColor = color;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(currentLife >= maxLife){
            Destroy(gameObject);
        }else{
            currentLife += Time.deltaTime;
        }
	}
}
