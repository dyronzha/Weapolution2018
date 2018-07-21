using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CForge : MonoBehaviour {

    int current_num = 0, total_num = 3, forgeID, heatPower = 0;
    int rockNum;
    float[] heatScale = new float[4] { 0.0f, 0.35f, 0.5f, 0.65f };
    private bool onForging = false;
    private CItem[] items;
    private SpriteRenderer[] images;
    SpriteRenderer hint;
    Transform fire;
    Animator animator;
    CPickItemSystem pickItemSystem;
    CEnemySystem enemySystem;
    public float fix_value;
    public bool showUp = false, isHeat = false;
    public Vector2 fixed_pos{
        get {
            return new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y +fix_value ); 
        }
        private set {}
        }

	// Use this for initialization
	void Awake() {
        items = new CItem[total_num];
        images = new SpriteRenderer[total_num];
        fire = transform.Find("fire");
        for (int i = 0; i < total_num; i++) {
            //Debug.Log(i);
            images[i] = transform.GetChild(1).transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
            images[i].enabled = false;
        }
        animator = this.GetComponent<Animator>();
        pickItemSystem = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>();
        enemySystem = GameObject.Find("EnemySystem").GetComponent<CEnemySystem>();
        hint = transform.Find("hint").GetComponent<SpriteRenderer>();
        hint.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!showUp && enemySystem.GetDeathNumber() >= 3) {
            ShowUp();
        }
        //if (Input.GetKeyDown(KeyCode.Z)) ShowUp();
        //if (heatPower > 0) {
        //    if (heatTime < 30.0f) heatTime += Time.deltaTime;
        //    else {
        //        heatPower--;
        //        heatTime = 0.0f;
        //        if (heatPower < 3) animator.SetTrigger("notHeat");
        //    }
        //}
	}

    public void ShowUp() {
        animator.Play("ShowUp");
        showUp = true;
        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //transform.GetComponent<BoxCollider2D>().enabled = true;
        for (int i = 0; i< 4; i++) {
            transform.GetChild(1).GetChild(i).GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    //public bool ThrowIn(int id)
    //{
    //    //如果丟進去的是火
    //    if (id == 3)
    //    {
    //        heatPower++;
    //        if (!isHeat && heatPower >= 3)
    //        {
    //            heatPower = 3;
    //            fire.localScale = new Vector3(heatScale[heatPower], heatScale[heatPower], 1.0f);
    //            isHeat = true;
    //            animator.Play("OnHeat");
    //            animator.SetBool("IsHeat", true);
    //        }
    //        return true;
    //    }
    //    else if (id == 2)
    //    {
    //        if (current_num < total_num)
    //        {
    //            items[current_num] = CItemDataBase.items[2];
    //            images[current_num].sprite = CItemDataBase.spriteList[2];
    //            images[current_num].enabled = true;
    //            current_num++;
    //            return true;
    //        }
    //        else return false;
    //    }
    //    else { return false; }
    //}

    public void ThrowFireIn() {
        if (onForging) return;
        heatPower++;
        animator.Play("ThrowIn");
        if (heatPower >= 1)
        {
            //heatPower = 1;
            if (heatPower >= 3) heatPower = 3;
            fire.localScale = new Vector3(heatScale[heatPower], heatScale[heatPower], 1.0f);
            isHeat = true;
            //animator.Play("OnHeat");
            animator.SetBool("IsHeat", true);
        }
    }

    public bool ThrowElementIn(int id) {
        if (onForging) return false;
        if (current_num < total_num && id > 0)
        {
            if (id == 2) rockNum++;
            items[current_num] = CItemDataBase.items[id];
            images[current_num].sprite = CItemDataBase.spriteList[id];
            images[current_num].enabled = true;
            current_num++;
            animator.Play("ThrowIn");
            return true;
        }
        else return false;
    }

    public void OnForging() {
        Debug.Log("OnForging  " + heatPower + "   " + onForging);
        if (heatPower <= 0 || onForging) return;
        
        onForging = true;
        animator.Play("DoneForge");
        //if (!isHeat) return;
    }

    public void ForgeOver() {
        onForging = false;
        heatPower--;
        if (heatPower <= 0) {
            heatPower = 0;
            isHeat = false;
            fire.GetComponent<SpriteRenderer>().enabled = false;
            animator.SetBool("IsHeat", false);
        } 
        fire.localScale = new Vector3(heatScale[heatPower], heatScale[heatPower], 1.0f);
        for (int i = 0; i < current_num; i++) {
            CPickItem temp;
            if (i < rockNum) {
                temp = pickItemSystem.SpawnInUsed(transform.position, 4);
            }
            else temp = pickItemSystem.SpawnInUsed(transform.position, CItemDataBase.items.Length - 1);
            temp.SetFall(1.5f,new Vector3(1,2.0f - 0.6f*i,0), 6.0f);
        }
        //if (canForge)  //能組合出的
        //{
        //    Transform temp = pickItemSystem.SpawnInUsed(transform.position, forgeID);
        //    temp.GetComponent<CPickItem>().SetFall(1.5f,new Vector3(1,2.0f,0), 6.0f);
        //}
        //else
        //{     //不能的變成灰燼

        //}
        ResetForge();
    }

    public void CheckForge() {
        //if (current_num <= 0 || last_num == current_num) return;
        //else {
        //    last_num = current_num;
        //    int tempForgeID = CItemDataBase.items[3].elementID; //先把火的id算進去
        //    for (int i = 0; i < current_num; i++) {
        //        tempForgeID += items[i].elementID;
        //    }
        //    for (int i = 0; i < CItemDataBase.items.Length; i++) {
        //        if (tempForgeID == CItemDataBase.items[i].craftingID) {
        //            images[total_num].sprite = CItemDataBase.spriteList[i];
        //            images[total_num].enabled = true;
        //            canForge = true;
        //            forgeID = i;
        //            return;
        //        } 
        //    }
        //    Debug.Log("fail forge");
        //    images[total_num].sprite = CItemDataBase.fail_sprite;
        //    images[total_num].enabled = true;
        //    canForge = false;
        //    forgeID = 0;
        //}
    }

    void ResetForge() {
        for (int i = 0; i<total_num; i++) {
            images[i].sprite = null;
        }
        rockNum = 0;
        current_num = 0;
    }

    public void SwitchHint(bool _enable) {
        hint.enabled = _enable;
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
