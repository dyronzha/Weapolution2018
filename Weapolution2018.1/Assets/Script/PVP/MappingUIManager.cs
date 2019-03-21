using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingUIManager : MonoBehaviour {

    int playerNum = -1;
    bool[] hasControl = new bool[5] { false, false, false, false, false };
    bool[] confirm = new bool[4] { false, false, false, false};
    int[] playerSlotID = new int[4] { 0, 0, 0, 0 };
    int[] slotPlayerNum = new int[5]{0,0,0,0,0};
    string[] controlName = new string[4];

    float playerGap = 90.0f;

    RectTransform[] playerUI = new RectTransform[4];

    PlayerUIController[] playerUIController = new PlayerUIController[4];
    Vector3[] slotPos = new Vector3[4];

    // Use this for initialization
    private void Awake()
    {
        Transform heads = transform.Find("PlayerHeadSlot");
        Transform UIs = transform.Find("PlayerUI");
        for (int i = 0; i < 4; i++) {
            slotPos[i] = heads.GetChild(i).GetComponent<RectTransform>().position;
            playerUI[i] = UIs.GetChild(i).GetComponent<RectTransform>();
            playerUI[i].gameObject.SetActive(false);
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetInput() {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!hasControl[0])
            {
                AddNewPlayer("keyboard");
                hasControl[0] = true;
            }
            else ConfirmSelect();
        }
        else if (Input.GetButtonDown("p1ButtonA")) {
            if (!hasControl[1]) {
                AddNewPlayer("p1");
                hasControl[1] = true;
            }
        }


        for (int i = 0; i < playerNum; i++) {
            if (Mathf.Sign(Input.GetAxis(controlName[i] + "LHorizontal")) > 0) {

            }
        }
    }

    void AddNewPlayer(string control) {
        playerNum++;
        controlName[playerNum] = control;
        playerSlotID[playerNum] = 0;
        playerUI[playerNum].gameObject.SetActive(true);
        playerUI[playerNum].position = slotPos[0] + new Vector3(0, playerGap * slotPlayerNum[0], 0);
        slotPlayerNum[0]++;
    }

    void ConfirmSelect() {

    }

}

class PlayerUIController {
    int slotID;
    string whichControl;
    Transform UI;
    float[] slotPos = new float[4];

    public void SetSlotPos(float[] g) {
        slotPos = g;
    }
    public void ChangePos()
    {
        slotPos[0] = 1000;
    }

    public void GetInput() {
        if (whichControl == null) return;
        if (whichControl == "keyboard") {

        }
    }
    public void SetWhichControl(string control) {
        whichControl = control;
    }
}

