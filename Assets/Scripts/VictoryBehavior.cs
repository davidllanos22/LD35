using UnityEngine;
using System.Collections;

public class VictoryBehavior : MonoBehaviour {

	void Update () {
        if(Input.GetMouseButtonDown(0)){
            Application.LoadLevel("Title");
        }
	}
}
