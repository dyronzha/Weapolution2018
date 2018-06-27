using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChoosePlayer : MonoBehaviour {
    
    SpriteRenderer buda_joystick1;
    SpriteRenderer buda_keyboard1;
    SpriteRenderer haka_joystick1;
    SpriteRenderer haka_keyboard1;
    GameObject scene;
    GameObject budaHands;
    GameObject hakaHands;
    bool targetOnBuda = true;
    bool AttackerSucces = false;
    bool CrafterSucces = false;
    bool p1Ready = false;
    bool p2Ready = false;
    TargetSelect TargetSelectScript;
    SelectStage SelectStageScript;
    StageManager stageManager;

    // Use this for initialization

    void Start () {
        buda_joystick1 = GameObject.Find("buda_joystick1").GetComponent<SpriteRenderer>();
        buda_keyboard1 = GameObject.Find("buda_keyboard1").GetComponent<SpriteRenderer>();
        haka_joystick1 = GameObject.Find("haka_joystick1").GetComponent<SpriteRenderer>();
        haka_keyboard1 = GameObject.Find("haka_keyboard1").GetComponent<SpriteRenderer>();
        stageManager = GameObject.Find("MappingPvE_BG").GetComponent<StageManager>();
        budaHands = GameObject.Find("budaHand");
        hakaHands = GameObject.Find("hakaHand");
        budaHands.SetActive(false);
        hakaHands.SetActive(false);
        TargetSelectScript = transform.GetComponent<TargetSelect>();
        SelectStageScript = GameObject.Find("SelectStage").GetComponent<SelectStage>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!p1Ready)
        {          
            setPlayer1();
            Cancel(true);
        }
        else
        {
            LightUpButton(true);
            Cancel(true);
        }
        if(!p2Ready)
        {
            SetPlayer2();
            Cancel(false);
        }
        else
        {
            LightUpButton(false);
            Cancel(false);
        }
        if (AttackerSucces && CrafterSucces)
        {
            Player.isMapped = true;           
            SelectStageScript.isChoosed = true;
            StageManager.nextStage +=1;
            stageManager.ChangeSceneBlackOut();


        }
        //Debug.Log("Player.p2controller" + Player.p2controller);
        //if (Input.GetMouseButtonDown(0)){
        //    SelectStageScript.isChoosed = true;
        //}
    }
    
    void setPlayer1()
    {
        if (!p2Ready)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Player.p1controller = false;
                Player.p1joystick = null;
                Debug.Log("SetPlayer01 succes by keyboard");
                Debug.Log(buda_keyboard1);
                p1Ready = true;

            }
            else if (Input.GetButtonDown("p1ButtonA"))
            {
                Player.p1joystick = "p1";
                Player.p1controller = true;
                Debug.Log("SetPlayer01 succes by joystick1");
                p1Ready = true;
            }
            else if (Input.GetButtonDown("p2ButtonA"))
            {
                Player.p1joystick = "p2";
                Player.p1controller = true;
                Debug.Log("SetPlayer01 succes by joystick2");
                p1Ready = true;
            }
        }
        else
        {
            if (!Player.p2controller) //p1 用鍵盤玩
            {
                if (Input.GetButtonDown("p2ButtonA"))
                {
                    Player.p1joystick = "p2";
                    Player.p1controller = true;
                    Debug.Log("SetPlayer01 succes by joystick2");
                    p1Ready = true;
                }
                else if (Input.GetButtonDown("p1ButtonA"))
                {
                    Player.p1joystick = "p1";
                    Player.p1controller = true;
                    Debug.Log("SetPlayer01 succes by joystick1");
                    p1Ready = true;
                }
            }
            else if (Player.p1joystick == "p1")//p1用搖桿1
            {
                if (Input.GetButtonDown("p2ButtonA"))
                {
                    Player.p1joystick = "p2";
                    Player.p1controller = true;
                    Debug.Log("SetPlayer01 succes by joystick2");
                    p1Ready = true;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Player.p1controller = false;
                    Player.p1joystick = null;
                    Debug.Log("SetPlayer01 succes by keyboard");
                    Debug.Log(buda_keyboard1);
                    p1Ready = true;
                }
            }
            else if (Player.p1joystick == "p2")//p1用搖桿2
            {
                if (Input.GetButtonDown("p1ButtonA"))
                {
                    Player.p1joystick = "p1";
                    Player.p1controller = true;
                    Debug.Log("SetPlayer01 succes by joystick1");
                    p1Ready = true;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Player.p1controller = false;
                    Player.p1joystick = null;
                    Debug.Log("SetPlayer01 succes by keyboard");
                    Debug.Log(buda_keyboard1);
                    p1Ready = true;
                }
            }
        }
        if (p1Ready)
        {
            TargetSelectScript.showP1Target = true;
        }
        
    }
    void SetPlayer2()
    {
        if (!Player.p1controller) //p1 用鍵盤玩
        {
            if (Input.GetButtonDown("p2ButtonA"))
            {
                Player.p2joystick = "p2";
                Player.p2controller = true;
                p2Ready = true;
            }
            else if (Input.GetButtonDown("p1ButtonA"))
            {
                Player.p2joystick = "p1";
                Player.p2controller = true;
                p2Ready = true;
            }
        }
        else if (Player.p1joystick == "p1")//p1用搖桿1
        {
            if (Input.GetButtonDown("p2ButtonA"))
            {
                Player.p2joystick = "p2";
                Player.p2controller = true;
                p2Ready = true;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Player.p2controller = false;
                Player.p2joystick = null;
                Debug.Log("SetPlayer02 succes by keyboard");
                p2Ready = true;
            }
        }
        else if (Player.p1joystick == "p2")//p1用搖桿2
        {
            if (Input.GetButtonDown("p1ButtonA"))
            {
                Player.p2joystick = "p1";
                Player.p2controller = true;
                p2Ready = true;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Player.p2controller = false;
                Debug.Log("SetPlayer02 succes by keyboard");
                p2Ready = true;
            }
        }
        if (p2Ready)
        {
            TargetSelectScript.showP2Target = true;
        }

    }
    
    void LightUpButton( bool isP1Target )
    {
        if (isP1Target) //p1target
        {
            if (!Player.p1controller && Input.GetKeyDown(KeyCode.Return))
            {
                if (!AttackerSucces && TargetSelectScript.P1targetOnBuda)
                {
                    buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard2");
                    AttackerSucces = true;
                    Player.p1charaType = false;
                    TargetSelectScript.p1IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if(!CrafterSucces && !TargetSelectScript.P1targetOnBuda)
                {
                    haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard2");
                    CrafterSucces = true;
                    Player.p1charaType = true;
                    TargetSelectScript.p1IsLocked = true;
                    hakaHands.SetActive(true);
                }
            }
            if (Player.p1joystick == "p1" && Input.GetButtonDown("p1ButtonA"))
            {
                if (!AttackerSucces && TargetSelectScript.P1targetOnBuda)
                {
                    buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    AttackerSucces = true;
                    Player.p1charaType = false;
                    TargetSelectScript.p1IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if (!CrafterSucces && !TargetSelectScript.P1targetOnBuda)
                {
                    haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    CrafterSucces = true;
                    Player.p1charaType = true;
                    TargetSelectScript.p1IsLocked = true;
                    hakaHands.SetActive(true);
                }
            }
            if (Player.p1joystick == "p2" && Input.GetButtonDown("p2ButtonA"))
            {
                if (!AttackerSucces && TargetSelectScript.P1targetOnBuda)
                {
                    buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    AttackerSucces = true;
                    Player.p1charaType = false;
                    TargetSelectScript.p1IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if (!CrafterSucces && !TargetSelectScript.P1targetOnBuda)
                {
                    haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    CrafterSucces = true;
                    Player.p1charaType = true;
                    TargetSelectScript.p1IsLocked = true;
                    hakaHands.SetActive(true);
                }
            }         
        }
        else //p2target
        {
            if (!Player.p2controller && Input.GetKeyDown(KeyCode.Return))
            {
                if (!AttackerSucces && TargetSelectScript.P2targetOnBuda)
                {
                    buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard2");
                    AttackerSucces = true;
                    Player.p2charaType = false;
                    TargetSelectScript.p2IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if (!CrafterSucces && !TargetSelectScript.P2targetOnBuda)
                {
                    haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard2");
                    CrafterSucces = true;
                    Player.p2charaType = true;
                    TargetSelectScript.p2IsLocked = true;
                    hakaHands.SetActive(true);
                   
                }
            }
            if (Player.p2joystick == "p1" && Input.GetButtonDown("p1ButtonA"))
            {
                if (!AttackerSucces && TargetSelectScript.P2targetOnBuda)
                {
                    buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    AttackerSucces = true;
                    Player.p2charaType = false;
                    TargetSelectScript.p2IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if (!CrafterSucces && !TargetSelectScript.P2targetOnBuda)
                {
                    haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    CrafterSucces = true;
                    Player.p2charaType = true;
                    TargetSelectScript.p2IsLocked = true;
                    hakaHands.SetActive(true);
                }
            }
            if (Player.p2joystick == "p2" && Input.GetButtonDown("p2ButtonA"))
            {
                if (!AttackerSucces && TargetSelectScript.P2targetOnBuda)
                {
                    buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    AttackerSucces = true;
                    Player.p2charaType = false;
                    TargetSelectScript.p2IsLocked = true;
                    budaHands.SetActive(true);
                }
                else if (!CrafterSucces && !TargetSelectScript.P2targetOnBuda)
                {
                    haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick2");
                    CrafterSucces = true;
                    Player.p2charaType = true;
                    TargetSelectScript.p2IsLocked = true;
                    hakaHands.SetActive(true);
                }
            }
        }
    }
    
    void Cancel(bool isP1Cancel)
    {
        if (isP1Cancel)
        {
           if(TargetSelectScript.p1IsLocked)
            {
                if (!Player.p1controller && Input.GetKeyDown(KeyCode.Escape))
                {
                    TargetSelectScript.p1IsLocked = false;
                    if (!Player.p1charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
                else if (Player.p1joystick == "p1" && Input.GetButtonDown("p1ButtonX"))
                {
                    TargetSelectScript.p1IsLocked = false;
                    if (!Player.p1charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
                else if (Player.p1joystick == "p2" && Input.GetButtonDown("p2ButtonX"))
                {
                    TargetSelectScript.p1IsLocked = false;
                    if (!Player.p1charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
            }
           else
            {
                if (!Player.p1controller && Input.GetKeyDown(KeyCode.Escape))
                {
                    p1Ready = false;
                    TargetSelectScript.showP1Target = false;
                }
                else if (Player.p1joystick == "p1" && Input.GetButtonDown("p1ButtonX"))
                {
                    p1Ready = false;
                    TargetSelectScript.showP1Target = false;
                }
                else if (Player.p1joystick == "p2" && Input.GetButtonDown("p2ButtonX"))
                {
                    p1Ready = false;
                    TargetSelectScript.showP1Target = false;
                }
            }

        }
        else //p2 want cancel
        {
            if (TargetSelectScript.p2IsLocked)
            {
                if (!Player.p2controller && Input.GetKeyDown(KeyCode.Escape))
                {
                    TargetSelectScript.p2IsLocked = false;
                    if (!Player.p2charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
                else if (Player.p2joystick == "p1" && Input.GetButtonDown("p1ButtonX"))
                {
                    TargetSelectScript.p2IsLocked = false;
                    if (!Player.p2charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
                else if (Player.p2joystick == "p2" && Input.GetButtonDown("p2ButtonX"))
                {
                    TargetSelectScript.p2IsLocked = false;
                    if (!Player.p2charaType)
                    {
                        buda_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        buda_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        AttackerSucces = false;
                        budaHands.SetActive(false);
                    }
                    else
                    {
                        haka_keyboard1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_keyboard1");
                        haka_joystick1.sprite = Resources.Load<Sprite>("image/Stage/ChooseCharacter/buda_joystick1");
                        CrafterSucces = false;
                        hakaHands.SetActive(false);
                    }
                }
            }
            else
            {
                if (!Player.p2controller && Input.GetKeyDown(KeyCode.Escape))
                {
                    p2Ready = false;
                    TargetSelectScript.showP2Target = false;
                }
                else if (Player.p2joystick == "p1" && Input.GetButtonDown("p1ButtonX"))
                {
                    p2Ready = false;
                    TargetSelectScript.showP2Target = false;
                }
                else if (Player.p2joystick == "p2" && Input.GetButtonDown("p2ButtonX"))
                {
                    p2Ready = false;
                    TargetSelectScript.showP2Target = false;
                }
            }
        }
    }
}
