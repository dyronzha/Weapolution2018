using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour {

    bool hasMax;
    float sizeRange, unitConversion;
    float minSizeY, minSizeX;
    float playerInPosX, playerInPosY, playerOutPosX, playerOutPosY;
    Camera mainCamera;
    CraftMenu craftMenu;
    public float size, minPosX, maxPosX, minPosY, maxPosY;
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
        playerOutPosY = playerOut.transform.position.y - 1.0f;
        if (playerInPosX > playerOutPosX)
        {
            playerInPosX += 2.0f;
            playerOutPosX-= 2.0f;
        }
        else {
            playerInPosX -= 2.0f;
           playerOutPosX += 2.0f;
        }

        if (playerInPosY > playerOutPosY) playerInPosY += 4.0f;
        else {
            playerOutPosY += 4.0f;
            playerInPosY -= 1.5f;
        } 
    }

    void SetCameraPos() {
        Vector3 pos = new Vector3((playerInPosX + playerOutPosX)*0.5f, (playerInPosY + playerOutPosY)*0.5f,
                                    transform.position.z);

        float _sizeX = mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height);
        if (_sizeX > 22.0f) _sizeX = 22.0f;
        if (pos.x + _sizeX > maxPosX) pos = new Vector3(maxPosX - _sizeX, pos.y, pos.z);
        else if (pos.x - _sizeX < minPosX) pos = new Vector3(minPosX + _sizeX, pos.y, pos.z);

        float _sizeY = mainCamera.orthographicSize;
        if (pos.y + _sizeY > maxPosY) pos = new Vector3(pos.x, maxPosY - _sizeY, pos.z);
        else if (pos.y - _sizeY < minPosY) pos = new Vector3(pos.x, minPosY + _sizeY, pos.z);

        transform.position = Vector3.Lerp(transform.position, pos, 0.1f);


        //if (mainCamera.orthographicSize <= 100.0f)
        //{
        //if (hasMax)
        //{
        //    transform.position = Vector3.Lerp(transform.position, pos, 0.15f);
        //    hasMax = false;
        //}
        //    else transform.position = pos;
        //}
        //else {
        //    hasMax = true;
        //    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 12.7f,0.15f);
        //    pos = new Vector3(0.0f, 1.5f, pos.z);
        //    //transform.position = pos;
        //    transform.position = Vector3.Lerp(transform.position, pos, 0.15f);
        //}
        //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime*5.0f);
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
                                             camSizeX * (float)Screen.height / (float)Screen.width, minSizeY);
        //Debug.Log(mainCamera.orthographicSize / 7.5f);
        //craftMenu.SetSizeOffset(mainCamera.orthographicSize / 7.5f);
    }
}
