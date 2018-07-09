using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestoreHealth : MonoBehaviour {

    bool isFirst = true;
    int storeState = -1, whichItem;
    float speed = 5.0f, moveTime = 0.0f, restoreTime = 0.0f, width, height, moveTimeOffset = 2.0f;
    Vector3 oringinPos, targetPos;
    Image[] healthyItemsImage = new Image[2];
    RectTransform[] healthyItems = new RectTransform[2];
    GameObject bloodImg;
    CanvasScaler canvasScaler;

    public Camera mainCamera;

	// Use this for initialization
	void Awake () {
        for (int i = 0; i < 2; i++) {
            healthyItems[i] = transform.GetChild(i).GetComponent<RectTransform>();
            healthyItemsImage[i] = healthyItems[i].GetComponent<Image>();
            healthyItems[i].gameObject.SetActive(false);
        }
        bloodImg = GameObject.Find("bloodImage");
        targetPos = bloodImg.GetComponent<RectTransform>().anchoredPosition;
        canvasScaler = transform.parent.GetComponent<CanvasScaler>();
        width = Screen.width;
        height = Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
        if (storeState >= 0) {
            if (storeState == 0) OnMovingItem();
            else if (storeState == 1) OnRestoreHealthing();
        }
        
	}

    public void SetRestore(Vector3 _oringinPos) {
        storeState = 0;
        if (isFirst) whichItem = 0;
        else whichItem = 1;
        healthyItems[whichItem].gameObject.SetActive(true);
        isFirst = !isFirst;
        
        oringinPos = mainCamera.WorldToScreenPoint(_oringinPos);
        Debug.Log("asdadasdadadasdasdasdasdasd" + oringinPos);
        oringinPos = new Vector3(oringinPos.x - width*0.5f, oringinPos.y - height*0.5f, 0);
        healthyItems[whichItem].anchoredPosition = oringinPos;
    }

    void OnMovingItem() {
        moveTime += Time.deltaTime*moveTimeOffset;
         if(moveTimeOffset > 0.2f)moveTimeOffset -= Time.deltaTime*2.0f;
        healthyItems[whichItem].anchoredPosition = 
                    Vector3.Lerp(oringinPos, targetPos, moveTime);
        if (moveTime >= 1.0f) {
            storeState = 1;
            moveTime = 0.0f;
            moveTimeOffset = 2.0f;
        } 
    }

    void OnRestoreHealthing() {
        
        if (restoreTime < 0.0f) {
            restoreTime += Time.deltaTime;
        }
        else if (restoreTime < 1.0f) {
            restoreTime += Time.deltaTime*2.5f;
            healthyItemsImage[whichItem].color = Color.Lerp(Color.white, new Color(1,1,1,0), restoreTime);
            healthyItems[whichItem].anchoredPosition += new Vector2(0, -80) * Time.deltaTime;
        }
        else {
            healthyItems[whichItem].gameObject.SetActive(false);
            TeamHp.ChangeHp(true, 0.15f);
            restoreTime = 0.0f;
            storeState = -1;
            ResetItem();
        }
        
    }

    void ResetItem() {
        storeState = -1;
        healthyItems[whichItem].gameObject.SetActive(false);
        healthyItemsImage[whichItem].color = Color.white;
        moveTime = 0.0f;
    }

}
