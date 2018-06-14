using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeChoose : MonoBehaviour
{

    public bool section;
    static public bool AttackerSucces;
    static public bool CrafterSucces;
    public GameObject scene;
    public Renderer render;

    private void Awake()
    {
        AttackerSucces = false;
        CrafterSucces = false;
        render = this.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (AttackerSucces && CrafterSucces)
        {
            scene.SendMessage("ChangeScene");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (section) //在Crafter的框框
        {
           
            if (collision.gameObject.name == "character1") //當碰撞物是p1
            {
                if (!Player.p1controller) //用鍵盤玩的p1
                {
                    if (Input.GetKeyDown("space"))
                    {
                        Player.p1charaType = true;
                        Debug.Log("SetPlayer01 as a Crafter");
                        CrafterSucces = true;
                    }
                }
                else //用搖桿玩的p1
                {
                    if (Input.GetButton(Player.p1joystick + "ButtonA"))
                    {
                        Player.p1charaType = true;
                        Debug.Log("SetPlayer01 as a Crafter");
                        CrafterSucces = true;
                    }
                }
                if (CrafterSucces)
                {
                    Player.p1moveAble = false;
                    render.material.color = Color.red;
                }
            }
            else if (collision.gameObject.name == "character2") //當碰撞物是p2
            {
                if (!Player.p2controller) //用鍵盤玩的p2
                {
                    if (Input.GetKeyDown("space"))
                    {
                        Player.p2charaType = true;
                        Debug.Log("SetPlayer02 as a Crafter");
                        CrafterSucces = true;

                    }
                }
                else //用搖桿玩的p2
                {
                    if (Input.GetButton(Player.p2joystick + "ButtonA"))
                    {
                        Player.p2charaType = true;
                        Debug.Log("SetPlayer02 as a Crafter");
                        CrafterSucces = true;
                    }
                }
                if (CrafterSucces)
                {
                    Player.p2moveAble = false;
                    render.material.color = Color.red;
                }
            }
        }
        else //在attacker的框框
        {

            if (collision.gameObject.name == "character1(Clone)")
            {
                if (!Player.p1controller) //用鍵盤玩的p1
                {
                    if (Input.GetKeyDown("space"))
                    {
                        Player.p1charaType = false;
                        Debug.Log("SetPlayer01 as an attacker");
                        AttackerSucces = true;
                    }
                }
                else //用搖桿玩的p1
                {
                    if (Input.GetButton(Player.p1joystick + "ButtonA"))
                    {
                        Player.p1charaType = false;
                        Debug.Log("SetPlayer01 as an attacker");
                        AttackerSucces = true;
                    }
                }
                if (AttackerSucces)
                {
                    Player.p1moveAble = false;
                    render.material.color = Color.green;
                }
            }
            else if (collision.gameObject.name == "character2(Clone)")
            {
                if (!Player.p2controller) //用鍵盤玩的p2
                {
                    if (Input.GetKeyDown("space"))
                    {
                        Player.p2charaType = false;
                        Debug.Log("SetPlayer02 as a attacker");
                        AttackerSucces = true;
                    }
                }
                else //用搖桿玩的p2
                {
                    if (Input.GetButton(Player.p2joystick + "ButtonA"))
                    {
                        Player.p2charaType = false;
                        Debug.Log("SetPlayer02 as a attacker");
                        AttackerSucces = true;
                    }
                }
                if (AttackerSucces)
                {
                    Player.p2moveAble = false;
                    render.material.color = Color.green;
                }
            }
        }

    }
}

