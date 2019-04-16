using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoControl : MonoBehaviour {

    UnityEngine.Video.VideoPlayer videoPlayer;
    Animator animator;

    // Use this for initialization
    void Awake () {
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        animator = GameObject.Find("BlackScene").GetComponent<Animator>();
    }

    private void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StageManager.nextStage = 4;
            animator.Play("BlackOut");
        } 
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Pause();
        StageManager.nextStage = 4;
        animator.Play("BlackOut");
    }
}
