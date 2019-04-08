using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerManager : MonoBehaviour {
    static string[] controller = new string[4] {"p1", "keyboard", "keyboard", "p2"};
    PlayerControl[] playerControls = new PlayerControl[4];

    // Use this for initialization
    private void Awake()
    {
        CharacterVoice characterVoice = GetComponent<CharacterVoice>();
        Transform controls = transform.Find("Players");
        for (int i  = 0; i < 4; i++) {
            playerControls[i] = controls.GetChild(i).GetComponent<PlayerControl>();
            playerControls[i].Init(this, characterVoice);
            if (i==1 || i== 3)playerControls[i].SetController(true, controller[i]);
            else playerControls[i].SetController(false, controller[i]);
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
