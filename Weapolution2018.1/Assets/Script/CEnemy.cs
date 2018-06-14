using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour {
    [HideInInspector]
    public int hurtValue;

    public bool test;
    public float f_speed, f_attack_dis, f_sight_dis;
    [Tooltip("怪物中心點的偏移距離")]
    public float f_pos_offsetY;
    [Tooltip("追逐時撞牆射線距離")]
    public float f_turn_dis;
    [Tooltip("偵測牆壁的方形碰撞器X邊長")]
    public float f_detect_longX;
    [Tooltip("偵測牆壁的方形碰撞器Y邊長")]
    public float f_detect_longY;
    [Tooltip("偵測牆壁的方形碰撞器中心點相較怪物位置的偏移")]
    public float f_center_offset, whichSide = 0.0f;
    //public float[] behavior_chance;
    public bool IsOut;
    public GameObject player;
    protected int hp = 5, state, lastState = -1,change_way, hitDir, rambleLayer;
    protected float state_time, inState_time, change_way_time, scaleX, scaleY, traceTime;
    protected bool b_trace = false, isForceState = false, b_keep_trace = false,attackOnce = false,
                                    getHurtOnce = false, isShowUp, isDead, canDetectAtk,traceAgain;
    protected Vector3 go_way, new_pos, attackPos,playerPos, self_pos, hitToDir;
    protected CEnemySystem enemySystem = null;
    protected RaycastHit2D detect_ray, detect1, detect2, detect3;
    protected Collider2D detect_box;
    protected Animator animator;
    //protected bool initAni = false;

    protected Rigidbody2D rigBody;
    // Use this for initialization
    public virtual void Awake() {
        inState_time = 0.0f;
        state_time = 1.0f;
        state = 0;
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0);
        animator = this.GetComponent<Animator>();
        rambleLayer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update() {
        if (!isShowUp) return;
        UpdatePos();
        StateMachine();
        
        //Ramble();
        //DetectObstacle();
    }

    public void SetEnemySystem(CEnemySystem system) {
        enemySystem = system;
    }

    //設定狀態
    public virtual void SetState(int num, bool _isForce) {
        if (isDead) return;
        if (num >= 0) state = num;
        else state = Random.Range(0, 10) % 2;
        state_time = 0.0f;
        inState_time = 0.0f;
        attackOnce = false;
        isForceState = _isForce;
    }

    public virtual void StateMachine() {
        DetectPlayer();  //若玩家夠近直接切成追逐模式，其他就隨機
        switch (state) {
            case 0:
                Idle();
                break;
            case 1:
                Ramble();
                break;
            case 2:
                Trace();
                break;
            case 3:
                Attack();
                break;
            case 4:
                GetHurt();
                break;
            case 5:
                Die();
                break;
        }
    }

    public void SetIddle() {
        SetState(0,false);
    }

    public virtual void Idle() {
        //Debug.Log("idle" + state_time);
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) state_time = Random.Range(1.5f, 3.0f);
        //if (inState_time > state_time) SetState(-1);
    }

    public virtual void Ramble() {
        //Debug.Log("ramble" + state_time);
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) {
            float face = Random.Range(-90.0f, 90.0f);
            state_time = Random.Range(2.0f, 4.5f);
            go_way = Quaternion.AngleAxis(face, new Vector3(0, 0, 1)) * (playerPos - self_pos).V3NormalizedtoV2();
        }
        if (DetectObstacle(new_pos)) RambleChangeWay();
        transform.position += f_speed * Time.deltaTime * go_way;
    }

    public virtual void Trace() {
        //if (CouculatePlayerDis(false, f_attack_dis)) { SetState(3); return; }
        Vector3 wana_trace = (playerPos - self_pos).V3NormalizedtoV2();
        detect_ray = Physics2D.Raycast(new_pos, wana_trace, f_turn_dis, 1 << LayerMask.NameToLayer("Obstacle"));
        if (detect_ray)
        {
            if (!b_keep_trace) //產生撞牆後的應變路線
            {
                RaycastHit2D detect1, detect2;
                Vector3 trace1, trace2;
                for (int i = 20; i < 360; i += 20)
                {
                    trace1 = Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * wana_trace;
                    trace2 = Quaternion.AngleAxis((360 - i), new Vector3(0, 0, 1)) * wana_trace;
                    detect1 = Physics2D.Raycast(new_pos, trace1, f_turn_dis, 1 << LayerMask.NameToLayer("Obstacle"));
                    detect2 = Physics2D.Raycast(new_pos, trace2, f_turn_dis , 1 << LayerMask.NameToLayer("Obstacle"));
                    Debug.DrawRay(new_pos, (f_turn_dis) * trace1, Color.blue);
                    Debug.DrawRay(new_pos, (f_turn_dis) * trace2, Color.white);
                    if (!detect1) { go_way = trace1; b_keep_trace = true; Debug.Log(i + " change trace" + go_way); break; }
                    if (!detect2) { go_way = trace2; b_keep_trace = true; Debug.Log(i + " change trace" + go_way); break; }
                    go_way = new Vector3(0, 0, 0);
                }
            }
            else { //應變路線後的撞牆判定
                detect3 = Physics2D.Raycast(new_pos, go_way, (f_turn_dis), 1 << LayerMask.NameToLayer("Obstacle"));
                if (detect3)
                {
                    Debug.Log("detect3 " + go_way);
                    b_keep_trace = false;
                }
            }
        }
        else {
            go_way = wana_trace;
            b_keep_trace = false;
            //Debug.Log("wana trace");
            
        }
        transform.position += f_speed * 1.3f * Time.deltaTime * go_way;
    }

    public virtual void Attack() {
        //if (!isForceState) {
        //    //animator.SetTrigger("IsAttack");
        //    //Debug.Log("attack");
        //    isForceState = true;

        //}
    }

    public virtual void SetOnAttackDetect() {
        canDetectAtk = true;
    }

    public virtual void SetOffAttackDetect()
    {
        canDetectAtk = false;
    }

    public virtual void AttackDetect() {
        if (!attackOnce) {
            attackOnce = true;
        }
    }

    public virtual void AttackOver() {
        isForceState = false;
        attackOnce = false;
    }

    public virtual bool DetectObstacle(Vector3 point) {
        Vector2 center;
        center = new Vector2(point.x + f_center_offset*go_way.x, point.y + f_center_offset*go_way.y);
        detect_box = Physics2D.OverlapBox(center, new Vector2(f_detect_longX,f_detect_longY), 0.0f, rambleLayer);
        Debug.DrawRay(center, f_detect_longX*go_way.normalized, Color.white);
        if (detect_box != null) return true;
        else return false;
    }

    void RambleChangeWay() {
        Vector3 normal, temp;
        temp = go_way;
        //根據設限所打到的障礙物，以此為反射面，進行路線反射的計算
        //障礙物先視同是橫擺的的，法向量根據此條件
        normal = new Vector3(0, (self_pos - detect_box.transform.position).y, 0).V3NormalizedtoV2();
        go_way = (go_way - 2 * (go_way.x * normal.x + go_way.y * normal.y) * normal).V3NormalizedtoV2();
        
        //如果反射後的路線又馬上撞壁，法向量調成是垂直擺的
        if (DetectObstacle(new_pos)) {
            normal = new Vector3((self_pos - detect_box.transform.position).x, 0, 0).V3NormalizedtoV2();
            go_way = (temp - 2 * (temp.x * normal.x + temp.y * normal.y) * normal).V3NormalizedtoV2();
        }
        //若短時間進行反復反射，則重新定義行為
        change_way++;
        ChangeWayCount();
    }

    //玩家是否夠近，決定追逐或攻擊或隨機
    public virtual void DetectPlayer() {
        if (isForceState) return;
        float offset = 0.0f;
        if (state == 2 || state == 3) offset = 1.5f * f_sight_dis;
        if (CouculatePlayerDis(false, 0.15f))
        {
            //因為視角關係，攻擊時y軸需要再多判定距離，像是在同一平面
            if (Mathf.Abs(self_pos.y - attackPos.y) < 0.1f) {
                if (state == 3) animator.SetTrigger("exist");
                SetState(3, true);
                Debug.Log("set to attack");
            } 
            else SetState(2, false);
        }
        else if (CouculatePlayerDis(false, f_sight_dis + offset))
        {
            SetState(2,false);
        }
        else {
            if (inState_time >= state_time &&state_time > 0.1f) SetState(-1, false);
        }
    }

    public virtual bool CouculatePlayerDis(bool more, float compare) {
        Vector2 _self_pos = new Vector2(self_pos.x, self_pos.y);
        Vector2 attack_pos = new Vector2(attackPos.x, attackPos.y);
        Vector2 player_pos = new Vector2(playerPos.x, playerPos.y);
        if (more)
        {
            Debug.Log("compare" + Vector2.Distance(_self_pos, attack_pos));
            if (Vector2.Distance(_self_pos, attack_pos) > compare) return true;
            else return false;
        }
        else {
            if (Vector2.Distance(_self_pos, attack_pos) < compare)return true;
            if (Vector2.Distance(_self_pos, player_pos) <= f_attack_dis) return true; //與玩家小於攻擊距離
            else return false;
        }
    }

    public virtual void UpdatePos() {
        new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0);
        self_pos = transform.position;
        playerPos = player.transform.position;
        if (!IsOut) {
            if (whichSide > 0.5f) attackPos = playerPos + new Vector3(f_attack_dis, 0.0f, 0.0f);
            else if (whichSide < -0.5f) attackPos = playerPos + new Vector3(-f_attack_dis, 0.0f, 0.0f);
            else attackPos = playerPos + new Vector3(Mathf.Sign(self_pos.x - playerPos.x) * f_attack_dis, 0.0f, 0.0f);
        }
    }

    public void ChangeWayCount() {
        change_way_time += Time.deltaTime;
        if (change_way_time < 1.0f)
        {
            if (change_way > 2)
            {
                change_way = 0;
                change_way_time = 0.0f;
                SetState(-1, false);
            }
        }
        else change_way_time = 0.0f;
    }

    public virtual void SetHurtValue(int _value, int _HitDir) {
        if (getHurtOnce) return;
        
        hurtValue = _value;
        SetState(4,true);
        hitDir = _HitDir;
        
    }

    public virtual void GetHurt()
    {
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) {
            state_time = 0.58f;
            hp -= hurtValue;
            Debug.Log("enemy : ouch");
            getHurtOnce = true;

            if (hitDir == 0) hitToDir = new Vector3(0, 1, 0);
            else if (hitDir == 1) hitToDir = new Vector3(0, -1, 0);
            else if (hitDir == 2) hitToDir = new Vector3(-1, 0, 0);
            else if (hitDir == 3) hitToDir = new Vector3(1, 0, 0);
            else hitToDir = new Vector3(0, 0, 0);
        }
        
        RaycastHit2D detect = Physics2D.Raycast(new_pos, hitToDir, 1.2f, 1 << LayerMask.NameToLayer("Obstacle"));
        if (!detect) {
            Debug.Log("botherrrrrrrrrr" + hitToDir);
            transform.position += Time.deltaTime * Vector3.Lerp(15.0f * hitToDir, Vector3.zero, inState_time*3.0f);
        }
        
        //if (!getHurtOnce) {
        //}
    }

    public virtual void GetHurtOver()
    {
        inState_time = 2.0f;
        getHurtOnce = false;
        isForceState = false;
        if (hp <= 0) SetState(5, true);
        Debug.Log("Hurt Over");
    }


    public virtual void BeTrapped(float trapTime)
    {
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) {
            state_time = trapTime;
            //enemySystem.PlaySound(0,1.0f);
        }
        
        if (inState_time >= state_time) isForceState = false;
    }

    public virtual void Die() {
        if (state_time < 0.1f) {
            state_time = 0.5f;
            inState_time = 0.0f;
            isDead = true;
        }
        inState_time += Time.deltaTime;
        if (inState_time >= state_time) {
            ResetEnemy();
            enemySystem.AddFreeList(this.transform);
        }
    }

    public virtual void ResetEnemy() {
        Debug.Log("reset enemy");
        enemySystem.RespawnEnemy();
        isForceState = false;
        state_time = 0.0f;
        inState_time = 0.0f;
        attackOnce = false;
        getHurtOnce = false;
        isDead = false;
        isShowUp = false;
        b_keep_trace = false;
        b_trace = false;
        hp = 5;
        state = 0;
        lastState = -1;
        change_way = 0;
        animator.SetTrigger("exist");
    }


    //public void Rcycle() {
    //    isShowUp = false;
    //}

    public virtual void ShowUp(float _time) {
        inState_time += Time.deltaTime;
        if (inState_time < _time) {
            if (state != 1) {
                state = 1;
                animator.Play("Walk");
            }
            //Debug.Log("ShowUp up run" + f_speed * Time.deltaTime * Vector3.down);
            transform.position += f_speed * Time.deltaTime * Vector3.down;
        } 
        else {
            //Debug.Log("end ShowUp" + this.transform.GetComponent<BoxCollider2D>());
            this.transform.GetComponent<BoxCollider2D>().enabled = true;
            isShowUp = true;
            if (test) SetState(0,false);
        } 
    }


    public virtual void SetAnimator() {
        if (go_way.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
        animator.SetInteger("state", state);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isForceState && collision.tag == "Player") {
        }
        if (IsOut && collision.tag == "Trap")
        {
            //SetState(4, true);
            //collision.GetComponent<CTrap>().Disapear();
            //Destroy(collision.gameObject);
        }
    }

}
