using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CChildProject : MonoBehaviour {

    //public bool bananaFly;
    public CChildProjectSystem system;
    CChildProject ene;

    protected Action callBack;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public virtual void SetOn() {
    }

    public virtual void SetOn(float _float)
    {
    }

    public virtual void SetOn(Transform _parent, float _float)
    {
    }

    public virtual void SetOn(bool _bool, Action _callBack) {

    }

    public virtual void ResetChild() {

    }

}
