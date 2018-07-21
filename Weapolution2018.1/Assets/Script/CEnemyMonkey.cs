using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyMonkey : CEnemy {

    float bananaTime = 0.0f;
    GetHurtEffect getHurtEffect;
    Transform bananas;
    CChildProjectSystem childProjectSystem;
    
    // Use this for initialization
    public override void Awake () {
        base.Awake();
        if(IsOut)childProjectSystem = transform.parent.parent.Find("Bananas").GetComponent<CChildProjectSystem>();
        getHurtEffect = GetComponent<GetHurtEffect>();

    }
	
	// Update is called once per frame
	void Update () {
        if (StageManager.timeUp) return;
        UpdatePos();
        if (!isShowUp && !IsOut) {
            ShowUp(1.5f);
            return;
        }
        StateMachine();
        SetAnimator();
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    SetHurtValue(1, 0);
        //}
    }

    public override void StateMachine()
    {
        if (!IsOut) {
            base.StateMachine();
        }
        else
        {
            SetBehaviorOutside();
            switch (state)
            {
                case 0:
                    Idle();
                    break;
                case 1:
                    Ramble();
                    break;
                case 2:
                    ThrowPeel();
                    break;
                case 3:
                    GoCrazy();
                    break;
                case 4:
                    BeTrapped(2.5f);
                    break;
            }
        }
    }

    void SetBehaviorOutside() {
        if (isForceState) return; //若是強制執行狀態如攻擊，直接return
        
        //丟香蕉基本秒數為三秒丟一次
        if ( bananaTime < 3.0f)
        {
            bananaTime += Time.deltaTime;
            if (inState_time >= state_time && state_time>0.1f) { //每個狀態的時間到換新的，為了不蓋過
                SetState(-1,false);                                                                       //到新狀態，狀態時間加一個判斷大於0.1
            } 
        }
        else {
            if (inState_time >= state_time) {
                //可丟香蕉數大於0且七成機率丟出香蕉
                if (childProjectSystem.GetFreeNum() > 0 && Random.Range(0.0f, 1.0f) > 0.7f) SetState(2,true);
                bananaTime = 0.0f;
            }
        }
        //new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0); //更新新的位置座標
    }

    public override void UpdatePos() {
        new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0);
        self_pos = transform.position;
        playerPos = player.transform.position;
        if (!IsOut)
        {
            if (whichSide > 0.5f) attackPos = playerPos + new Vector3(f_attack_dis, 0.0f, 0.0f);
            else if (whichSide < -0.5f) attackPos = playerPos + new Vector3(-f_attack_dis, 0.0f, 0.0f);
            else attackPos = playerPos + new Vector3(Mathf.Sign(self_pos.x - playerPos.x) * f_attack_dis, 0.0f, 0.0f);
        }
    }

    void ThrowPeel() {
        if(state_time < 0.1f)state_time = 1.5f;
        
    }

    void GoCrazy()
    {

    }

    public void OnThrowingPeel()
    {
        childProjectSystem.AddUsed(self_pos + new Vector3(0.0f, 1.0f, 0.0f));
    }
    public void ThrowOver()
    {
        isForceState = false;
        inState_time = 2.0f; //故意設定時間，才可以丟完的時候換其他狀態
        enemySystem.PlaySound(1,0.1f);
    }

    public override void Trace()
    {
        traceTime += Time.deltaTime;
        if (Mathf.Abs(whichSide) < 0.3f) {
            if(state != 0)SetState(0, false);
            return;
        }
        //go_way = (attackPos - self_pos).V3NormalizedtoV2();
        Vector3 attackDis = attackPos + Mathf.Sign(playerPos.x - attackPos.x) * new Vector3(0.5f, 0, 0);
        RaycastHit2D detect;
        detect = Physics2D.Linecast(attackPos, playerPos ,1<<LayerMask.NameToLayer("Obstacle"));
        if (detect)
        {
            Debug.Log("out field" + attackPos +"  " + attackDis);
            if (!traceAgain) traceAgain = true;
            else {
                Vector3 temp = (playerPos + attackPos) * 0.5f;
                attackDis = temp + Mathf.Sign(playerPos.x - attackPos.x) * new Vector3(0.5f, 0, 0);
                detect = Physics2D.Linecast(temp, playerPos, 1 << LayerMask.NameToLayer("Obstacle"));
                Debug.Log("out field again" + temp + "  " + attackDis);
                if (detect)
                {
                    if(state != 0)SetState(0, false);
                    return;
                }
                
            }
        }
        else {
            traceAgain = false;
        }
        if (Mathf.Sign(playerPos.x - attackPos.x) * Mathf.Sign(playerPos.x - self_pos.x) < 0.0f)
        {
            Debug.Log("back side");
            Vector3 tempPos = new Vector3(attackPos.x, attackPos.y + 0.6f + Mathf.Sign(self_pos.y - playerPos.y) * 2.5f, 0.0f);
            go_way = (tempPos - transform.position).V3NormalizedtoV2();
        }
        else
        {
            if (traceTime > 0.7f)
            {  //延遲怪物判斷，比較不會追那麼緊
                
                if (traceAgain) attackPos = (attackPos + playerPos) * 0.5f;
                go_way = (attackPos - self_pos).V3NormalizedtoV2();
                traceTime = 0.0f;
                Debug.Log("attackpos" + attackPos + "     goway" + go_way);
            }
        }
        transform.position += f_speed * 1.3f * Time.deltaTime * go_way;
    }

    public override void Attack()
    {
        if (canDetectAtk) AttackDetect();
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) state_time = 0.69f;
    }


    public override void AttackDetect()
    {
        //Debug.Log(attackOnce + "  "+ CouculatePlayerDis(false, 0.08f) + "  "+ Mathf.Abs(transform.position.y - attackPos.y) + "  "+ (playerPos.x - transform.position.x) * transform.localScale.x );
        if (!attackOnce && CouculatePlayerDis(false, 0.1f)
               && Mathf.Abs(transform.position.y - attackPos.y) < 0.12f 
               && (playerPos.x - transform.position.x) * transform.localScale.x <0.0f)
        {
            canDetectAtk = false;
            attackOnce = true;
            player.GetComponent<Player>().GetHurt();
            Debug.Log("hit");
        }
    }

    public override void AttackOver()
    {
        traceTime = 2.0f;  //追擊重新計算延遲時間
        inState_time = 2.0f;
        attackOnce = false;
        isForceState = false;
        Debug.Log("hit over");
    }

    public override void SetHurtValue(int _value, int _HitDir)
    {
        //base.SetHurtValue(_value, _HitDir); 
        if (getHurtOnce) return;
        Vector3 effectPos = new Vector3(0, 0, 0);
        if (_HitDir == 0) effectPos = new Vector3(new_pos.x, new_pos.y + 1.7f, -200.0f);
        else if (_HitDir == 1) effectPos = new Vector3(new_pos.x, new_pos.y - 1.7f, -200.0f);
        else if (_HitDir == 2) effectPos = new Vector3(new_pos.x - 1.7f, new_pos.y, -200.0f);
        else if (_HitDir == 3) effectPos = new Vector3(new_pos.x + 1.7f, new_pos.y, -200.0f);
        if(!IsOut)getHurtEffect.SetEffect(effectPos, 0.4f);

        enemySystem.PlaySound(0, 1.0f);
        hurtValue = _value;
        SetState(4, true);
        hitDir = _HitDir;
    }

    public override void SetAnimator()
    {
        
        if (!isForceState) {
            if (IsOut)
            {
                transform.localScale = new Vector3(-Mathf.Sign(go_way.x)*scaleX, scaleY, 1);
            }
            else {
                if (playerPos.x > self_pos.x) transform.localScale = new Vector3(-scaleX, scaleY, 1);
                else transform.localScale = new Vector3(scaleX, scaleY, 1);
            }
        }
        if (state != lastState)
        {
            Debug.Log("setAnimator" + gameObject.name);
            animator.SetInteger("state", state);
            animator.SetTrigger("exist");
            lastState = state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isForceState && collision.tag == "Player")
        {
        }
        if (IsOut && collision.tag == "Trap")
        {
            SetState(4,true);
            collision.gameObject.SetActive(false); 
        }
    }

    //public override void DetectPlayer()
    //{
    //    if (b_attacking) return;
    //    Debug.Log(b_attacking + "detect player" + f_attack_dis);
    //    if (CouculatePlayerDis(false, f_attack_dis))
    //    {
    //        Debug.Log("attack");
    //        SetState(3);
    //    }
    //    else
    //    {
    //        Debug.Log("trace");
    //        SetState(2);
    //    }
    //}


}
