using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPCraftSystem : MonoBehaviour {

    int final_id;
    float throwDegree = 0.0f, LBFixedTime;
    bool can_pick = false, b_handling, can_collect = false, LBFixed = true;
    //bool[] craft_records;
    Vector3 throwVec3;
    Transform Arrow, collectBarBk, collectBar;
    GameObject handling_item;
    SpriteRenderer handle, slot_img, slot_item_img;
    Animator crafterAnimator;
    CPickItem picking_item;
    CPickCollection pick_collect;
    CItem craftA, craftB, craftC;
    CraftMenu craftMenu;
    public HealEffects healEffect;
    [HideInInspector]
    public bool craftFunc = true, useController = false;
    [HideInInspector]
    public string whichPlayer;
    public float throwSpeed;
    public CItem[] items;
    public GameObject Slot;

    PlayerControl playerControl;
    SpriteRenderer tool;
    public CChildProjectSystem trapSystem;
    public Sprite ToolImg, unToolImg;
    int trapNum;
    bool unTool;
    float unBuildTime;

    // CCraftItem craftReslut;

    // Use this for initialization
    void Awake()
    {
        handle = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        handle.enabled = false;
        //items = ItemDataBase.GetItemDataBase();
        //ItemDataBase.SetSpriteList();
        //CItemDataBase.SetItemDataBase();
        //CItemDataBase.SetSpriteList();
        items = CItemDataBase.items;
        slot_img = Slot.GetComponent<SpriteRenderer>();
        slot_item_img = Slot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        collectBarBk = transform.Find("CollectBar").GetChild(0);
        collectBar = transform.Find("CollectBar").GetChild(1);
        slot_img.enabled = true;
        slot_item_img.enabled = true;
        //craftReslut = Slot.GetComponent<CCraftItem>();
        ChangeSlot(true, 0);
        //craft_records = new bool[items.Length];
        Arrow = this.transform.Find("Arrow");
        //craftMenu = GameObject.Find("CraftMenu").GetComponent<CraftMenu>();
        crafterAnimator = transform.parent.GetComponent<Animator>();
        craftFunc = true;
        //healEffect = GameObject.Find("HealEffects").GetComponent<HealEffects>();

        playerControl = transform.parent.GetComponent<PlayerControl>();
        tool = transform.parent.Find("Tool").GetComponent<SpriteRenderer>();
        playerControl.SubCraftFunc(SetTrapOver, CollectOver);
    }

    private void Start()
    {
       
        //Debug.Log(whichPlayer);
        //whichPlayer = "p2";
        //useController = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (StageManager.timeUp) return;
        if (can_pick) CheckPickItem();
        ThrowItem();
        PickItem();
        Collect();
        InputFixed();
        UseTrap();
        UnBuildTrape();
        //是否靠近鍛造爐
        //if (forge.showUp && Vector2.Distance(this.transform.position, forge.fixed_pos) < forge_dis)
        //    NearForge();
        //else ThrowItem();



        //if (Input.GetButton(whichPlayer + "ButtonA"))
        //{
        //    Debug.Log("trap");
        //    if (spikeNum < 3 && !trapOnce) {
        //        Instantiate(spike, transform.parent.position, Quaternion.identity);
        //        spikeNum++;
        //        trapOnce = true;
        //    } 
        //}
        //if (trapTime < 1.0f) trapTime += Time.deltaTime;
        //else {
        //    trapTime = 0.0f;
        //    trapOnce = false;
        //} 

    }

    public void Init(CharacterVoice voice, string con) {
        if (con == "keyboard") useController = false;
        else useController = true;
        whichPlayer = con;
    }

    void InputFixed()
    {
        if (!LBFixed)
        {
            if (LBFixedTime > 0.2f)
            {
                LBFixed = true;
                LBFixedTime = 0.0f;
            }
            LBFixedTime += Time.deltaTime;
        }
    }

    void CheckPickItem()
    {
        if (!craftFunc) return;
        if (picking_item != null)
        {
            if (!picking_item.gameObject.activeSelf)
            {
                can_pick = false;
                picking_item = null;
                ChangeSlot(true, -1);
            }
        }
    }

    void PickItem()
    {
        if (!can_pick || picking_item == null || !craftFunc) return;
        if (useController)
        {
            if (Input.GetButtonDown(whichPlayer + "LB"))
            {
                if (!LBFixed) return;
                LBFixed = false;
                if (CItemDataBase.items[picking_item.id].elementID < -20)
                {
                    playerControl.RecoverHp(3.0f);
                    healEffect.SetHealEffectAni();
                    picking_item.GetComponent<COutLine>().SetOutLine(false);
                    picking_item.SetInFree();
                    ChangeSlot(true, -1);
                }
                else
                {
                    handle.enabled = true;
                    if (!b_handling)
                    {
                        b_handling = true;
                        craftA = items[picking_item.id];
                        handling_item = picking_item.gameObject;
                        handling_item.transform.parent = transform.GetChild(0);
                        handling_item.transform.position = this.transform.position;
                        handling_item.GetComponent<COutLine>().SetOutLine(false);
                        handling_item.SetActive(false);
                        handle.sprite = CItemDataBase.spriteList[craftA.id];
                        handling_item.GetComponent<CPickItem>().SetLifeTime(0.0f);
                        ChangeSlot(true, 0);
                        ArrowEnable(true);
                    }
                    else
                    {
                        OnCrafting();
                    }
                }
                can_pick = false;
                picking_item = null;

                //if (picking_item == null) return;
                //crafterAnimator.SetBool("is_gather", true);
                // Debug.Log("設動畫");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (picking_item == null) return;
                //crafterAnimator.SetBool("is_gather", true);

                if (CItemDataBase.items[picking_item.id].elementID < -20)
                {
                    playerControl.RecoverHp(3.0f);
                    healEffect.SetHealEffectAni();
                    picking_item.GetComponent<COutLine>().SetOutLine(false);
                    picking_item.SetInFree();
                    ChangeSlot(true, -1);
                }
                else
                {
                    handle.enabled = true;
                    if (!b_handling)
                    {
                        b_handling = true;
                        craftA = items[picking_item.id];
                        handling_item = picking_item.gameObject;
                        handling_item.transform.parent = transform.GetChild(0);
                        handling_item.transform.position = this.transform.position;
                        handling_item.GetComponent<COutLine>().SetOutLine(false);
                        handling_item.SetActive(false);
                        handle.sprite = CItemDataBase.spriteList[craftA.id];
                        handling_item.GetComponent<CPickItem>().SetLifeTime(0.0f);
                        ChangeSlot(true, 0);
                        ArrowEnable(true);
                    }
                    else
                    {
                        OnCrafting();
                    }
                }
                can_pick = false;
                picking_item = null;
            }
        }

    }

    void CheckCrafting()
    {
        //if (craftA.id == 3 || craftB.id == 3 || 
        //    craftA.id == items.Length-1 || craftB.id == items.Length - 1) return; //合成元素為火
        final_id = craftA.elementID + craftB.elementID;
        for (int i = 0; i < items.Length; i++)
        {
            if (final_id == items[i].craftingID)
            {
                ChangeSlot(false, i);
                //if (craft_records[i]) ChangeSlot(false, i);
                //else ChangeSlot(false, 0);
                Debug.Log("craft final" + final_id + items[i].image);
                craftC = items[i];
                return;
            }
        }
        //Debug.Log();
        ChangeSlot(false, -1);
        //ChangeSlot(false, 0);
        craftC = items[items.Length - 1];
    }

    void OnCrafting()
    {
        if (craftC == null) return;
        ChangeSlot(true, 0);
        craftA = craftC;
        picking_item.SetInFree();

        if (craftC.id > 0)
        {
            handle.sprite = CItemDataBase.spriteList[craftA.id];
            handling_item.GetComponent<CPickItem>().SetPickItem(craftA.id);
            //if (!craft_records[craftA.id])
            //{
            //    craft_records[craftA.id] = true;
            //    //craftMenu.UpdateMenuInfo(craftA.id);
            //}
        }
        else
        {
            handle.sprite = CItemDataBase.spriteList[items.Length - 1];
            handling_item.GetComponent<CPickItem>().SetPickItem(items.Length - 1);
        }
        craftC = null;
    }

    void ChangeSlot(bool clear, int id)
    {
        if (clear)
        {
            slot_img.enabled = false;
            slot_item_img.sprite = null;
        }
        else
        {
            slot_img.enabled = true;
            slot_item_img.enabled = true;
            if (id < 0)
            {
                slot_item_img.sprite = CItemDataBase.fail_sprite;
                return;
            }
            slot_item_img.sprite = CItemDataBase.spriteList[id];
            Debug.Log("craft final" + id + CItemDataBase.spriteList[id].name);
        }

    }

    void ThrowItem()
    {
        if (!b_handling || !craftFunc) return;
        //int throw_way = -1;
        //if (Input.GetKeyDown(KeyCode.W)) throw_way = 0;
        //else if (Input.GetKeyDown(KeyCode.S)) throw_way = 1;
        //else if (Input.GetKeyDown(KeyCode.A)) throw_way = 2;
        //else if (Input.GetKeyDown(KeyCode.D)) throw_way = 3;
        //ArrowEnable(true);
        bool goingThrow = false;
        if (useController)
        {
            Vector3 throw_vec3 = new Vector3(Input.GetAxis(whichPlayer + "RHorizontal"), Input.GetAxis(whichPlayer + "RVertical")).V3NormalizedtoV2();
            if (Mathf.Abs(throw_vec3.x) + Mathf.Abs(throw_vec3.y) > 0.15f)
                throwVec3 = throw_vec3;
            throwDegree = -Mathf.Atan2(throwVec3.x, throwVec3.y) * Mathf.Rad2Deg;
            Arrow.rotation = Quaternion.Euler(0, 0, throwDegree);
            if (Input.GetButton(whichPlayer + "RB")) goingThrow = true;
        }
        else
        {
            throwVec3 = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).V3NormalizedtoV2();
            throwDegree = -Mathf.Atan2(throwVec3.x, throwVec3.y) * Mathf.Rad2Deg;
            Arrow.rotation = Quaternion.Euler(0, 0, throwDegree);
            if (Input.GetMouseButtonDown(0)) goingThrow = true;
        }

        if (goingThrow)
        {
            handling_item.SetActive(true);
            handling_item.GetComponent<CPickItem>().SetInField();
            handling_item.transform.position = this.transform.GetChild(0).position;
            handling_item.GetComponent<CPickItem>().SetThrow(throwVec3, throwSpeed);
            ArrowEnable(false);
            if (can_pick) ChangeSlot(false, picking_item.id);
            else ChangeSlot(true, -1);
            //can_pick = false;
            //picking_item = null;
            handle.enabled = false;
            b_handling = false;
            goingThrow = false;
        }

    }

    //void NearForge() {
    //    ArrowEnable(false);
    //    if (b_handling && craftA.id == 3)forge.CheckForge();  //初步確認目前熔爐內容物並改圖片
    //    if (useController)
    //    {
    //        //往鍛造爐丟材料
    //        if (b_handling && Input.GetButtonDown(whichPlayer + "RB"))
    //        {
    //            if (forge.ThrowIn(craftA.id))
    //            {
    //                handling_item.GetComponent<CPickItem>().SetInFree();
    //                ChangeSlot(true, 0);
    //                picking_item = null;
    //                handle.enabled = false;
    //                b_handling = false;
    //                ArrowEnable(false);
    //            }
    //        }
    //        //鍛造
    //        if (Input.GetButtonDown(whichPlayer + "LB"))
    //        {
    //            forge.OnForging();
    //        }
    //    }
    //    else
    //    {
    //        //往鍛造爐丟材料
    //        if (b_handling && Input.GetMouseButtonDown(0))
    //        {
    //            if (forge.ThrowIn(craftA.id))
    //            {
    //                handling_item.GetComponent<CPickItem>().SetInFree();
    //                ChangeSlot(true, 0);
    //                picking_item = null;
    //                handle.enabled = false;
    //                b_handling = false;
    //                ArrowEnable(false);
    //            }
    //        }
    //        //鍛造
    //        if(Input.GetKeyDown(KeyCode.E)) {
    //            forge.OnForging();
    //        }
    //    }

    //}

    void CollectAni()
    {
        Vector2 offset = new Vector2(pick_collect.transform.position.x - transform.parent.position.x,
                                       pick_collect.transform.position.y - transform.parent.position.y);
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            if (offset.x >= 0.0f) crafterAnimator.SetInteger("face_way", 3);
            else crafterAnimator.SetInteger("face_way", 2);
        }
        else
        {
            if (offset.y >= 0.0f) crafterAnimator.SetInteger("face_way", 0);
            else crafterAnimator.SetInteger("face_way", 1);
        }
        crafterAnimator.SetBool("is_gather", true);
        crafterAnimator.SetBool("is_walk", false);
    }

    void Collect()
    {
        if (!can_collect || !craftFunc) return;
        if (useController)
        {
            if (Input.GetButtonDown(whichPlayer + "LB"))
            {
                if (!LBFixed) return;
                LBFixed = false;
                CollectAni();
                playerControl.SetCollect();
                StartCoroutine("OnCollecting");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CollectAni();
                playerControl.SetCollect();
                StartCoroutine("OnCollecting");
            }
        }
    }

    IEnumerator OnCollecting()
    {
        Debug.Log(this.gameObject.name + "coroutine");
        can_collect = false;
        craftFunc = false;
        float time = 0.0f;
        collectBarBk.GetComponent<SpriteRenderer>().enabled = true;
        collectBar.GetComponent<SpriteRenderer>().enabled = true;
        while (time < 1.0f)
        {
            time += Time.deltaTime * 1.25f;
            collectBar.localScale = new Vector3(Mathf.Lerp(0.0f, 1.05f, time), 0.1f, 1.0f);
            yield return null;
        }
        //playerControl.SetIDle();
    }

    void CollectOver() {
        //crafterAnimator.SetBool("is_gather", false);
        collectBarBk.GetComponent<SpriteRenderer>().enabled = false;
        collectBar.GetComponent<SpriteRenderer>().enabled = false;
        pick_collect.ThrowItemOut();
        pick_collect = null;
        craftFunc = true;
    }

    public void ThrowOut()
    {
        handling_item.GetComponent<CPickItem>().SetInFree();
        ChangeSlot(true, 0);
        picking_item = null;
        handle.enabled = false;
        b_handling = false;
        ArrowEnable(false);
    }

    public void ArrowEnable(bool enable)
    {
        Arrow.GetChild(0).GetComponent<SpriteRenderer>().enabled = enable;
        Arrow.GetChild(1).GetComponent<SpriteRenderer>().enabled = enable;
    }
    public CItem CheckHandle()
    {
        if (!b_handling) return items[0];
        return craftA;
    }

    public bool CheckIsFree()
    {
        if (can_pick || can_collect) return false;
        else return true;
    }

    void UseTrap() {
        if (!craftFunc || trapNum >= 2) return;
        if (useController)
        {
            if (Input.GetButtonDown(whichPlayer + "ButtonX") && CanSetTrap())
            {
                if (playerControl.SetUseTrap()) {
                    crafterAnimator.SetTrigger("usingTrape");
                    craftFunc = false;
                    trapNum++;
                }  
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Q) && CanSetTrap()) {
                if (playerControl.SetUseTrap())
                {
                    crafterAnimator.SetTrigger("usingTrape");
                    trapNum++;
                    craftFunc = false;
                }
            }
        }
        
    }
    bool CanSetTrap() {
        tool.enabled = true;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.8f, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            unTool = true;
            tool.sprite = unToolImg;
            return false;
        }
        else {
            tool.sprite = ToolImg;
            return true;
        } 
    }
    public void SetTrapOver() {
        trapSystem.AddUsed(transform.position);
        trapSystem.GetNewestChild().SetOn(false, RecycleTrape);
        tool.enabled = false;
        craftFunc = true;
    }
    void UnBuildTrape()
    {
        if (unTool)
        {
            unBuildTime += Time.deltaTime;
            if (unBuildTime > 0.5f)
            {
                unBuildTime = 0.0f;
                tool.enabled = false;
                unTool = false;
            }

        }
    }
    public void RecycleTrape()
    {
        if (trapNum > 0) trapNum--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("enter");
        if (collision.tag == "PickItem")
        {
            if (!can_pick && !can_collect && craftFunc)
            {
                picking_item = collision.gameObject.GetComponent<CPickItem>();
                picking_item.GetComponent<COutLine>().SetOutLine(true);
                if (!b_handling)
                {
                    Debug.Log("going to pick");
                    ChangeSlot(false, picking_item.id);
                }
                else
                {
                    Debug.Log("going to craft");
                    craftB = items[picking_item.id];
                    CheckCrafting();
                }
                can_pick = true;
            }
        }
        else if (!can_collect && collision.tag == "PickCollection" && !can_pick)
        {
            if (!craftFunc) return;
            can_collect = true;
            pick_collect = collision.transform.GetComponent<CPickCollection>();
            pick_collect.GetComponent<COutLine>().SetOutLine(true);
            Debug.Log(pick_collect.gameObject.name);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("collision stay" + collision.name);
        if (collision.tag == "PickItem")
        {
            if (!can_pick && !can_collect && craftFunc)
            {
                Debug.Log("empty");
                picking_item = collision.GetComponent<CPickItem>();
                picking_item.GetComponent<COutLine>().SetOutLine(true);
                if (!b_handling)
                {
                    ChangeSlot(false, picking_item.id);
                }
                else
                {
                    craftB = items[picking_item.id];
                    CheckCrafting();
                }
                can_pick = true;
            }
        }
        else if (!can_collect && collision.tag == "PickCollection" && !can_pick && craftFunc)
        {
            can_collect = true;
            pick_collect = collision.transform.GetComponent<CPickCollection>();
            pick_collect.GetComponent<COutLine>().SetOutLine(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PickItem")
        {
            //if (pick_collect == null) return;
            if (picking_item != null && collision.gameObject == picking_item.gameObject)
            {
                Debug.Log("the same");
                picking_item.GetComponent<COutLine>().SetOutLine(false);
                picking_item = null;
                ChangeSlot(true, 0);
                if (can_pick) can_pick = false;
            }
            //if (can_pick) can_pick = false;
        }
        if (can_collect && collision.tag == "PickCollection")
        {
            Debug.Log("LEAVE COLLECT");
            //if (pick_collect == null) return;
            if (pick_collect != null && collision.gameObject == pick_collect.gameObject)
            {
                pick_collect.GetComponent<COutLine>().SetOutLine(false);
                can_collect = false;
                pick_collect = null;
            }

        }
    }
}
