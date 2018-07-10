using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyBear : CEnemy {

    float dizzy_time = 0.0f, break_time = 0.0f, thunderOffsetX, 
        thunderOffsetY, totalTraceTime = 0.0f, traceFace = 0.0f;
    //BoxCollider2D box1, box2;
    bool b_attacking = false, isTracing = false;
    bool showOnce = false, traceChange = false; 
    int attack_number = 0, punch_number = 0, subAniState = 0, thunderNum = 0;
    Vector3 changeTraceLoc;
    CChildProjectSystem thunders;
    GetHurtEffect getHurtEffect;
    CPickItemSystem pickItemSystem;
    // Use this for initialization
    public override void Awake () {
        base.Awake();
        hp = 25;
        lastState = -1;
        state = 0;
        //box1 = transform.Find("PlayerDetect").GetChild(0).GetComponent<BoxCollider2D>();
       // box2 = transform.Find("PlayerDetect").GetChild(1).GetComponent<BoxCollider2D>();
        //behavior_chance = new float[6] {0.4f,0.8f,1.0f,0,0,0};
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        thunders = transform.parent.Find("Thunders").GetComponent<CChildProjectSystem>();
        rigBody = this.GetComponent<Rigidbody2D>();
        getHurtEffect = GetComponent<GetHurtEffect>();
        pickItemSystem = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (StageManager.timeUp) return;
        if (!isShowUp)
        {
            ShowUp(1.5f);
            return;
        }
        //if (Input.GetKeyDown(KeyCode.J)) {
        //    SetHurtValue(1,4);
        //    //GetHurt();
        //} 
        UpdatePos();
        StateMachine();
        SetAnimator();
    }

    public override void SetState(int num, bool _isForce) {
        //Debug.Log("setstate pre" + state);
        totalTraceTime = 0.0f;
        if (num >= 0) state = num;
        else Behavior();
        traceAgain = false;
        //Debug.Log("setstate after" + state);
    }

    public override void StateMachine()
    {
        if (totalTraceTime > 7.0f)  //若卡住超過7秒直接發飆
        {
            Debug.Log("tracing too long" + totalTraceTime);
            SetState(3, false);
            animator.SetTrigger("exist");
            return;
        }
        //Debug.Log("statemachine" + state);
        switch (state)
        {
            case 0:
                //box1.enabled = true;
                //box2.enabled = false;
                AttackPunch();
                break;
            case 1:
                //box1.enabled = false;
                //box2.enabled = true;
                //AttackWheel();
                break;
            case 2:
                AttackDash();
                break;
            case 3:
                ThunderAttack();
                break;
            case 4:
                Dizzy(1.7f);
                break;
            case 6:
                //TakeBreak(0.5f);
                Die();
                break;
        }
    }

    public override void UpdatePos()
    {
        new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0);
        self_pos = transform.position;
        playerPos = player.transform.position;
        attackPos = playerPos + new Vector3(Mathf.Sign(self_pos.x - playerPos.x) * f_attack_dis, 0.0f, 0.0f);
        if (traceAgain)
        {
            float tempY;
            if (!traceChange) {
                traceChange = true;
                traceFace = Mathf.Sign(playerPos.x - self_pos.x) * f_attack_dis;
            }
            if (playerPos.y > self_pos.y ) tempY = -1.2f;
            else tempY = 2.5f;
            changeTraceLoc = new Vector3(playerPos.x + traceFace, playerPos.y + tempY, 0.0f);
            if ((changeTraceLoc.x - self_pos.x) * (changeTraceLoc.x - self_pos.x) +
                (changeTraceLoc.y - self_pos.y) * (changeTraceLoc.y - self_pos.y) < 0.03f) {
                traceAgain = false;
                traceChange = false;
                Debug.Log("switch trace again to false");
            }
        }

}

    void Behavior() {  //攻擊行為，用亂數決定攻擊方式
        if (b_attacking) return;
        //Debug.Log("behavior pre" + state);
        //Debug.Log("punch num" + punch_number);
        //Debug.Log("attack num" + attack_number);
        if (attack_number > 6)
        {
            attack_number = 0;
            state = 3;
        }
        else {
            if (punch_number >= 3)
            {
                punch_number = 0;
                //if (!firstThunder) {
                //    firstThunder = true;
                //    state = 3;
                //}
                state = 2;
            }
            else state = 0;
            
        }
        //Debug.Log("behavior after" + state);

        //float temp_probability;
        //if (CouculatePlayerDis(true, 10.0f))
        //{
        //    temp_probability = Random.Range(0.0f, 1.0f);
        //    if (temp_probability <= 0.65f) SetState(2, false);
        //    else SetState(0, false);
        //}
        //else {
        //    Debug.Log("near atk");
        //    if (punch_number < 3) SetState(0, false);
        //    else {
        //        punch_number = 0;
        //        SetState(1, false);
        //    }
        //}
    }

    public override void Trace()
    {
        //Debug.Log("trace");
        totalTraceTime += Time.deltaTime;
        if (!isTracing) {
            animator.SetInteger("state", 0);
            isTracing = true;
        }
        RaycastHit2D hitWall;
        Vector3 start = new Vector3(self_pos.x + Mathf.Sign(self_pos.x - playerPos.x) * 1.2f,
                                                            self_pos.y, self_pos.z);
        go_way = (attackPos - self_pos).V3NormalizedtoV2();
        if (go_way.y > 0.0f)
            hitWall = Physics2D.Raycast(attackPos, -go_way, 1.0f, 1 << LayerMask.NameToLayer("Obstacle"));
        else
            hitWall = Physics2D.Raycast(attackPos, -go_way, 3.0f, 1 << LayerMask.NameToLayer("Obstacle"));
        if (!traceAgain)
        {
            if (hitWall) traceAgain = true;
            //Debug.Log("traceagain false" + attackPos);
        }
        else {
            //Debug.Log("traceagain true" + changeTraceLoc);
            go_way = (changeTraceLoc - self_pos).V3NormalizedtoV2();
            RaycastHit2D hitWallAgain = Physics2D.Linecast(start,self_pos, 1 << LayerMask.NameToLayer("Obstacle"));
            //RaycastHit2D hitWallAgain = Physics2D.Raycast(start, -go_way, 0.5f, 1 << LayerMask.NameToLayer("Obstacle"));
            if (hitWallAgain ) traceAgain = false;
        }
        //rigBody.velocity = new Vector2(f_speed*go_way.x, f_speed*go_way.y);
        transform.position += f_speed * Time.deltaTime * go_way;
        //Debug.Log("trace" + f_speed * Time.deltaTime * go_way);
    }

    void AttackPunch() {
        //Debug.Log("punch pre");
        if (b_attacking) return;
        //Debug.Log("punch after");
        if (CouculatePlayerDis(true, 0.1f)) Trace();
        else {
            if (!b_attacking) {
                Debug.Log("punch");
                //rigBody.velocity = Vector2.zero;
                enemySystem.PlaySound(3,1.0f);
                animator.SetInteger("state", -1);
                animator.Play("EnemyBearPunch");
                //animator.SetTrigger("exist");
                isTracing = false;
                b_attacking = true;
                punch_number++;
            } 
        }
    }

    //void AttackWheel() {
    //    if (b_attacking) return;
    //    if (!can_attack) Trace();
    //    else
    //    {
    //        if (!b_attacking)
    //        {
    //            animator.Play("EnemyBearWheel");
    //            b_attacking = true;
    //        }
    //    }
    //}

    void AttackDash() {
        if (subAniState == 0) {
            if (!b_attacking) {
                //rigBody.velocity = Vector2.zero;
                this.transform.GetComponent<BoxCollider2D>().enabled = false;
                state_time = 1.0f;
                inState_time = 0.0f;
                b_attacking = true;
            }
            inState_time += Time.deltaTime;
            if (inState_time >= state_time) Debug.Log("dash step0 end");
        }
        if (subAniState == 1) {
            if (state_time < 0.1f)
            {
                Debug.Log("dash step1 init");
                //float face = Mathf.Sign(playerPos.x - transform.position.x);
                //float attackDisX = face * (f_attack_dis + 0.7f);
                //attackPos = transform.position + new Vector3(attackDisX, 1.3f, 0.0f);
                go_way = (playerPos - self_pos).normalized;
                state_time = 2.0f;
                inState_time = 0.0f;
                enemySystem.PlaySound(4, 1.0f);
            }
            inState_time += Time.deltaTime;
            if (inState_time < state_time)
            {
                //Debug.Log("dash step1  ");
                if (Mathf.Abs(go_way.x) >= 0.75f)
                {
                    detect_ray = Physics2D.Raycast(new_pos, go_way, 4.0f, 1 << LayerMask.NameToLayer("Obstacle"));
                    //Debug.DrawRay(new_pos,4.0f * go_way, Color.blue);
                }
                else if (Mathf.Abs(go_way.x) >= 0.5f)
                {
                    detect_ray = Physics2D.Raycast(new_pos, go_way, 3.0f, 1 << LayerMask.NameToLayer("Obstacle"));
                    //Debug.DrawRay(new_pos, 2.0f * go_way, Color.blue);
                }
                else 
                {
                    detect_ray = Physics2D.Raycast(new_pos, go_way, 2.2f, 1 << LayerMask.NameToLayer("Obstacle"));
                    //Debug.DrawRay(new_pos, 2.0f * go_way, Color.blue);
                }
                if (detect_ray)
                    go_way = (playerPos - self_pos).normalized;
                //rigBody.velocity = new Vector2(f_speed * 1.6f*go_way.x, f_speed * 1.6f * go_way.y);
                transform.position += f_speed * 1.6f * Time.deltaTime * go_way;
                transform.localScale = new Vector3(-Mathf.Sign(go_way.x)*scaleX, scaleY,1);
            }
            else {
                //Debug.Log("dash step 1 end");D:\Weapolution\Assets\Script\CEnemy.cs
                state_time = 0.0f;
                inState_time = 0.0f;
                subAniState++;
                animator.SetBool("nextStep", true);
            }
        }
        else if (subAniState == 2) {
            if (state_time <= 0.1f) {
                //Debug.Log("dash step2 init");
                //rigBody.velocity = Vector2.zero;
                animator.SetBool("nextStep", false);
                state_time = 4.0f;
                inState_time = 0.0f;
                this.transform.GetComponent<BoxCollider2D>().enabled = true;
            }
            inState_time += Time.deltaTime;
            //Debug.Log("dash step 2  " + inState_time + " state time " + state_time);
            if (inState_time >= state_time) {
                //Debug.Log("dash step 2   end" + inState_time);
                //animator.SetBool("nextStep", true);
                animator.SetTrigger("dashExist");
                AttackOver();
                subAniState = 0;
            } 
        }
    }

    public void StartDashEnd()
    {
        //Debug.Log("startDashEnd");
    }

    public void StartDash() {
        subAniState++;
        inState_time = 0.0f;
        state_time = 0.0f;
        //Debug.Log("startDash");
    }

    public void StartThunder() {
        subAniState++;
    }

    public void EndThunder() {
        attack_number = 0;
        animator.SetBool("nextStep", false);
    }

    void ThunderAttack() {
        if (!b_attacking) {
            //rigBody.velocity = Vector2.zero;
            b_attacking = true;
            enemySystem.PlaySound(5, 1.0f);
        } 
        inState_time += Time.deltaTime;
        if (subAniState == 1)
        {
            if (state_time < 0.1f)
            {
                state_time = 1.0f;
                
                //for (int i = 0; i < 3; i++) {
                //    float degree = Random.Range(0.0f, 350.0f);
                //    float dis = Random.Range(0.5f,3.0f);
                //    Vector3 range = dis * ( Quaternion.AngleAxis(degree, new Vector3(0,0,1)) * Vector3.up);
                //    thunderOffsetX = Mathf.Clamp(playerPos.x + range.x, -6.8f, 7.0f);
                //    thunderOffsetY = Mathf.Clamp(playerPos.y + range.y, -1.8f, 8.2f);
                //    //if (thunderOffsetX < -6.8f) thunderOffsetX = -6.8f;
                //    //else if(thunderOffsetX > 7.0f) thunderOffsetX = 7.0f;
                //    //if (thunderOffsetY < -1.8f) thunderOffsetY = -1.8f;
                //    //else if (thunderOffsetY > 8.2f) thunderOffsetY = 8.2f;
                //    Vector3 thunderPos = new Vector3(thunderOffsetX, thunderOffsetY, 0);
                //    thunders.AddUsed(thunderPos);
                //}
                //thunders.AddUsed(playerPos + new Vector3(0,-0.3f,0.0f));
                //打樹
                Vector3 firtreePos = pickItemSystem.SetThunderLoc();
                if(firtreePos.x > -50.0f) thunders.AddUsed(firtreePos); //故意設置超過-50即不著火
                //thunderNum++;
            }
            
            if (thunderNum < 4) {
                if (inState_time > 1.2f)
                {
                    inState_time = 0.0f;
                    float degree = Random.Range(0.0f, 350.0f);
                    int thunderLimitNum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        //float disX = Mathf.Clamp(playerPos.x + Random.Range(3.5f, 5.5f), -13.4f, 0.32f);
                        //float disY = Mathf.Clamp(playerPos.x + Random.Range(2.0f, 4.0f), -3.0f, 6.8f);
                        //Vector3 thunderPos = new Vector3(disX, disY, 0);
                        //float dis = 3.0f; //Random.Range(0.5f, 3.0f);
                        Vector3 range = (Quaternion.AngleAxis(degree, new Vector3(0, 0, 1.0f)) * (2.5f * Vector3.up));

                        thunderOffsetX = playerPos.x + range.x;
                        thunderOffsetY = playerPos.y + range.y;
                        if (thunderOffsetX < -7.5f || thunderOffsetX > 10.3f ||
                            thunderOffsetY < -3.3f || thunderOffsetY > 9.2f) {
                            i--;
                            degree += 20.0f;
                            thunderLimitNum++;
                            if (thunderLimitNum > 13) break;
                            continue;
                        }
                        Vector3 thunderPos = new Vector3(thunderOffsetX, thunderOffsetY, 0);
                        thunders.AddUsed(thunderPos);
                        //enemySystem.PlaySound(6, 1.0f);
                        degree += 40.0f;
                    }
                    thunders.AddUsed(playerPos + new Vector3(0, -0.3f, 0.0f));
                    enemySystem.PlaySound(6, 1.0f);
                    thunderNum++;
                }
            }
            else 
                {
                subAniState++;
                animator.SetBool("nextStep", true);
                state_time = 0.0f;
                inState_time = 0.0f;
                //Debug.Log(thunderNum);
            }
        }
        else if (subAniState == 2)
        {
            if (state_time <= 0.1f)
            {
                inState_time = 0.0f;
                state_time = 4.0f;
            }
            if (inState_time >= state_time)
            {
                //Debug.Log("thunder switch2");
                animator.SetBool("nextStep", true);
                subAniState++;
                inState_time = 0.0f;
                state_time = 0.0f;
            }
        }
        else if (subAniState == 3)
        { 
        }
    }

    public override void ShowUp(float _time)
    {
        inState_time += Time.deltaTime;
        if (inState_time < _time)
        {
            if (!showOnce) { animator.Play("EnemyBearWalk");
                showOnce = true;
            }
            //Debug.Log("ShowUp up run" + f_speed * Time.deltaTime * Vector3.down);
            transform.position += f_speed * 0.5f*Time.deltaTime * Vector3.down;
        }
        else
        {
            //Debug.Log("end ShowUp" + this.transform.GetComponent<BoxCollider2D>());
            this.transform.GetComponent<BoxCollider2D>().enabled = true;
            isShowUp = true;
        }
    }

    void Dizzy(float time) {
        if (dizzy_time < time)
        {
            dizzy_time += Time.deltaTime;
        }
        else {
            dizzy_time = 0.0f;
            SetState(-1, false);
        }
    }

    public override void AttackOver()
    {
        //Debug.Log("attackOver");
        animator.SetBool("nextStep", false);
        b_attacking = false;
        attack_number++;
        SetState(-1, false);
        state_time = 0.0f;
        inState_time = 0.0f;
    }

    public void ThunderToOver() {
        subAniState = 0;
        thunderNum = 0;
        AttackOver();
    }

    public override void SetHurtValue(int _value, int _HitDir)
    {
        Vector3 effectPos = new Vector3(0,0,0) ;
        if (_HitDir == 0) effectPos = new Vector3(new_pos.x, new_pos.y - 1.5f, -200.0f);
        else if (_HitDir == 1) effectPos = new Vector3(new_pos.x, new_pos.y + 1.5f, -200.0f);
        else if(_HitDir == 2) effectPos = new Vector3(new_pos.x + 1.5f, new_pos.y , -200.0f);
        else if (_HitDir == 3) effectPos = new Vector3(new_pos.x - 1.5f, new_pos.y, -200.0f);
        getHurtEffect.SetEffect(effectPos,0.8f);
        enemySystem.PlaySound(7, 1.0f);
        hurtValue = _value;
        hp -= hurtValue;
        if (hp <= 0) {
            animator.Play("EnemyBearDie");
            enemySystem.NextStage();
            SetState(5, true);
            
        }
    }

    public override void Die()
    {
        
    }

    public void DieOver() {
        this.gameObject.SetActive(false);
        
    }

    public void TakeBreak(float total_time) {
        if (break_time < total_time) break_time += Time.deltaTime;
        else {
            break_time = 0.0f;
            SetState(-1, false);
        }
    }

    public override void SetAnimator()
    {
        //Debug.Log("SetAnimator" + state + "   " + lastState);
        if (!b_attacking) {
            if (playerPos.x > transform.position.x)
                transform.localScale = new Vector3(-scaleX, 1, 1);
            else transform.localScale = new Vector3(scaleX, 1, 1);
        }
        if (state != lastState) animator.SetInteger("state", state);
        lastState = state;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (b_attacking)
        {
            if (!collision.isTrigger && collision.tag == "Player") {
                player.GetComponent<Player>().GetHurt();
                Debug.Log("boss hit");
            }
        }
        else {
            //StageManager ii;
        }
     
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (!b_attacking && !can_attack) {
        //    if (!collision.isTrigger && collision.tag == "Player" && !can_attack)
        //    {
        //        can_attack = true;
        //    }
        //}
    }

}
