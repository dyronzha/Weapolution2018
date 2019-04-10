using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPAttacker : MonoBehaviour {

    PlayerControl playerControl;
    PickWeaponPVP pickWeapon;
    bool isKeyboard;
    string control;

    Animator animator;
    CharacterVoice effectAudio;

    // Use this for initialization
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        pickWeapon = transform.Find("PickWeapon").GetComponent<PickWeaponPVP>();
        animator = transform.parent.GetComponent<Animator>();
    }
    void Start() {
        playerControl.SubAtkFunc(AttackOver);

        control = playerControl.GetControl();
        if (control == "keyboard") isKeyboard = true;
        else isKeyboard = false;
        pickWeapon.SetController(!isKeyboard, control);
    }

    // Update is called once per frame
    void Update() {
        GetInput();
    }

    public void SetVoice(CharacterVoice voice) {
        effectAudio = voice;
    }

    void GetInput() {
        if (isKeyboard)
        {
            if (Input.GetMouseButtonDown(0) && pickWeapon.holdWeapon.ani_type >= 0)
            {
                playerControl.SetAttackState(pickWeapon.holdWeapon.ani_type);
                Attack();
            }
            if (Input.GetMouseButtonDown(1)){
                playerControl.SetDashState();
            }
        }
        else {
            if (Input.GetButtonDown(control + "ButtonA") && pickWeapon.holdWeapon.ani_type >= 0)
            {
                playerControl.SetAttackState(pickWeapon.holdWeapon.ani_type);
                Attack();
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
        if (pickWeapon.UsingWeaponTillBroken())
        {
            //projectile_num = 0;
        }
        Debug.Log("OverAttack");
    }

}
