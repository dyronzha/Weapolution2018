using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : CChildProject {
    bool fadeOut;
    int aniID, aniNum;
    float aniTime, fadeOutTime, scale;
    SpriteRenderer Img;
    public Sprite[] effectAni;
    // Use this for initialization
    private void Awake()
    {
        //scale = transform.localScale.x;
        Img = transform.Find("Img").GetComponent<SpriteRenderer>();
        aniNum = effectAni.Length;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        aniTime += Time.deltaTime;
        if (aniTime > 0.05f)
        {
            aniID++;
            aniTime = 0.0f;
            if (aniID < aniNum)
            {
                transform.localScale = new Vector3(scale + aniID * 0.2f, scale + aniID * 0.2f, 1);
                Img.sprite = effectAni[aniID];
                if (aniID == aniNum - 2) fadeOut = true;
            }
            else
            {
                transform.localScale = new Vector3(scale, scale, 1);
                aniID = 0;
                fadeOut = false;
                fadeOutTime = 0.0f;
                Img.sprite = effectAni[aniID];
                system.AddFree(transform);
            }

        }
        if (fadeOut) {
            fadeOutTime += Time.deltaTime * 3.0f;
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeOutTime);
            Img.color = new Color(Img.color.r, Img.color.g, Img.color.b, alpha); 
        }
	}

    public override void SetOn(float _scale)
    {
        scale = _scale;
        transform.localScale = new Vector3(scale,scale,scale);
    }

    public override void SetOn(Transform _parent, float _scale)
    {
        scale = _scale;
        transform.localScale = new Vector3(scale, scale, scale);
        transform.parent = _parent;
    }

}
