using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour {

    public Player playerIn, playerOut;
    CEnemy enemyMonkey;
    CEnemyBear enemyBear;

	// Use this for initialization
	void Awake () {
        //enemyMonkey.attackDetect += playerOut.OnHurt;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
