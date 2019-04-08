using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {


    public static bool timeUp;
    public static  int currentStage =3, nextStage =4;

	public bool inMenuState;
    public bool stageBegin;

    bool inTransState, stageOver, isWin;
    bool inChanging;
    float slowTime;
    string whichPlayerControl;
    Dialog dialog;
    TeamHp teamHP;
    //GameObject dialog;
    SceneTransRender transRender;
    AudioSource BGM, CharacterSound,MonsterSound;
    Animator animator;
    GameObject BlackScene;
    

    private void Awake()
    {
        inChanging = false;
        //if (stageManager == null) stageManager = this;
        animator = GameObject.Find("BlackScene").GetComponent<Animator>();
        if (currentStage >= 3)
        {
            transRender = Camera.main.GetComponent<SceneTransRender>();
            transRender.stageManager = this;
            BGM = GameObject.Find("map").GetComponent<AudioSource>();
            MonsterSound = GameObject.Find("MonsterAudio").GetComponent<AudioSource>();
            CharacterSound = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
            if (currentStage > 3) {
                dialog = GameObject.Find("Dialog").GetComponent<Dialog>();
                teamHP = GameObject.Find("TeamHp").GetComponent<TeamHp>();
                dialog.gameObject.SetActive(false);
            }
            if (!timeUp) timeUp = true;

        }
 
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) {
        //    nextStage = 0;
        //    Player.isMapped = false;
        //    StartCoroutine(OnChangingScene(0.0f));
        //}
        if (currentStage == 1) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                nextStage = 2;
                StartCoroutine(OnChangingScene(0.0f));
            }
        }
        if (currentStage < 3)  return;
        if (!stageBegin)
        {
            if (!inChanging)
            {

                inChanging = true;
                if (currentStage == 3) animator.Play("BlackIn");
                else transRender.SetIsGoIn(true);
                //ToStageBegin();
            }
        }
         else{

            GetInput();
            //if (Input.GetKeyDown(KeyCode.Space)) SetCurStageOver(true);
        }

	}

    public void ToStageBegin() {
        stageBegin = true;
        timeUp = false;
        //Player.isMapped = false;
    }



    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            //StartCoroutine(SlowDown(0.5f,true));
            if (inMenuState)
            {
                timeUp = false;
                inMenuState = false;
            }
            else
            {
                inMenuState = true;
                timeUp = true;
            }

        }
        if (timeUp)
        {
            if (stageOver)
            {

            }

        }
    }



    public void StartBegining() {
        nextStage = 1;
        StartCoroutine(OnChangingScene(0.0f));
    }

    //public void ChangeSceneBlackOutForMapping()
    //{
    //    inChanging = true;
    //    animator.Play("BlackOutForMapping");
    //}

    public void ChangeSceneBlackOut() {
        inChanging = true;
        animator.Play("BlackOut");
    }

    public void SetCurStageOver(bool _isWin) {
        //if (stageOver) return;
        isWin = _isWin;
        if (isWin) nextStage = currentStage + 1;
        else nextStage = currentStage;
        stageOver = true;
        timeUp = true;
        dialog.gameObject.SetActive(true);
        dialog.SetDialogOn(_isWin, SetToTransState, StartToTrans);
        //transRender.SetTransRenderOn(SceneTransRender.shaderType.glitch);
    }


    public void SetToTransState() {
        //inTransState = true;
        //dialog.gameObject.SetActive(false);
        //teamHP.CloseHpUi();
        transRender.SetIsGoIn(false);
        transRender.SetTransRenderOn(SceneTransRender.shaderType.glitch);
    }


    public void StartToTrans() {
        dialog.gameObject.SetActive(false);
        teamHP.CloseHpUi();
        transRender.SetStartTrans();
        StartCoroutine(OnChangingScene(1.0f));  //白屏後切場景
    }

    public void CloseGame() {
        Application.Quit();
    }

    public IEnumerator SlowDown(float slowTime, bool _isWin) {
        Time.timeScale = 0.2f;
        BGM.pitch = 0.35f;
        MonsterSound.pitch = 0.35f;
        CharacterSound.pitch = 0.35f;
        yield return new WaitForSecondsRealtime(slowTime);
        Time.timeScale = 1.0f;
        BGM.pitch = 1.0f;
        MonsterSound.pitch = 1.0f;
        CharacterSound.pitch = 0.35f;
        yield return null;
        SetCurStageOver(_isWin);
    }

    public IEnumerator OnChangingScene(float time) {
        
        yield return new WaitForSeconds(time);
        
        if (nextStage >= 6) nextStage = 2;
        currentStage = nextStage;
        
        SceneManager.LoadScene(nextStage);
    }

}
