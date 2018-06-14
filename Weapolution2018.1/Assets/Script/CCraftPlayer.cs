using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCraftPlayer : MonoBehaviour {
    public float f_speed = 15.0f;
    SpriteRenderer img;
    public Sprite[] imgs = new Sprite[4];
    int face_way = 1;
	// Use this for initialization
	void Start () {
        img = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow)) transform.position += new Vector3(0, f_speed * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.DownArrow))transform.position += new Vector3(0, -f_speed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.LeftArrow))transform.position += new Vector3(-f_speed * Time.deltaTime, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow))transform.position += new Vector3(f_speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.W)) face_way = 0;
        else if(Input.GetKeyDown(KeyCode.S)) face_way = 1;

        if (Input.GetKeyDown(KeyCode.A)) face_way = 2;
        else if (Input.GetKeyDown(KeyCode.D)) face_way = 3;

        img.sprite = imgs[face_way];
    }

}
