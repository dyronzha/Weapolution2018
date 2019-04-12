using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPAttacker : MonoBehaviour {

    PlayerControl playerControl;
    PickWeaponPVP pickWeapon;
    bool isKeyboard;
    string control;
    int projectileNum = 0;

    Transform projectileSystem, weapon;

    Animator animator;
    CharacterVoice effectAudio;

    // Use this for initialization
    private void Awake()
    {
        projectileSystem = GameObject.Find("ProjectileSystem").transform;
        weapon = transform.GetChild(1).GetChild(0);
    }
    void Start() {
        

        
    }

    // Update is called once per frame
    void Update() {
        GetInput();
    }

    public void Init(CharacterVoice voice, string con) {
        playerControl = GetComponent<PlayerControl>();
        animator = transform.parent.GetComponent<Animator>();
        effectAudio = voice;

        if (con == "keyboard") isKeyboard = true;
        else isKeyboard = false;

        pickWeapon = transform.Find("PickWeapon").GetComponent<PickWeaponPVP>();
        pickWeapon.SetController(!isKeyboard, control);

        playerControl.SubAtkFunc(AttackOver);
    }

    void GetInput() {
        if (isKeyboard)
        {
            if (Input.GetMouseButtonDown(0) && pickWeapon.holdWeapon.ani_type >= 0)
            {
                if(playerControl.SetAttackState(pickWeapon.holdWeapon.ani_type)) Attack();
            }
            if (Input.GetMouseButtonDown(1)){
                playerControl.SetDashState();
            }
        }
        else {
            if (Input.GetButtonDown(control + "ButtonA") && pickWeapon.holdWeapon.ani_type >= 0)
            {
                if (playerControl.SetAttackState(pickWeapon.holdWeapon.ani_type)) Attack();
            }
            if (Input.GetAxis(control + "LT") >= 0.5f)
            {
                playerControl.SetDashState();
            }
        }
    }
    void Attack()
    {
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

    void AttackOver()
    {
        pickWeapon.UsingWeaponTillBroken();
        Debug.Log("OverAttack");

        //if (pickWeapon.UsingWeaponTillBroken())
        //{
        //    //projectile_num = 0;
        //}

    }

    public void ShootProjectile()
    {
        Transform tempProjectile = projectileSystem.GetChild(projectileNum);
        tempProjectile.position = weapon.position;
        tempProjectile.gameObject.SetActive(true);
        tempProjectile.GetComponent<PVPProjectile>().SetProjectileImg(playerControl.GetFaceDir(), pickWeapon.holdWeapon);
        projectileNum++;
        if (projectileNum >= projectileSystem.transform.childCount)
        {
            projectileNum = 0;
        }
        //if (projectile_num >= weapon.durability) //大於武器耐久
        //{
        //    projectile_num = 0;
        //    outOfProjectile = true;
        //    pickWeaponScript.ThrowWeapon();
        //    //GameObject.Find("PickWeapon").GetComponent<CPickWeapon>().ThrowWeapon();
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtArea" && playerControl.GetState() == PlayerControl.State.attack)
        {
            collision.GetComponent<PlayerControl>().GetHurt(pickWeapon.holdWeapon.attack);
        }
    }

}
