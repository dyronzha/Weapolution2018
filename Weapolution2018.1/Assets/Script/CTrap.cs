using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrap : MonoBehaviour {
    float disapear = 0.0f;
    public CraftSystem craftSystem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (disapear < 10.0f)
        {
            disapear += Time.deltaTime;
        }
        else {
            Disapear();
            Destroy(this.gameObject);
        }
	}

    public void Disapear() {
        craftSystem.spikeNum--;
    }

}
