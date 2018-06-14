using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChildProjectSystem : MonoBehaviour {
    int freeNum;
    Transform FreeList, UsedList;
    public enum typeForm {
        banana = 0,
        thunder = 1,
        web = 2,
        mouseTape = 3,
        poison = 4,
    }

    public typeForm type;
    public Vector3 offset;
    public GameObject player;
    // Use this for initialization
    void Awake () {
        FreeList = transform.GetChild(0);
        UsedList = transform.GetChild(1);
        freeNum = FreeList.childCount;
        for (int i =0; i<freeNum; i++) {
            Transform temp = FreeList.GetChild(i);
            temp.GetComponent<CChildProject>().system = this;
            temp.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddFree(Transform temp) {
        temp.parent = FreeList;
        temp.gameObject.SetActive(false);
        freeNum++;
    }
    public void AddUsed( Vector3 position) {
        if (freeNum <= 0) return;
        Transform temp = FreeList.GetChild(0);
        temp.gameObject.SetActive(true);
        temp.parent = UsedList;
        temp.position = position;
        temp.GetComponent<CChildProject>().SetOn();
        freeNum--;
    }


    public CChildProject GetNewestChild() {
        return UsedList.GetChild(UsedList.childCount - 1).GetComponent<CChildProject>();
    }

    public Transform GetUsedChildTransform(int id) {
        return UsedList.GetChild(id);
    }

    public void RecycleAllChild() {
        int tempNum = UsedList.childCount;
        for (int i = 0; i < tempNum; i++) {
            Transform temp = UsedList.GetChild(0);
            temp.GetComponent<CChildProject>().ResetChild();
            temp.parent = FreeList;
            temp.gameObject.SetActive(false);
            freeNum++;
        }
    }

    public int GetFreeNum() {
        return freeNum;
    }

}
