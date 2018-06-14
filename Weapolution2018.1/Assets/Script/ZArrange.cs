using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ZArrange : MonoBehaviour {
    public float f_base;
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.SetVectorZ(f_base + transform.position.y);
	}
}
