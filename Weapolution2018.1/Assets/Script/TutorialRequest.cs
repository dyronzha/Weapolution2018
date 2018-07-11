using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRequest : MonoBehaviour {
    int doneCraftNum, doneAttackNum;
    float craftMoveSec = 0.0f, attackMoveSec = 0.0f;
    TutorialDialog tutorialDialog;
    Player player;
    CraftSystem craftSystem;
    CPickWeapon pickWeapon;
    TrapSystem trapSystem;
    Projectile[] projectile;
	// Use this for initialization
	void Start () {
        tutorialDialog = this.transform.GetComponent<TutorialDialog>();
        player = GameObject.Find("character1").GetComponent<Player>();
        craftSystem = GameObject.Find("CraftSystem").GetComponent<CraftSystem>();
        pickWeapon = GameObject.Find("PickWeapon").GetComponent<CPickWeapon>();
        trapSystem = GameObject.Find("character2").GetComponent<TrapSystem>();
        tutorialDialog.tutorialRequest = this;
        Debug.Log(GameObject.Find("CraftSystem"));
        craftSystem.tutorialRequest = this;
        pickWeapon.tutorialRequest = this;
        player.tutorialRequest = this;
        trapSystem.tutorialRequest = this;
        Transform tempProjectile = GameObject.Find("ProjectileSystem").transform;
        projectile = new Projectile[tempProjectile.childCount];
        for (int i = 0; i<projectile.Length; i++) {
            projectile[i] = tempProjectile.GetChild(i).GetComponent<Projectile>();
            projectile[i].tutorialRequest = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (tutorialDialog.progress == 0) GetInput();  //假的移動輸入，直接去參照player那的移動感覺很浪費
	}

    void GetInput()
    {
        if (!Player.p1controller)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
             DoneMove(Player.p1charaType);
            }
        }
        else {
            if (Input.GetAxis(Player.p1joystick + "LHorizontal") > 0.5f ||
                Input.GetAxis(Player.p1joystick + "LVertical") > 0.5f) {
                DoneMove(Player.p1charaType);
            }
        }

        if (!Player.p2controller)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                DoneMove(Player.p2charaType);
            }
        }
        else
        {
            if (Mathf.Abs(Input.GetAxis(Player.p2joystick + "LHorizontal")) > 0.5f ||
                 Mathf.Abs(Input.GetAxis(Player.p2joystick + "LVertical")) > 0.5f)
            {
                DoneMove(Player.p2charaType);
            }
        }

    }

    public void DoneMove(bool isCrafter)
    {
        if (tutorialDialog.progress != 0) return;
        if (isCrafter && craftMoveSec > -10.0f) craftMoveSec += Time.deltaTime;
        if (!isCrafter && attackMoveSec > -10.0f) attackMoveSec += Time.deltaTime;
        if (attackMoveSec > 1.5f) {
            tutorialDialog.AttackCompleteRequest();
            attackMoveSec = -20.0f;
        }
        if (craftMoveSec > 1.5f) {
            tutorialDialog.CraftCompleteRequest();
            craftMoveSec = -20.0f;
        } 
    }

    public void DonePickUp(bool isCrafter) {
        if (tutorialDialog.progress != 1) return;
        if (isCrafter)
        {
            tutorialDialog.CraftCompleteRequest();
        }
        else {
            tutorialDialog.AttackCompleteRequest();
        } 
    }

    public void DoneAttack()
    {
        if (tutorialDialog.progress != 3) return;
        doneAttackNum++;
        tutorialDialog.CompleteTime(true, doneAttackNum);
        if (doneAttackNum >= 2) {
            tutorialDialog.AttackCompleteRequest();
            doneAttackNum = 0;
        }
        
    }
    public void DoneCraft()
    {
        if (tutorialDialog.progress != 3) return;
        doneCraftNum++;
        tutorialDialog.CompleteTime(false, doneCraftNum);
        if (doneCraftNum >= 2) {
            tutorialDialog.CraftCompleteRequest();
            doneCraftNum = 0;
        } 
    }

    public void DoneRoll() {
        if (tutorialDialog.progress != 2) return;
        doneAttackNum++;
        tutorialDialog.CompleteTime(true, doneAttackNum);
        if (doneAttackNum >= 2) {
            tutorialDialog.AttackCompleteRequest();
            doneAttackNum = 0;
        } 
    }

    public void DoneThrow() {
        if (tutorialDialog.progress != 2) return;
        doneCraftNum++;
        tutorialDialog.CompleteTime(false, doneCraftNum);
        if (doneCraftNum >= 2) {
            tutorialDialog.CraftCompleteRequest();
            doneCraftNum = 0;
        } 
    }
    public void DoneCollect()
    {
        if (tutorialDialog.progress != 4) return;
        doneCraftNum++;
        tutorialDialog.CompleteTime(false, doneCraftNum);
        if (doneCraftNum >= 3) {
            tutorialDialog.CraftCompleteRequest();
            doneCraftNum = 0;
        } 
    }

    public void DoneTrap() {
        if (tutorialDialog.progress != 5) return;
        doneCraftNum++;
        tutorialDialog.CompleteTime(false, doneCraftNum);
        if (doneCraftNum >= 2) {
            tutorialDialog.CraftCompleteRequest();
            doneCraftNum = 0;
        } 
    }

    public void DoneHitEnemy() {
        if (tutorialDialog.progress != 4) return;
        doneAttackNum++;
        tutorialDialog.CompleteTime(true, doneAttackNum);
        if (doneAttackNum >= 3)
        {
            tutorialDialog.AttackCompleteRequest();
            doneAttackNum = 0;
        }
    }

    public void doneThrowWeapon() {
        if (tutorialDialog.progress != 5) return;
        doneAttackNum++;
        tutorialDialog.CompleteTime(true, doneAttackNum);
        if (doneAttackNum >= 2)
        {
            tutorialDialog.AttackCompleteRequest();
            doneAttackNum = 0;
        }
    }

}
