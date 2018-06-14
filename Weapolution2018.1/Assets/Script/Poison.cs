using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour {
    bool setOn, hasreached;
    float time, lifeTime;
    Vector2 targetDropPos;
    Vector3 flyWay;
    Animator animator;

    public float posZ;
    public PoisonManager system;

    // Use this for initialization
    void Awake () {
        lifeTime = 5.0f;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasreached) Drop();
        else OnGround();
	}

    void Drop() {
        Vector2 dst = new Vector2(transform.position.x - targetDropPos.x, transform.position.y - targetDropPos.y);

        if (Vector2.SqrMagnitude(dst) > 0.16f)
        {
            transform.position += flyWay * 20.0f * Time.deltaTime;
        }
        else {
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetTrigger("onGround");
            hasreached = true;
        }
    }

    void OnGround()
    {
        if (time < lifeTime)
        {
            time += Time.deltaTime;
        }
        else {
            hasreached = false;
            time = 0.0f;
            system.RecycleFree(this);
        }
    }

    public void Init(Vector3 _dropPos) {
        targetDropPos = new Vector2(_dropPos.x, _dropPos.y);
        flyWay = new Vector3(_dropPos.x - transform.position.x, _dropPos.y - transform.position.y, 0.0f).V3NormalizedtoV2();
        
        transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(flyWay.x, -flyWay.y) * Mathf.Rad2Deg);
        //setOn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            //Debug.Log("asdasdasdasdasdasd hitttttt");
            if (Mathf.Abs(collision.transform.position.z - transform.position.z) < 5.0f) {
                Player _player = collision.transform.parent.GetComponent<Player>();
                if(_player != null)
                    collision.transform.parent.GetComponent<Player>().GetHurt();
            }
        }
    }
}
