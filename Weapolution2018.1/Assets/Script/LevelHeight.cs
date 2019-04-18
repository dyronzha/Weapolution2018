using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHeight : MonoBehaviour {
    public bool autoMovable, isIn;
    MapInfo.Area[] areaHeight;
    ZArrange zArrange;
	// Use this for initialization
	void Awake () {
        zArrange = GetComponent<ZArrange>();
        areaHeight = GameObject.Find("map").GetComponent<MapInfo>().heightArea;
	}
	
	// Update is called once per frame
	void Update () {
        //if (autoMovable) SetHeight();
	}

    public void SetHeight() {
        Vector2 pos = transform.position;
        if (pos.x >= areaHeight[0].width.x && pos.x <= areaHeight[0].width.y &&
            pos.y >= areaHeight[0].height.x && pos.y <= areaHeight[0].height.y)
        {
            if (pos.x >= areaHeight[1].width.x && pos.x <= areaHeight[1].width.y &&
                pos.y >= areaHeight[1].height.x && pos.y <= areaHeight[1].height.y)
            {
                if (pos.x >= areaHeight[2].width.x && pos.x <= areaHeight[2].width.y &&
                pos.y >= areaHeight[2].height.x && pos.y <= areaHeight[2].height.y)
                {
                    zArrange.f_base = 100.0f;
                }
                else
                {
                    zArrange.f_base = 0.0f;
                }

            }
            else
            {
                zArrange.f_base = -110.0f;
            }
        }
        else zArrange.f_base = -110.0f;
    }

    public float GetHeight() {
        return zArrange.f_base;
    }

    public void isOnGround() {
        zArrange.f_base += 10.0f;
    }

    public void SetSpecificHeight(float _base) {
        zArrange.f_base = _base;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Stair") {
    //        if (!isIn) {
    //            if (collision.transform.position.y < transform.position.y)
    //            {
    //                zArrange.f_base = -110.0f;
    //            }
    //            //else zArrange.f_base = 0.0f; ;
    //        }
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "StairIn" && isIn) {
            if (collision.transform.position.y > transform.position.y)
            {
                zArrange.f_base = 100.0f;
            }
            else zArrange.f_base = 0.0f;
        }

        else if (collision.tag == "StairOut" && !isIn) {
            if (collision.transform.position.y < transform.position.y)
            {
                zArrange.f_base = 0.0f;
            }
            else zArrange.f_base = -110.0f;
        }

        //if ( collision.tag == "Stair")
        //{
        //    if (isIn)
        //    {
        //        if (collision.transform.position.y > transform.position.y)
        //        {
        //            zArrange.f_base = 100.0f;
        //        }
        //        else zArrange.f_base = 0.0f;
        //    }
        //    else {
        //        if (collision.transform.position.y < transform.position.y)
        //        {
        //            zArrange.f_base = 0.0f;
        //        }
        //        else zArrange.f_base = -110.0f;
        //    }
        //}
    }

}
