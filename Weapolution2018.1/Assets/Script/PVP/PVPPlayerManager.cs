using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerManager : MonoBehaviour {
    static string[] controller = new string[4] {"p1", "keyboard", "keyboard", "p2"};
    PlayerControl[] playerControls = new PlayerControl[4];
    StageManager stageManager;
    PVPTeamHP teamHp;

    // Use this for initialization
    private void Awake()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        teamHp = GetComponent<PVPTeamHP>();
        CharacterVoice characterVoice = GetComponent<CharacterVoice>();
        Transform controls = transform.Find("Players");
        for (int i  = 0; i < 4; i++) {
            playerControls[i] = controls.GetChild(i).GetComponent<PlayerControl>();
            if(i<2)playerControls[i].Init(this, characterVoice, true);
            else playerControls[i].Init(this, characterVoice, false);
            if (i == 1 || i == 3) {
                playerControls[i].SetController(true, controller[i]);
                playerControls[i].GetComponent<PVPCraftSystem>().Init(characterVoice, controller[i]);
            } 
            else {
                playerControls[i].SetController(false, controller[i]);
                playerControls[i].GetComponent<PVPAttacker>().Init(characterVoice, controller[i]);
            } 
        }

        teamHp.Init(PVPGameOver);

    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SetAllController(int id, string con) {
        controller[id] = con;
    }

    public void PVPGameOver(int team) {
        if (team == 0)
        {
            playerControls[0].GoDie();
            playerControls[1].GoDie();
        }
        else {
            playerControls[2].GoDie();
            playerControls[3].GoDie();
        }
        StartCoroutine(stageManager.SlowDown(2.2f, false));
    }

    public void SetHP(bool teamA, float value) {
        teamHp.ChangeHp(teamA, value);
    }

}
