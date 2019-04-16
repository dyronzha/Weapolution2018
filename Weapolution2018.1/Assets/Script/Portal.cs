using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject player;
    private Animator animator;
 
    // Use this for initialization
    void Start () {
        player = GameObject.Find("character2");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "character2" || collision.gameObject.name == "character4") {
            if (gameObject.name == "leftTP")
            {
                collision.transform.position = new Vector3(14.2f, 6, 0);
            }
            if (gameObject.name == "rightTP")
            {
                collision.transform.position = new Vector3(-12.8f, 6, 0);
            }
        }


        //if (collision.gameObject.name == "character2")
        //{
            

        //    if (gameObject.name == "leftTP")
        //    {
        //        player.transform.position = new Vector3(14.2f, 6, 0);              
        //    }
        //    if (gameObject.name == "rightTP")
        //    {
        //        player.transform.position = new Vector3(-12.8f, 6, 0);             
        //    }

        //}
    }
 
}
