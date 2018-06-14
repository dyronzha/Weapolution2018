using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public string Player;
    public bool mode;
    //mode 0 == 鍵盤
    //mode 1 == 搖桿
    float L_JoyX = 0.0f;
    float L_JoyY = 0.0f;
    float Speed = 1;


	void Update () {
        if (mode)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(Speed, 0, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(Speed, 0, 0);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, Speed, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position -= new Vector3(0, Speed, 0);
            }
        }
        else
        {
            L_JoyX = Input.GetAxis(Player + "LHorizontal");
            L_JoyY = Input.GetAxis(Player + "LVertical");
            transform.position += new Vector3(L_JoyX, 0, 0);
            transform.position += new Vector3(0, L_JoyY, 0);
        }
        

    }

   
}
