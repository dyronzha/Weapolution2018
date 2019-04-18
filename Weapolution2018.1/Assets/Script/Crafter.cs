using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour {

    int face_way;
    public int last_way; //上一個面向

    public bool test;
    public int character_num;

    float L_JoyX = 0.0f;
    float L_JoyY = 0.0f;
    float K_JoyX = 0.0f;
    float K_JoyY = 0.0f;
    public float Speed = 5.0f;
    int inFuntionTime = 0;
    float gatherStartTime;
    float gatherTime = 1f;

    bool isSticked;

    public Canon CanonScript;

    public Animator animator;
    SpriteRenderer img;

    Web web;

    public Player PlayerScript;

    LayerMask unWalkable;

    CraftSystem craftSystem;

    private void Awake()
    {
        unWalkable = 1 << LayerMask.NameToLayer("Obstacle") |
                      1 << LayerMask.NameToLayer("ObstacleForOut");
        craftSystem = transform.Find("CraftSystem").GetComponent<CraftSystem>();
    }

    void Start () {
        animator = GetComponent<Animator>();
        animator.SetInteger("face_way", 5);
        //animator.SetInteger("animation_state", 0);
        PlayerScript = GameObject.Find("character1").GetComponent<Player>();
        
       
    }
	
	// Update is called once per frame
	void Update () {
        if (StageManager.timeUp) return;
        if (PlayerScript.p2_die) return;
        if (Player.p2charaType)
        {
            Movement(Player.p2moveAble, Player.p2controller, Player.p2joystick);
        }
        else
        {
            Movement(Player.p1moveAble, Player.p1controller, Player.p1joystick);
        }
        
        //CheckHp();
    }

    public void Gameover()
    {
        PlayerScript.p2_die = true;
        if (inFuntionTime == 0)
        {
            animator.SetBool("is_die", true);
            Player.p2moveAble = false;
            inFuntionTime++;
        }

    }
    public void GetHurt()
    {
        if (test) return;
        if (PlayerScript.p2_die || !craftSystem.GetFunc()) return;
        K_JoyX = 0;
        K_JoyY = 0;
        animator.SetTrigger("is_hurt");
        if (Player.p2charaType) Player.p2moveAble = false;
        else Player.p1moveAble = false;
        TeamHp.ChangeHp(false, 0.05f);
        GameObject.Find("MonsterAudio").GetComponent<MonsterVoice>().SetAudio(2,1f);
        GameObject.Find("CharacterAudio").GetComponent<CharacterVoice>().SetAudio(1);
        craftSystem.SetFuncBusy(true);
    }
    public void OverBeHurt()
    {
        if (Player.p2charaType) Player.p2moveAble = true;
        else Player.p1moveAble = true;
        craftSystem.SetFuncBusy(false);
    }
    void BeSticked()
    {
        if (test) return;
        if (Player.p2charaType) Player.p2moveAble = false;
        else Player.p1moveAble = false;
        animator.SetTrigger("is_sticked");
        K_JoyX = 0;
        K_JoyY = 0;
        GameObject.Find("CharacterAudio").GetComponent<CharacterVoice>().SetAudio(1); //受傷音效
    }
    void OverBeingStcik()
    {
        if (Player.p2charaType) Player.p2moveAble = true;
        else Player.p1moveAble = true;
        isSticked = false;
        web.RecycleSelf();
    }

    public void Gathering()
    {
        animator.SetBool ("is_gather",true);
        if (Player.p2charaType) Player.p2moveAble = false;
        else Player.p1moveAble = false;
        if(inFuntionTime == 0)
        {
            gatherStartTime = Time.time;
            inFuntionTime++;
        }
    }

    void OverGathering()
    {
        //animator.SetBool("is_gather", false);        
        inFuntionTime = 0;
        K_JoyX = 0;
        K_JoyY = 0;
        animator.SetBool("is_gather", false);
        if (StageManager.currentStage == 7)
        {
            if (CanonScript.startFilled) {
                CanonScript.startFilled = false;
                CanonScript.OverFinlling();
                CanonScript.CallCraftSystemFucion();
            }
        }
            
        if (Player.p2charaType) Player.p2moveAble = true;
        else Player.p1moveAble = true;

    }

    void Movement(bool move, bool ctrlmode, string Joystick_num)
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
                        K_JoyY = -1 * Speed;

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

                if (Mathf.Abs(L_JoyX) <= 0.1f && Mathf.Abs(L_JoyY) <= 0.1f) //idle
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

    void DetectWall(bool isKey)
    {
        float speedX = 0, speedY = 0;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D hitWall0 = Physics2D.Raycast(pos, new Vector3(0, 1, 0),
                                        1.1f, unWalkable);
        RaycastHit2D hitWall1 = Physics2D.Raycast(pos, new Vector3(0, -1, 0),
                                        0.15f, unWalkable);
        RaycastHit2D hitWall2 = Physics2D.Raycast(pos, new Vector3(-1, 0, 0),
                                        0.5f, unWalkable);
        RaycastHit2D hitWall3 = Physics2D.Raycast(pos, new Vector3(1, 0, 0),
                                        0.5f, unWalkable);

        if (isKey)
        {
            speedX = K_JoyX;
            speedY = K_JoyY;
            if (hitWall0 && K_JoyY > 0.0f) K_JoyY = 0.0f;
            if (hitWall1 && K_JoyY < 0.0f) K_JoyY = 0.0f;
            if (hitWall2 && K_JoyX < 0.0f) K_JoyX = 0.0f;
            if (hitWall3 && K_JoyX > 0.0f) K_JoyX = 0.0f;
        }
        else
        {
            speedX = L_JoyX;
            speedY = L_JoyY;
            if (hitWall0 && L_JoyY > 0.0f) L_JoyY = 0.0f;
            if (hitWall1 && L_JoyY < 0.0f) L_JoyY = 0.0f;
            if (hitWall2 && L_JoyX < 0.0f) L_JoyX = 0.0f;
            if (hitWall3 && L_JoyX > 0.0f) L_JoyX = 0.0f;
        }



    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Web" && !isSticked && craftSystem.GetFunc())
        {
            Debug.Log("stickkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
            web = collision.GetComponent<Web>();
            isSticked = true;
            BeSticked();
        }
    }
}
