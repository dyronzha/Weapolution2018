using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSystem : MonoBehaviour {
    int freeNum;
    public List<EnemyBullet> freeList, usedList;

	// Use this for initialization
	void Awake () {
        freeList = new List<EnemyBullet>();
        usedList = new List<EnemyBullet>();
        for (int i = 0; i < transform.childCount; i++) {
            Transform temp = transform.GetChild(i);
            temp.GetComponent<EnemyBullet>().SetSystem(this);
            freeList.Add(temp.GetComponent<EnemyBullet>());
            temp.gameObject.SetActive(false);
            freeNum++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetFreeNum() {
        return freeNum;
    }

    public void AddFree(EnemyBullet _bullet) {
        //_bullet.ResetChild();
        freeList.Add(_bullet);
        usedList.Remove(_bullet);
        _bullet.gameObject.SetActive(false);
        freeNum++;
    }
    public void AddUsed(Vector3 pos, Vector3 _shootWay) {
        EnemyBullet bullet = freeList[0];
        bullet.gameObject.SetActive(true);
        bullet.transform.position = pos;
        usedList.Add(bullet);
        freeList.RemoveAt(0);
        bullet.SetWay(_shootWay);
        freeNum--;
    }

}
