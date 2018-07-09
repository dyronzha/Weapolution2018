using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectTrap : MonoBehaviour {

    CEnemy enemy;

	// Use this for initialization
	void Awake () {
        enemy = transform.parent.GetComponent<CEnemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            enemy.OnTriggerTrap();
            collision.GetComponent<Trape>().ResetChild();
        }
    }

}
