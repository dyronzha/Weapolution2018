using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {

    SpriteRenderer hint;

    public bool CanonisfillingPowder = false;

    public bool startFilled = false;
    public bool CanoncanFiiled = false;
    public bool CanonTriigerIN = false;

    public int CanonPowderNum = 0;

    public bool CanonFilled = false;

    bool readyToShoot = false;
    string whichPlayer = "p1";

    GameObject RightCanon, LeftCanon;
    public int CanonNum;
    Crafter CrafterScript;
    public CraftSystem CraftSystemScript;

    COutLine outLine;

    private void Awake()
    {
        CrafterScript = GameObject.Find("character2").GetComponent<Crafter>();
        RightCanon = GameObject.Find("Canon");
        LeftCanon = GameObject.Find("Canon (1)");
        outLine = transform.GetComponent<COutLine>();
        hint = transform.Find("hint").GetComponent<SpriteRenderer>();
        hint.enabled = false;
        //CraftSystemScript = GameObject.Find("CraftSystem").GetComponent<CraftSystem>();
        //Debug.Log(GameObject.Find("CraftSystem").GetComponent<CraftSystem>());
        //Debug.Log("CraftSystemScript：  " + CraftSystemScript);
    }
    private void Start()
    {
        if (Player.p2charaType) whichPlayer = Player.p2joystick;
        else whichPlayer = Player.p1joystick;
    }
    // Update is called once per frame
    void Update()
    {
        if (CanoncanFiiled)
        {
            if(CanonTriigerIN && CraftSystemScript.CheckHandle().id == 3) FillingInPowder();
            if (startFilled) CrafterScript.Gathering();
        }

    }

    
    void FillingInPowder()
    {
        if (Player.p2charaType)
        {
            if (Player.p2controller && Input.GetButtonDown(whichPlayer + "LB"))
            {
                Player.p2moveAble = false;
                startFilled = true;
                CanonPowderNum++;
            }
            else if (!Player.p2controller && Input.GetKeyDown(KeyCode.E))
            {
                Player.p2moveAble = false;
                startFilled = true;
                CanonPowderNum++;
            }
        }
        else
        {
            if (Player.p1controller && Input.GetButtonDown(whichPlayer + "LB"))
            {
                Player.p1moveAble = false;
                startFilled = true;
                CanonPowderNum++;
            }
            else if (!Player.p1controller && Input.GetKeyDown(KeyCode.E))
            {
                Player.p1moveAble = false;
                startFilled = true;
                CanonPowderNum++;
            }
        }


    }

    public void OverFinlling()
    {
        if (CanonFilled)
        {
            CanoncanFiiled = true;
            if (CanonPowderNum != 0)
            {
                CanonisfillingPowder = true;
            }
            if (CanonPowderNum > 2)
            {
                CanonisfillingPowder = true;
                CanoncanFiiled = false;
            }
            CanonFilled = false;
        }

    }
    public void CallCraftSystemFucion()
    {
        CraftSystemScript.ThrowOut();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanonTriigerIN = true;
        if (collision.tag == "Player")
        {
            //Debug.Log("1111111111111111111111111111111111111");
            if (CraftSystemScript.CheckHandle().id == 3)
            {
                outLine.SetOutLine(true);
                CanoncanFiiled = true;
                CanonFilled = true;
                hint.enabled = true;
            }

            else return;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CanonTriigerIN = false;
        outLine.SetOutLine(false);
        hint.enabled = false;
    }

}
