using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    bool invincible;
    int weaponUsedTimes = 0;
    static public bool gamemode;
    //mode 0 == PvE
    //mode 1 == PvP
    static public bool p1controller = false;
    //mode 0 == 鍵盤
    //mode 1 == 搖桿
    static public bool p2controller = false;
    //mode 0 == 鍵盤
    //mode 1 == 搖桿
    static public string p1joystick = "p1";
    static public string p2joystick = "p2";

    static public bool p1charaType = false;
    static public bool p2charaType = true;
    //mode 0 == Attacker
    //mode 1 == Crafter

    static public bool p1moveAble;
    static public bool p2moveAble;
    //mode 0 == can't walk
    //mode 1 == can walk

    bool WhichCharacter, WhichController;
    string WhichJoy;

    static public int face_way;
    static public int last_way; //上一個面向

    public static bool isMapped;

    string which_player;

    float p1Ap;
    public bool p1_die = false;
    public bool p2_die = false;
    bool p1Ap_enough;
    bool p2Ap_enough;

    public float L_JoyX = 0.0f;
    public float L_JoyY = 0.0f;
    public float K_JoyX = 0.0f;
    public float K_JoyY = 0.0f;

    float beingHurt_time = 0;
    float unbeatable_time = 1;
    Vector3 rollWay;
    float roll_time = 0;
    float rollCDtime = 0.7f;
    float clickTime = 0;

    public float Speed = 5.0f;
    int animation_type;
   
    public int character_num;

    GameObject projectileSystem;
    int projectile_num;
    static public bool outOfProjectile = false;

    private Animator animator;
    AnimatorStateInfo stateinfo;
    SpriteRenderer img;

    CharacterVoice EffectAudio;
    int inFuntionTime = 0;

    CPickWeapon pickWeaponScript;
    static public CItem weapon;
    public TutorialRequest tutorialRequest;
    public bool test;

    LayerMask unWalkable;

    public float downDetect;

    void Awake()
    {
        if (!isMapped)
        {
            p1charaType = false;
            p2charaType = true;

            p1controller = false; //預設鍵盤
            p1joystick = null;
            p2controller = true; //預設手把
            p2joystick = "p1";

        }
        unWalkable = 1 << LayerMask.NameToLayer("Obstacle") |
                      1 << LayerMask.NameToLayer("ObstacleForIn");
    }
    

    void Start()
    {
        pickWeaponScript = transform.Find("PickWeapon").GetComponent<CPickWeapon>();    
        animator = transform.GetComponent<Animator>();
        //Debug.Log("animator" + animator);
        animator.SetInteger("face_way", 1);
        animator.SetInteger("weapon_type", 0);
        animation_type = 0;
        stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        projectileSystem = GameObject.Find("ProjectileSystem");
        projectile_num = 0;

        EffectAudio = GameObject.Find("CharacterAudio").GetComponent<CharacterVoice>();

        p1moveAble = true;
        p2moveAble = true;

        p1Ap_enough = true;
        p2Ap_enough = true;


        //Debug.Log("p1controller" + p1controller + "p1joystick" + p1joystick + "p1charaType" + p1charaType);
    }
    
    void Update () {
        if (StageManager.timeUp) return;
        if (p1_die) return;
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKeyDown(KeyCode.X)) CharacterRespawn();
        if (!p1charaType) //p1是attacker
        {
            Movement(p1moveAble, p1controller, p1joystick);
            #region 翻滾
            if (!p1controller) //鍵盤
            {
                if (p1moveAble && (Input.GetMouseButtonDown(1)) && Time.time - clickTime > rollCDtime)
                {
                    if (Mathf.Abs(K_JoyY) < 0.1f && Mathf.Abs(K_JoyX) < 0.1f) return;
                    clickTime = Time.time;
                    animation_type = 1;
                    invincible = true;

                }
            }
            else
            {
                if(p1joystick == "p1")
                {
                    if (p1moveAble && Input.GetAxis(p1joystick + "LT") > 0.5f && Time.time - clickTime > rollCDtime)
                    {
                        if (Mathf.Abs(L_JoyY) < 0.1f && Mathf.Abs(L_JoyX) < 0.1f) return;
                        clickTime = Time.time;
                        animation_type = 1;
                        invincible = true;
                        Debug.Log("enter roll");

                    }
                }
                else
                {
                    if (p1moveAble && Input.GetAxis(p1joystick + "LT") > 0.5f && Time.time - clickTime > rollCDtime)
                    {
                        if (Mathf.Abs(L_JoyY) < 0.1f && Mathf.Abs(L_JoyX) < 0.1f) return;
                        clickTime = Time.time;
                        animation_type = 1;
                        invincible = true;
                        Debug.Log("enter roll");

                    }
                }
                
            }

            #endregion

            #region 攻擊
            if (!p1controller) //鍵盤
            {
                if ((Input.GetMouseButtonDown(0))) //|| p1_RT > 0.1f
                {
                    animation_type = 2;
                }
            }
            else
            {
                if (Input.GetButtonDown(p1joystick + "ButtonA")) animation_type = 2;
            }

            #endregion
            switch (animation_type)
            {
                case 0:
                    break;
                case 1:
                    Debug.Log("roll");
                    Roll(1, face_way);
                    break;
                case 2:
                    Attack();
                    break;
                case 3:

                    break;
            }
        }
        else
        {
            Movement(p2moveAble, p2controller, p2joystick);
            #region 翻滾
            if (!p2controller) //鍵盤
            {

                if (p2moveAble && (Input.GetMouseButtonDown(1)) && Time.time - clickTime > rollCDtime)
                {
                    if (Mathf.Abs(K_JoyY) < 0.1f && Mathf.Abs(K_JoyX) < 0.1f) return;
                    clickTime = Time.time;
                    animation_type = 1;
                    invincible = true;

                }
            }
            else
            {
                if (p2joystick == "p1")
                {
                    if (p2moveAble && Input.GetAxis(p2joystick + "LT") > 0.5f && Time.time - clickTime > rollCDtime)
                    {
                        if (Mathf.Abs(L_JoyY) < 0.1f && Mathf.Abs(L_JoyX) < 0.1f) return;
                        clickTime = Time.time;
                        animation_type = 1;
                        invincible = true;
                        Debug.Log("enter roll");

                    }
                }
                else
                {
                    if (p2moveAble && Input.GetAxis(p2joystick + "LT") > 0.5f && Time.time - clickTime > rollCDtime)
                    {
                        if (Mathf.Abs(L_JoyY) < 0.1f && Mathf.Abs(L_JoyX) < 0.1f) return;
                        clickTime = Time.time;
                        animation_type = 1;
                        invincible = true;
                        Debug.Log("enter roll");

                    }
                }
            }

            #endregion

            #region 攻擊
            if (!p2controller) //鍵盤
            {
                if ((Input.GetMouseButtonDown(0))) //|| p1_RT > 0.1f
                {
                    animation_type = 2;
                }
            }
            else
            {
                if (Input.GetButtonDown(p2joystick + "ButtonA")) animation_type = 2;
            }

            #endregion
            switch (animation_type)
            {
                case 0:
                    break;
                case 1:
                    Debug.Log("roll");
                    Roll(2, face_way);
                    break;
                case 2:
                    Attack();
                    break;
                case 3:

                    break;
            }
        }


       
    }

    void CharacterRespawn()
    {
        this.gameObject.transform.position = new Vector3(-12f, -6f, 0);
    }
    void Movement( bool move , bool ctrlmode , string Joystick_num)
    {
        if (move)
        {
            
            if (!ctrlmode) //鍵盤
            {
                if (K_JoyX == 0 && K_JoyY == 0)
                {
                    animator.SetBool("is_walk", false);
                    if (Input.GetKey(KeyCode.D))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position += Time.deltaTime * new Vector3(Speed, 0, 0);
                        animator.SetInteger("face_way", 3);
                        face_way = 3;
                        K_JoyX = Speed;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position -= Time.deltaTime * new Vector3(Speed, 0, 0);
                        animator.SetInteger("face_way", 2);
                        face_way = 2;
                        K_JoyX = -1 * Speed;

                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position += Time.deltaTime * new Vector3(0, Speed, 0);
                        animator.SetInteger("face_way", 0);
                        face_way = 0;
                        K_JoyY = Speed;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position -= Time.deltaTime * new Vector3(0, Speed, 0);
                        animator.SetInteger("face_way", 1);
                        face_way = 1;
                        K_JoyY = -1*Speed;
                    }
                    if (last_way != face_way)
                    {
                        animator.SetTrigger("change_face");
                    }
                    last_way = face_way;
                    //else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
                    //{
                    //    animator.SetBool("is_walk", false);
                    //    K_JoyY = 0;
                    //    K_JoyX = 0;
                    //}
                }
                else if (K_JoyX != 0 || K_JoyY != 0) //至少按著一個鍵
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position += Time.deltaTime * new Vector3(Speed, 0, 0);
                        //face_way = 3;
                        K_JoyX = Speed;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position -= Time.deltaTime * new Vector3(Speed, 0, 0);
                        //face_way = 2;
                        K_JoyX = -1 * Speed;
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position += Time.deltaTime * new Vector3(0, Speed, 0);
                        //face_way = 0;
                        K_JoyY = Speed;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        animator.SetBool("is_walk", true);
                        //transform.position -= Time.deltaTime * new Vector3(0, Speed, 0);
                        //face_way = 1;
                        K_JoyY = -1 * Speed;
                    }
                    if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
                    {
                        K_JoyY = 0;
                        K_JoyX = 0;
                    }
                    DetectWall(true);
                    transform.position += Time.deltaTime * new Vector3(K_JoyX, K_JoyY, 0);
                }


            }
            else //搖桿
            {

                L_JoyX = Input.GetAxis(Joystick_num + "LHorizontal");
                L_JoyY = Input.GetAxis(Joystick_num + "LVertical");

                if ( Mathf.Abs(L_JoyX) <= 0.1f && Mathf.Abs(L_JoyY) <= 0.1f) //idle
                {
                    animator.SetBool("is_walk", false);
                }
                #region 可斜走
                else 
                {
                    if (Mathf.Abs(L_JoyX) > Mathf.Abs(L_JoyY))
                    {
                        animator.SetBool("is_walk", true);

                        if (L_JoyX > 0)
                        {
                            animator.SetInteger("face_way", 3);
                            //transform.position += Time.deltaTime * new Vector3(L_JoyX, 0, 0) * Speed;
                            face_way = 3;
                        }
                        else
                        {
                            animator.SetInteger("face_way", 2);
                            //transform.position += Time.deltaTime * new Vector3(L_JoyX, 0, 0) * Speed;
                            face_way = 2;
                        }
                        //if (Mathf.Abs(L_JoyY) >= 0.2f) transform.position += Time.deltaTime * new Vector3(0, L_JoyY, 0) * Speed;
                        if (last_way != face_way)
                        {
                            animator.SetTrigger("change_face");
                            last_way = face_way;
                        }
                        //else if (Mathf.Abs(L_JoyY) >= 0.2f) transform.position -= Time.deltaTime * new Vector3(0, L_JoyY, 0) * Speed;
                    }
                    else if (Mathf.Abs(L_JoyX) < Mathf.Abs(L_JoyY))
                    {
                        animator.SetBool("is_walk", true);
                        if (L_JoyY > 0)
                        {
                            animator.SetInteger("face_way", 0);
                            //transform.position += Time.deltaTime * new Vector3(0, L_JoyY, 0) * Speed;
                            face_way = 0;
                        }
                        else
                        {
                            animator.SetInteger("face_way", 1);
                            //transform.position += Time.deltaTime * new Vector3(0, L_JoyY, 0) * Speed;
                            face_way = 1;
                        }
                        //if (Mathf.Abs(L_JoyX) >= 0.2f) transform.position += Time.deltaTime * new Vector3(L_JoyX, 0, 0) * Speed;
                        if (last_way != face_way)
                        {
                            animator.SetTrigger("change_face");
                            last_way = face_way;
                        }
                        //else if(Mathf.Abs(L_JoyX) >= 0.2f)transform.position += Time.deltaTime * new Vector3(L_JoyX, 0, 0) * Speed;
                    }
                    
                    DetectWall(false);
                    transform.position += Time.deltaTime * new Vector3(L_JoyX * Speed, L_JoyY * Speed, 0);
                }

                

            }
                #endregion
                #region 不可斜走  
                //else if (Mathf.Abs(L_JoyX) > Mathf.Abs(L_JoyY))
                //{
                //    if (L_JoyX > 0)
                //    {
                //        animator.SetInteger("face_way", 3);
                //    }
                //    else
                //    {
                //        animator.SetInteger("face_way", 2);
                //    }
                //    transform.position += Time.deltaTime * new Vector3(L_JoyX * Speed, 0, 0);
                //}
                //else if (Mathf.Abs(L_JoyX) < Mathf.Abs(L_JoyY))
                //{
                //    if (L_JoyY > 0)
                //    {
                //        animator.SetInteger("face_way", 0);
                //    }
                //    else
                //    {
                //        animator.SetInteger("face_way", 1);
                //    }
                //    transform.position += Time.deltaTime * new Vector3(0, L_JoyY * Speed, 0);
                //}
                #endregion
            }
        
        
        
    }

    void DetectWall(bool isKey) {
        float speedX = 0, speedY = 0;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D hitWall0 = Physics2D.Raycast(pos, new Vector3(0, 1, 0),
                                        1.5f, unWalkable);
        RaycastHit2D hitWall1 = Physics2D.Raycast(pos, new Vector3(0, -1, 0),
                                        downDetect, unWalkable);
        RaycastHit2D hitWall2 = Physics2D.Raycast(pos, new Vector3(-1, 0, 0),
                                        0.8f, unWalkable);
        RaycastHit2D hitWall3 = Physics2D.Raycast(pos, new Vector3(1, 0, 0),
                                        0.8f, unWalkable);

        if (isKey)
        {
            speedX = K_JoyX;
            speedY = K_JoyY;
            if (hitWall0 && K_JoyY > 0.0f) K_JoyY = 0.0f;
            if (hitWall1 && K_JoyY < 0.0f) K_JoyY = 0.0f;
            if (hitWall2 && K_JoyX < 0.0f) K_JoyX = 0.0f;
            if (hitWall3 && K_JoyX > 0.0f) K_JoyX = 0.0f;
        }
        else {
            speedX = L_JoyX;
            speedY = L_JoyY;
            if (hitWall0 && L_JoyY > 0.0f) L_JoyY = 0.0f;
            if (hitWall1 && L_JoyY < 0.0f) L_JoyY = 0.0f;
            if (hitWall2 && L_JoyX < 0.0f) L_JoyX = 0.0f;
            if (hitWall3 && L_JoyX > 0.0f) L_JoyX = 0.0f;
           
        }
    }

    void Roll(int player_number, int face_way)
    {
        if (inFuntionTime == 0) {
            inFuntionTime++;
            if (player_number == 1) p1moveAble = false;
            if (player_number == 2) p2moveAble = false;
            float rollSpeedX = 0.0f, rollSpeedY = 0.0f;
            Vector3 inputWay;
            if (!p1charaType)
            {
                if (!p1controller) inputWay = new Vector3(K_JoyX, K_JoyY, 0).V3NormalizedtoV2();
                else inputWay = new Vector3(L_JoyX, L_JoyY, 0).V3NormalizedtoV2();
            }
            else
            {
                if (!p2controller) inputWay = new Vector3(K_JoyX, K_JoyY, 0).V3NormalizedtoV2();
                else inputWay = new Vector3(L_JoyX, L_JoyY, 0).V3NormalizedtoV2();
            }

            rollSpeedX = inputWay.x * 50.0f;
            rollSpeedY = inputWay.y * 40.0f;
            rollWay = new Vector3(rollSpeedX, rollSpeedY, 0);
            animator.SetBool("is_rolling", true);
        }
        if (roll_time < 0.4f) {
            if (!invincible) invincible = true;
            roll_time += Time.deltaTime;

            RaycastHit2D hitWall = Physics2D.Raycast(transform.position, rollWay,
                                                    1.2f, unWalkable);
            if (!hitWall) Debug.Log("rollrollroll" + rollWay + "   " + roll_time);
            if (!hitWall) transform.position += roll_time * Time.deltaTime * rollWay;
        }
       
        
        //if (Mathf.Abs(K_JoyX) > 3) rollSpeedX = 50.0f * Mathf.Sign(K_JoyX);
        //if (Mathf.Abs(K_JoyY) > 3) rollSpeedY = 40.0f * Mathf.Sign(K_JoyY);
        //if (Mathf.Abs(K_JoyX) > 3 && Mathf.Abs(K_JoyY) > 3)
        //{
        //    rollSpeedX /= 1.41f;
        //    rollSpeedY /= 1.41f;
        //}
        
        
    }
   
    public void Gameover()
    {
        p1_die = true;
        animator.SetTrigger("is_die");

        if (inFuntionTime == 0)
        {
            p1moveAble = false;
            inFuntionTime++;
        }
        
    }
 

    public void Attack() {
        //animation_type = 0;
        if (weapon.ani_type < 0) {//空手
            animation_type = 0;
            return;
        } 
        if (inFuntionTime == 0)
        {
            if (weapon.ani_type == 0) //持近距離武器
            {
                animator.SetInteger("weapon_type", 0);
            }
            else if (weapon.ani_type == 1)//持遠距離武器
            {
                animator.SetInteger("weapon_type", 1);
                if (!p1charaType) p1moveAble = false;
                else p2moveAble = false;
            }
            else if (weapon.ani_type == 2) //放陷阱
            {
                animator.SetInteger("weapon_type", 2);
                if (!p1charaType) p1moveAble = false;
                else p2moveAble = false;
            }
            animator.SetBool("is_attack", true);
            EffectAudio.SetAudio(weapon.audio_source);            
            inFuntionTime++;
        }   
    }
    public void ShootProjectile()
    {
        Transform tempProjectile = projectileSystem.transform.GetChild(projectile_num);
        tempProjectile.position = transform.GetChild(1).GetChild(0).position;
        tempProjectile.gameObject.SetActive(true);
        tempProjectile.GetComponent<Projectile>().SetProjectileImg(face_way);
        projectile_num++;
        if (projectile_num >= projectileSystem.transform.childCount) {
            projectile_num = 0;
        }
        //if (projectile_num >= weapon.durability) //大於武器耐久
        //{
        //    projectile_num = 0;
        //    outOfProjectile = true;
        //    pickWeaponScript.ThrowWeapon();
        //    //GameObject.Find("PickWeapon").GetComponent<CPickWeapon>().ThrowWeapon();
        //}
    }

    public void RollStart() {
        Debug.Log("RollStart");
    }

    public void GetHurt()
    {
        if (test || invincible) return;
        if (p1_die) return;
        //Debug.Log("getHurt");

        if (animation_type == 2)//如果被打到時正在攻擊，被斷招
        { 
            animation_type = 0;
            inFuntionTime = 0;
            animator.SetBool("is_attack", false);
        }

        beingHurt_time = Time.time;
        animator.SetTrigger("is_hurt");
        if (!p1charaType) p1moveAble = false;
        else p2moveAble = false;
        invincible = true;
        if (inFuntionTime == 0)
        {
          L_JoyX = 0.0f;
          L_JoyY = 0.0f;
          K_JoyX = 0.0f;
          K_JoyY = 0.0f;
          TeamHp.ChangeHp(false, 0.05f);
          inFuntionTime++;
          EffectAudio.SetAudio(1);
        }

        
    }

    public void OverRoll()
    {
        K_JoyY = 0;
        K_JoyX = 0;
        animator.SetBool("is_rolling", false);
        animation_type = 0;
        if (!p1charaType)  p1moveAble = true;
        else p2moveAble = true;
        if (test)tutorialRequest.DoneRoll();
        Debug.Log("OverRoll");
        roll_time = 0;
        invincible = false;
        inFuntionTime = 0;
    }

    public void OverAttack()
    {
        animator.SetBool("is_attack", false);
        animation_type = 0;
        inFuntionTime = 0;
        weaponUsedTimes++;
        invincible = false;
        if (weaponUsedTimes >= weapon.durability)
        {
            pickWeaponScript.DestroyWeapon();
            weapon = CItemDataBase.items[0];
            projectile_num = 0;
            weaponUsedTimes = 0;
        }
        if (test) tutorialRequest.DoneAttack();
        Debug.Log("OverAttack");
        if (!p1charaType) p1moveAble = true;
        else p2moveAble = true;
        K_JoyY = 0;
        K_JoyX = 0;
    }

    public void OverBeHurt()
    {
        //Debug.Log(gameObject.name + "hurt over");
        if (!p1charaType) p1moveAble = true;
        else p2moveAble = true;
        inFuntionTime = 0;
        beingHurt_time = 0;
        invincible = false;
        //TeamHp.checkGameOver = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && animation_type == 2)
        {
            if (weapon.ani_type != 0) return;
            Debug.Log("atttttack waaaaaaay" + face_way + "   " + gameObject.name);
            Debug.Log("hit enemy");
            collision.transform.GetComponent<CEnemy>().SetHurtValue(weapon.attack, face_way);
            //GameObject.Find("MonsterAudio").GetComponent<MonsterVoice>().SetAudio(0 ,1f);
            if (test)tutorialRequest.DoneHitEnemy();
            //collision.transform.GetComponent<CEnemy>().SetState(4, true);
            //attackDetect();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") {
            //Debug.Log("hit enemy");
            //attackDetect();
        }
    }

}
