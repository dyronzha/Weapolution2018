using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    bool transUp, onMoving;
    float offset, transTime;
    public Vector2 minPos, maxPos, targetMinPos, targetMaxPos;
    public Vector3 oringinPos;
    public Transform target;
	// Use this for initialization
	void Start () {
        transUp = true;
        oringinPos = transform.position;
        offset = (maxPos.y - minPos.y) / (targetMaxPos.y - targetMinPos.y);
	}
	
	// Update is called once per frame
	void Update () {
        OnChangeY();
	}


    void OnChangeY() {
        if (!onMoving)
        {
            if (target.position.y > 1.5f)
            {
                if (transUp) {
                    onMoving = true;
                    oringinPos = transform.position;
                } 
            }
            else if(target.position.y < -1.5f) {
                if (!transUp) {
                    onMoving = true;
                    oringinPos = transform.position;
                } 
            }
        }
        else {
            transTime += Time.deltaTime * 2.0f;
            if (transUp)
            {
                transform.position = Vector3.Lerp(oringinPos, new Vector3(0, maxPos.y, -500), transTime);
            }
            else {
                transform.position = Vector3.Lerp(oringinPos, new Vector3(0, minPos.y, -500), transTime);
            }
            if (transTime >= 1.0f) {
                transTime = 0.0f;
                transUp = !transUp;
                onMoving = false;
            } 
        }
    }

}
