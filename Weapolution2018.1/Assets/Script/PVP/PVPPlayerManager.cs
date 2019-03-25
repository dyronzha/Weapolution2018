using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerManager : MonoBehaviour {
    static string[] controller = new string[4];
    PlayerControl[] playerControls = new PlayerControl[4];

    // Use this for initialization
    private void Awake()
    {
        Transform controls = transform.Find("Players");
        for (int i  = 0; i < 4; i++) {
            playerControls[i] = controls.GetChild(i).GetComponent<PlayerControl>();
            playerControls[i].SetController(controller[i]);
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SetAllController(int id, string con) {
        controller[id] = con;
    }
}
