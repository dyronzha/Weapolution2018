using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPickWeapon : MonoBehaviour {
    bool canPick = false, ishold = false, ButtonXFixed = true;
    bool useControll = true;
    int lastID = -1;
    float ButtonXTime;
    string whichPlayer = "p1";//Player.p1joystick;
    CPickItem pickWeapon, tempPick, lastPick;
    List<CPickItem> pickUsed;
    public CItem weapon; //這類別裡有一個attack變數是攻擊力
    SpriteRenderer playerWeaponImg;
    public List<Sprite> weaponSprite;
    public bool test;
    public TutorialRequest tutorialRequest;

    void Start () {
        pickUsed = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>().usedPickItemList;
        playerWeaponImg = transform.parent.Find("Weapon").GetChild(0).GetComponent<SpriteRenderer>();
        Player.weapon = CItemDataBase.items[0]; //空手
        playerWeaponImg.sprite = weaponSprite[0];
        //useControll = Player.p1controller;
        //whichPlayer = Player.p1joystick;
        if (!Player.p1charaType)
        {
            if (Player.p1controller) //p1用搖桿
            {
                useControll = true;
                whichPlayer = Player.p1joystick;
            }
            else
            {
                useControll = false;

            }

        }
        else
        {
            if (Player.p2controller) //p1用搖桿
            {
                useControll = true;
                whichPlayer = Player.p2joystick;
            }
            else
            {
                useControll = false;

            }
        }
        //Debug.Log(whichPlayer);
        //whichPlayer = "p2";
        //useControll = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (StageManager.timeUp) return;
        IsAwayWeapon();
        IsNearWeapon();
        OnPickWeapon();
        ThrowWeapon();
        FixedInput();
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("p1ButtonY")) ThrowWeapon();


    }

    void FixedInput() {
        if (!ButtonXFixed) {
            ButtonXTime += Time.deltaTime;
            if (ButtonXTime > 0.2f) {
                ButtonXFixed = true;
                ButtonXTime = 0.0f;
            }
        }
    }

    void OnPickWeapon() {
        if (canPick && Player.weapon.id == 0) {
            if (useControll)
            {
                if (Input.GetButtonDown(whichPlayer + "ButtonX"))
                {
                    if (!ButtonXFixed) return;
                    ButtonXFixed = false;
                    pickWeapon = lastPick;
                    Player.weapon = CItemDataBase.items[pickWeapon.id];
                    pickWeapon.transform.parent = this.transform;
                    pickWeapon.gameObject.SetActive(false);
                    ishold = true;
                    canPick = false;
                    playerWeaponImg.sprite = weaponSprite[Player.weapon.id];
                    if(test)tutorialRequest.DonePickUp(false);
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.E)) {
                    pickWeapon = lastPick;
                    Player.weapon = CItemDataBase.items[pickWeapon.id];
                    pickWeapon.transform.parent = this.transform;
                    pickWeapon.gameObject.SetActive(false);
                    ishold = true;
                    canPick = false;
                    playerWeaponImg.sprite = weaponSprite[Player.weapon.id];
                    if (test) tutorialRequest.DonePickUp(false);
                }
            }
        }
        
    }
    public void ThrowWeapon() {
        if (ishold)
        {
            bool goThrow = false;
            
            Vector3 fallWay = new Vector3(0,0,0);
            if (useControll)
            {
                if (Input.GetButtonDown(whichPlayer + "ButtonX")) {
                    if (!ButtonXFixed) return;
                    ButtonXFixed = false;
                    goThrow = true;
                }
                    
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    goThrow = true;
            }
            if (goThrow) {
                switch (Player.face_way)
                {
                    case 0:
                        fallWay = new Vector3(0, 1, 0);
                        break;
                    case 1:
                        fallWay = new Vector3(0, -1, 0);
                        break;
                    case 2:
                        fallWay = new Vector3(-1, 0, 0);
                        break;
                    case 3:
                        fallWay = new Vector3(1, 0, 0);
                        break;
                }
                pickWeapon.gameObject.SetActive(true);
                pickWeapon.SetInField();
                pickWeapon.transform.position = transform.position + new Vector3(0,2.0f,0);
                pickWeapon.SetFall(2.0f, fallWay, 4.0f);
                if(test)tutorialRequest.doneThrowWeapon();
                ishold = false;
                weapon = null;
                Player.weapon = CItemDataBase.items[0]; //空手
                playerWeaponImg.sprite = weaponSprite[0];
            }
        }
    }
    public void DestroyWeapon() {
        pickWeapon.gameObject.SetActive(false);
        pickWeapon.SetInFree();
        ishold = false;
        weapon = null;
        playerWeaponImg.sprite = weaponSprite[0];
    }
    void IsNearWeapon() {
        if (ishold) return;
        int temp = -1;
        float temp_dis,least_dis = 2.5f,current_dis;
        
        CItem tempWeapon;
        for (int i = 0; i < pickUsed.Count; i++) {
            tempPick = pickUsed[i];
            tempWeapon = CItemDataBase.items[tempPick.id];
            if (tempWeapon.attack > 0) {
                temp_dis = Vector2.Distance(this.transform.position, tempPick.transform.position);
                if (temp_dis < 2.0f)
                {
                    current_dis = temp_dis;
                    //if (i == 0)least_dis = current_dis;
                    if (current_dis < least_dis)
                    {
                        least_dis = current_dis;
                        temp = i;
                    }
                }
            }
                 
        }
        if (temp < 0) {
            canPick = false;
            pickWeapon = null;
        }
        else {
            if (pickUsed[temp] != lastPick && canPick) {
                lastPick.GetComponent<COutLine>().SetOutLine(false);
            }
            canPick = true;
            lastPick = pickUsed[temp];
            lastPick.GetComponent<COutLine>().SetOutLine(true);
            //pickWeapon = lastPick;
        } 
    }

    void IsAwayWeapon() {
        if (canPick) {
            //Debug.Log("lastid"+lastID);
            float temp_dis = Vector2.Distance(this.transform.position, lastPick.transform.position);
            if (temp_dis >= 2.0f) {
                canPick = false;
                lastPick.GetComponent<COutLine>().SetOutLine(false);
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "PickItem" && CItemDataBase.items[collision.GetComponent<CPickItem>().id].attack > 0) {
    //        canPick = true;
    //        pickWeapon = collision.GetComponent<CPickItem>();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "PickItem" && pickWeapon!=null &&collision.gameObject == pickWeapon.gameObject) {
    //        canPick = false;
    //        pickWeapon = null;
    //    }
    //}





}
