using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonManager : MonoBehaviour {
    MapInfo mapInfo;
    CPickItemSystem pickItemSystem;
    public List<Poison> freePoisons, usedPoisons;

    public int freeNum;
    // Use this for initialization
    void Awake () {
        pickItemSystem = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>();
        mapInfo = GameObject.Find("map").GetComponent<MapInfo>();
        freePoisons = new List<Poison>();
        usedPoisons = new List<Poison>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);
            freePoisons.Add(temp.GetComponent<Poison>());
            freePoisons[freePoisons.Count - 1].system = this;
            temp.gameObject.SetActive(false);
            freeNum++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPosition() {

    }
    public void AddUsedPosition(Vector3 pos, Vector3 _targetPos) {
        usedPoisons.Add(freePoisons[0]);
        freePoisons[0].transform.position = pos;
        freePoisons[0].gameObject.SetActive(true);
        freePoisons[0].Init(_targetPos);
        freePoisons.RemoveAt(0);
        freeNum--;
    }

    public void RecycleFree(Poison poison) {
        Vector3 posOffset = new Vector3(poison.transform.position.x, poison.transform.position.y - 0.8f, poison.transform.position.z);
        freePoisons.Add(poison);
        usedPoisons.Remove(poison);
        if (IsOnHeight3(posOffset) && posOffset.z < -50.0f) {
            CPickItem tempPick= pickItemSystem.SpawnInUsed(posOffset, 3);
            tempPick.SetZBase(-100.0f);
        }
        poison.gameObject.SetActive(false);
        freeNum++;
    }

    bool IsOnHeight3(Vector3 pos) {
        Vector2 tempPos = new Vector2(pos.x, pos.y);
        if (tempPos.x < mapInfo.heightArea[1].width.x || tempPos.x > mapInfo.heightArea[1].width.y ||
           tempPos.y < mapInfo.heightArea[1].height.x || tempPos.y > mapInfo.heightArea[1].height.y)
        {
            return true;
        }
        else return false;
    }

}
