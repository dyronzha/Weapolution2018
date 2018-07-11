using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouse : CEnemy {
    bool pathFind, forceIdle;
    int rangeAttackNum, pathIndex;
    float rangeDetectTime, lastAwayDst, attackTime, traceTotalTime;
    Vector3 keepAway, shootPos, shootWay;
    Path path;
    EnemyBulletSystem bullets;
    GetHurtEffect getHurtEffect;

    public float stoppingDis, turnDis, turnSpeed;
    public LayerMask obstacleMask;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        rambleLayer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Enemy") |
                        1 << LayerMask.NameToLayer("ObstacleForIn");
        state = 0;
        bullets = GameObject.Find("EnemyMouseBullets").GetComponent<EnemyBulletSystem>();
        hp = 4;
        getHurtEffect = GetComponent<GetHurtEffect>();
    }
    
    // Update is called once per frame
    void Update () {
        if (StageManager.timeUp) return;
        //if (Input.GetKeyDown(KeyCode.J)) SetHurtValue(1,0);
        UpdatePos();
        if (!isShowUp)
        {
            state = -1;
            return;
        }
        StateMachine();
        //Trace();
        SetAnimator();
	}

    public override void StateMachine()
    {
        DetectPlayer();
        switch(state){
            case -1:
                break;
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
                RangeAttack();
                break;
            case 4:
                KeepRangeDst();
                break;
            case 5:
                NormalAttack();
                break;
            case 6:
                GetHurt();
                break;
            case 7:
                BeTrapped();
                break;
            case 8:
                Die();
                break;
        }
    }
    public override void DetectPlayer()
    {
        if (isForceState || forceIdle) return;
        //float offset = 0.0f;
        //float rangeAtkOffset = 0.0f;
        //if (state == 2 || state == 3) offset = 1.5f * f_sight_dis;
        //if (state == 3) rangeAtkOffset = 3.5f;
        if (CouculatePlayerDis(false, 2.0f)) {
            if (state == 5) animator.SetTrigger("exist");
            SetState(5,true);
        }
        //遠距離攻擊移到trace裡面，夠近就攻擊
        //else if (CouculatePlayerDis(false, 5.0f))
        //{
        //    if (state != 4 && pathIndex >= path.slowDownIndex) { //在不是閃躲狀況下，才進入遠攻狀態
        //        Vector3 playerNewPos = playerPos += new Vector3(0.0f, 0.4f, 0.0f);
        //        detect_ray = Physics2D.Linecast(new_pos, playerPos, obstacleMask);
        //        if (!detect_ray)
        //        {
        //            SetState(3, true); //range attack
        //            if (state == 3) animator.SetTrigger("exist");
        //        }
        //    }
        //}
        else if (CouculatePlayerDis(false, f_sight_dis))
        {
            if (state < 2) { //在idle和閒晃才會由這進trace狀態
                SetState(2, false);
            }
            
        }
        else
        {
            if (inState_time >= state_time && state_time > 0.1f) SetState(-1, false);
        }
    }

    public override bool CouculatePlayerDis(bool more, float compare)
    {
        Vector2 _self_pos = new Vector2(self_pos.x, self_pos.y);
        //Vector2 attack_pos = new Vector2(attackPos.x, attackPos.y);
        Vector2 player_pos = new Vector2(playerPos.x, playerPos.y);
        if (more)
        {
            Vector2 dst = _self_pos - player_pos;
            if (Vector2.SqrMagnitude(dst) > compare*compare) return true;
            else return false;
        }
        else
        {
            Vector2 dst = _self_pos - player_pos;
            if (Vector2.SqrMagnitude(dst) < compare * compare) return true;
            else return false;
        }
    }

    public override void Idle()
    {
        //Debug.Log("idle" + state_time);
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) state_time = Random.Range(1.5f, 2.0f);

        if (inState_time > state_time) {
            SetState(-1, false);
            if (forceIdle)
            {
                forceIdle = false;
            }
        }

    }

    public override void Trace()
    {
        if (state_time < 0.1f) {
            go_way = Vector3.zero;
            state_time = 5.0f;
            //pathIndex = 0;
            pathFind = false;
            traceTime = 0.0f;
            PathRequestManager.RequestPath(self_pos, playerPos, OnPathFound);
        }
        if (inState_time - traceTime > 0.5f) {
            traceTime = inState_time;
            //pathIndex = 0;
            pathFind = false;
            PathRequestManager.RequestPath(self_pos, playerPos, OnPathFound);
        }

        if (pathFind)
        {
            //Debug.Log("trace path find");
            Vector2 Pos2d = new Vector2(self_pos.x, self_pos.y);
            if (path.turnBoundaries[pathIndex].HasCrossedLine(Pos2d))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    //Debug.Log("catch player  " + path.finishLineIndex);
                    pathFind = false;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (pathIndex >= path.slowDownIndex && stoppingDis > 0)
            {
                if (CouculatePlayerDis(false, 5.0f))
                {
                    Vector3 playerNewPos = playerPos += new Vector3(0.0f, 0.4f, 0.0f);
                    //shootPos = self_pos + new Vector3();
                    detect_ray = Physics2D.Linecast(new_pos, playerPos, obstacleMask);
                    if (!detect_ray)
                    {
                        if (bullets.GetFreeNum() > 0)
                        {
                            SetState(3, true); //range attack
                            
                            traceTotalTime = 0.0f;
                            //Debug.Log("start shoot");
                            pathFind = false;
                            //if (state == 3) animator.SetTrigger("exist");
                        }

                    }
                }
            }
            if (pathIndex == 0) go_way = (path.lookPoints[0] - self_pos).V3NormalizedtoV2();
            else
            {
                Vector3 nextWay = (path.lookPoints[pathIndex] - self_pos).V3NormalizedtoV2();
                go_way = Vector3.Lerp(go_way, nextWay, Time.deltaTime * turnSpeed);
            }
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            transform.position += go_way * f_speed * 1.7f * Time.deltaTime;
        }
        else {
            traceTotalTime += Time.deltaTime;
            if (traceTotalTime > 1.5f && bullets.GetFreeNum() > 0) {
                SetState(3,true);
                traceTotalTime = 0.0f;
            }
        }


        inState_time += Time.deltaTime;
    }

    public void OnPathFound(Vector3[] wayPoints, bool pathSuccessful)
    {
        //transform.Translate(Vector3.forward,Space.Self);
        if (pathSuccessful)
        {
            pathIndex = 0;
            //Debug.Log(gameObject.name + "find path");
            pathFind = pathSuccessful;
            path = new Path(wayPoints, self_pos, turnDis, stoppingDis);
        }
    }


    public override void ShowUp(float _time)
    {
        if (state_time < 0.0f) {
            state_time = 1.0f;
        }
    }

    public void ShowUpOver() {
        state = 0;
        isShowUp = true;
    }

    void RangeAttack()
    {
        if (state_time < 0.1f)
        {
            rangeAttackNum++;
            state_time = 1.0f;
            shootWay = new Vector3(playerPos.x - self_pos.x, playerPos.y - self_pos.y, 0.0f);
        }
        inState_time += Time.deltaTime;
    }

    public void OnRangeAttacking() {
        
        Vector3 shootOutPos = new Vector3(self_pos.x - Mathf.Sign(transform.localScale.x) * 0.5f , self_pos.y + 0.65f, self_pos.z);
        //float offset = Random.Range(-10.0f, 10.0f);
        //shootWay = Quaternion.AngleAxis(offset, Vector3.forward) * shootWay;
        bullets.AddUsed(shootOutPos, shootWay);
    }

    public void RangeAttackOver()
    {
        SetState(4, false);
        //Debug.Log("RangeAttackOver");
        //keepAway = new Vector3(playerPos.x - keepAway.x, playerPos.y - keepAway.y, 0.0f).V3NormalizedtoV2();
    }

    void KeepRangeDst() {
        if (rangeDetectTime > 1.0f) {
            if (CouculatePlayerDis(true, 8.0f))  //閃避過程距離拉太大，重新追逐
            {
                //Debug.Log("dodge but too far");
                SetState(2, false);
                rangeDetectTime = 0.0f;
                return;
            }
            ////閃避一段時間，遠距離攻擊
            //Vector3 playerNewPos = playerPos += new Vector3(0.0f, 0.4f, 0.0f);
            ////shootPos = self_pos + new Vector3();
            //detect_ray = Physics2D.Linecast(new_pos, playerPos, obstacleMask);
            //if (!detect_ray && bullets.GetFreeNum() > 0) SetState(3, true);
            //else {
            //    Debug.Log("shoot but obstacle");
            //    SetState(2, false);
            //} 
            //rangeDetectTime = 0.0f;
            //return;
        }
        if (state_time < 0.1f)
        {
            if (Random.Range(0.0f, 1.0f) > 0.5f)
            {
                state_time = 2.5f;
            }
            else {
                forceIdle = true;
                SetState(0, false);
                return;
            }
            rangeDetectTime = 0.0f;
            attackTime = 0.0f;
            keepAway = new Vector3(self_pos.x - playerPos.x, self_pos.y - playerPos.y, 0.0f).V3NormalizedtoV2();
            //Vector2 lastPlayer = new Vector2(self_pos.x - keepAway.x, self_pos.y - keepAway.y);
            //Vector2 currentPlayer = new Vector2(self_pos.x - playerPos.x, self_pos.y - playerPos.y);
            //if (Vector2.SqrMagnitude(currentPlayer) <= Vector2.SqrMagnitude(lastPlayer))
            //    goKeepAway = true;
        }
        if (inState_time - attackTime > 0.5f) {  //每0.5秒更新玩家位置
            attackTime = inState_time;
            keepAway= new Vector3(self_pos.x - playerPos.x, self_pos.y - playerPos.y,0.0f).V3NormalizedtoV2();
        }

        RaycastHit2D detect = Physics2D.Raycast(new_pos, keepAway, f_turn_dis, obstacleMask);
        Debug.DrawRay(new_pos, (f_turn_dis) * keepAway, Color.red);
        if (detect)
        {
            RaycastHit2D detect1, detect2;
            Vector3 trace1, trace2;
            for (int i = 20; i < 360; i += 20)
            {
                trace1 = Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * keepAway;
                trace2 = Quaternion.AngleAxis((360 - i), new Vector3(0, 0, 1)) * keepAway;
                detect1 = Physics2D.Raycast(new_pos, trace1, f_turn_dis, obstacleMask);
                detect2 = Physics2D.Raycast(new_pos, trace2, f_turn_dis, obstacleMask);
                Debug.DrawRay(new_pos, (f_turn_dis) * trace1, Color.blue);
                Debug.DrawRay(new_pos, (f_turn_dis) * trace2, Color.white);
                if (!detect1) { keepAway = Quaternion.AngleAxis(i + 10.0f, new Vector3(0, 0, 1)) * keepAway;
                    Debug.Log(i + " keep away change" + keepAway); break; }
                if (!detect2) { keepAway = Quaternion.AngleAxis((360 - (i + 10.0f)), new Vector3(0, 0, 1)) * keepAway;
                    Debug.Log(i + " keep away change" + keepAway); break; }
                if(i >= 340)keepAway = new Vector3(0, 0, 0);
            }
        }
        
        transform.position += keepAway * f_speed * 1.7f * Time.deltaTime;
        inState_time += Time.deltaTime;
        rangeDetectTime += Time.deltaTime;
        if (inState_time >= state_time) SetState(-1, false);
    }

    void NormalAttack() {
        if (state_time < 0.1f) {
            state_time = 2.0f;
        }
        if (attackOnce)
        {
            RaycastHit2D detect = Physics2D.Raycast(new_pos, go_way, 0.5f, obstacleMask);
            if (!detect)
            {
                transform.position += Time.deltaTime * Vector3.Lerp(12.0f * go_way, Vector3.zero, (inState_time - 0.4f));
            }
        }
        
        inState_time += Time.deltaTime;
    }

    public void NormalAttackOver() {
        inState_time = 2.0f;
        attackOnce = false;
        isForceState = false;
        SetState(2, false);
        Debug.Log("hit over");
    }

    public override void SetOnAttackDetect()
    {
        base.SetOnAttackDetect();
        go_way = new Vector3(playerPos.x - self_pos.x, playerPos.y - self_pos.y, 0).V3NormalizedtoV2();
        attackOnce = true;
    }

    public override void SetOffAttackDetect()
    {
        base.SetOffAttackDetect();
        //attackOnce = false;
    }

    void BeTrapped() {
        if (state_time < 0.1f) {
            state_time = 1.0f;
            //isForceState = true;
            hp -= 2;
        }
    }

    public void TrappedOver() {
        isForceState = false;
        SetState(-1, false);
    }

    public override void SetHurtValue(int _value, int _HitDir)
    {
        Vector3 effectPos = new Vector3(0, 0, 0);
        if (_HitDir == 0) effectPos = new Vector3(new_pos.x, new_pos.y + 1.7f, -200.0f);
        else if (_HitDir == 1) effectPos = new Vector3(new_pos.x, new_pos.y - 1.7f, -200.0f);
        else if (_HitDir == 2) effectPos = new Vector3(new_pos.x - 1.7f, new_pos.y, -200.0f);
        else if (_HitDir == 3) effectPos = new Vector3(new_pos.x + 1.7f, new_pos.y, -200.0f);
        Debug.Log("get hurt efffffffect" + gameObject.name);
        getHurtEffect.SetEffect(effectPos, 0.5f);
        hurtValue = _value;
        hitDir = _HitDir;
        SetState(6, true);
        enemySystem.PlaySound(1, 1.0f);
    }

    public override void GetHurtOver()
    {
        inState_time = 2.0f;
        getHurtOnce = false;
        isForceState = false;
        if (hp <= 0) SetState(8, true);
        else SetState(2, false);
        Debug.Log("Hurt Over");
    }

    public override void Die()
    {
        if (state_time < 0.1f)
        {
            state_time = 0.9f;
            inState_time = 0.0f;
            isDead = true;
        }
        inState_time += Time.deltaTime;
        if (inState_time >= state_time)
        {
            ResetEnemy();
            enemySystem.AddFreeList(this.transform);
        }
    }

    public override void SetAnimator()
    {

        if (!isForceState)
        {
            if (IsOut)
            {
                transform.localScale = new Vector3(-Mathf.Sign(go_way.x) * scaleX, scaleY, 1);
            }
            else
            {
                if (state > 1)
                {
                    if (playerPos.x > self_pos.x) transform.localScale = new Vector3(-scaleX, scaleY, 1);
                    else transform.localScale = new Vector3(scaleX, scaleY, 1);
                }
                else {
                    if(go_way.x >= 0) transform.localScale = new Vector3(-scaleX, scaleY, 1);
                    else transform.localScale = new Vector3(scaleX, scaleY, 1);
                }
            }
        }
        if (state != lastState)
        {
            //Debug.Log("setAnimator" + gameObject.name);
            animator.SetInteger("state", state);
            animator.SetTrigger("exist");
            lastState = state;
        }
        
    }


    public override void OnTriggerTrap() {
        SetState(7, true);
        enemySystem.PlaySound(0, 1.0f);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canDetectAtk)
            {
                canDetectAtk = false;
                player.GetComponent<Player>().GetHurt();
                Debug.Log("hitPlayer");
            }
        }
        //else if (collision.tag == "Trap") {
        //    collision.GetComponent<Trape>().ResetChild();
        //}
    }

}
