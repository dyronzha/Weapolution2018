using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPTeamHP : MonoBehaviour {

    float[] teamHp = new float[2]; //滿血是1
    
    System.Action<int> gameOver;

    public Image[] teamHPImg = new Image[2];
    public Image[] teamAppearance = new Image[2];
    public Sprite[] teamAppearanceSprite;

    void Awake()
    {
        teamHp[0] = 1.0f;
        teamHp[1] = 1.0f;
    }
    void Start()
    {

    }

    private void Update()
    {
    }

    public void Init(System.Action<int> cbk) {
        gameOver = cbk;
    }

    void RenderUI(int id)
    {
        if (teamHp[id] < 0) teamHp[id] = .0f;
        teamHPImg[id].fillAmount = teamHp[id]; //render

        if (teamHp[id] > 0.5f)
        {
            teamHPImg[id].color = new Color32(44, 244, 44, 255);
            teamAppearance[id].sprite = teamAppearanceSprite[0 + id*4];
            //HpBoarder[id].sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood100_");
        }

        else if (teamHp[id] > 0.2f) //hp 30%~50%
        {
            teamHPImg[id].color = new Color32(255, 176, 92, 255);
            teamAppearance[id].sprite = teamAppearanceSprite[1 + id * 4];
            //HpBoarder.sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood50_");

        }
        else if (teamHp[id] > 0.0f) //hp 0~20%
        {
            teamHPImg[id].color = new Color32(249, 79, 68, 255);
            teamAppearance[id].sprite = teamAppearanceSprite[2 + id * 4];
        }
        else
        {
            teamAppearance[id].sprite = teamAppearanceSprite[3 + id * 4];
            //HpBoarder.sprite = Resources.Load<Sprite>("image/Stage/1/HpImage/blood0_");
        }

    }
    public void CheckHp(int id)
    {
        if (teamHp[id] <= 0)
        {
            gameOver(id);
        }
        else if (teamHp[id] > 1.0f) {
            teamHp[id] = 1.0f;
        }
    }

    public void CloseHpUi()
    {
        for (int i = 0; i < 2; i++) {
            teamAppearance[i].enabled = false;
            teamHPImg[i].enabled = false;
        }

    }

    public void ChangeHp(bool teamA, float _value)
    {

        if (teamA)
        {
            teamHp[0] += _value;
            CheckHp(0);
            RenderUI(0);
            

        }
        else {
            teamHp[1] += _value;
            CheckHp(1);
            RenderUI(1);
            
        }
        

     
    }

}
