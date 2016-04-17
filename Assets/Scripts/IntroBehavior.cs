using UnityEngine;
using System.Collections;

public class IntroBehavior : MonoBehaviour {


    private GameObject image2;
    private int counter = 0;

	void Start () {
        image2 = transform.GetChild(1).gameObject;
	}
	
	void Update () {
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0)){
            counter++;
            if(counter == 1){
                image2.SetActive(true);
            }else{
                Application.LoadLevel("Main");
            }
        }
	}
}
