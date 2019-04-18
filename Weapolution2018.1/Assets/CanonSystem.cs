using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonSystem : MonoBehaviour {

    GameObject RightCanon;
    public GameObject RightAim;
    bool useControler, showUp, hasFillAni;
    string whichPlayer = "p1";
    int CanonNum;
    float p1_L_JoyX;
    float p1_L_JoyY;
    float p2_L_JoyX;
    float p2_L_JoyY;
    Canon CanonScript;
    Animator CanonAnimator;
    public List<Vector3> DefaultAimPos;
    public List<Vector2> Rayway;
    bool ShowRightAim = false, ShowLeftAim = false;
    float speed = 10f;
    float RayDistant = 1f;
    LayerMask unWalkable;
    RaycastHit2D hitWall1;
    RaycastHit2D hitWall2;
    RaycastHit2D hitWall3;
    RaycastHit2D hitWall4;
    GameObject Explosion;
    public List<bool> IsHitted;

    CharacterVoice characterVoice;
    CEnemySystem enemySystem;

    private void Awake() 
    {
        RightAim.SetActive(false);
        CanonScript = transform.Find("Canon").GetComponent<Canon>();
        CanonAnimator = GameObject.Find("CanonSystem").GetComponent<Animator>();
        RightCanon = GameObject.Find("Canon");
        Explosion = GameObject.Find("Explosion");
        Explosion.SetActive(false);
        Rayway[0] = new Vector2(0, 1f); //上下左右
        Rayway[1] = new Vector2(0, -1f);
        Rayway[2] = new Vector2(-1f, 0);
        Rayway[3] = new Vector2(1f, 0);

        characterVoice = GameObject.Find("CharacterAudio").GetComponent<CharacterVoice>();
        enemySystem = GameObject.Find("EnemySystem").GetComponent<CEnemySystem>();

    }
    private void Start()
    {
        if (Player.p2charaType)
        {
            if (Player.p2controller)
            {
                useControler = true;
                whichPlayer = Player.p2joystick;
            }
        }
        else {
            if (Player.p1controller)
            {
                useControler = true;
                whichPlayer = Player.p1joystick;
            }
        } 
        DefaultAimPos[0] = new Vector3(13f, -6f, 0);
        DefaultAimPos[1] = new Vector3(-13f, -6f, 0);
        unWalkable = 1 << LayerMask.NameToLayer("Obstacle") |
                      1 << LayerMask.NameToLayer("ObstacleForIn")|
                       1 << LayerMask.NameToLayer("ObstacleForOut");
    }
    void Update()
    {
        if (!showUp)
        {
            if (enemySystem.GetDeathNumber() >= 4)
            {
                showUp = true;
                CanonAnimator.SetBool("CanonDisable", false);

            }
        }
        else {
            
            //LeftListener();
            //OutOfBullet();
            if (CanonScript.CanonisfillingPowder)
            {
                CanonAnimtion();
                if (ShowRightAim)
                {
                    if (useControler)
                    {
                        if (Input.GetButtonDown(whichPlayer + "ButtonA")) ShootAndExplosion();
                        if (Input.GetButtonDown(whichPlayer + "ButtonB")) CancelShoot();
                    }
                    else {
                        if (Input.GetKeyDown(KeyCode.Space)) ShootAndExplosion();
                        if (Input.GetKeyDown(KeyCode.Z)) CancelShoot();
                    } 
                    RaycastHitWall();
                    AimControl(RightAim);                   
                }
                else {
                    if (CanonScript.CanonTriigerIN) {
                        if (useControler) {
                            if (Input.GetButtonDown(whichPlayer + "ButtonA")) {
                                ReadyToShoot(true);
                                CanonScript.CraftCantFunc(true);
                            } 
                        }
                        else {
                            if (Input.GetKeyDown(KeyCode.Space)) {
                                ReadyToShoot(true);
                                CanonScript.CraftCantFunc(true);
                            } 
                        }
                    }
                }
            }
        }
    }

    void CanonAnimtion()
    {
        if (!hasFillAni) {
            hasFillAni = true;
            CanonAnimator.SetBool("HavePowder", true);
        } 
        //Debug.Log(CanonScript + "   " + CanonAnimator);
        //if (CanonScript.CanonPowderNum == 0) CanonAnimator.SetBool("HavePowder", false);
        //else CanonAnimator.SetBool("HavePowder", true);

    } 
    void ReadyToShoot(bool isRightCanon)
    {
        ShowAim(isRightCanon);
        if (Player.p2charaType) Player.p2moveAble = false;
        else Player.p1moveAble = false;
    }
    void ShootAndExplosion()
    {
        Explosion.SetActive(true);
        Explosion.transform.position = RightAim.transform.position;
        CanonAnimator.SetTrigger("Shoot");
        CanonScript.CanonPowderNum--;
        characterVoice.SetAudio(5);
        OutOfBullet();
    }

    public void ExplosionOver() {
        Explosion.SetActive(false);
    }

    void OutOfBullet()
    {
        if (CanonScript.CanonPowderNum == 0)
        {
            CanonScript.OutOfPowder();
            hasFillAni = false;
            CanonAnimator.SetBool("HavePowder", false);
            CancelShoot();
        }
    }
    void CancelShoot()
    {
        RightAim.SetActive(false);
        ShowRightAim = false;
        CanonScript.CraftCantFunc(false);
        if (Player.p2charaType) Player.p2moveAble = true;
        else Player.p1moveAble = true;     
    }
    void ShowAim(bool isRightCanon)
    {
        RightAim.SetActive(true);
        RightAim.transform.position = DefaultAimPos[0];
        ShowRightAim = true;
    }
    void LeftListener()
    {
        p1_L_JoyX = Input.GetAxis("p1LHorizontal");
        p1_L_JoyY = Input.GetAxis("p1LVertical");
        p2_L_JoyX = Input.GetAxis("p2LHorizontal");
        p2_L_JoyY = Input.GetAxis("p2LVertical");
    }

    void AimControl(GameObject whichAim)
    {
        float speedX = 0, speedY = 0;

        if (useControler)
        {

            speedX = Input.GetAxis(whichPlayer + "LHorizontal");
            speedY = Input.GetAxis(whichPlayer + "LVertical");

            if (speedX > 0.1f) speedX = 1.0f;
            else if (speedX < -0.1f) speedX = -1.0f; 
            if (speedY > 0.1f) speedY = 1.0f;
            else if (speedY < -0.1f) speedY = -1.0f;
            //if (whichPlayer == "p1")
            //{
            //    if (Mathf.Abs(p1_L_JoyY) > 0.1f) speedY = Mathf.Sign(p1_L_JoyY);
            //    if (Mathf.Abs(p1_L_JoyX) > 0.1f) speedX = Mathf.Sign(p1_L_JoyX);

                //}
                //else
                //{
                //    if (Mathf.Abs(p2_L_JoyY) > 0.1f) speedY = Mathf.Sign(p1_L_JoyY);
                //    if (Mathf.Abs(p2_L_JoyX) > 0.1f) speedX = Mathf.Sign(p1_L_JoyX);
                //}
        }
        else {
            if (Input.GetKey(KeyCode.W)) speedY = 1.0f;
            else if (Input.GetKey(KeyCode.S)) speedY = -1.0f;
            if (Input.GetKey(KeyCode.D)) speedX = 1.0f;
            else if (Input.GetKey(KeyCode.A)) speedX = -1.0f;
        }
        if (IsHitted[0] && speedY > 0.0F) speedY = 0.0f;
        if (IsHitted[1] && speedY < 0.0F) speedY = 0.0f;
        if (IsHitted[2] && speedX < 0.0F) speedX = 0.0f;
        if (IsHitted[3] && speedX > 0.0F) speedX = 0.0f;
        whichAim.transform.position += Time.deltaTime * speed * new Vector3(speedX, speedY, 0);
    }

    void RaycastHitWall()
    {

        hitWall1 = Physics2D.Raycast(RightAim.transform.position, Rayway[0],
                                                   RayDistant, unWalkable);
        hitWall2 = Physics2D.Raycast(RightAim.transform.position, Rayway[1],
                            RayDistant, unWalkable);

        hitWall3 = Physics2D.Raycast(RightAim.transform.position, Rayway[2],
                            RayDistant, unWalkable);

        hitWall4 = Physics2D.Raycast(RightAim.transform.position, Rayway[3],
                            RayDistant, unWalkable);
        if (hitWall1.collider != null) IsHitted[0] = true;
        else IsHitted[0] = false;
        if (hitWall2.collider != null) IsHitted[1] = true;
        else IsHitted[1] = false;
        if (hitWall3.collider != null) IsHitted[2] = true;
        else IsHitted[2] = false;
        if (hitWall4.collider != null) IsHitted[3] = true;
        else IsHitted[3] = false;
    }
}
