﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPPlayerManager : MonoBehaviour {
    static string[] controller = new string[4] { "p3", "keyboard", "p2", "p1" };
    PlayerControl[] playerControls = new PlayerControl[4];
    StageManager stageManager;
    PVPTeamHP teamHp;
    PVPDialog pvpDialog;

    public PVPCraftMenu teamAMenu, teamBMenu;

    // Use this for initialization
    private void Awake()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        teamHp = GetComponent<PVPTeamHP>();
        CharacterVoice characterVoice = GetComponent<CharacterVoice>();

        for (int i = 0; i < 4; i++) {
            playerControls[i] = transform.GetChild(i).GetComponent<PlayerControl>();
            if (i < 2) playerControls[i].Init(this, characterVoice, true);
            else playerControls[i].Init(this, characterVoice, false);
            if (i == 1 || i == 3) {
                playerControls[i].SetController(true, controller[i]);
                playerControls[i].transform.Find("CraftSystem").GetComponent<PVPCraftSystem>().Init(characterVoice, controller[i]);
                Debug.Log(playerControls[i] + "   " + teamAMenu);
                if (i == 1) teamAMenu.Init(controller[i], this.playerControls[i]);
                else teamBMenu.Init(controller[i], this.playerControls[i]);
            }
            else {
                playerControls[i].SetController(false, controller[i]);
                playerControls[i].GetComponent<PVPAttacker>().Init(characterVoice, controller[i]);
            }
        }

        teamHp.Init(PVPGameOver);
        pvpDialog = GameObject.Find("Dialog").GetComponent<PVPDialog>();
    }
    void Start() {
        for (int i = 0; i < 4; i++) {
            Debug.Log(controller[i]);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public static void SetAllController(int id, string con) {
        Debug.Log(id + "  is " + con);
        controller[id] = con;
    }

    public void PVPGameOver(int team) {

        if (team == 0)
        {
            playerControls[0].GoDie();
            playerControls[1].GoDie();
            StartCoroutine(OnDialog("B組"));
        }
        else {
            playerControls[2].GoDie();
            playerControls[3].GoDie();
            StartCoroutine(OnDialog("A組"));
        }
        StartCoroutine(stageManager.SlowDown(2.2f, false));
    }

    public void SetHP(bool teamA, float value) {
        teamHp.ChangeHp(teamA, value);
    }

    IEnumerator OnDialog(string team) {
        yield return new WaitForSecondsRealtime(2.2f);
        pvpDialog.SetOn(team);
        StageManager.timeUp = true;
    }


}
