using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CBanana : CChildProject {
    bool bananaFly;
    int aniImgID = 0;
    float flyTime,currentTime, throwAngle =- 90.0f;
    bool initFly = false, bePlaced = false, boom = false, damageOnce = false;
    Vector3 fly_dir, addVec3, oringinPos, flyRecord;
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
    void Update () {
        if (StageManager.timeUp) return;
        if (bananaFly) Flying();
        if (boom) BoomAni();
        //if (bePlaced) {
        //    transform.position = oringinPos;
        //    transform.SetScaleX( oringinScaleX * Mathf.Sign(transform.parent.parent.parent.localScale.x)) ;
        //}
        if (time < life_time) time += Time.deltaTime;
        else {
            ResetChild();
            system.AddFree(this.transform);
        }
	}

    public override void SetOn()
    {
        bananaFly = true;
    }

    public override void ResetChild()
    {
        time = 0.0f;
        boom = false;
        bananaFly = false;
        bePlaced = false;
        boomDetect.enabled = false;
        damageOnce = false;
        initFly = false;
        image.sprite = boomImgs[8];
        aniImgID = 0;
    }
    public void SetFly(Vector2 dir) {

        //fly_dir = dir.normalized;
        
    }

    void Flying() {
        
        if (!initFly) {
            oringinPos = transform.position;
            throwAngle = Random.Range(0.0f,360.0f);
            fly_dir = Quaternion.AngleAxis(throwAngle , new Vector3(0, 0, 1)) * Vector3.up;
            RaycastHit2D hitWall = Physics2D.Raycast(oringinPos, fly_dir, 6.0f, 1 << LayerMask.NameToLayer("Obstacle"));
            //Debug.DrawRay(oringinPos, 2.8f*fly_dir,Color.blue);
            int hitTimes=0;
            while (hitWall && hitTimes < 20) {
                throwAngle -= 20.0f;
                fly_dir = Quaternion.AngleAxis(throwAngle, new Vector3(0, 0, 1)) * Vector3.up;
                hitWall = Physics2D.Raycast(oringinPos, fly_dir, 5.0f, 1 << LayerMask.NameToLayer("Obstacle"));
                hitTimes++;
            }
            //若太垂直往下丟，重力加速度小一點
            if (fly_dir.y < -0.7f) addVec3 *= 0.8f;
            else if (fly_dir.y > -0.3f) fly_dir = Quaternion.AngleAxis(throwAngle - 15.0f, new Vector3(0, 0, 1)) * Vector3.up;
            fly_dir *= speed;
            initFly = true;
            shadowRender.enabled = false;
        }
        
        currentTime += Time.deltaTime;
        if (currentTime * currentTime <= flyTime)
        {
            Vector3 trans = fly_dir * currentTime + 0.5f * addVec3 * currentTime * currentTime;
            this.transform.Rotate(new Vector3(0,0,-Mathf.Sign(fly_dir.x)*900.0f*Time.deltaTime));
            this.transform.position = oringinPos + trans;
            flyRecord = oringinPos + trans;
            //Debug.Log("fly record" + flyRecord);
        }
        else{
            //Debug.Log("fly over record" + flyRecord);
            this.transform.position = flyRecord;
            //oringinPos = flyRecord;
            transform.rotation = Quaternion.Euler(0,0,0);
            currentTime = 0.0f;
            initFly = false;
            bananaFly = false;
            bePlaced = true;
            shadowRender.enabled = true;
            Debug.Log("loc position" + oringinPos);
            //oringinScaleX *= Mathf.Sign(this.transform.parent.parent.parent.localScale.x) ;
        }

    }

    void BoomAni() {
        if (aniTime > 0.15f)
        {
            if(aniImgID <= 7)image.sprite = boomImgs[aniImgID];
            aniTime = 0.0f;
            aniImgID++;
        }
        aniTime += Time.deltaTime;
        if (aniImgID >= 8) {
            boom = false;
            bananaFly = false;
            bePlaced = false;
            boomDetect.enabled = false;
            damageOnce = false;
            initFly = false;
            image.sprite = boomImgs[aniImgID];
            aniImgID = 0;
            system.AddFree(this.transform);
        } 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (boom && collision.tag == "Player")
        {
            if (!damageOnce) {
                Debug.Log("give damage");
                damageOnce = true;
                collision.transform.parent.GetComponent<Crafter>().GetHurt();
            }
        }
        else {
            if (bePlaced && collision.tag == "Player")
            {
                boom = true;
                boomDetect.enabled = true;
                shadowRender.enabled = false;
                image.sortingOrder = 1;
            }
        }
    }

}
