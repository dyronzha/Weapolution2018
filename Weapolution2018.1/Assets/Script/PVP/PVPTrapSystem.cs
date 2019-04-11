using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPTrapSystem : MonoBehaviour {

    public CChildProjectSystem traps;
    PlayerControl playerControl;

    // Use this for initialization
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
