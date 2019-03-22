using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingUIManager : MonoBehaviour {

    int playerNum = -1;
    bool[] hasControl = new bool[5] { false, false, false, false, false };
    bool[] slotConfirm = new bool[4] { false, false, false, false};
    int[] slotPlayerNum = new int[5]{0,0,0,0,0};
    PlayerController[] playerControllers = new PlayerController[4];

    float playerGap = 90.0f;

    RectTransform[] playerUI = new RectTransform[4];

    Vector3[] slotPos = new Vector3[4];

    struct PlayerController {
        //public bool confirm;
        public string control;
        public int slotID;

        bool isHold;
        float holdTime, getAxis;

        public int isMove() {
            getAxis = Input.GetAxis(control + "LHorizontal");

            if (Mathf.Abs(getAxis) > 0.7f) {
                if (!isHold)
                {
                    isHold = true;
                    return (getAxis > 0.0f) ? 1 : 0;
                }
                else {
                    holdTime += Time.deltaTime;
                    if (holdTime > 0.15f)
                    {
                        holdTime = 0.0f;
                        return (getAxis > 0.0f) ? 1 : 0;
                    }
                    else return 0;
                }
            }
            else {
                isHold = false;
                holdTime = 0.0f;
                getAxis = 0.0f;
                return 0;
            } 

        }
    }


    // Use this for initialization
    private void Awake()
    {
        Transform heads = transform.Find("PlayerHeadSlot");
        Transform UIs = transform.Find("PlayerUI");
        for (int i = 0; i < 4; i++) {
            slotPos[i] = heads.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            playerUI[i] = UIs.GetChild(i).GetComponent<RectTransform>();
            playerUI[i].gameObject.SetActive(false);
            playerControllers[i] = new PlayerController();
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(Input.GetAxis("keyboardLHorizontal")) > 0) {
            Debug.Log("fffuck  " + Input.GetAxis("keyboardLHorizontal"));
        }
        if (Input.GetButtonDown("keyboardButtonA")) {
            Debug.Log("down confirm");
        }
        else if (Input.GetButtonUp("keyboardButtonA")) {
            Debug.Log("up confirm");
        }
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
            int move = playerControllers[i].isMove();
            int lastID = playerControllers[i].slotID;
            if (move > 0)
            {
                do {
                    playerControllers[i].slotID++;
                } while (slotConfirm[playerControllers[i].slotID]);
            }
            else if (move < 0) {
                do{
                    playerControllers[i].slotID--;
                } while (slotConfirm[playerControllers[i].slotID]);
            }
            if (lastID != playerControllers[i].slotID) {
                playerUI[i].anchoredPosition = slotPos[playerControllers[i].slotID] + new Vector3(0, playerGap * slotPlayerNum[playerControllers[i].slotID], 0);
                slotPlayerNum[lastID]--;
            }

        }
    }

    void AddNewPlayer(string control) {
        playerNum++;
        playerControllers[playerNum].control = control;
        playerControllers[playerNum].slotID = 0;
        playerUI[playerNum].gameObject.SetActive(true);
        playerUI[playerNum].anchoredPosition = slotPos[0] + new Vector3(0, playerGap * slotPlayerNum[0], 0);
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

