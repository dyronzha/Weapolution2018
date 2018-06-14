using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerSystem : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<Player>().Speed = speed;
        transform.GetChild(1).GetComponent<Player>().Speed = speed;
        if (Player.p1charaType)
        {
            transform.GetChild(0).transform.position = new Vector3(-25.0f, 0.0f, 0.0f);
            transform.GetChild(1).transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else {
            transform.GetChild(0).transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            transform.GetChild(1).transform.position = new Vector3(-25.0f, 0.0f, 0.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
