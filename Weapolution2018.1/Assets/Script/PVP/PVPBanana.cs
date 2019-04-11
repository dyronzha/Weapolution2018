using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPBanana : CChildProject
{
    bool bananaFly;
    int aniImgID = 0;
    float flyTime, currentTime, throwAngle = -90.0f;
    bool bePlaced = false, boom = false, damageOnce = false;
    Vector3 addVec3, oringinPos, flyRecord;
    float life_time = 15.0f, time, aniTime;
    SpriteRenderer image, shadowRender;
    BoxCollider2D boomDetect;
    public float height, gravity, speed;
    public CEnemyMonkey monkey;
    public Sprite[] boomImgs;

    // Use this for initialization
    private void Awake()
    {
        flyTime = 2.0f * height / gravity;
        addVec3 = new Vector3(0, -gravity, 0);
        oringinPos = transform.position;
        image = this.GetComponent<SpriteRenderer>();
        shadowRender = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        boomDetect = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.timeUp) return;
        if (bananaFly) Flying();
        if (boom) BoomAni();
        if (bePlaced) {
            if (time < life_time) time += Time.deltaTime;
            else
            {
                ResetChild();
                system.AddFree(this.transform);
            }
        }

    }

    public override void SetOn()
    {
        bananaFly = true;
        shadowRender.enabled = false;
        oringinPos = transform.position;
    }

    public override void ResetChild()
    {
        time = 0.0f;
        boom = false;
        bananaFly = false;
        bePlaced = false;
        boomDetect.enabled = false;
        damageOnce = false;
        image.sprite = boomImgs[8];
        aniImgID = 0;
    }
    public void SetFly(Vector2 dir)
    {

        //fly_dir = dir.normalized;

    }

    void Flying()
    {

        currentTime += Time.deltaTime;
        if (currentTime * currentTime <= flyTime)
        {
            Vector3 trans = new Vector3(0,-1,0)* currentTime + 0.5f * addVec3 * currentTime * currentTime;
            this.transform.position = oringinPos + trans;
            flyRecord = oringinPos + trans;
        }
        else
        {
            this.transform.position = flyRecord;
            currentTime = 0.0f;
            bananaFly = false;
            bePlaced = true;
            shadowRender.enabled = true;
            Debug.Log("loc position" + oringinPos);
            //oringinScaleX *= Mathf.Sign(this.transform.parent.parent.parent.localScale.x) ;
        }

    }

    void BoomAni()
    {
        if (aniTime > 0.15f)
        {
            if (aniImgID <= 7) image.sprite = boomImgs[aniImgID];
            aniTime = 0.0f;
            aniImgID++;
        }
        aniTime += Time.deltaTime;
        if (aniImgID >= 8)
        {
            boom = false;
            bananaFly = false;
            bePlaced = false;
            boomDetect.enabled = false;
            damageOnce = false;
            image.sprite = boomImgs[aniImgID];
            aniImgID = 0;
            system.AddFree(this.transform);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            if (!boom) {
                collision.transform.parent.GetComponent<Crafter>().GetHurt();
                boom = true;
                image.sortingOrder = 1;
                bePlaced = false;
            }
        }

        //if (boom && collision.tag == "Player")
        //{
        //    if (!damageOnce)
        //    {
        //        Debug.Log("give damage");
        //        damageOnce = true;
        //        collision.transform.parent.GetComponent<Crafter>().GetHurt();
        //    }
        //}
        //else
        //{
        //    if (bePlaced && collision.tag == "Player")
        //    {
        //        boom = true;
        //        boomDetect.enabled = true;
        //        shadowRender.enabled = false;
        //        image.sortingOrder = 1;
        //    }
        //}
    }
}
