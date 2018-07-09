using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScene : MonoBehaviour {

    Animator animator;

    StageManager StageManagerScript;

    void Start () {
        animator = GameObject.Find("BlackScene").GetComponent<Animator>();
        StageManagerScript = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    

    void CallChangeSceneBlackOut()
    {
        StageManagerScript.ChangeSceneBlackOut();
    }
    void CallChangeScene()
    {
        Debug.Log("currentsdadasdasda" + StageManager.nextStage);
        StartCoroutine( StageManagerScript.OnChangingScene(0));
    }
    public void ToStageBegin()
    {
        StageManagerScript.stageBegin = true;
        StageManager.timeUp = false;
        //Player.isMapped = false;
    }
}
