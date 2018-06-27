using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour {

    float sizeRange, unitConversion;
    float minSizeY, minSizeX;
    float playerInPosX, playerInPosY, playerOutPosX, playerOutPosY;
    Camera mainCamera;
    CraftMenu craftMenu;
    public float size;
    public Transform playerIn, playerOut;

	// Use this for initialization
	void Awake () {
        UpdatePos();
        mainCamera = GetComponent<Camera>();
        minSizeY = mainCamera.orthographicSize;
        minSizeX = minSizeY * Screen.width / Screen.height;
        craftMenu = transform.Find("CraftMenu").GetComponent<CraftMenu>();
        SetCameraPos();
        SetCameraSize();
    }
	
	// Update is called once per frame
    private void LateUpdate()
    {
        UpdatePos();
        SetCameraPos();
        SetCameraSize();
    }

    void UpdatePos() {
        playerInPosX = playerIn.transform.position.x;
        playerInPosY = playerIn.transform.position.y;
        playerOutPosX = playerOut.transform.position.x;
        playerOutPosY = playerOut.transform.position.y;
        if (playerInPosY > playerOutPosY) playerInPosY += 3.0f;
        else playerOutPosY += 3.0f;
    }

    void SetCameraPos() {
        Vector3 pos = new Vector3((playerInPosX + playerOutPosX)*0.5f, (playerInPosY + playerOutPosY)*0.5f,
                                    transform.position.z);
        transform.position = pos;
    }

    void SetCameraSize()
    {
        //horizontal size is based on actual screen ratio
        
        //multiplying by 0.5, because the ortographicSize is actually half the height
        float width = (Mathf.Abs(playerInPosX - playerOutPosX) + 3.0f) * 0.5f;
        float height = Mathf.Abs(playerInPosY - playerOutPosY ) * 0.5f;
        //computing the size
        float camSizeX = Mathf.Max(width, minSizeX);
        mainCamera.orthographicSize = Mathf.Max(height,
                                             camSizeX * Screen.height / Screen.width, minSizeY);
        //Debug.Log(mainCamera.orthographicSize / 7.5f);
        craftMenu.SetSizeOffset(mainCamera.orthographicSize / 7.5f);
    }
}
