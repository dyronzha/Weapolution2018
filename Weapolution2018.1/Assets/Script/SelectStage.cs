using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour {

    Transform camera;
    bool isStart, isLock, isMove, goLeft;
    int curStage = 0;
    float moveTime;

    bool[] isUnLock = new bool[6] { true, false, false, false, false, false };

    Vector3 leftPos, middlePos, rightPos;
    StageManager stageManager;

    public Transform[] stages;
    public GameObject LArrow, RArrow;
    public Text StageName;
    public string[] stageNames;


    private void Awake()
    {
        camera = GameObject.Find("Main Camera").transform;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        UnLock();
    }
    private void Start()
    {
        middlePos = new Vector3(27,0,0);
        rightPos = new Vector3(42, 0, 0);
        leftPos = new Vector3(12,0,0);
        StageName.text = string.Empty;
    }

    private void Update()
    {
        if (isStart) {
            if (!isMove) GetInput();
            else ImageMove();
        }
    }

    void UnLock() {
        for (int i = 1; i <= 2; i++) {
            if (i+5 <= StageManager.stageRecord) {
                stages[i].Find("Lock").gameObject.SetActive(false);
                isUnLock[i] = true;
            }
        }
    }

    public void TransCamera() {
        StartCoroutine(MoveCamera());
    }
    IEnumerator MoveCamera() {
        while (camera.position.x < 27.0f) {
            camera.position += Time.deltaTime * 30.0f * new Vector3(1,0,0);
            yield return null;
        }
        camera.position = new Vector3(27.0f,0,-500);
        isStart = true;
        StageName.text = stageNames[0];
        Debug.Log("camera over");
        yield return null;
    }

    void GetInput() {
        if (Player.p1controller)
        {
            if (Input.GetButtonDown(Player.p1joystick + "ButtonA")) {
                if (!isUnLock[curStage]) return;
                isStart = false;
                StageManager.nextStage = (curStage + 5);
                stageManager.ChangeSceneBlackOut();
            }
            if (Input.GetAxis(Player.p1joystick + "LHorizontal") > 0.5f && curStage < 5)
            {
                goLeft = true;
                isMove = true;
                return;
            }
            else if (Input.GetAxis(Player.p1joystick + "LHorizontal") < -0.5f && curStage > 0)
            {
                goLeft = false;
                isMove = true;
                return;
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isUnLock[curStage]) return;
                isStart = false;
                StageManager.nextStage = curStage + 5;
                stageManager.ChangeSceneBlackOut();
            }
            if (Input.GetKeyDown(KeyCode.D) && curStage < 5)
            {
                goLeft = true;
                isMove = true;
                return;
            }
            else if (Input.GetKeyDown(KeyCode.A) && curStage > 0) {
                goLeft = false;
                isMove = true;
                return;
            }
        }

        if (Player.p2controller)
        {
            if (Input.GetButtonDown(Player.p2joystick + "ButtonA"))
            {
                if (!isUnLock[curStage]) return;
                isStart = false;
                StageManager.nextStage = curStage + 5;
                stageManager.ChangeSceneBlackOut();
            }
            if (Input.GetAxis(Player.p2joystick + "LHorizontal") > 0.5f && curStage < 5)
            {
                goLeft = true;
                isMove = true;
            }
            else if (Input.GetAxis(Player.p2joystick + "LHorizontal") < -0.5f && curStage > 0)
            {
                goLeft = false;
                isMove = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isUnLock[curStage]) return;
                isStart = false;
                StageManager.nextStage = curStage + 5;
                stageManager.ChangeSceneBlackOut();
            }
            if (Input.GetKeyDown(KeyCode.D) && curStage < 5)
            {
                goLeft = true;
                isMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.A) && curStage > 0)
            {
                goLeft = false;
                isMove = true;
            }
        }

    }

    void ImageMove() {
        if (moveTime < 1.0f)
        {
            moveTime += Time.deltaTime*2.0f;
            if (goLeft)
            {
                stages[curStage].position = Vector3.Lerp(middlePos, leftPos, moveTime);
                stages[curStage+1].position = Vector3.Lerp(rightPos, middlePos, moveTime);
            }
            else {
                stages[curStage].position = Vector3.Lerp(middlePos, rightPos, moveTime);
                stages[curStage - 1].position = Vector3.Lerp(leftPos, middlePos, moveTime);
            }
        }
        else {
            if (goLeft)
            {
                stages[curStage].position = leftPos;
                stages[curStage + 1].position = middlePos;
                curStage++;
                if (curStage == 5) RArrow.SetActive(false);
                else if (curStage == 1) LArrow.SetActive(true);
            }
            else {
                stages[curStage].position = rightPos;
                stages[curStage - 1].position = middlePos;
                curStage--;
                if (curStage == 0) LArrow.SetActive(false);
                else if (curStage == 4) RArrow.SetActive(true);
            }
            moveTime = .0f;
            isMove = false;
            StageName.text = stageNames[curStage];
           
        }
        
       
    }


}



/*
    public bool isChoosed = false;
    bool PicisMoved = false;
    bool isControl = false;
    bool startChange;
    GameObject Camera;
    public float p1_L_JoyX = 0.0f;
    public float p1_L_JoyY = 0.0f;
    public float p2_L_JoyX = 0.0f;
    public float p2_L_JoyY = 0.0f;
    public List<GameObject> StageImage;
    public List<GameObject> StageText;
    public List<Vector3> stageImagePos;
    Image rightTarget;
    Image leftTarget;
    int stageNum = 0;
    int CenterNum , LeftNum , RightNum;
    int fuctionTimes = 0;
    int PicfuctionTimes = 0;
    float clickTime;
    bool isGoingRight;
    bool isMoving = false;
    bool isLocked = false;
    bool TargetMoveOnce = false;
    bool p1MoveOnlyOnce = false;
    bool p2MoveOnlyOnce = false;
    bool readyToChoose = false;
    StageManager stageManager;
    GameObject Canvas;
    // Use this for initialization
    void Awake () {
        Camera = GameObject.Find("Main Camera");       
        StageText[1].SetActive(false);
        StageText[2].SetActive(false);
        stageImagePos[0] =new Vector3( 12f, 0, 0);
        stageImagePos[1] = new Vector3(27f, 0, 0);
        stageImagePos[2] = new Vector3(42f, 0, 0);
        CenterNum = stageNum;
        LeftNum = 1;
        RightNum = 2;
        stageManager = GameObject.Find("MappingPvE_BG").GetComponent<StageManager>();
        if (Player.isMapped) Camera.transform.position = new Vector3(27f, 0, 0);
        rightTarget = GameObject.Find("RightTarget").GetComponent<Image>();
        leftTarget = GameObject.Find("LeftTarget").GetComponent<Image>();
        Canvas = GameObject.Find("Canvas");
        Canvas.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
        if (isLocked) return;
        if (isChoosed || Player.isMapped)
        {          
            if (isChoosed) MoveCamera();
            else if (Player.isMapped)
            {
                Camera.transform.position = new Vector3(27f, 0, -500f);
                readyToChoose = true;

            }
            LeftListener();
            Canvas.SetActive(true);                    
            ChoosingStage();
            if (!isMoving)
            {
                PictureControll(CenterNum ,isGoingRight);
                if (isControl) OverMovig();
                LockChoice();
            }
            else
            {
                MovingPicture();          
            }
            
        }
        
    }
    void MoveCamera()
    {
        if (Camera.transform.position.x < 27)
        {
            Camera.transform.position += new Vector3(0.5f, 0, 0);

        }
        else if (Camera.transform.position.x >27)
        {
            Camera.transform.position = new Vector3(27f, 0, -500f);
        }
        else if (Camera.transform.position.x == 27)
        {
            readyToChoose = true;
            return;
        }
    }

    void LeftListener()
    {
        p1_L_JoyX = Input.GetAxis("p1LHorizontal");
        p1_L_JoyY = Input.GetAxis("p1LVertical");
        p2_L_JoyX = Input.GetAxis("p2LHorizontal");
        p2_L_JoyY = Input.GetAxis("p2LVertical");
    }

    void ChoosingStage() //target左右動
    {
        if (Time.time - clickTime >0.75f)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightButtonClick();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftButtonClick();
            }

            if (0.5f <= Mathf.Abs(p1_L_JoyX) && !p1MoveOnlyOnce)
            {
                if (p1_L_JoyX>0)
                {
                    RightButtonClick();
                    p1MoveOnlyOnce = true;
                }
                else
                {
                    LeftButtonClick();
                    p1MoveOnlyOnce = true;

                }

            }
            if (0.5f >= Mathf.Abs(p1_L_JoyX) && p1MoveOnlyOnce)
            {
                p1MoveOnlyOnce = false;

            }

            if (0.5f <= Mathf.Abs(p2_L_JoyX) && !p2MoveOnlyOnce)
            {
                if (p2_L_JoyX>0)
                {
                    RightButtonClick();
                    p2MoveOnlyOnce = true;
                }
                else
                {
                    LeftButtonClick();
                    p2MoveOnlyOnce = true;

                }
            }
            if (0.5f >= Mathf.Abs(p2_L_JoyX) && p2MoveOnlyOnce)
            {
                p2MoveOnlyOnce = false;
            }

        }
        


    }

    void MovingPicture() //pic移動
    {
        fuctionTimes++;
        if (fuctionTimes > 30)
        {
            isMoving = false;
            fuctionTimes = 0;       
        }
        else
        {
            PicisMoved = true;
            if (isGoingRight)
            {
                StageImage[0].transform.position += new Vector3(0.5f, 0, 0);
                StageImage[1].transform.position += new Vector3(0.5f, 0, 0);
                StageImage[2].transform.position += new Vector3(0.5f, 0, 0);
                rightTarget.sprite= Resources.Load<Sprite>("image/Stage/ChooseStage/RightTarget2");
            }
            else
            {
                StageImage[0].transform.position -= new Vector3(0.5f, 0, 0);
                StageImage[1].transform.position -= new Vector3(0.5f, 0, 0);
                StageImage[2].transform.position -= new Vector3(0.5f, 0, 0);
                leftTarget.sprite = Resources.Load<Sprite>("image/Stage/ChooseStage/LeftTarget2");

            }
        }     
    }

    void PictureControll(int CenterNum,bool isGoingRight)
    {
        if (!PicisMoved) return;
        
        if (PicfuctionTimes == 0)
        {
            if (isGoingRight)
            {
                if (CenterNum == 0)
                {
                    LeftNum = 1;
                    RightNum = 2;
                }
                else if (CenterNum == 1)
                {
                    LeftNum = 2;
                    RightNum = 0;
                }
                else if (CenterNum == 2)
                {
                    LeftNum = 0;
                    RightNum = 1;
                }
                StageImage[RightNum].transform.position = stageImagePos[0];
                PicfuctionTimes++;
                isControl = true;
            }
            else
            {
                if (CenterNum == 0)
                {
                    LeftNum = 1;
                    RightNum = 2;
                }
                else if (CenterNum == 1)
                {
                    LeftNum = 2;
                    RightNum = 0;
                }
                else if (CenterNum == 2)
                {
                    LeftNum = 0;
                    RightNum = 1;
                }
                StageImage[LeftNum].transform.position = stageImagePos[2];
                PicfuctionTimes++;
                isControl = true;
            }
        }

    }

    void OverMovig()
    {
        rightTarget.sprite = Resources.Load<Sprite>("image/Stage/ChooseStage/RightTarget");
        leftTarget.sprite = Resources.Load<Sprite>("image/Stage/ChooseStage/LeftTarget");
        isGoingRight = false;
    }

    void LockChoice()
    {
        if (!readyToChoose) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!startChange) {
                isLocked = true;
                startChange = true;
                StageManager.nextStage = stageNum + 3;
                //Canvas.SetActive(false);
                stageManager.ChangeSceneBlackOut();
                //StartCoroutine(stageManager.OnChangingScene(1f));
            }
        }
        else if (Input.GetButtonDown("p1ButtonA"))
        {
            if (!startChange)
            {
                isLocked = true;
                startChange = true;
                StageManager.nextStage = stageNum + 3;
                //Canvas.SetActive(false);
                stageManager.ChangeSceneBlackOut();
                //StartCoroutine(stageManager.OnChangingScene(1f));
            }
        }
        else if (Input.GetButtonDown("p2ButtonA"))
        {
            if (!startChange)
            {
                isLocked = true;
                startChange = true;
                StageManager.nextStage = stageNum + 3;
                //Canvas.SetActive(false);
                stageManager.ChangeSceneBlackOut();
                //StartCoroutine(stageManager.OnChangingScene(1f));
            }
        }
    }

    public void RightButtonClick()
    {
        Debug.Log("RightButtonClick");
        if (isLocked) return;
        StageText[stageNum].SetActive(false);
        CenterNum = stageNum;
        if (stageNum == 2) stageNum = 0;
        else stageNum += 1;
        StageText[stageNum].SetActive(true);
        isMoving = true;
        isGoingRight = true;
        PicfuctionTimes = 0;
        clickTime = Time.time;
    }

    public void LeftButtonClick()
    {
        Debug.Log("LeftButtonClick");
        if (isLocked) return;
        StageText[stageNum].SetActive(false);
        CenterNum = stageNum;
        if (stageNum == 0) stageNum = 2;
        else stageNum -= 1;
        StageText[stageNum].SetActive(true);
        isMoving = true;
        isGoingRight = false;
        PicfuctionTimes = 0;
        clickTime = Time.time;
    } 

    */
