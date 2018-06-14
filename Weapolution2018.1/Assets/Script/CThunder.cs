using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CThunder : CChildProject {

    bool isOn = false;
    public float AttackRangeX, AttackRangeY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isOn) {
            Debug.Log("thuuuuunder");
            Vector3 playerPos = system.player.transform.position;
            Vector3 selfPos = transform.position;
            if (playerPos.x < selfPos.x + 0.8f && playerPos.x > selfPos.x - 0.8f
                && playerPos.y > selfPos.y && playerPos.y < selfPos.y + 0.65f) {
                system.player.GetComponent<Player>().GetHurt();
            }
        }
	}


    public void TurnOn() {
        isOn = true;
    }
    public void TurnOff() {
        isOn = false;
        system.AddFree(this.transform);
    }

}
