using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    bool isKeyboard;
    string control;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetController(string con) {
        control = con;
        if (con == "keyboard") isKeyboard = true;
    }
}
