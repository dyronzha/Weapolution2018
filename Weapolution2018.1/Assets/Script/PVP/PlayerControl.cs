using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    bool isCraft, isKeyboard, dashHit, invincible, die;
    int faceDir = 1, lastDir = -1;
    string control;
    float speedX, speedY, dashInputTime = 1.0f, dashTime;
    Vector2 dashDir;
    Vector3 moveDir = Vector3.zero;
    LayerMask moveMask;
    Animator animator;
    CraftSystem craftSystem;
    PickWeaponPVP pickWeapon;
    PVPPlayerManager playerManager;
    CharacterVoice effectAudio;

    enum State {
        idle, move, dash, hurt, die, attack
    }
    State state = State.idle;
    State lastState = State.die;

    public float speed = 5.0f;

    // Use this for initialization
    private void Awake()
    {
        moveMask = 1 << LayerMask.NameToLayer("Obstacle");
        animator = GetComponent<Animator>();
        pickWeapon = transform.Find("PickWeapon").GetComponent<PickWeaponPVP>();
    }
    void Start () {
        pickWeapon.SetController(!isKeyboard, control);
	}
	
	// Update is called once per frame
	void Update () {
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
                    moveMask = 1 << LayerMask.NameToLayer("Obstacle");
                }
                GetInput();
                Move();
                break;
            case State.dash:
                if (FirstInState()) {
                    animator.SetBool("is_rolling", true);
                    moveMask = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Player");
                    dashDir = new Vector2(speedX, speedY).normalized;
                    dashHit = false;
                    dashTime = .0f;
                }
                Dash();
                break;
            case State.hurt:
                break;
            case State.die:
                break;
            case State.attack:
                if (FirstInState()) {
                    Attack();
                }
                break;

        }
	}

    public void Init(PVPPlayerManager manager, CharacterVoice voice) {
        playerManager = manager;
        effectAudio = voice;
    }
    public void SetController(bool crafter, string con) {
        isCraft = crafter;
        control = con;
        if (con == "keyboard") isKeyboard = true;
        if (crafter) {
            craftSystem = transform.Find("CraftSystem").GetComponent <CraftSystem>();
        }
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
        if (isKeyboard)
        {
            //同時按兩鍵以上
            if (Mathf.Abs(speedX) > 0.1f && speedY > 0.1f) {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (speedY < .0f) faceDir = 0;
                    speedY = 1.0f;
                }
                else if (Input.GetKeyDown(KeyCode.S)) {
                    if (speedY > .0f) faceDir = 1;
                    speedY = -1.0f;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (speedX < .0f) faceDir = 3;
                    speedX = 1.0f;
                }
                else if (Input.GetKeyDown(KeyCode.A)) {
                    if (speedX > .0f) faceDir = 2;
                    speedX = -1.0f;
                } 


            }
            else { //只按一鍵
                if (Input.GetKeyDown(KeyCode.W))
                {
                    faceDir = 0;
                    speedY = 1.0f;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    faceDir = 1;
                    speedY = -1.0f;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    faceDir = 3;
                    speedX = 1.0f;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    faceDir = 2;
                    speedX = -1.0f;
                }
            }
            if ((Input.GetKeyUp(KeyCode.W) && speedY > 0.1f) || (Input.GetKeyUp(KeyCode.S) && speedY < 0.1f)) speedY = .0f;
            if ((Input.GetKeyUp(KeyCode.D) && speedX > 0.1f) || (Input.GetKeyUp(KeyCode.A) && speedX < 0.1f)) speedX = .0f;

            //判斷是否移動
            if (Mathf.Abs(speedY) < 0.1f && Mathf.Abs(speedX) < 0.1f)
            {
                state = State.idle;
            }
            else
            {// 有移動輸入再偵測翻滾輸入
                if(Input.GetMouseButtonDown(1)) state = State.dash;
                else state = State.move;
            }

            //判斷攻擊
            if (Input.GetMouseButtonDown(0) && pickWeapon.holdWeapon.ani_type >= 0) {
                state = State.attack;
            }
        }
        else { //手把移動
            speedX = Input.GetAxis(control + "LHorizontal");
            speedY = Input.GetAxis(control + "LVertical");
            if (Mathf.Abs(speedX) >= Mathf.Abs(speedY))
            {
                if (speedX <= .0f) faceDir = 1;
                else faceDir = 0;
            }
            else {
                if (speedY <= .0f) faceDir = 2;
                else faceDir = 3;
            }

            //判斷是否移動
            if (Mathf.Abs(speedY) < 0.1f && Mathf.Abs(speedX) < 0.1f)
            {
                state = State.idle;
                dashInputTime = 1.0f;
            }
            else
            {// 有移動輸入再偵測翻滾輸入
                if (GetControllerDash()) state = State.dash;
                else state = State.move;
            }

            //判斷攻擊
            if (Input.GetButtonDown(control + "ButtonA") && pickWeapon.holdWeapon.ani_type >= 0)
            {
                state = State.attack;
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

    bool GetControllerDash() {

        if (dashInputTime < 0.7f) {
            dashInputTime += Time.deltaTime;
            return false;
        }
        else {
            if (Input.GetAxis(control + "LT") < 0.7f)
                return false;
            else {
                dashInputTime = 0.0f;
                return true;
            } 
        }
    }

    void Move()
    {
        //偵測障礙物，讓速度歸零
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D upHit = Physics2D.Raycast(pos, new Vector3(0, 1, 0),
                                        1.5f, moveMask);
        RaycastHit2D downHit = Physics2D.Raycast(pos, new Vector3(0, -1, 0),
                                        0.5f, moveMask);
        RaycastHit2D leftHit = Physics2D.Raycast(pos, new Vector3(-1, 0, 0),
                                        0.8f, moveMask);
        RaycastHit2D rightHit = Physics2D.Raycast(pos, new Vector3(1, 0, 0),
                                        0.8f, moveMask);
        if (speedY > .0f && upHit) speedY = 0.0f;
        else if (speedY < .0f && downHit) speedY = 0.0f;
        if (speedX > .0f && rightHit) speedX = .0f;
        else if (speedX < .0f && leftHit) speedX = .0f;

        Vector2 move = Vector2.zero;
        if (isKeyboard)move = new Vector2(speedX, speedY).normalized;
        transform.position += Time.deltaTime * speed * new Vector3(move.x, move.y, 0);
    }

    void Dash() {
        dashTime += Time.deltaTime;
        if (dashTime > 0.4f) state = State.idle;
        if (!dashHit) {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.2f);
            if (Physics2D.Raycast(pos, dashDir, 2.5f, moveMask))
                dashDir = Vector2.zero;
            dashHit = true;
        }
        transform.position += Time.deltaTime * dashTime * new Vector3(dashDir.x * 50.0f, dashDir.y * 40.0f);

    }
    public void OverRoll()
    {
        speedX= 0;
        speedY = 0;
        animator.SetBool("is_rolling", false);
        animator.SetBool("is_walk", false);
        Debug.Log("OverRoll");
        //roll_time = 0;
        //invincible = false;
        //inFuntionTime = 0;
    }

    void Attack() {
        if (pickWeapon.holdWeapon.ani_type == 0) //持近距離武器
        {
            animator.SetInteger("weapon_type", 0);
        }
        else if (pickWeapon.holdWeapon.ani_type == 1)//持遠距離武器
        {
            animator.SetInteger("weapon_type", 1);
        }
        else if (pickWeapon.holdWeapon.ani_type == 2) //放陷阱
        {
            animator.SetInteger("weapon_type", 2);
        }
        animator.SetBool("is_attack", true);
        effectAudio.SetAudio(pickWeapon.holdWeapon.audio_source);
    }

    public void OverAttack()
    {
        animator.SetBool("is_attack", false);
        state = State.idle;
        invincible = false;
        if (pickWeapon.UsingWeaponTillBroken())
        {
            //projectile_num = 0;
        }
        Debug.Log("OverAttack");
        speedX = .0f;
        speedY = .0f;
    }

    public void GetHurt()
    {
        //Debug.Log("getHurt");

        state = State.hurt;
        invincible = true;
        effectAudio.SetAudio(1);
        animator.SetTrigger("is_hurt");
        speedX = .0f;
        speedY = .0f;
        if (state == State.attack)//如果被打到時正在攻擊，被斷招
        {
            
            animator.SetBool("is_attack", false);
        }

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DamageToPlayer" && !invincible && !die) {
            GetHurt();
        }
    }

}
