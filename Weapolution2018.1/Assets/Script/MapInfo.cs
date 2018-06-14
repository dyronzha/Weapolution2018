using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {

    [System.Serializable]
    public struct Area
    {
        public Vector2 width;
        public Vector2 height;
    }

    public Area[] heightArea;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
