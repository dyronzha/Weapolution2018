using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public struct CraftMenuListStruct
//{
//    public int num;
//    public int ID;
//    public bool haCrafted;
//    public Sprite[] sprites;
//}


public class CraftMenu : MonoBehaviour {
    bool scroll = false, scrollUp = false, showUp = false, onMoving;
    bool useController;
    int firstColNum, UpMoveCount = 0;
    int currentCol = 0;
    float scrollTime = 0.0f, moveTime, cameraSizeOffset = 1.0f;
    float[] posY = new float[4];
    string whichPlayer;
    Vector2 oldCameraPos, cameraOffset;
    Vector3 headColPos, TailColPos, oringinPos;
    Transform mainCamera;
    Transform[] craftColumn;
    public int menuListNum;
    //public float[] posY;
    //public CraftMenuListStruct[] craftMenuList;
	// Use this for initialization
	void Awake () {
        firstColNum = 0;
        mainCamera = Camera.main.transform;
        oldCameraPos = mainCamera.position;
        //for (int i = 0; i<3; i++) {
        //    num2offsetX[i] = transform.GetChild(1).GetChild(i+1).position.x;
        //}
        //for (int i = 0; i < 4; i++)
        //{
        //    num3offsetX[i] = transform.GetChild(2).GetChild(i + 1).position.x;
        //}
        craftColumn = new Transform[menuListNum];
        for (int i = 0; i < menuListNum; i++)
        {
            craftColumn[i] = transform.GetChild(i);
            if(i < 4)posY[i] = craftColumn[i].position.y;
            //if (i <= upHalf) {
            //    posY[upHalf - i] = craftColumn[upHalf].position.y - 1.6f * i;
            //    posY[downHalf + i] = craftColumn[downHalf].position.y + 1.6f * i;
            //}
            
        }
        headColPos = craftColumn[0].position + new Vector3(10.2f,0.0f,0.0f); //顯示外上的第一個位置
        TailColPos = craftColumn[3].position + new Vector3(10.2f,0.0f,0.0f); //顯示外下的第一個位置
        oringinPos = transform.position;
    }

    private void Start()
    {
        if (Player.p1charaType)
        {
            if (Player.p1controller) //p1用搖桿
            {
                useController = true;
                whichPlayer = Player.p1joystick;
            }
            else
                useController = false;
        }
       else
        {
            if (Player.p2controller) //p2用搖桿
            {
                useController = true;
                whichPlayer = Player.p2joystick;
            }
            else
                useController = false;
        }
        //useController = false;
        //whichPlayer = "p1";
    }

    // Update is called once per frame
    void Update () {
        if (StageManager.timeUp) return;
        cameraOffset = new Vector2(mainCamera.position.x - oldCameraPos.x, mainCamera.position.y - oldCameraPos.y);
        GetInput();
        OnMoving();
        OnScrolling();

        if (Player.p1charaType)
        {
            if (showUp) Player.p1moveAble = false;
        }
        else
        {
            if (showUp) Player.p2moveAble = false;
        }
    }

    void GetInput() {
        if (useController)
        {
            if (!onMoving && Input.GetButtonDown(whichPlayer + "ButtonY"))
            {
                
                showUp = !showUp;
                onMoving = true;
            }
        }
        else
        {
            if (!onMoving && Input.GetKeyDown(KeyCode.R))
            {
                showUp = !showUp;
                onMoving = true;
            }
        }
        if (showUp)
        {
            if (!scroll)
            {
                if (useController)
                {
                    if (Input.GetAxis(whichPlayer + "LVertical") > 0.85f)
                    {
                        //Debug.Log("GetInput UP");
                        scroll = true;
                        scrollUp = true;
                    }
                    else if (Input.GetAxis(whichPlayer + "LVertical") < -0.85f)
                    {
                        //Debug.Log("GetInput Down");
                        scroll = true;
                        scrollUp = false;
                    }
                }
                else {
                    if (Input.GetKey(KeyCode.W))
                    {
                        //Debug.Log("GetInput UP");
                        scroll = true;
                        scrollUp = true;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        //Debug.Log("GetInput Down");
                        scroll = true;
                        scrollUp = false;
                    }
                }
            }
        }
       
    }
    void OnMoving()
    {
        if (onMoving)
        {
            float diffX;
            moveTime += Time.deltaTime * 2.0f;
            if (showUp)
                diffX = Mathf.Lerp(oringinPos.x, oringinPos.x + 10.2f, moveTime);
            else
                diffX = Mathf.Lerp(oringinPos.x + 10.2f, oringinPos.x, moveTime);
            transform.position = new Vector3((diffX + cameraOffset.x) * cameraSizeOffset,
                                            (oringinPos.y + cameraOffset.y) * cameraSizeOffset,
                                            oringinPos.z);
            if (moveTime >= 1.0f)
            {
                if (Player.p1charaType)
                {
                    if (showUp) Player.p1moveAble = false;
                    else Player.p1moveAble = true;
                }
                else
                {
                    if (showUp) Player.p2moveAble = false;
                    else Player.p2moveAble = true;
                }
                moveTime = 0.0f;
                onMoving = false;
            }
        }
        else {
            transform.position = new Vector3((oringinPos.x+ cameraOffset.x) * cameraSizeOffset,
                                            (oringinPos.y + cameraOffset.y) * cameraSizeOffset,
                                            oringinPos.z);
        }
    }
    void OnScrolling() {
        if (onMoving) return;
        if (scroll)
        {
            if (scrollTime <= 1.0f)
            {
                if (scrollUp)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        int temp = currentCol + i;
                        if (temp >= menuListNum) temp = temp - menuListNum;
                        float offsetY = Mathf.Lerp(posY[i], posY[i-1] , scrollTime);
                        if (scrollTime >= 1.0f) offsetY = posY[i - 1];
                        craftColumn[temp].position = new Vector3((craftColumn[temp].position.x + cameraOffset.x) * cameraSizeOffset, 
                                                                (offsetY + cameraOffset.y) * cameraSizeOffset, 
                                                                craftColumn[temp].position.z);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int temp = currentCol + i;
                        if (temp >= menuListNum) temp = temp - menuListNum;
                        float offsetY = Mathf.Lerp(posY[i], posY[i + 1], scrollTime);
                        if (scrollTime >= 1.0f) offsetY = posY[i + 1];
                        //craftColumn[temp].SetVectorY(offsetY + cameraOffset.y);
                        craftColumn[temp].position = new Vector3((craftColumn[temp].position.x + cameraOffset.x) * cameraSizeOffset, 
                                                                (offsetY + cameraOffset.y) * cameraSizeOffset, 
                                                                craftColumn[temp].position.z);
                        //craftColumn[i].position -= new Vector3(0, 6.4f * Time.deltaTime, 0);
                    }
                }
                scrollTime += Time.deltaTime*4.0f;
            }
            else {
                Debug.Log("currentCol:" + currentCol + "     upMoveCount:" + UpMoveCount);
                SetMenuList();
                scroll = false;
                scrollTime = 0.0f;
            }
        }
    }
    void SetMenuList() {
        if (scrollUp)
        {
            UpMoveCount++;
            if (UpMoveCount >= menuListNum-3) {
                UpMoveCount = menuListNum - 4;
                TailColPos = new Vector3((TailColPos.x + cameraOffset.x) * cameraSizeOffset, 
                                        (TailColPos.y +cameraOffset.y) * cameraSizeOffset, 
                                        TailColPos.z);
                craftColumn[firstColNum].position = TailColPos;
                //lastColNum = firstColNum;
                firstColNum++;
                if (firstColNum >= menuListNum) firstColNum = 0;
            }
            currentCol++;
            if (currentCol >= menuListNum) currentCol = 0;
            Debug.Log("firstColNum" + firstColNum);
        }
        else
        {
            UpMoveCount--;
            if (UpMoveCount < 0) {
                UpMoveCount = 0;
                int lastColNum = firstColNum - 1;
                if (lastColNum < 0) lastColNum += menuListNum;
                firstColNum = lastColNum;
                Debug.Log("sdasdasdasdasdadadad" + lastColNum);
                headColPos = new Vector3((headColPos.x + cameraOffset.x) * cameraSizeOffset, 
                                            (headColPos.y + cameraOffset.y) * cameraSizeOffset, 
                                            headColPos.z);
                craftColumn[lastColNum].position = headColPos;
                //firstColNum = lastColNum;
                //lastColNum--;
                //if (lastColNum < 0) lastColNum = menuListNum - 1;
            }
            currentCol--;
            if (currentCol < 0) currentCol = menuListNum - 1;
            Debug.Log("lastColNum" + (firstColNum-1));
        } 
    }

    void ChangeMenuImage(int column, int id) {

    }

    public void SetSizeOffset(float _value) {
        cameraSizeOffset = _value;
        transform.localScale = new Vector3(_value, _value, _value);
    }

    //public void UpdateMenuInfo(int id) {
    //    for (int i = 0; i < craftMenuList.Length; i++) {
    //        if (id == craftMenuList[i].ID) {
    //            craftMenuList[i].haCrafted = true;
    //            for (int j = 0; j < 3; j++) {
    //                craftColumn[i].GetChild(j+1).GetComponent<SpriteRenderer>().sprite = craftMenuList[i].sprites[j];
    //            }
    //            break;
    //        }
    //    }
    //}

}
