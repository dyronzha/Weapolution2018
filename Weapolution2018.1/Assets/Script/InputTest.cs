using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour
{
    public float p1_L_JoyX = 0.0f;
    public float p1_L_JoyY = 0.0f;
    public float p2_L_JoyX = 0.0f;
    public float p2_L_JoyY = 0.0f;
    // 左邊類比訊號
    public float p1_R_JoyX = 0.0f;
    public float p1_R_JoyY = 0.0f;
    public float p2_R_JoyX = 0.0f;
    public float p2_R_JoyY = 0.0f;
    // 右邊類比訊號
    public float p1_LT = 0.0f;
    public float p1_RT = 0.0f;
    public float p2_LT = 0.0f;
    public float p2_RT = 0.0f;
    // 兩邊Trigger
    void Update()
    {
        p1_L_JoyX = Input.GetAxis("p1LHorizontal");
        p1_L_JoyY = Input.GetAxis("p1LVertical");
        p2_L_JoyX = Input.GetAxis("p2LHorizontal");
        p2_L_JoyY = Input.GetAxis("p2LVertical");

        p1_R_JoyX = Input.GetAxis("p1RHorizontal");
        p1_R_JoyY = Input.GetAxis("p1RVertical");
        p2_R_JoyX = Input.GetAxis("p2RHorizontal");
        p2_R_JoyY = Input.GetAxis("p2RVertical");

        p1_LT = Input.GetAxis("p1LT");
        p1_RT = Input.GetAxis("p1RT");
        p2_LT = Input.GetAxis("p2LT");
        p2_RT = Input.GetAxis("p2RT");

        #region 鍵盤debugLog
        if (Input.GetButton("Horizontal"))
        {
            print("Get Horizontal input from keyboard = " + Input.GetAxis("Horizontal").ToString());
        }
        else if (Input.GetButton("Vertical"))
        {
            print("Get Vertical input from keyboard = " + Input.GetAxis("Vertical").ToString());
        }
        #endregion

        #region player01搖桿debugLog
        else if (p1_L_JoyX != 0.0f)
        {
            Debug.Log("Get p1 Left Horizontal input from Joystick = " + p1_L_JoyX.ToString());
        }
        else if (p1_L_JoyY != 0.0f)
        {
            Debug.Log("Get p1 Left Vertical input from Joystick = " + p1_L_JoyY.ToString());
        }
        else if (p1_R_JoyX != 0.0f)
        { 
            Debug.Log("Get p1 Right Horizontal input from Joystick = " + p1_R_JoyX.ToString());
        }
        else if (p1_R_JoyY != 0.0f)
        {
            Debug.Log("Get p1 Right Vertical input from Joystick = " + p1_R_JoyY.ToString());
        }
        else if (p1_LT != 0.0f )
        {
            Debug.Log("Get p1 Left Trigger input from Joystick = " + p1_LT.ToString());
        }
        else if (p1_RT != 0.0f)
        {
            Debug.Log("Get p1 Right Trigger input from Joystick = " + p1_RT.ToString());
        }
        else if (Input.GetButton("p1ButtonA"))
        {
            Debug.Log("p1ButtonA clicked ");
        }
        else if (Input.GetButton("p1ButtonB"))
        {
            Debug.Log("p1ButtonB clicked ");
        }
        else if (Input.GetButton("p1ButtonX"))
        {
            Debug.Log("p1ButtonX clicked ");
        }
        else if (Input.GetButton("p1ButtonY"))
        {
            Debug.Log("p1ButtonY clicked ");
        }
        else if (Input.GetButton("p1LB"))
        {
            Debug.Log("p1LB clicked ");
        }
        else if (Input.GetButton("p1RB"))
        {
            Debug.Log("p1RB clicked ");
        }
        #endregion

        #region player02搖桿debugLog
        else if (Input.GetButton("p2LHorizontal"))
        {//m_JoyX != 0.0f ){  
            Debug.Log("Get p2 Left Horizontal input from Joystick = " + p2_L_JoyX.ToString());
        }
        else if (p2_L_JoyY != 0.0f)
        {
            Debug.Log("Get p2 Left Vertical input from Joystick = " + p2_L_JoyY.ToString());
        }
        else if (Input.GetButton("p2RHorizontal"))
        {
            Debug.Log("Get p2 Right Horizontal input from Joystick = " + p2_R_JoyX.ToString());
        }
        else if (p2_R_JoyY != 0.0f)
        {
            Debug.Log("Get p2 Right Vertical input from Joystick = " + p2_R_JoyY.ToString());
        }
        else if (p2_LT != 0.0f)
        {
            Debug.Log("Get p1 Left Trigger input from Joystick = " + p2_LT.ToString());
        }
        else if (p2_RT != 0.0f)
        {
            Debug.Log("Get p1 Right Trigger input from Joystick = " + p2_RT.ToString());
        }
        else if (Input.GetButton("p2ButtonA"))
        {
            Debug.Log("p2ButtonA clicked ");
        }
        else if (Input.GetButton("p2ButtonB"))
        {
            Debug.Log("p2ButtonB clicked ");
        }
        else if (Input.GetButton("p2ButtonX"))
        {
            Debug.Log("p2ButtonX clicked ");
        }
        else if (Input.GetButton("p2ButtonY"))
        {
            Debug.Log("p2ButtonY clicked ");
        }
        else if (Input.GetButton("p2LB"))
        {
            Debug.Log("p2LB clicked ");
        }
        else if (Input.GetButton("p2RB"))
        {
            Debug.Log("p2RB clicked ");
        }
        #endregion




    }
}