using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCameraFollow : MonoBehaviour {

    public Transform targetPlayer;
    public Vector2 posLimitX, posLimitY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FollowPlayer();
	}

    void FollowPlayer() {
        //Vector3 selfPos = transform.position;
        Debug.Log(targetPlayer.position);
        float posX = 0.0f, posY = 0.0f;
        float targetPoxX = targetPlayer.position.x;
        float targetPosY = targetPlayer.position.y;
        //float posX = Mathf.Clamp(posLimitX.x, posLimitX.y, targetPlayer.position.x);
        //float posY = Mathf.Clamp(posLimitY.x, posLimitY.y, targetPlayer.position.y);
        if (targetPoxX < posLimitX.x) posX = posLimitX.x;
        else if (targetPoxX > posLimitX.y) posX = posLimitX.y;
        else posX = targetPoxX;

        if (targetPosY < posLimitY.x) posY = posLimitY.x;
        else if (targetPosY > posLimitY.y) posY = posLimitY.y;
        else posY = targetPosY;

        transform.position = new Vector3(posX,posY, -200.0f);
    }

}
