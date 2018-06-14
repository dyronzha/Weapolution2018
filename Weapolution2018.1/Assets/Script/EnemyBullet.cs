using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    bool startShoot, isEndAni;
    int endAniID;
    float aniTime;
    
    Vector3 shootWay;
    SpriteRenderer Img;
    EnemyBulletSystem system;

    public float shootSpeed;
    public Sprite[] endAni;

    private void Awake()
    {
        Img = transform.Find("Img").GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (startShoot) {
            transform.position += Time.deltaTime* shootSpeed * shootWay;
        }
        if (isEndAni) {
            aniTime += Time.deltaTime;
            if (aniTime > 0.07f) {
                if (endAniID > 1) {
                    isEndAni = false;
                    aniTime = 0.0f;
                    endAniID = 0;
                    Img.sprite = endAni[endAniID];
                    system.AddFree(this);
                } 
                else {
                    //Debug.Log(endAniID + "     " + endAni.Length);
                    endAniID++;
                    Img.sprite = endAni[endAniID];
                    aniTime = 0.0f;
                }  
            } 
        }
    }

    public void SetSystem(EnemyBulletSystem _system){
        system = _system;
    }

    public void SetWay(Vector3 _shootWay) {
        shootWay = new Vector3(_shootWay.x, _shootWay.y, 0.0f).V3NormalizedtoV2();
        startShoot = true;
    }

    public  void ResetChild()
    {
        startShoot = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEndAni)
        {
            if (collision.tag == "Player")
            {
                //Debug.Log("sadasdasdsadadasdsdsad hit player");
                isEndAni = true;
                endAniID++;
                Img.sprite = endAni[endAniID];
                startShoot = false;
                collision.GetComponent<Player>().GetHurt();

            }
            if (collision.tag == "Wall")
            {
                //Debug.Log("sadasdasdsadadasdsdsad hit wall");
                isEndAni = true;
                endAniID++;
                Img.sprite = endAni[endAniID];
                startShoot = false;
            }
        }
          
    }

}
