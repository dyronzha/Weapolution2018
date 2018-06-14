using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHint : CChildProject
{
    bool init;
    float time, lifeTime;
    LevelHeight levelHieght;
    // Use this for initialization

    private void Awake()
    {
        lifeTime = 1.5f;
        levelHieght = GetComponent<LevelHeight>();
    }

    // Update is called once per frame
    void Update () {
        if (time < lifeTime)
        {
            if (!init)
            {
                init = true;
                levelHieght.SetHeight();
            }
            //time += Time.deltaTime;
        }
        else
        {
            ResetChild();
            system.AddFree(this.transform);
        }
    }
    public override void ResetChild()
    {
        init = false;
        time = 0.0f;
    }
}
