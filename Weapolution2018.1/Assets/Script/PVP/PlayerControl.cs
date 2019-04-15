using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour {

    bool isCraft, isKeyboard, dashHit, invincible, die;
    bool teamA, meleeWeapon, menuMode = false;
    int faceDir = 1, lastDir = -1, lastXInput = 0, lastYInput = 0;
    string control;
    float speedX, speedY, dashInputTime = 1.0f, dashTime;
    float[] directTime = new float[2] { -1.0f, -1.0f };
    float[] inputTime = new float[4] { -1.0f, -1.0f, -1.0f, -1.0f };
    Vector2 dashDir;
    Vector3 moveDir = Vector3.zero;
    LayerMask moveMask;
    Animator animator;
    //CraftSystem craftSystem;
    //PickWeaponPVP pickWeapon;
    PVPPlayerManager playerManager;
    CharacterVoice effectAudio;

    Action AttackOver, TrapOver, CollectOver;

    public enum State {
        idle, move, dash, hurt, die, attack,
        collect, useTrap
    }
    State state = State.idle;
    State lastState = State.die;

    public float speed = 5.0f;

    // Use this for initialization
    private void Awake()
    {
        moveMask = 1 << LayerMask.NameToLayer("Obstacle");
        animator = GetComponent<Animator>();
    }
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (StageManager.timeUp) return;
        switch (state) {
            case State.idle:
                if (FirstInState()) {
                    animator.SetBool("is_walk", false);
                }
                GetInput();
                break;
            case State.move:
                if (FirstInState())
                {
                    animator.SetBool("is_walk", true);
                    moveMask = 1 << LayerMask.NameToLayer("Obstacle") ;//| 1 << LayerMask.NameToLayer("Player")
                }
                GetInput();
                Move();
                break;
            case State.dash:
                if (FirstInState()) {
                    animator.SetBool("is_rolling", true);
                    moveMask = 1 << LayerMask.NameToLayer("Obstacle");
                    dashDir = new Vector2(speedX, speedY).normalized;
                    dashHit = false;
                    dashTime = .0f;
                }
                Dash();
                break;
            case State.hurt:
                if(FirstInState()) animator.SetTrigger("is_hurt");
                GetInput();
                Move();
                break;
            case State.die:
                if(FirstInState()) animator.SetBool("is_die", true);
                break;
            case State.attack:
                if(FirstInState()) animator.SetBool("is_attack", true);
                if (meleeWeapon) {
                    GetInput();
                    Move();
                }
                break;
            case State.collect:

                break;
            case State.useTrap:
                //if (FirstInState()) animator.SetTrigger("is_dig");
                break;

        }
    }

    public void Init(PVPPlayerManager manager, CharacterVoice voice, bool team) {
        playerManager = manager;
        effectAudio = voice;
        teamA = team;
        
    }
    public void SetController(bool crafter, string con) {
        isCraft = crafter;
        control = con;
        if (con == "keyboard") isKeyboard = true;
    }
    public void SubAtkFunc( Action atkOver) {
        AttackOver = atkOver;
    }
    public void SubCraftFunc(Action trapOver, Action colOver) {
        TrapOver = trapOver;
        CollectOver = colOver;
    }

    bool FirstInState() {
        if (state != lastState) {
            lastState = state;
            return true;
        }
        return false;
    }

    void GetInput()
    {
        if (menuMode) {
            return;
        }
        if (isKeyboard)
        {
            bool stop = false;
            bool inputUp = Input.GetKey(KeyCode.W);
            bool inputDown = Input.GetKey(KeyCode.S);
            bool inputLeft = Input.GetKey(KeyCode.A);
            bool inputRight = Input.GetKey(KeyCode.D);
            if (!inputUp && !inputDown && !inputRight && !inputLeft) {
                speedX = .0f;
                speedY = .0f;
                lastXInput = 0;
                lastYInput = 0;
                stop = true;
            }
            switch (faceDir)
            {
                case 0:
                case 1:
                    if (inputRight && inputLeft)
                    {
                        if (lastXInput <= 0 && inputRight) speedX = 1.0f;
                        if (lastXInput >= 0 && inputLeft) speedX = -1.0f;
                    }
                    else
                    {
                        if (inputRight)
                        {
                            speedX = 1.0f;
                            lastXInput = 1;
                        }
                        else if (inputLeft)
                        {
                            speedX = -1.0f;
                            lastXInput = -1;
                        }
                        else
                        {
                            speedX = .0f;
                            lastXInput = 0;
                        }
                    }

                    if (!inputUp && !inputDown)
                    {
                        if (speedX > 0.1f) faceDir = 3;
                        else if(speedX < -0.1f) faceDir = 2;
                        speedY = .0f;
                        lastYInput = 0;
                        break;
                    }
                    if (faceDir == 0)
                    {
                        //if (!inputDown && lastYInput <= 0) lastYInput = 1;
                        if (inputUp && inputDown)
                        {
                            if (lastYInput > 0 && inputDown)
                            {
                                speedY = -1.0f;
                                faceDir = 1;
                            }
                        }
                        else if (inputDown)
                        {
                            speedY = -1.0f;
                            faceDir = 1;
                        }
                        else if (inputUp) {
                            speedY = 1.0f;
                            lastYInput = 1;
                        } 

                    }
                    else if (faceDir == 1)
                    {
                        //if (!inputUp && lastYInput >= 0) lastYInput = -1;
                        if (inputUp && inputDown)
                        {
                            if (lastYInput < 0 && inputUp)
                            {
                                speedY = 1.0f;
                                faceDir = 0;
                            }
                        }
                        else if (inputUp)
                        {
                            speedY = 1.0f;
                            faceDir = 0;
                        }
                        else if (inputDown) {
                            speedY = -1.0f;
                            lastYInput = -1;
                        } 

                    }

                    break;

                case 2:
                case 3:
                    if (inputUp && inputDown)
                    {
                        if (lastYInput <= 0 && inputUp) speedY = 1.0f;
                        if (lastYInput >= 0 && inputDown) speedY = -1.0f;
                    }
                    else
                    {
                        if (inputUp)
                        {
                            speedY = 1.0f;
                            lastYInput = 1;
                        }
                        else if (inputDown)
                        {
                            speedY = -1.0f;
                            lastYInput = -1;
                        }
                        else
                        {
                            speedY = .0f;
                            lastYInput = 0;
                        }
                    }

                    if (!inputRight && !inputLeft)
                    {
                        if (speedY > 0.1f) faceDir = 0;
                        else if(speedY < -0.1f) faceDir = 1;
                        speedX = .0f;
                        lastXInput = 0;
                        break;
                    }
                    if (faceDir == 2)
                    {
                        //if (!inputRight && lastXInput >= 0) lastXInput = -1;
                        if (inputLeft && inputRight)
                        {
                            if (lastXInput < 0 && inputRight)
                            {
                                speedX = 1.0f;
                                faceDir = 3;
                            }
                        }
                        else if (inputRight)
                        {
                            speedX = 1.0f;
                            faceDir = 3;
                        }
                        else if (inputLeft) {
                            speedX = -1.0f;
                            lastXInput = -1;
                        } 

                    }
                    else if (faceDir == 3)
                    {
                        //if (!inputLeft && lastXInput <= 0) lastXInput = 1;
                        if (inputLeft && inputRight)
                        {
                            if (lastXInput > 0 && inputLeft)
                            {
                                speedX = -1.0f;
                                faceDir = 2;
                            }
                        }
                        else if (inputLeft)
                        {
                            speedX = -1.0f;
                            faceDir = 2;
                        }
                        else if (inputRight) {
                            speedX = 1.0f;
                            lastXInput = 1;
                        } 
                    }
                    break;
            }
            //if (stop)
            //{
            //    if (inputUp)
            //    {
            //        faceDir = 0;
            //        speedY = 1.0f;
            //        lastYInput = 1;
            //    }
            //    else if (inputDown)
            //    {
            //        faceDir = 1;
            //        speedY = -1.0f;
            //        lastYInput = -1;
            //    }
            //    if (inputRight)
            //    {
            //        faceDir = 3;
            //        speedX = 1.0f;
            //        lastXInput = 1;
            //    }
            //    else if (inputLeft)
            //    {
            //        faceDir = 2;
            //        speedX = -1.0f;
            //        lastXInput = -1;
            //    }
            //}
            //else
            //{

            //}


            //如果在攻擊狀態跳過下面狀態切換只接收移動輸入
            if (state != State.idle && state != State.move) return;

            //判斷是否移動
            if (stop)
            {
                state = State.idle;
            }
            else
            {// 有移動輸入再偵測翻滾輸入
                state = State.move;
            }
        }
        else { //手把移動
            speedX = Input.GetAxis(control + "LHorizontal");
            speedY = Input.GetAxis(control + "LVertical");
            if (Mathf.Abs(speedX) >= Mathf.Abs(speedY))
            {
                if (speedX < -0.1f) faceDir = 2;
                else if(speedX > 0.1f) faceDir = 3;
            }
            else {
                if (speedY < -0.1f) faceDir = 1;
                else if(speedY > 0.1f) faceDir = 0;
            }

            //如果在攻擊狀態跳過下面狀態切換只接收移動輸入
            if (state != State.idle && state != State.move) return;

            //判斷是否移動
            if (Mathf.Abs(speedY) < 0.1f && Mathf.Abs(speedX) < 0.1f)
            {
                state = State.idle;
                //dashInputTime = 1.0f;
            }
            else
            {
                 state = State.move;
            }
        }
        if (lastDir != faceDir)
        {
            lastDir = faceDir;
            animator.SetInteger("face_way", faceDir);
            animator.SetTrigger("change_face");
        }

        //if (Mathf.Abs(speedY) < 0.1f && Mathf.Abs(speedX) < 0.1f) {
        //    state = State.idle;
        //} 
        //else {// 有移動輸入再偵測翻滾輸入
        //    if ((isKeyboard && Input.GetMouseButtonDown(1)) || (!isKeyboard && GetControllerDash()))
        //        state = State.dash;
        //    else
        //        state = State.move;
        //}

    }

    public State GetState() {
        return state;
    }

    public void SetIDle() {
        state = State.idle;
    }
    public bool SetAttackState(int type) {
        if (state == State.idle || state == State.move)
        {
            state = State.attack;
            if (type == 0) meleeWeapon = true;
            else meleeWeapon = false;
            return true;
        }
        else return false;
    }
    public void SetDashState() {
        if (state == State.move)
        {
            state = State.dash;
        }
    }
    public bool SetCollect() {
        if (state == State.idle || state == State.move)
        {
            state = State.collect;
            return true;
        }
        else return false;
    }
    public bool SetUseTrap()
    {
        if (state == State.idle || state == State.move)
        {
            state = State.useTrap;
            return true;
        }
        else return false;
    }

    public int GetFaceDir() {
        return faceDir;
    }

    public void SetMenuMode(bool value) {
        speedX = 0;
        speedY = 0;
        menuMode = value;
    }

    void Move()
    {
        //偵測障礙物，讓速度歸零
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D upHit = Physics2D.Raycast(pos, new Vector3(0, 1, 0),
                                        1.5f, moveMask);
        RaycastHit2D downHit = Physics2D.Raycast(pos, new Vector3(0, -1, 0),
                                        0.45f, moveMask);
        RaycastHit2D leftHit = Physics2D.Raycast(pos, new Vector3(-1, 0, 0),
                                        0.8f, moveMask);
        RaycastHit2D rightHit = Physics2D.Raycast(pos, new Vector3(1, 0, 0),
                                        0.8f, moveMask);
        if (speedY > .0f && upHit) speedY = 0.0f;
        else if (speedY < .0f && downHit) speedY = 0.0f;
        if (speedX > .0f && rightHit) speedX = .0f;
        else if (speedX < .0f && leftHit) speedX = .0f;

        if (upHit || downHit || leftHit || rightHit) {
            Debug.Log(isCraft + "   hit wall");
        }

        Vector2 move = Vector2.zero;
        if (isKeyboard) move = new Vector2(speedX, speedY).normalized;
        else move = new Vector2(speedX, speedY);
        Debug.Log(isKeyboard + "  is move   " + move);
        transform.position += Time.deltaTime * speed * new Vector3(move.x, move.y, 0);
    }

    void Dash() {
        dashTime += Time.deltaTime;
        //if (dashTime > 0.4f) state = State.idle;
        if (!dashHit) {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.2f);
            if (Physics2D.Raycast(pos, dashDir, 1.0f, moveMask)) {
                dashDir = Vector2.zero;
                dashHit = true;
            }    
        }
        transform.position += Time.deltaTime * dashTime * new Vector3(dashDir.x * 50.0f, dashDir.y * 40.0f);

    }
    public void OverRoll()
    {
        speedX= 0;
        speedY = 0;
        animator.SetBool("is_rolling", false);
        animator.SetBool("is_walk", false);
        state = State.idle;
        Debug.Log("OverRoll");
        //roll_time = 0;
        //invincible = false;
        //inFuntionTime = 0;
    }


    public void OverAttack()
    {
        Debug.Log(gameObject.name);
        animator.SetBool("is_attack", false);
        state = State.idle;
        Debug.Log("OverAttack");
        speedX = .0f;
        speedY = .0f;
        //invincible = false;

        AttackOver();
    }

    public void GetHurt(float value)
    {
        if (invincible) return;
        //Debug.Log("getHurt");

        state = State.hurt;
        invincible = true;
        effectAudio.SetAudio(1);
        //animator.SetTrigger("is_hurt");
        speedX = .0f;
        speedY = .0f;
        if (state == State.attack)//如果被打到時正在攻擊，被斷招
        {
            animator.SetBool("is_attack", false);
        }
        playerManager.SetHP(teamA, -value);

        //if (inFuntionTime == 0)
        //{
        //    L_JoyX = 0.0f;
        //    L_JoyY = 0.0f;
        //    K_JoyX = 0.0f;
        //    K_JoyY = 0.0f;
        //    TeamHp.ChangeHp(false, 0.05f);
        //    inFuntionTime++;
            
        //}


    }

    public void OverBeHurt()
    {
        //Debug.Log(gameObject.name + "hurt over");
        state = State.idle;
        invincible = false;
        //TeamHp.checkGameOver = true;

    }


    public void OverGathering() {
        Debug.Log("oooovvvveeerrrr gatjerrrrr");
        CollectOver();
        animator.SetBool("is_gather", false);
        //animator.SetBool("is_walk", false);
        state = State.idle;
    }
    public void SetTrapOver() {
        TrapOver();
        animator.SetBool("is_walk", false);
        state = State.idle;
    }


    public void GoDie() {
        state = State.die;
        die = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DamageToPlayer" && !invincible && !die) {
            GetHurt(collision.GetComponent<PVPProjectile>().GetATKValue());
        }
    }

}



            ////同時按兩鍵以上
            //if (!stop) {
            //    if (Input.GetKeyDown(KeyCode.W))
            //    {
            //        if (speedY< .0f) faceDir = 0;
            //        speedY = 1.0f;
            //    }
            //    else if (Input.GetKeyDown(KeyCode.S)) {
            //        if (speedY > .0f) faceDir = 1;
            //        speedY = -1.0f;
            //    }
            //    if (Input.GetKeyDown(KeyCode.D))
            //    {
            //        if (speedX< .0f) faceDir = 3;
            //        speedX = 1.0f;
            //    }
            //    else if (Input.GetKeyDown(KeyCode.A)) {
            //        if (speedX > .0f) faceDir = 2;
            //        speedX = -1.0f;
            //    }


            //}
            //else { //只按一鍵
            //    if (Input.GetKeyDown(KeyCode.W))
            //    {
            //        faceDir = 0;
            //        speedY = 1.0f;
            //    }
            //    else if (Input.GetKeyDown(KeyCode.S))
            //    {
            //        faceDir = 1;
            //        speedY = -1.0f;
            //    }
            //    if (Input.GetKeyDown(KeyCode.D))
            //    {
            //        faceDir = 3;
            //        speedX = 1.0f;
            //    }
            //    else if (Input.GetKeyDown(KeyCode.A))
            //    {
            //        faceDir = 2;
            //        speedX = -1.0f;
            //    }
            //}
            //if ((Input.GetKeyUp(KeyCode.W) && speedY > 0.1f) || (Input.GetKeyUp(KeyCode.S) && speedY< 0.1f)) {
            //    if (Input.GetKey(KeyCode.W)) speedY = 1.0f;
            //    else if (Input.GetKey(KeyCode.S)) speedY = -1.0f;
            //    else speedY = .0f;
            //    if (speedX > .1f) faceDir = 3;
            //    else if (speedX< -.1f) faceDir = 2;
            //}
            //if ((Input.GetKeyUp(KeyCode.D) && speedX > 0.1f) || (Input.GetKeyUp(KeyCode.A) && speedX< 0.1f)) {
            //    if (Input.GetKey(KeyCode.D)) speedX = 1.0f;
            //    else if (Input.GetKey(KeyCode.A)) speedX = -1.0f;
            //    else speedX = .0f;
            //    if (speedY > .1f) faceDir = 0;
            //    else if (speedY< -.1f) faceDir = 1;
            //} 