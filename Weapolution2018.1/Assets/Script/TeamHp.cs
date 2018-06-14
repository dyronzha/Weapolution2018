using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamHp : MonoBehaviour {
    StageManager stageManager;
    Image teamHPImg;


    public GameObject TeamHpLine;
    public Player PlayerScript;
    public Crafter CrafterScript;
    PuaseOnCanvas PuaseScript;
    public Image HpBoarder;


    bool changeColor01;
    bool changeColor02;
    static public float teamHp = 1; //滿血是1
    static public bool checkGameOver;
    int inFuctionTime = 0;

    void Awake () {
        teamHp = 1;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        
    }
    void Start()
     {
        TeamHpLine = GameObject.Find("TeamHp");
        PlayerScript = GameObject.Find("character1").GetComponent<Player>();
        CrafterScript = GameObject.Find("character2").GetComponent<Crafter>();
        PuaseScript = GameObject.Find("map").GetComponent<PuaseOnCanvas>();
        HpBoarder = GameObject.Find("bloodImage").GetComponent<Image>();
        teamHPImg = TeamHpLine.GetComponent<Image>();
        changeColor01 = false;
        changeColor02 = false;

    }

    private void Update()
    {
        RenderUI();
        if(checkGameOver)CheckHp();
        if (Input.GetKeyDown(KeyCode.G) && teamHp < 1.0f) teamHp += 0.05f;
    }

    void RenderUI()
     {
        teamHPImg.fillAmount = teamHp; //render

        if (teamHp > 0.2f && teamHp <= 0.5f) //hp 30%~50%
        {
            if (!changeColor01)
            {
                teamHPImg.color = new Color32(255, 176, 92, 255);
                HpBoarder.sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood50_");
                changeColor01 = true;
            }
            else return;
        }
        else if (teamHp > 0 && teamHp <= 0.2f) //hp 0~20%
        {
            if (!changeColor02)
            {
                teamHPImg.color = new Color32(249, 79, 68, 255);
                HpBoarder.sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood20_");
                changeColor02 = true;
            }
            else return;
        }
        else if (teamHp <= 0)
        {
            HpBoarder.sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood0_");
        }

    }
    public void CheckHp()
    {
        if (teamHp<0 && inFuctionTime ==0)
        {
            checkGameOver = false;
            Debug.Log(inFuctionTime + "///" + this.gameObject.name);
            PlayerScript.Gameover();
            CrafterScript.Gameover();      
            inFuctionTime++;
            StartCoroutine(stageManager.SlowDown(2.2f,false));
        }
    }

    public void CloseHpUi() {
        HpBoarder.enabled = false;
        this.GetComponent<Image>().enabled = false;
    }

}
