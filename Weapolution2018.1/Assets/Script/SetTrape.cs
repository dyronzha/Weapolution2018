using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrape : MonoBehaviour {
    CChildProjectSystem childProjectSystem;
    // Use this for initialization
    void Awake () {
        childProjectSystem = GameObject.Find("Trapes").GetComponent<CChildProjectSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBuildingTrape()
    {
        if (childProjectSystem.GetFreeNum() <= 0) return;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
        childProjectSystem.AddUsed(pos);
        childProjectSystem.GetNewestChild().SetOn(false, fakeCallBack);
    }

    public void fakeCallBack() {

    }

}
