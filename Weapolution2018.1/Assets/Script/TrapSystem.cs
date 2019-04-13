using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSystem : MonoBehaviour {
    bool digging = false, useController, unShovel;
    int trapNum;
    float unShovelTime;
    bool[] trapUse = new bool[2];
    bool[] trapCatch = new bool[2];
    float[] trapingTime = new float[2];
    string whichPlayer;
    SpriteRenderer Tool;
    Transform[] enemyInTrap = new Transform[2];
    Vector3[] enemyPos = new Vector3[2];
    public Sprite shovelImg,unShovelImg;
    public GameObject[] traps = new GameObject[2];
    public Transform enemyOut;
    public TutorialRequest tutorialRequest;
    public bool test;

    CraftSystem craftSystem;

    private void Awake()
    {
        Tool = transform.Find("Tool").GetComponent<SpriteRenderer>();
        craftSystem = transform.Find("CraftSystem").GetComponent<CraftSystem>();
    }

    // Use this for initialization
    void Start () {
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
        //useController = false;
        //whichPlayer = "p1";
        //enemyMonkeyOut = GameObject.Find("outside").transform;
        //for (int i = 0; i < enemyMonkeyOut.childCount; i++) {
        //    enemyMonkeyOut.GetChild(i).GetComponent<CEnemyMonkey>().trapSystem = this;
        //}
    }
	
	// Update is called once per frame
	void Update () {
        UseTrap();
        UnShovel();
        TrapDetect();
        CountTrapTime();
	}

    void UseTrap() {
        if (digging || !craftSystem.GetFunc()) return;
        if (!useController)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Tool.enabled = true;
                if (!CanSet() || trapNum >= 2) {//如果位置不能設陷阱，顯示不能挖
                    unShovel = true;
                    Tool.sprite = unShovelImg;
                    return;
                } 
                for (int i = 0; i < 2; i++)
                {
                    if (!trapUse[i])
                    {
                        Tool.sprite = shovelImg;
                        this.GetComponent<Animator>().SetTrigger("usingTrape");
                        digging = true;
                        switchMove(false);
                        if (test) tutorialRequest.DoneTrap();
                        if (trapNum < 2) trapNum++;
                        craftSystem.SetFuncBusy(true);
                        return;
                    }
                }
            }
        }
        else {
            if (Input.GetButtonDown(whichPlayer + "ButtonX"))
            {
                Tool.enabled = true;
                
                if (!CanSet() || trapNum >= 2) //如果位置不能設陷阱，顯示不能挖
                { 
                    unShovel = true;
                    Tool.sprite = unShovelImg;
                    return;
                }
                for (int i = 0; i < 2; i++)
                {
                    if (!trapUse[i])
                    {
                        Tool.sprite = shovelImg;
                        this.GetComponent<Animator>().SetTrigger("usingTrape");
                        digging = true;
                        switchMove(false);
                        if (test) tutorialRequest.DoneTrap();
                        if (trapNum < 2) trapNum++;
                        Debug.Log("shovel" + trapNum);
                        craftSystem.SetFuncBusy(true);
                        return;
                    }
                }
            }
        }
    }
    bool CanSet() {
        bool beenTrap = false;
        RaycastHit2D detect = Physics2D.Raycast(transform.position, new Vector2(0,-1),1.0f,
                                                1 << LayerMask.NameToLayer("Obstacle"));
        for (int i = 0; i < 2; i++) {
            if (trapUse[i]) {
                if (Vector2.Distance(traps[i].transform.position,
                    transform.position + new Vector3(0, -0.6f, 0)) < 0.5f) {
                    beenTrap = true;
                }
            }
        }
        if (detect || beenTrap)
        {
            return false;
        }
        else return true;
    }

    void UnShovel() {
        if (unShovel) {
            unShovelTime += Time.deltaTime;
            if (unShovelTime > 0.5f) {
                unShovelTime = 0.0f;
                Tool.enabled = false;
                unShovel = false;
            }
            
        }
    }

    void TrapDetect() {
        for (int i = 0; i < enemyOut.childCount; i++) {
            if (trapUse[0] && !trapCatch[0]) {
                //Transform monkey = enemyMonkeyOut.GetChild(i);
                Transform enemy = enemyOut.GetChild(i);
                if (enemy.transform.position.x < traps[0].transform.position.x + 0.5f &&
                    enemy.position.x > traps[0].transform.position.x - 0.3f &&
                    enemy.position.y < traps[0].transform.position.y + 0.3f &&
                    enemy.position.y > traps[0].transform.position.y - 0.15f) {

                    trapCatch[0] = true;
                    enemy.GetComponent<CEnemy>().SetState(0,false);
                    enemy.gameObject.SetActive(false);
                    enemyPos[0] = enemy.position;
                    enemyInTrap[0] = enemy;
                    //monkey.gameObject.SetActive(false);
                    //monkeyPos.Add(monkey.position);
                    //monkeyInTrape.Add(monkey);
                    //trapTime.Add(0.0f);
                    traps[0].GetComponent<Animator>().Play("FallInHole");
                    //if (!trapCatch[1]) firstTrapUse = 0;
                    continue;
                }
            }
            if (trapUse[1] && !trapCatch[1]) {
                //Transform monkey = enemyMonkeyOut.GetChild(i);
                Transform enemy = enemyOut.GetChild(i);
                if (enemy.transform.position.x < traps[1].transform.position.x + 0.5f &&
                    enemy.position.x > traps[1].transform.position.x - 0.3f &&
                    enemy.position.y < traps[1].transform.position.y + 0.3f &&
                    enemy.position.y > traps[1].transform.position.y - 0.15f)
                {
                    trapCatch[1] = true;
                    enemy.GetComponent<CEnemy>().SetState(0, false);
                    enemy.gameObject.SetActive(false);
                    enemyPos[1] = enemy.position;
                    enemyInTrap[1] = enemy;
                    trapCatch[1] = true;
                    //monkey.gameObject.SetActive(false);
                    //monkeyPos.Add(monkey.position);
                    //monkeyInTrape.Add(monkey);
                    //trapTime.Add(0.0f);
                    traps[1].GetComponent<Animator>().Play("FallInHole");
                    //if (!trapCatch[0]) firstTrapUse = 1;
                    continue;
                }
            }
            
        }

    }

    void CountTrapTime() {
        for (int i = 0; i < 2; i++)
        {
            if (trapCatch[i]) {
                trapingTime[i] += Time.deltaTime;
                if (trapingTime[i] >= 3.7f) {
                    enemyInTrap[i].position = enemyPos[i];
                    enemyInTrap[i].gameObject.SetActive(true);
                    traps[i].SetActive(false);
                    trapCatch[i] = false;
                    trapUse[i] = false;
                    trapingTime[i] = 0.0f;
                    if (trapNum > 0) trapNum--;
                }
            }
        }

        //for (int i = 0; i < trapTime.Count; i++) {
        //    trapTime[i] += Time.deltaTime;
        //    if (trapTime[i] > 3.5f) {
        //        monkeyInTrape[0].position = monkeyPos[0];
        //        monkeyInTrape[0].gameObject.SetActive(true);
        //        monkeyInTrape.RemoveAt(0);
        //        trapTime.RemoveAt(0);
        //        traps[firstTrapUse].SetActive(false);
        //        trapUse[firstTrapUse] = false;
        //        if (firstTrapUse == 0)
        //        {
        //            firstTrapUse = 1;
        //            trapCatch[0] = false;
        //        }
        //        else if (firstTrapUse == 1) {
        //            firstTrapUse = 0;
        //            trapCatch[1] = false;
        //        } 
        //    }
        //}
    }

    public void SetTrapOver() {
        for (int i = 0; i < 2; i++) {
            if (!trapUse[i])
            {
                trapUse[i] = true;
                traps[i].SetActive(true);
                traps[i].transform.position = this.transform.position + new Vector3(0,-0.6f,0);
                digging = false;
                switchMove(true);
                Tool.enabled = false;
                craftSystem.SetFuncBusy(false);
                return;
            }
        }
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
