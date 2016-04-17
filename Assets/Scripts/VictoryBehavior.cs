using UnityEngine;
using System.Collections;

public class VictoryBehavior : MonoBehaviour {

	void Update () {
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0)){
            Application.LoadLevel("Title");
        }
	}
}
