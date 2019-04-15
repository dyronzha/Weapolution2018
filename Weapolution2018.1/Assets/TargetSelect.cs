using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect : MonoBehaviour {

    Animator budaAnimator;
    Animator buda_light;
    Animator hakaAnimator;
    Animator haka_light;
    GameObject p1_target;
    GameObject p2_target;

    float p1_L_JoyX = 0.0f;
    float p2_L_JoyX = 0.0f;

    Vector3 p1_target_pos = new Vector3(1.5f, 4f, 0);
    Vector3 p2_target_pos = new Vector3(5.7f, 2.1f, 0);
    Vector3 BudatargetSection_Center = new Vector3(1.5f, 4f, -1);
    Vector3 hakatargetSection_Center = new Vector3(5.7f, 2.1f, -1);
    Vector3 BudatargetSection_Left = new Vector3(0.7f, 4f, -1);
    Vector3 BudatargetSection_Right = new Vector3(2f, 4f, -4);
    Vector3 hakatargetSection_Left = new Vector3(5.2f, 2f, -1);
    Vector3 hakatargetSection_Right = new Vector3(6.5f, 2f, -1);

    int TargetType;
    bool p1MoveOnlyOnce = false;
    bool p2MoveOnlyOnce = false;


    public bool showP1Target = false;
    public bool showP2Target = false;

    public bool P1targetOnBuda = true;
    public bool P2targetOnBuda = false;

    public bool p1IsLocked = false;
    public bool p2IsLocked = false;

    void Start () {
        budaAnimator = GameObject.Find("buda_pic").GetComponent<Animator>();
        buda_light = GameObject.Find("buda_light").GetComponent<Animator>();
        hakaAnimator = GameObject.Find("haka_pic").GetComponent<Animator>();
        haka_light = GameObject.Find("haka_light").GetComponent<Animator>();
        p1_target = GameObject.Find("1P");
        p2_target = GameObject.Find("2P");
        p1_target.SetActive(false);
        p2_target.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        LeftListener();
        targetSectionAssign();


        StartAnimation();
        if (!p1IsLocked)
        {
            if (showP1Target)
            {
                p1_target.SetActive(true);
                targetVerticalMoving(true);         
                TargetChooseCharacter(Player.p1controller, Player.p1joystick, true);
            }
            else
            {
                p1_target.SetActive(false);
                p1_target.transform.position = new Vector3(-2, -2, -1);                
            }

        }
        else
        {
            if (P1targetOnBuda)
            {
                budaAnimator.SetBool("is_choosing", false);
                buda_light.SetBool("is_choosing", false);
                p1_target.transform.position = BudatargetSection_Center;
                p2_target_pos = hakatargetSection_Center;
                P2targetOnBuda = false;
            }
            else
            {
                hakaAnimator.SetBool("is_choosing", false);
                haka_light.SetBool("is_choosing", false);
                p1_target.transform.position = hakatargetSection_Center;
                p2_target_pos = BudatargetSection_Center;
                P2targetOnBuda = true;
            }
        }
        if (!p2IsLocked)
        {
            if (showP2Target)
            {
                p2_target.SetActive(true);
                targetVerticalMoving(false);             
                TargetChooseCharacter(Player.p2controller, Player.p2joystick, false);
            }
            else
            {
                p2_target.SetActive(false);
                p2_target.transform.position = new Vector3(-2, -2, -1);
            }

        }
        else //p2 locked
        {
            if (P2targetOnBuda)
            {
                budaAnimator.SetBool("is_choosing", false);
                buda_light.SetBool("is_choosing", false);
                p2_target.transform.position = BudatargetSection_Center;
                p1_target_pos = hakatargetSection_Center;
                P1targetOnBuda = false;
            }
            else
            {
                hakaAnimator.SetBool("is_choosing", false);
                haka_light.SetBool("is_choosing", false);
                p2_target.transform.position = hakatargetSection_Center;
                p1_target_pos = BudatargetSection_Center;
                P1targetOnBuda = true;
            }
        }

    }

    void LeftListener()
    {
        p1_L_JoyX = Input.GetAxis("p1LHorizontal");
        p2_L_JoyX = Input.GetAxis("p2LHorizontal");
    }

    float PingPong(float t, float minLength, float maxLength)
    {
        return Mathf.PingPong(t, maxLength - minLength) + minLength;
    }

    void StartAnimation()
    {

        if (!showP1Target && !showP2Target) 
        {
            budaAnimator.SetBool("is_choosing", false);
            buda_light.SetBool("is_choosing", false);
            hakaAnimator.SetBool("is_choosing", false);
            haka_light.SetBool("is_choosing", false);
            return;        
        }       
        if (showP1Target || showP2Target)
        {
            if ((1 <= p1_target.transform.position.x && p1_target.transform.position.x <= 2) || (1 <= p2_target.transform.position.x && p2_target.transform.position.x <= 2))
            {
                budaAnimator.SetBool("is_choosing", true);
                buda_light.SetBool("is_choosing", true);
            }
            else
            {
                budaAnimator.SetBool("is_choosing", false);
                buda_light.SetBool("is_choosing", false);
            }
            if ((5 <= p1_target.transform.position.x && p1_target.transform.position.x <= 7) || (5 <= p2_target.transform.position.x && p2_target.transform.position.x <= 7))
            {
                hakaAnimator.SetBool("is_choosing", true);
                haka_light.SetBool("is_choosing", true);
            }
            else
            {
                hakaAnimator.SetBool("is_choosing", false);
                haka_light.SetBool("is_choosing", false);
            }
        }
        

    }

    void targetVerticalMoving(bool whichTarget) //target上下飄
    {
        if (whichTarget) //true = p1
        {
            p1_target.transform.position = new Vector3(p1_target_pos.x, PingPong(Time.time / 2, p1_target_pos.y, p1_target_pos.y + 0.4f), p1_target_pos.z);
        }
        else
        {
            p2_target.transform.position = new Vector3(p2_target_pos.x, PingPong(Time.time / 2, p2_target_pos.y, p2_target_pos.y + 0.4f), p2_target_pos.z);
        }

    }

    void targetSectionAssign()
    {
        if (p1IsLocked)
        {
            if (P1targetOnBuda) p2_target_pos = hakatargetSection_Center;
            else p2_target_pos = BudatargetSection_Center;
        }
        else if (p2IsLocked)
        {
            if(P2targetOnBuda) p1_target_pos = hakatargetSection_Center;
            else p1_target_pos = BudatargetSection_Center;
        }
        else if(!p1IsLocked && !p2IsLocked)
        {
            if (showP1Target && showP2Target)
            {
                if (P1targetOnBuda && P2targetOnBuda)
                {
                    p1_target_pos = BudatargetSection_Left;
                    p2_target_pos = BudatargetSection_Right;
                }
                else if (!P1targetOnBuda && !P2targetOnBuda)
                {
                    p1_target_pos = hakatargetSection_Left;
                    p2_target_pos = hakatargetSection_Right;
                }
                else if (P1targetOnBuda || P2targetOnBuda)
                {
                    if (P1targetOnBuda) p1_target_pos = BudatargetSection_Center;
                    else p1_target_pos = hakatargetSection_Center;
                    if (P2targetOnBuda) p2_target_pos = BudatargetSection_Center;
                    else p2_target_pos = hakatargetSection_Center;
                }
            }
            else if (showP1Target && !showP2Target)
            {
                if (P1targetOnBuda) p1_target_pos = BudatargetSection_Center;
                else p1_target_pos = hakatargetSection_Center;
            }
            else if (!showP1Target && showP2Target)
            {
                if (P2targetOnBuda) p2_target_pos = BudatargetSection_Center;
                else p2_target_pos = hakatargetSection_Center;
            }
        }
        

    }

    void TargetChooseCharacter(bool Controller,string WhichStick,bool isP1target)
    {
        if (isP1target && p2IsLocked) return;
        if(!isP1target && p1IsLocked) return;
        if (!Controller) //用鍵盤
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) //target左右動
            {
                if (isP1target && !p1IsLocked) P1targetOnBuda = !P1targetOnBuda;
                if(!isP1target && !p2IsLocked) P2targetOnBuda = !P2targetOnBuda;
            }
        }
        else 
        {
            if ( WhichStick == "p1")
            {
                if ( 0.5f <= Mathf.Abs(p1_L_JoyX) && !p1MoveOnlyOnce )
                {
                    if (isP1target && !p1IsLocked) P1targetOnBuda = !P1targetOnBuda;
                    if (!isP1target && !p2IsLocked) P2targetOnBuda = !P2targetOnBuda;
                    p1MoveOnlyOnce = true;
                }
                if (0.5f >= Mathf.Abs(p1_L_JoyX) && p1MoveOnlyOnce)
                {
                    p1MoveOnlyOnce = false;
                }
            }
            else if (WhichStick == "p2")
            {
                if (0.5f <= Mathf.Abs( p2_L_JoyX )&& !p2MoveOnlyOnce)
                {
                    if (isP1target && !p1IsLocked) P1targetOnBuda = !P1targetOnBuda;
                    if (!isP1target && !p2IsLocked) P2targetOnBuda = !P2targetOnBuda;
                    p2MoveOnlyOnce = true;
                }
                if (0.5f >= Mathf.Abs(p2_L_JoyX) && p2MoveOnlyOnce)
                {
                    p2MoveOnlyOnce = false;
                }
            }
        }
    }

    
}
