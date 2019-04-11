using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTrapeSystem : MonoBehaviour {

    bool useController, OnBuildTrapping, unTool, usingTool;
    int trapeNum;
    float unBuildTime;
    string whichPlayer;
    SpriteRenderer Tool;
    CChildProjectSystem childProjectSystem;


    public Sprite ToolImg, unToolImg;
    public LayerMask trapeMask;

    CraftSystem craftSystem;

    private void Awake()
    {
        Tool = transform.Find("Tool").GetComponent<SpriteRenderer>();
        childProjectSystem = GameObject.Find("Trapes").GetComponent<CChildProjectSystem>();
        craftSystem = transform.Find("CraftSystem").GetComponent<CraftSystem>();
    }

    // Use this for initialization
    void Start() {
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
            if (Player.p2controller) //p1用搖桿
            {
                useController = true;
                whichPlayer = Player.p2joystick;
            }
            else
                useController = false;
        }
    }

    // Update is called once per frame
    void Update() {
        UseTrap();
        UnBuildTrape();
    }

    void UseTrap()
    {
        if (OnBuildTrapping) return;
        if (!useController)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Tool.enabled = true;
                if (!CanSet() || trapeNum >= 2)
                {//如果位置不能設陷阱，顯示不能挖
                    unTool = true;
                    Tool.sprite = unToolImg;
                }
                else {
                    OnBuildTrapping = true;
                    Tool.sprite = ToolImg;
                    this.GetComponent<Animator>().SetTrigger("usingTrape");
                    usingTool = true;
                    switchMove(false);
                    if (trapeNum < 2) trapeNum++;

                    craftSystem.SetFuncBusy(true);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown(whichPlayer + "ButtonX")) {
                Tool.enabled = true;
                if (!CanSet() || trapeNum >= 2)
                {//如果位置不能設陷阱，顯示不能挖
                    unTool = true;
                    Tool.sprite = unToolImg;
                }
                else
                {
                    OnBuildTrapping = true;
                    Tool.sprite = ToolImg;
                    this.GetComponent<Animator>().SetTrigger("usingTrape");
                    usingTool = true;
                    switchMove(false);
                    if (trapeNum < 2) trapeNum++;
                    craftSystem.SetFuncBusy(true);
                }
            }
        }
    }
    bool CanSet()
    {
        bool beenTrap = false;
        RaycastHit2D detect = Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.6f,
                                                trapeMask);
        if (detect)
        {
            return false;
        }
        else return true;
    }

    void UnBuildTrape()
    {
        if (unTool)
        {
            unBuildTime += Time.deltaTime;
            if (unBuildTime > 0.5f)
            {
                unBuildTime = 0.0f;
                Tool.enabled = false;
                unTool = false;
            }

        }
    }

    public void RecycleTrape(){
        if(trapeNum > 0)trapeNum--;
    }

    public void SetTrapOver() {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.6f,transform.position.z);
        childProjectSystem.AddUsed(pos);
        childProjectSystem.GetNewestChild().SetOn(true, RecycleTrape);
        Tool.enabled = false;
        OnBuildTrapping = false;
        switchMove(true);
        craftSystem.SetFuncBusy(false);

    }

    void switchMove(bool enable)
    {
        if (enable)
        {
            if (Player.p1charaType) Player.p1moveAble = true;
            else Player.p2moveAble = true;
        }
        else
        {
            if (Player.p1charaType) Player.p1moveAble = false;
            else Player.p2moveAble = false;
        }

    }


}
