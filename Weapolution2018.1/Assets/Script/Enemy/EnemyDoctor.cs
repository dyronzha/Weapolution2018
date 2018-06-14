using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoctor : CEnemy
{
    bool pathFind, onAttacking;
    int pathIndex, normalAtkNum, totalAtkNum, poisonNum;
    float totalTraceTime, breakTime, poisonAtkNum;
    PoisonManager poisonManager;
    Vector3 poisonOffset;
    Vector3[] poisonAtksPos = new Vector3[4];
    SpriteRenderer render;
    SprintAttacks sprintAtks;
    Path path;
    CChildProjectSystem poisonAlerts;
    LevelHeight levelHieght;
    GetHurtEffect getHurtEffect;

    public float stoppingDis, turnDis, turnSpeed;
    public LayerMask obstacleMask;

    public override void Awake()
    {
        attackOnce = false;
        base.Awake();
        SetState(0,false);
        hp = 20;
        sprintAtks = GetComponent<SprintAttacks>();
        render = transform.Find("img").GetComponent<SpriteRenderer>();
        poisonAlerts = GameObject.Find("PoisonHints").GetComponent<CChildProjectSystem>();
        poisonManager = GameObject.Find("Poisons").GetComponent<PoisonManager>();
        rambleLayer = 1 << LayerMask.NameToLayer("Obstacle")
                        | 1 << LayerMask.NameToLayer("ObstacleForIn");
        levelHieght = GetComponent<LevelHeight>();
        getHurtEffect = GetComponent<GetHurtEffect>();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.timeUp) return;
        UpdatePos();
        if (Input.GetKeyDown(KeyCode.J)) SetHurtValue(1,0);
        if (!isShowUp)
        {
            state = -1;
            return;
        }
        StateMachine();
        //Ramble();
        //transform.localScale = new Vector3(-Mathf.Sign(go_way.x) * scaleX, scaleY, 1);
        SetAnimator();
    }

    public override void SetState(int num, bool _isForce)
    {
        //Debug.Log("setstate pre" + state);
        //totalTraceTime = 0.0f;
        inState_time = 0.0f;
        state_time = 0.0f;
        attackOnce = false;
        if (num >= 0) state = num;
        else Behavior();
        //traceAgain = false;
        //Debug.Log("setstate after" + state);
    }

    void Behavior() //攻擊行為，用亂數決定攻擊方式
    { 
        if (totalAtkNum > 6)
        {
            totalAtkNum = 0;
            state = 4;
            //if (CouculatePlayerDis(false, 2.0f)) state = 2;
            //else state = 1;
        }
        else
        {
            if (normalAtkNum >= 3)
            {
                normalAtkNum = 0;
                state = 3;
            }
            else {
                if (CouculatePlayerDis(false, 2.0f)) state = 2;
                else state = 1;
            } 
        }
    }

    public override void StateMachine()
    {
        
        switch (state) {
            case -1:
                break;
            case 0:
                Idle(breakTime);
                break;
            case 1:
                Trace();
                break;
            case 2:
                NormalAtk();
                break;
            case 3:
                SprintAtk();
                break;
            case 4:
                PoisonAttack();
                break;
            case 5:
                Die();
                break;
            
        }
    }

    public override void ShowUp(float _time)
    {
        if (state_time < 0.0f)
        {
            state_time = 1.0f;
        }
    }

    public void ShowUpOver()
    {
        state = 1;
        isShowUp = true;
    }

    public  void Idle(float _idleTime)
    {
        //Debug.Log("idle" + state_time);
        inState_time += Time.deltaTime;
        if (state_time < 0.1f) {
            levelHieght.SetHeight();
            state_time = _idleTime;
        } 
        if (inState_time > state_time) {
            animator.SetTrigger("nextState");
            SetState(-1, false);
        }
        //if (inState_time > state_time) SetState(-1);
    }

    public override void Trace()
    {
        if (totalTraceTime > 5.0f) {
            animator.SetTrigger("nextState");
            SetState(3,true);
            totalTraceTime = 0.0f;
            return;
        }
        if (state_time < 0.1f)
        {
            go_way = Vector3.zero;
            state_time = 5.0f;
            //pathIndex = 0;
            pathFind = false;
            traceTime = 0.0f;
            PathRequestManager.RequestPath(self_pos, playerPos, OnPathFound);
        }
        if (inState_time - traceTime > 0.5f)
        {
            traceTime = inState_time;
            //pathIndex = 0;
            pathFind = false;
            PathRequestManager.RequestPath(self_pos, playerPos, OnPathFound);
        }
        if (pathFind)
        {
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
                if (CouculatePlayerDis(false, stoppingDis))
                {
                    //Debug.Log("atkatkatkatkatktaktaktaktktaktakatkatkatkataktak ");
                    animator.SetTrigger("nextState");
                    //onAttacking = true;
                    SetState(2, false);
                    totalTraceTime = 0.0f;
                    //normalAtkNum++;
                    //totalAtkNum++;
                }
            }
            if (pathIndex == 0) go_way = (path.lookPoints[0] - self_pos).V3NormalizedtoV2();
            else
            {
                Vector3 nextWay = (path.lookPoints[pathIndex] - self_pos).V3NormalizedtoV2();
                go_way = Vector3.Lerp(go_way, nextWay, Time.deltaTime * turnSpeed);
            }
            transform.position += go_way * f_speed * 1.7f * Time.deltaTime;
        }
        else {
            if (CouculatePlayerDis(false, stoppingDis))
            {
                animator.SetTrigger("nextState");
                SetState(2, false);
                totalTraceTime = 0.0f;
            }
            go_way = (playerPos - self_pos).V3NormalizedtoV2();
            transform.position += go_way * f_speed * 1.7f * Time.deltaTime;
        }
        inState_time += Time.deltaTime;
        totalTraceTime += Time.deltaTime;
    }

    void NormalAtk() {
//Debug.Log("inATTack");
        if (state_time < 0.1f) {
            state_time = 1.0f;
            onAttacking = true;
            normalAtkNum++;
            totalAtkNum++;
            enemySystem.PlaySound(2, 1.0f);
        }
    }

    public override void AttackOver() {
        animator.SetTrigger("nextState");
        onAttacking = false;
        SetState(-1, true);
    }

    public void OnPathFound(Vector3[] wayPoints, bool pathSuccessful)
    {
        //transform.Translate(Vector3.forward,Space.Self);
        if (pathSuccessful)
        {
            pathIndex = 0;
            Debug.Log(gameObject.name + "find path");
            pathFind = pathSuccessful;
            path = new Path(wayPoints, self_pos, turnDis, stoppingDis);
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
            if (Vector2.SqrMagnitude(dst) > compare * compare) return true;
            else return false;
        }
        else
        {
            Vector2 dst = _self_pos - player_pos;
            if (Vector2.SqrMagnitude(dst) < compare * compare) return true;
            else return false;
        }
    }

    public void StartSprint() {
        render.enabled = false;
        sprintAtks.SetStartSprint();
        enemySystem.PlaySound(4, 1.0f);
    } 

    void SprintAtk() {
        //Debug.Log("sprintattack");
        if (state_time < 0.1f) {
            onAttacking = true;
            state_time = 2.0f;
            totalAtkNum++;
            sprintAtks.SetSprintWay(playerPos, SprintAtkOver);
            enemySystem.PlaySound(3,1.0f);
            transform.localScale = new Vector3(-Mathf.Sign(playerPos.x - self_pos.x),1,1);
            //Vector3 sprintWay = (playerPos - self_pos).V3NormalizedtoV2();

        }
    }

     void SprintAtkOver(Vector2 pos) {
        animator.SetTrigger("nextState");
        //breakTime = 1.5f;
        //onAttacking = false;
        //SetState(0, false);
        transform.position = new Vector3(pos.x, pos.y, self_pos.z);
        render.enabled = true;
    }

    public void SprintOver() {
        Debug.Log("sprint over");
        breakTime = 2.5f;
        onAttacking = false;
        SetState(0, false);
    }

    void PoisonAttack() {
        if (state_time < 0.1f) {
            onAttacking = true;
            state_time = 10.0f;
            enemySystem.PlaySound(5, 1.0f);
        }
    }

    public void MakePoisonOver() {
        float angle = -20.0f;
        float dst = 2.0f;
        //RaycastHit2D detect = Physics2D.Raycast(new Vector2(playerPos.x , playerPos.y), new Vector2(-1.0f,0.0f),4.0f);
        //RaycastHit2D detect2 = Physics2D.Raycast(new Vector2(playerPos.x, playerPos.y), new Vector2(1.0f, 0.0f), 4.0f);
        bool detectWall = (playerPos.y > -4.3f && playerPos.y < 2.6f && (playerPos.x < -11.3f || playerPos.x > 11.3f)) ? true : false;
        if (detectWall)
        {
            
            //float nearRight = ((playerPos.x - detect1.transform.position.x) > (detect2.transform.position.x - playerPos.x))
            //    ? -1.0f : 1.0f;
            if (Random.Range(0.0f, 1.0f) > 0.5f) poisonOffset = new Vector3(0.0f, 1.0f, 0.0f);
            else poisonOffset = new Vector3(0.0f, -1.0f, 0.0f);
        }
        else {
            if (Random.Range(0.0f, 1.0f) > 0.5f) poisonOffset = new Vector3(1.0f, 0.0f, 0.0f);
            else poisonOffset = new Vector3(-1.0f, 0.0f, 0.0f);
        } 

        poisonAtksPos[0] = playerPos + new Vector3(0.0f, 0.5f,0.0f) ;
        poisonAlerts.AddUsed(poisonAtksPos[0]);
        for (int i = 1; i < 4; i++) {
            poisonAtksPos[i] = poisonAtksPos[0] + (Quaternion.AngleAxis(angle, Vector3.forward) * poisonOffset * dst);
            dst += 1.0f;
            poisonAlerts.AddUsed(poisonAtksPos[i]);
            angle *= -1;
        }

        Vector3 tempSelfPos = Vector3.zero;

        if (Mathf.Abs(poisonOffset.y) > 0.5f)
        {
            if (poisonOffset.y > 0.0f) tempSelfPos = new Vector3(poisonAtksPos[0].x - 1.0f, poisonAtksPos[0].y - 1.0f, transform.position.z);
            else tempSelfPos = new Vector3(poisonAtksPos[0].x + 1.0f, poisonAtksPos[0].y + 1.0f, transform.position.z);
        }
        else
        {
            if (poisonOffset.x > 0.0f) tempSelfPos = new Vector3(poisonAtksPos[0].x - 1.0f, poisonAtksPos[0].y + 1.0f, transform.position.z);
            else tempSelfPos = new Vector3(poisonAtksPos[0].x - Mathf.Sign(poisonOffset.x), poisonAtksPos[0].y + 1.0f, transform.position.z);
        }
        transform.position = tempSelfPos;
        transform.localScale = new Vector3(-Mathf.Sign(poisonOffset.x),1.0f,1.0f);
        levelHieght.SetSpecificHeight(-110.0f);
    }

    public void StartPoisonBlast() {
        //Vector2 locOffset = new Vector2(1.0f, 1.0f);
        //RaycastHit2D detect = Physics2D.Raycast(poisonAtksPos[3], locOffset);
        enemySystem.PlaySound(6, 1.0f);
        for (int i = 0; i < 4; i++) {
            Vector3 blastLoc = new Vector3(self_pos.x + Mathf.Sign(poisonOffset.x)*2.0f, self_pos.y + 2.0f, poisonAlerts.GetUsedChildTransform(poisonNum).position.z);
            if (poisonManager.freeNum > 0) poisonManager.AddUsedPosition(blastLoc, poisonAtksPos[i]);
            else break;
            poisonNum++;
        }
        if (poisonNum >= 4)
        {
            poisonAlerts.RecycleAllChild();
            poisonNum = 0;
            poisonAtkNum++;
            if (poisonAtkNum >= 3)
            {
                poisonAtkNum = 0;
                animator.SetBool("poisonOver", true);
            }
        }
       

    }

    public void PoisonOver() {
        inState_time = 15.0f;
        breakTime = 4.0f;
        onAttacking = false;
        SetState(0, false);
        
    }

    public override void SetHurtValue(int _value, int _HitDir)
    {
        //getHurtEffect.SetEffectOn();
        hurtValue = _value;
        hp -= hurtValue;

        Vector3 effectPos = new Vector3(0, 0, 0);
        if (_HitDir == 0) effectPos = new Vector3(new_pos.x, new_pos.y - 1.5f, -200.0f);
        else if (_HitDir == 1) effectPos = new Vector3(new_pos.x, new_pos.y + 1.5f, -200.0f);
        else if (_HitDir == 2) effectPos = new Vector3(new_pos.x + 1.5f, new_pos.y, -200.0f);
        else if (_HitDir == 3) effectPos = new Vector3(new_pos.x - 1.5f, new_pos.y, -200.0f);
        getHurtEffect.SetEffect(effectPos, 1.2f);
        enemySystem.PlaySound(1,1.0f);
        if (hp <= 0)
        {
            animator.Play("EnemyDoctorDie");
            SetState(5, true);
            enemySystem.NextStage();
        }
    }

    public override void Die()
    {
        
    }

    public void DieOver()
    {
        this.gameObject.SetActive(false);
        enemySystem.NextStage();
    }

    public override void SetAnimator()
    {
        //Debug.Log("SetAnimator" + state + "   " + lastState);
        if (!onAttacking)
        {
            if (playerPos.x > transform.position.x)
                transform.localScale = new Vector3(-scaleX, 1, 1);
            else transform.localScale = new Vector3(scaleX, 1, 1);
        }
        if (state != lastState) {
            if (lastState == 4) animator.SetBool("poisonOver", false);
            animator.SetInteger("state", state);
            lastState = state;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !attackOnce) {
            attackOnce = true;
            collision.GetComponent<Player>().GetHurt();
        }
        if (collision.tag == "Explosion") {
            SetHurtValue(3, 1);
        }
    }

}