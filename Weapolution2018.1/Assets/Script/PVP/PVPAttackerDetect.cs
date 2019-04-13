using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPAttackerDetect : MonoBehaviour {
    PVPAttacker attacker;
	// Use this for initialization
	void Awake () {
        attacker = transform.parent.GetComponent<PVPAttacker>();
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attacker.HitSomeone(collision);
    }
}
