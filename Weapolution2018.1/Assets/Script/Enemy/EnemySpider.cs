using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : CEnemy
{

    float webTime;
    Transform webs;
    CChildProjectSystem childProjectSystem;
    
    public override void Awake()
    {
        base.Awake();
        childProjectSystem = transform.parent.parent.Find("Webs").GetComponent<CChildProjectSystem>();
        rambleLayer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("ObstacleForOut");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
        StateMachine();
        SetAnimator();
    }

    public override void StateMachine()
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
                Weave();
                break;
            case 3:
                BeTrapped(3.0f);
                break;
        }
    }

    void SetBehaviorOutside()
    {
        if (isForceState) return; //若是強制執行狀態如攻擊，直接return

        if (webTime < 3.0f)
        {
            webTime += Time.deltaTime;
            if (inState_time >= state_time && state_time > 0.1f)//每個狀態的時間到換新的，為了不蓋過
            { 
                SetState(-1, false);                                                                       //到新狀態，狀態時間加一個判斷大於0.1
            }
        }
        else
        {
            if (inState_time >= state_time)
            {
                Vector2 selfPos2d = new Vector2(new_pos.x, new_pos.y);
                LayerMask webLayer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("ObstacleForOut") |
                                        1 << LayerMask.NameToLayer("DamageToPlayer");
                Collider2D detectWeb = Physics2D.OverlapCircle(selfPos2d, 0.3f, webLayer);
                if (detectWeb == null)
                {
                    if (childProjectSystem.GetFreeNum() > 0 && Random.Range(0.0f, 1.0f) > 0.7f) SetState(2, true);
                }
                webTime = 0.0f;
            }
        }
        //new_pos = transform.position + new Vector3(0, f_pos_offsetY, 0); //更新新的位置座標
    }

    void Weave() {

        if (state_time < 0.1f) {
            state_time = 1.0f;
        } 
        inState_time += Time.deltaTime;
    }

    public void OnWeaving() {
        childProjectSystem.AddUsed(self_pos);
        
    }

    public void WeaveOver() {
        //Debug.Log("weave over");
        inState_time = 2.0f;
        isForceState = false;
    }

    public virtual void BeTrapped(float trapTime)
    {
        inState_time += Time.deltaTime;
        if (state_time < 0.1f)
        {
            state_time = trapTime;
            enemySystem.PlaySound(0,1.0f);
        }

        if (inState_time >= state_time) isForceState = false;
    }

    public void BeTrapedOver() {
        inState_time = 5.0f;
        isForceState = false;
    }

    public override void SetAnimator()
    {

        if (!isForceState)
        {
            transform.localScale = new Vector3(-Mathf.Sign(go_way.x) * scaleX, scaleY, 1);
        }
        if (state != lastState)
        {
            //Debug.Log("setAnimator" + gameObject.name);
            animator.SetInteger("state", state);
            animator.SetTrigger("exist");
            lastState = state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            Trape temp = collision.GetComponent<Trape>();
            if (temp.IsOut()) {
                
                SetState(3, true);
                temp.ResetChild();
            }
            
        }
    }

}
