using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPProjectile : MonoBehaviour {

    SpriteRenderer projectile_img;
    float Speed;
    int flight_way, atkValue, weaponID;
    public List<Sprite> ProjectileSprite;

    // Use this for initializatio
    void Start()
    {
        //Debug.Log("projectile start");
        Speed = 20;
        projectile_img = transform.GetChild(0).GetComponent<SpriteRenderer>();
        flight_way = -1;
        //SetProjectileImg(0);
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        flight(flight_way);

    }
    public void SetProjectileImg(int _flight_way, CItem weapon)
    {
        atkValue = weapon.attack;

        if (weapon.id == 6) //木頭弓箭
            projectile_img.sprite = ProjectileSprite[0];
        else if (weapon.id == 7)
            projectile_img.sprite = ProjectileSprite[1];
        else if (weapon.id == 11)
            projectile_img.sprite = ProjectileSprite[2];


        //Debug.Log("setflight" + _flight_way);
        flight_way = _flight_way;
        switch (flight_way)
        {
            case 0:
                //transform.position = weapon.transform.position;
                transform.rotation = Quaternion.Euler(0, 0, -90);
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

    public float GetATKValue() {
        StartCoroutine(Recycle());
        return atkValue;
    }

    IEnumerator Recycle() {
        yield return null;
        flight_way = -1;
        atkValue = 0;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (flight_way < 0) return;
        //if (collision.collider.tag == "Enemy")
        //{
        //    collision.transform.GetComponent<CEnemy>().SetHurtValue(Player.weapon.attack, flight_way);
        //    flight_way = -1;
        //    gameObject.SetActive(false);
        //}
        //else if (collision.collider.tag == "Wall")
        //{
        //    flight_way = -1;
        //    gameObject.SetActive(false);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (flight_way < 0) return;
        if (collision.tag == "Wall")
        {
            flight_way = -1;
            gameObject.SetActive(false);
        }
    }
}
