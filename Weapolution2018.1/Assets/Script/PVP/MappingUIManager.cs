using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingUIManager : MonoBehaviour {
    bool countDownState;
    int playerNum = 0, confirmNum, countDownNum = 3;
    bool[] hasControl = new bool[5] { false, false, false, false, false };
    bool[] slotConfirm = new bool[4] { false, false, false, false};
    float countDownTime = .0f;
    PlayerController[] playerControllers = new PlayerController[4];

    float playerGap = 120.0f;

    RectTransform[] playerUI = new RectTransform[4];

    //第4為中間，其餘由左至右為0~3
    Vector3[] slotPos = new Vector3[5];
    UnityEngine.UI.Image[] ready = new UnityEngine.UI.Image[4];
    UnityEngine.UI.Text info;

    StageManager stageManager;
    PVPDialog pvpDialog;

    struct PlayerController {
        //public bool confirm;
        public string control;
        public int slotID;
        public bool isChosen, isConfirm; 

        bool isHold;
        int lastAxis;
        float holdTime, getAxis;

        public int isMove() {
            float inputAxis = Input.GetAxis(control + "LHorizontal");
            if (Mathf.Abs(inputAxis) > 0.7f) {
                if (!isHold || (inputAxis * getAxis < .0f))
                {
                    getAxis = inputAxis;
                    isHold = true;
                    return lastAxis = (getAxis > 0.0f) ? 1 : -1;
                }
                else {
                    holdTime += Time.deltaTime;
                    if (holdTime > 0.3f)
                    {
                        holdTime = 0.0f;
                        return lastAxis = (getAxis > 0.0f) ? 1 : -1;
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
        Transform readys = transform.Find("Readys");
        slotPos[4] = new Vector3(0,0,0);
        for (int i = 0; i < 4; i++) {
            slotPos[i] = heads.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            playerUI[i] = UIs.GetChild(i).GetComponent<RectTransform>();
            playerUI[i].gameObject.SetActive(false);
            playerControllers[i] = new PlayerController();
            ready[i] = readys.GetChild(i).GetComponent<UnityEngine.UI.Image>();
            ready[i].enabled = false;
        }
        info = transform.Find("InfoText").GetComponent<UnityEngine.UI.Text>();

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        pvpDialog = GameObject.Find("Dialog").GetComponent<PVPDialog>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Mathf.Abs(Input.GetAxis("keyboardLHorizontal")) > 0) {
        //    Debug.Log("fffuck  " + Input.GetAxis("keyboardLHorizontal"));
        //}
        //if (Input.GetButtonDown("keyboardButtonA")) {
        //    Debug.Log("down confirm");
        //}
        //else if (Input.GetButtonUp("keyboardButtonA")) {
        //    Debug.Log("up confirm");
        //}
        if (StageManager.timeUp || !pvpDialog.start) return;
        GetInput();
        if(countDownState)CountDown();
	}

    void GetInput() {

        //加入玩家
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!hasControl[0])
            {
                AddNewPlayer("keyboard");
                hasControl[0] = true;
            }
        }
        else if (Input.GetButtonDown("p1ButtonA")) {
            if (!hasControl[1])
            {
                AddNewPlayer("p1");
                hasControl[1] = true;
            }
        }
        else if (Input.GetButtonDown("p2ButtonA"))
        {
            if (!hasControl[2])
            {
                AddNewPlayer("p2");
                hasControl[2] = true;
            }
        }
        else if (Input.GetButtonDown("p3ButtonA"))
        {
            if (!hasControl[3])
            {
                AddNewPlayer("p3");
                hasControl[3] = true;
            }
        }
        else if (Input.GetButtonDown("p4ButtonA"))
        {
            if (!hasControl[4])
            {
                AddNewPlayer("p4");
                hasControl[4] = true;
            }
        }

        //移動選擇和確認
        for (int i = 0; i < 4; i++) {
            if (!playerControllers[i].isChosen) continue;
            if (!playerControllers[i].isConfirm) MoveSelect(i);
            ConfirmCancleSelect(i);
        }
    }

    void AddNewPlayer(string control) {
        if (playerNum >= 4) return;
        for (int i = 0; i < 4; i++) {
            if (!playerControllers[i].isChosen) {
                playerControllers[i].isChosen = true;
                playerNum++;
                playerControllers[i].control = control;
                playerControllers[i].slotID = 4;
                playerUI[i].gameObject.SetActive(true);
                playerUI[i].anchoredPosition = slotPos[4] + new Vector3(0, -playerGap * i, 0);
                break;
            }
        }
       
    }

    void MoveSelect(int id) {
        int move = playerControllers[id].isMove();
        int lastID = playerControllers[id].slotID;
        if (move > 0)
        {
            do
            {
                if (playerControllers[id].slotID == 4) playerControllers[id].slotID = 2;
                else playerControllers[id].slotID = (playerControllers[id].slotID == 3) ? 0 : playerControllers[id].slotID + 1;
            } while (slotConfirm[playerControllers[id].slotID]);
        }
        else if (move < 0)
        {
            do
            {
                if (playerControllers[id].slotID == 4) playerControllers[id].slotID = 1;
                else playerControllers[id].slotID = (playerControllers[id].slotID == 0) ? 3 : playerControllers[id].slotID - 1;
            } while (slotConfirm[playerControllers[id].slotID]);
        }
        if (lastID != playerControllers[id].slotID)
        {
            playerUI[id].anchoredPosition = slotPos[playerControllers[id].slotID] + new Vector3(0, -playerGap * id, 0);
        }
    }

    void ConfirmCancleSelect(int id) {
        if (playerControllers[id].slotID < 4) {
            if (Input.GetButtonDown(playerControllers[id].control + "ButtonA"))
            {
                confirmNum++;
                slotConfirm[playerControllers[id].slotID] = true;
                ready[playerControllers[id].slotID].enabled = true;
                playerControllers[id].isConfirm = true;
                for (int i = 0; i < 4; i++)
                {
                    if (playerControllers[i].isChosen && id != i && playerControllers[i].slotID == playerControllers[id].slotID)
                    {
                        playerUI[i].anchoredPosition = slotPos[4] + new Vector3(0, -playerGap * i, 0);
                        playerControllers[i].slotID = 4;
                    }
                }

                if (confirmNum >= 4)
                {
                    Debug.Log("all ready");
                    countDownState = true;
                    info.text = "3";
                }
            }
            else if(Input.GetButtonDown(playerControllers[id].control + "ButtonB"))
            {
                if (playerControllers[id].isConfirm)
                {
                    slotConfirm[playerControllers[id].slotID] = false;
                    ready[playerControllers[id].slotID].enabled = false;
                    playerControllers[id].isConfirm = false;
                    confirmNum--;
                    if (countDownState) {
                        countDownState = false;
                        info.text = "VS.";
                        countDownTime = .0f;
                        countDownNum = 3;
                    }
                }
                else {
                    playerNum--;
                    playerUI[id].gameObject.SetActive(false);
                    playerControllers[id].isChosen = false;
                    if (playerControllers[id].control == "keyboard") hasControl[0] = false;
                    else if (playerControllers[id].control == "p1") hasControl[1] = false;
                    else if (playerControllers[id].control == "p2") hasControl[2] = false;
                    else if (playerControllers[id].control == "p3") hasControl[3] = false;
                    else if (playerControllers[id].control == "p4") hasControl[4] = false;
                }
            }
        }
    }

    void CountDown() {
        if (countDownTime < 1.0f) countDownTime += Time.deltaTime;
        else {
            countDownNum--;

            if (countDownNum > 0)
            {
                info.text = countDownNum.ToString();
            }
            else {
                info.text = "GO!!!";
                for (int i = 0; i < 4; i++)
                {
                    PVPPlayerManager.SetAllController(playerControllers[i].slotID, playerControllers[i].control);
                }
                StageManager.nextStage = 2;
                stageManager.ChangeSceneBlackOut();
            } 
        }
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

