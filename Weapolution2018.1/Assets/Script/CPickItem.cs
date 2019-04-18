using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPickItem : MonoBehaviour {
    CItem item;
    public bool test;
    public int id;
    SpriteRenderer render, shadowRender;
    public CPickItemSystem pickitem_system = null;
    bool b_flying = false, b_falling = false, setColliderOnce = false;
    public float f_gravity,ground_height, f_resistance;
    Vector3 fly_dir_vec3, add_velocity_vec3, origin_pos_vec3, trans_vec3;
    float f_flyTime, f_fallingTime, f_fallTime, lifeTime = 15.0f, time = 0.0f, blinkTime = 1.0f;
    float blinkGapTime = 0.5f;
    int i_fly_way;
    Collider2D collider;
    LevelHeight levelHeight;
    ZArrange zArrange;

    int weaponUsedTime = 0;

    private void Awake()
    {
        render = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        shadowRender = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        collider = this.GetComponent<Collider2D>();
        f_flyTime = 2.0f * ground_height / f_gravity;
        if(StageManager.currentStage == 7)levelHeight = GetComponent<LevelHeight>();
        zArrange = GetComponent<ZArrange>();
    }

    private void Update()
    {
        if (StageManager.timeUp) return;
        if (b_flying) ThrowFly();
        if (b_falling) OnFalling();
        if (item != null && item.attack < 0) {
            if (time < lifeTime) {
                if(!test) time += Time.deltaTime;
                if (time > lifeTime - 2.0f) {
                    OnDisappear();
                }
            }
            else
            {
                blinkGapTime = 0.5f;
                blinkTime = 1.0f;
                time = 0.0f;
                SetInFree();
            }
        }

    }


    public void SetSystem(CPickItemSystem system) {
        this.pickitem_system = system;
    }

    public void SetPickItem(int identity) {
        render.sprite = CItemDataBase.spriteList[identity];
        shadowRender.sprite = CItemDataBase.spriteList[identity];
        id = identity;
        item = CItemDataBase.items[id];
    }
    public void SetInFree() {
        weaponUsedTime = 0;

        time = 0.0f;
        f_fallingTime = 0.0f;
        b_falling = false;
        b_flying = false;
        setColliderOnce = false;
        render.enabled = true;
        shadowRender.enabled = false;
        this.GetComponent<COutLine>().SetOutLine(false);
        pickitem_system.AddFreeList(this);
        this.gameObject.SetActive(false);

        //this.transform.parent = pickitem_system.gameObject.transform.GetChild(0);
    }
    public void SetInField() {
        this.transform.parent = pickitem_system.gameObject.transform.GetChild(1);
    }

    public void SetFall(float _height, Vector3 _flyDir, float speed)
    {
        //float ground_height = _height;
        f_fallTime = 2.0f * _height / f_gravity;
        origin_pos_vec3 = transform.position;
        b_falling = true;
        collider.enabled = false;
        fly_dir_vec3 = new Vector3(speed * _flyDir.x, speed * _flyDir.y, 0);
        add_velocity_vec3 = new Vector3(0.0f, -f_gravity, 0.0f);
        shadowRender.enabled = false;
    }
    public void SetFall(float _height,Vector3 _flyDir, float speed, float height) {
        //float ground_height = _height;
        f_fallTime = 2.0f * _height / f_gravity;
        origin_pos_vec3 = transform.position;
        b_falling= true;
        collider.enabled = false;
        fly_dir_vec3 = new Vector3(speed * _flyDir.x, speed * _flyDir.y, 0);
        add_velocity_vec3 = new Vector3(0.0f, -f_gravity, 0.0f);
        shadowRender.enabled = false;
        levelHeight.SetSpecificHeight(height);
    }

    void OnFalling() {
        f_fallingTime += Time.deltaTime;
        if (f_fallingTime * f_fallingTime <= f_fallTime)
        {
            trans_vec3 = f_fallingTime * fly_dir_vec3 + 0.5f * f_fallingTime * f_fallingTime * add_velocity_vec3;
            transform.position = origin_pos_vec3 + trans_vec3;
            if (!setColliderOnce && f_fallingTime * f_fallingTime > f_fallTime * 0.64f)
            {
                setColliderOnce = true;
                collider.enabled = true;
            }
        }
        else
        {
            setColliderOnce = false;
            f_fallingTime = 0.0f;
            b_falling = false;
            shadowRender.enabled = true;
            if(StageManager.currentStage == 7)levelHeight.SetHeight();
        }
    }

    public void SetThrow(Vector3 _flyDir, float speed)
    {
        origin_pos_vec3 = transform.position;
        b_flying = true;
        //i_fly_way = fly_dir;
        collider.enabled = false;
        //若是比較平拋橫向速度較大
        if (Mathf.Abs(_flyDir.x) < 0.85f) fly_dir_vec3 = new Vector3(speed * _flyDir.x, speed * 0.8f * _flyDir.y, 0);
        else fly_dir_vec3 = new Vector3(speed * 0.72f * _flyDir.x, speed * 0.8f * _flyDir.y, 0);
        add_velocity_vec3 = new Vector3(0.0f, -f_gravity, 0.0f);
        //若是往下丟，重力需要較小
        if (_flyDir.y < 0.0f) add_velocity_vec3 = new Vector3(0.0f, -f_gravity - (_flyDir.y * 25.0f), 0.0f);
        shadowRender.enabled = false;
        SetZBase(-3.0f);

    }

    public void SetThrow(Vector3 _flyDir, float speed, float height) {
        origin_pos_vec3 = transform.position;
        b_flying = true;
        //i_fly_way = fly_dir;
        collider.enabled = false;
        //若是比較平拋橫向速度較大
        if(Mathf.Abs(_flyDir.x) < 0.85f )fly_dir_vec3 = new Vector3(speed*_flyDir.x, speed *0.8f* _flyDir.y, 0);
        else fly_dir_vec3 = new Vector3(speed * 0.72f * _flyDir.x, speed *0.8f* _flyDir.y, 0);
        add_velocity_vec3 = new Vector3(0.0f, -f_gravity, 0.0f);
        //若是往下丟，重力需要較小
        if (_flyDir.y < 0.0f) add_velocity_vec3 = new Vector3(0.0f, -f_gravity - (_flyDir.y*25.0f), 0.0f);
        shadowRender.enabled = false;
        levelHeight.SetSpecificHeight(height - 3.0f);
        //SetZBase(-3.0f);
        
        //switch (fly_dir)
        //{
        //    case 0:
        //        fly_dir_vec3 = new Vector3(0.0f, speed*0.7f, 0.0f);
        //       add_velocity_vec3 = new Vector3(0.0f, -f_resistance, 0.0f);
        //        break;
        //    case 1:
        //        fly_dir_vec3 = new Vector3(0.0f, -speed*1.15f, 0.0f);
        //        add_velocity_vec3 = new Vector3(0.0f, f_resistance, 0.0f);
        //        break;
        //    case 2:
        //        fly_dir_vec3 = new Vector3(-speed, 0.0f, 0.0f);
        //        add_velocity_vec3 = new Vector3 (f_resistance, -f_gravity, 0.0f);
        //        break;
        //    case 3:
        //        fly_dir_vec3 = new Vector3(speed, 0.0f, 0.0f);
        //        add_velocity_vec3 = new Vector3(-f_resistance, -f_gravity, 0.0f);
        //        break;
        //}

    }

    void ThrowFly() {
        f_fallingTime += Time.deltaTime;
        if (f_fallingTime * f_fallingTime <= f_flyTime)
        {
            //if (Mathf.Abs(fly_dir_vec3.x + fly_dir_vec3.y) < 0.2f) fly_dir_vec3 = Vector3.zero;
            trans_vec3 = f_fallingTime * fly_dir_vec3 + 0.5f*f_fallingTime*f_fallingTime * add_velocity_vec3;
            transform.position = origin_pos_vec3 + trans_vec3;
            if (!setColliderOnce && f_fallingTime * f_fallingTime > f_flyTime * 0.81f) {
                setColliderOnce = true;
                collider.enabled = true;
            } 
            //transform.position += 0.5f * Time.deltaTime * Time.deltaTime * resistance_vec3;
        }
        else {
            if (StageManager.currentStage != 7) SetZBase(0.0f);
            else levelHeight.SetHeight();
            setColliderOnce = false;
            f_fallingTime = 0.0f;
            b_flying = false;
            shadowRender.enabled = true;
        } 
    }

    public void SetZBase(float _base) {
        zArrange.f_base = _base;
    }

    void OnDisappear() {
        if (blinkTime > blinkGapTime) {
            render.enabled = !render.enabled;
            shadowRender.enabled = !shadowRender.enabled;
            blinkTime = 0.0f;
            if(blinkGapTime > 0.4f)blinkGapTime -= 0.2f; //原本閃比較慢，後來變0.15
            else if(blinkGapTime > 0.15f) blinkGapTime -= 0.1f;
        }
        blinkTime += Time.deltaTime;
    }

    public void SetLifeTime(float _time) {
        time = _time;
        render.enabled = true;
    }

    public void SetShadowEnable(bool _isOn) {
        shadowRender.enabled = _isOn;
    }


    public int BeingUsed() {
        weaponUsedTime++;
        return (weaponUsedTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

}
