using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SprintAttacks : MonoBehaviour {
    bool reach;
    int aniID;
    float time, sprintTime, sprintTimeOffset;
    Vector2 targetPos;
    Vector3[] sprintWay;
    Transform sprints;
    Transform[] sprint;
    SpriteRenderer[] sprintRender;
    Action<Vector2> callBack;

    public bool startSprint;
    public float speed;
    public Sprite[] AniImg;

	// Use this for initialization
	void Awake () {
        sprints = transform.Find("SprintAttacks");
        sprintRender = new SpriteRenderer[3];
        sprint = new Transform[3];
        sprintWay = new Vector3[3];
        for (int i = 0; i < 3; i++) {
            sprint[i] = sprints.GetChild(i);
            sprintRender[i] = sprint[i].GetComponent<SpriteRenderer>();
        }
        sprints.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (startSprint) {
            for (int i = 0; i < 3; i++)
            {
                if (time > 0.1f) {
                    if (i == 2) {
                        aniID++;
                        time = 0.0f;
                    }
                    if(aniID < 5)sprintRender[i].sprite = AniImg[aniID];
                }
                //if (i == 1) sprint[1].position = Vector3.Lerp(oringinPos, targetPos, sprintTime);
                sprint[i].position += Time.deltaTime * sprintWay[i] * speed;
            }
            //if (aniID >= 5) {
            //    FinishSprint();
            //}
            time += Time.deltaTime;
            //if (!reach) {
            //    Vector2 dstV2 = new Vector2(targetPos.x - sprint[1].position.x, targetPos.y - sprint[1].position.y);
            //    float dst = Vector2.SqrMagnitude(dstV2);
            //    //Debug.Log("fasdsdsdadadadsdsadasdsadsaasdassadadadasd" + dst);
            //    if (dst <= 0.09f)
            //    {
                    
            //        sprintWay[1] = Vector3.zero;
            //        reach = true;
            //        // FinishSprint();
            //    }
            //}
            if (sprintTime < 0.7f)
            {
                sprintTime += Time.deltaTime;
            }
            else {
                sprintTime = 1.0f;
                FinishSprint();
            }
            //if (sprintTime < 1.0f) {
            //    if (sprintTime < 0.1f) {
            //        Vector2 dstV2 = new Vector2(oringinPos.x - targetPos.x, oringinPos.y - targetPos.y);
            //        float dst = Vector2.SqrMagnitude(dstV2);
            //        sprintTimeOffset = 2.0f * (900.0f / dst);
            //        sprintTime = 0.15f;
            //        Debug.Log(dst + "   " + sprintTimeOffset);
            //    }
            //    sprintTime += Time.deltaTime * sprintTimeOffset;
            //} 
            //else {
            //    sprintTime = 1.0f;
            //    FinishSprint();
            //} 
        }
	}

    public void SetStartSprint() {
        startSprint = true;
        sprints.gameObject.SetActive(true);
        sprintTime = 0.0f;
        time = 0.0f;
    }

    public void SetSprintWay(Vector3 _playerPos, Action<Vector2> _callBack) {
        Vector3 selfPos = transform.position;
        Vector3 playerPos = new Vector3(_playerPos.x, _playerPos.y , selfPos.z);
        Vector3 way = (playerPos - selfPos).V3NormalizedtoV2();
        targetPos = new Vector2(playerPos.x - way.x, playerPos.y - way.y);
        sprintWay[1] = way;
        sprintWay[0] = Quaternion.AngleAxis(-25.0f, Vector3.forward) * way;
        sprintWay[2] = Quaternion.AngleAxis(25.0f, Vector3.forward) * way;
        callBack = _callBack;
    }

    void FinishSprint() {
        startSprint = false;
        callBack(new Vector2(targetPos.x, targetPos.y - 1.0f));
        for (int i = 0; i < 3; i++)
        {
            sprint[i].position = sprints.position + new Vector3(0.0f, 2.0f, 0.0f);
        }
        aniID = 0;
        sprints.gameObject.SetActive(false);
        
    }

}
