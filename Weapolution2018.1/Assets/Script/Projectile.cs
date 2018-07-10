using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    SpriteRenderer projectile_img;
    GameObject weapon;
    float Speed;
    int flight_way;
    public List<Sprite> ProjectileSprite;
    public TutorialRequest tutorialRequest;
    public bool test;
    public int StageNum;
    // Use this for initializatio
    void Start () {
        //Debug.Log("projectile start");
        Speed = 20;
        projectile_img = transform.GetChild(0).GetComponent<SpriteRenderer>();
        weapon = GameObject.Find("weaponImg");
        flight_way = -1;
        //SetProjectileImg(0);
        this.gameObject.SetActive(false);
    }

	void Update () {       
        flight(flight_way);


    }
    public void SetProjectileImg(int _flight_way)
    {
        if (StageNum == 1)
        {
            if (Player.weapon.id == 6) //木頭弓箭
                projectile_img.sprite = ProjectileSprite[0];
            else if (Player.weapon.id == 7)
                projectile_img.sprite = ProjectileSprite[1];
            else if (Player.weapon.id == 11)
                projectile_img.sprite = ProjectileSprite[1];

        }
        if (StageNum == 2)
        {
            if (Player.weapon.id == 8) 
                projectile_img.sprite = ProjectileSprite[0];
            else if (Player.weapon.id == 10)
                projectile_img.sprite = ProjectileSprite[1];
        }
        
        //Debug.Log("setflight" + _flight_way);
        flight_way = _flight_way;
        switch (flight_way)
        {
            case 0:
                //transform.position = weapon.transform.position;
                transform.rotation = Quaternion.Euler(0 , 0, -90);
                break;
            case 1:
                //transform.position = weapon.transform.position;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                //transform.position = weapon.transform.position;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 3:
                //transform.position = weapon.transform.position;
                transform.rotation = Quaternion.Euler(0, 0, -180);
                break;
        }
        //if (Player.outOfProjectile)
        //{
        //    GameObject.Find("PickWeapon").GetComponent<CPickWeapon>().ThrowWeapon();
        //    Player.outOfProjectile = false;
        //}
    }

    void flight(int Player_faceWay)
    {
        if (Player_faceWay < 0) return;
        switch (Player_faceWay)
        {
            case 0:               
                transform.position += Time.deltaTime * new Vector3(0, Speed, 0);
                break;
            case 1:
                transform.position -= Time.deltaTime * new Vector3(0, Speed, 0);
                break;
            case 2:
                transform.position -= Time.deltaTime * new Vector3(Speed, 0, 0);
                break;
            case 3:
                transform.position += Time.deltaTime * new Vector3(Speed, 0, 0);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.transform.GetComponent<CEnemy>().SetHurtValue(Player.weapon.attack, flight_way);
            if(test)tutorialRequest.DoneHitEnemy();
            flight_way = -1;
            gameObject.SetActive(false);
        }
        else if (collision.collider.tag == "Wall")
        {
            flight_way = -1;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.transform.GetComponent<CEnemy>().SetHurtValue(Player.weapon.attack, flight_way);
            if (test) tutorialRequest.DoneHitEnemy();
            flight_way = -1;
            gameObject.SetActive(false);
        }
        else if (collision.tag == "Wall")
        {
            flight_way = -1;
            gameObject.SetActive(false);
        }
    }

}
