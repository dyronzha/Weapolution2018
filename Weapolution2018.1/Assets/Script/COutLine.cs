using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class COutLine : MonoBehaviour {

    public bool isOutLine;
    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake () {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (isOutLine && !once)
        //{
        //    ToggleOutLine(true);
        //    once = true;
        //}
        //else if (!isOutLine && once) {
        //    ToggleOutLine(false);
        //    once = false;
        //} 
        //if (isOutLine) ToggleOutLine(true);
        //else ToggleOutLine(false);
    }

    public void SetOutLine(bool isOn) {
        ToggleOutLine(isOn);
    }

    void ToggleOutLine(bool show)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", show ? 1f : 0);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
