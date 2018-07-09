using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffects : MonoBehaviour {
    bool isAni;
    int aniId;
    float aniFrameTime, aniTotalTime;
    SpriteRenderer render;

    public Sprite[] aniImg;

	// Use this for initialization
	void Awake () {
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        render.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAni) OnHealEffectAni();
	}

    void OnHealEffectAni() {
        aniFrameTime += Time.deltaTime;
        aniTotalTime += Time.deltaTime;
        if (aniFrameTime > 0.08f) {
            aniId++;
            if (aniId >= aniImg.Length) aniId = 0;
            render.sprite = aniImg[aniId];
            aniFrameTime = 0.0f;
        }
        if (aniTotalTime > 1.2f)
        {
            aniTotalTime = 0.0f;
            aniFrameTime = 0.0f;
            aniId = 0;
            isAni = false;
            render.enabled = false;
        }
    }

    public void SetHealEffectAni() {
        aniTotalTime = 0.0f;
        isAni = true;
        render.enabled = true;
    }

}
