using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : CChildProject
{
    bool init;
    bool damageOnce;
    float lifeTime, time;

    LevelHeight levelHieght;

    private void Awake()
    {
        lifeTime = 15.0f;
        levelHieght = GetComponent<LevelHeight>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (time < lifeTime) {
            if (!init) {
                init = true;
                levelHieght.SetHeight();
                levelHieght.isOnGround();
            }
            time += Time.deltaTime;
        } 
        else
        {
            RecycleSelf();
        }
    }

    public override void ResetChild()
    {
        init = false;
        time = 0.0f;
        damageOnce = false;
    }

    public void RecycleSelf() {
        ResetChild();
        system.AddFree(this.transform);
    }

}
