using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMainStory : MonoBehaviour {

    public MovieTexture movTexture; //電影材質
    private AudioSource movAudio; //影片音軌
    GameObject loadScece;

    bool videoStart;
    int beCalledTime = 0;
    bool videoOver = false;
    float videotime = 43f;
    float videoStartTime = 0;
    StageManager StageManagerScript;
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = movTexture;
        loadScece = GameObject.Find("LoadingScreen");
        StageManagerScript = GameObject.Find("StageManager").GetComponent<StageManager>();
        movTexture.loop = false;
        videoStart = false;
        movAudio = transform.GetComponent<AudioSource>();
        beCalledTime = 0;
    }
    void Update()
    {

        if ( !videoStart && movTexture.isPlaying == false)
        {
            //GetComponent<AudioSource>().Play();
            movTexture.Play();
            movAudio.Play();
            videoStart = true;
            videoStartTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<AudioSource>().Pause();
            movTexture.Pause();
            movAudio.Pause();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movTexture.Stop();
            movAudio.Stop();
        }
        if (Time.time - videoStartTime >= videotime)
        {
            videoOver = true;
        }
        if (videoOver && beCalledTime == 0)
        {
            beCalledTime++;
            StageManager.nextStage = StageManager.currentStage + 1;
            //StageManager.currentStage++;
            StageManager.nextStage = 2;
            StartCoroutine(StageManagerScript.OnChangingScene(1f));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<AudioSource>().Pause();
            movTexture.Pause();
            movAudio.Pause();
            StageManager.nextStage = StageManager.currentStage + 1;
            //StageManager.currentStage++;
            StageManager.nextStage = 2;
            StartCoroutine(StageManagerScript.OnChangingScene(1f));
        }
    }
}
    
    


