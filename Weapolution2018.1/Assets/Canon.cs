using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {

    SpriteRenderer hint;

    public bool CanonisfillingPowder = false, startFilled = false;

    bool CanoncanFiiled = false;
    public bool CanonTriigerIN = false;

    public int CanonPowderNum = 0;

    //public bool CanonFilled = false;

    //bool readyToShoot = false;
    string whichPlayer = "p1";

    GameObject RightCanon, LeftCanon;
    //public int CanonNum;
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
        CraftSystemScript = CrafterScript.GetComponentInChildren<CraftSystem>();
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


        CanonState();
        if (CanoncanFiiled)
        {
            FillingInPowder();
        }

    }

    void CanonState() {
        if (!CanoncanFiiled)
        {
            if (CanonTriigerIN && CraftSystemScript.CheckHandle().id == 3)
            {
                outLine.SetOutLine(true);
                CanoncanFiiled = true;
                hint.enabled = true;
            }
        }
        else {
            if (!CanonTriigerIN || CraftSystemScript.CheckHandle().id != 3) {
                outLine.SetOutLine(false);
                if (!CanonisfillingPowder) hint.enabled = false;
                CanoncanFiiled = false;
            }
        }
    }


    void FillingInPowder()
    {
        if (Player.p2charaType)
        {
            if (Player.p2controller && Input.GetButtonDown(whichPlayer + "LB") && !startFilled)
            {
                Debug.Log("start fiiiiiiiiiiillllllllllllllll");
                Player.p2moveAble = false;
                startFilled = true;
                CrafterScript.Gathering();
                CraftCantFunc(true);
                CanonPowderNum++;
            }
            else if (!Player.p2controller && Input.GetKeyDown(KeyCode.E) && !startFilled)
            {
                Debug.Log("start fiiiiiiiiiiillllllllllllllll");
                Player.p2moveAble = false;
                startFilled = true;
                CrafterScript.Gathering();
                CraftCantFunc(true);
                CanonPowderNum++;
            }
        }
        else
        {
            if (Player.p1controller && Input.GetButtonDown(whichPlayer + "LB") && !startFilled)
            {
                Debug.Log("start fiiiiiiiiiiillllllllllllllll");
                startFilled = true;
                Player.p1moveAble = false;
                CrafterScript.Gathering();
                CraftCantFunc(true);
                CanonPowderNum++;
            }
            else if (!Player.p1controller && Input.GetKeyDown(KeyCode.E) && !startFilled)
            {
                Debug.Log("start fiiiiiiiiiiillllllllllllllll");
                Player.p1moveAble = false;
                startFilled = true;
                CrafterScript.Gathering();
                CraftCantFunc(true);
                CanonPowderNum++;
            }
        }


    }

    public void OverFinlling()
    {
        Debug.Log("Over fillllllllllllllllllllll");
        startFilled = false;
        CraftCantFunc(false);
        if (CanonPowderNum > 0) {
            CanonisfillingPowder = true;
        }
        //if (CanonFilled)
        //{
        //    CanoncanFiiled = true;
        //    if (CanonPowderNum != 0)
        //    {
        //        CanonisfillingPowder = true;
        //    }
        //    if (CanonPowderNum > 2)
        //    {
        //        CanonisfillingPowder = true;
        //        CanoncanFiiled = false;
        //    }
        //    CanonFilled = false;
        //}

    }
    public void CraftCantFunc(bool busy)
    {
        Debug.Log("crrrrrrrrrrrrafffffffffffffffffttt go busy  " + busy);
        CraftSystemScript.SetFuncBusy(busy);

    }

    public void OutOfPowder() {
        CanonisfillingPowder = false;
        hint.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player" && collision.gameObject.name == "character2")
        {
            CanonTriigerIN = true;
            if (CanonisfillingPowder) hint.enabled = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.name == "character2") {
            CanonTriigerIN = false;
            if (CanonisfillingPowder) hint.enabled = false;
        }

    }

}
