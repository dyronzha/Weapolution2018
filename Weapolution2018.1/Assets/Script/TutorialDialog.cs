using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class TutorialDialog : MonoBehaviour {
    bool aniFirst = true, changeRequest;
    int dialogLine = 1, needNumber = 0;
    float aniFrameTime = 0.0f, requestTime, requestFadeTime;
    Vector3 lastRequestPos, currentRequestPos;
    Text Tutorialtext, craftRequestNumber, attackerRequestNumber;
    TextAsset dataText;
    List<string[]> data;
    Image BKImage, requestImage, requestImageLast, crafterConfirm, attackerConfirm;
    CPickItemSystem pickItemSystem;
    CEnemySystem enemySystem;

    StageManager stageManager;

    public int progress;
    public bool attackComplete = false, craftComplete = false;
    public List<Sprite> allRequestImage_0, allRequestImage_1;
    public TutorialRequest tutorialRequest;

    // Use this for initialization

    private void Awake()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Start() {
        Tutorialtext = transform.GetChild(1).GetComponent<Text>();
        dataText = (TextAsset)Resources.Load("TextCsv/TutorialDialog");
        string[] st = dataText.text.Split('*');
        data = new List<string[]>();
        for (int i = 0; i < st.Length; i++)
        {
            data.Add(st[i].Split(','));
        }
        Tutorialtext.text = data[dialogLine][1];
        BKImage = transform.Find("Bk").GetComponent<Image>();
        requestImage = transform.Find("RequestImage").GetComponent<Image>();
        requestImageLast = transform.Find("RequestImageLast").GetComponent<Image>();
        crafterConfirm = transform.Find("CrafterConfirm").GetComponent<Image>();
        attackerConfirm = transform.Find("AttackerConfirm").GetComponent<Image>();
        craftRequestNumber = transform.Find("CrafterRequest").GetComponent<Text>();
        attackerRequestNumber = transform.Find("AttackerRequest").GetComponent<Text>();
        pickItemSystem = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>();
        enemySystem = GameObject.Find("EnemySystem").GetComponent<CEnemySystem>();
        lastRequestPos = requestImageLast.transform.position;
        progress = -1;
        currentRequestPos = requestImageLast.transform.position;
        lastRequestPos = requestImage.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (StageManager.timeUp) return;
        GetInput();
        OnSpawnNeed();
        if (progress >= 0) TutorialDialogAnimate();
        if (Input.GetKeyDown(KeyCode.Z)) SkipRequest();
    }
    void OnNext() {
        //Debug.Log("on next" + progress);
        if (progress < 0)
        {
            //Debug.Log("on next" + data[dialogLine][0] + "  " + dialogLine);
            if (data[dialogLine][0].Contains("@"))
            {
                dialogLine++;
                Tutorialtext.text = data[dialogLine][1];
            }
            else
            {
                if (progress < -1)
                {
                    StageManager.nextStage = 4;
                    stageManager.ChangeSceneBlackOut();
                }
                else {
                    progress++;
                    //BKImage.enabled = false;
                    Tutorialtext.enabled = false;
                    requestImage.enabled = true;
                    StopCoroutine(RequestImgFade());
                    StartCoroutine(RequestImgFade());
                }
                
            }
        }
    }

    void OnNextTutorial() {
        if (progress >= 0) {
            progress++;
            if (progress < 6)
            {
                attackerConfirm.enabled = false;
                crafterConfirm.enabled = false;
                attackComplete = false;
                craftComplete = false;
                aniFrameTime = 1.0f;
                aniFirst = true;
                ProgressMission();
                CompleteTime(true, 0);
                CompleteTime(false, 0);
                requestTime = 0.0f;
            }
            else
            {
                progress = -2;
                requestImage.enabled = false;
                attackerConfirm.enabled = false;
                crafterConfirm.enabled = false;
                attackerRequestNumber.enabled = false;
                craftRequestNumber.enabled = false;
                //BKImage.enabled = true;
                Tutorialtext.enabled = true;
                dialogLine++;
                Tutorialtext.text = data[dialogLine][1];
            }
            StopCoroutine(RequestImgFade());
            StartCoroutine(RequestImgFade());
        }
    }

    void ProgressMission() {
        if (progress == 1)
        {
            pickItemSystem.SpawnInUsed(new Vector3(-14.0f, 3.0f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(4.3f, 2.5f, 0.0f), 5);
        }
        else if (progress == 2)
        {
            pickItemSystem.SpawnInUsed(new Vector3(-14.0f, 3.0f, 0.0f), 1);
            //pickItemSystem.SpawnInUsed(new Vector3(-15.6f, -1.0f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(-12.0f, -8.0f, 0.0f), 2);
            craftRequestNumber.enabled = true;
            attackerRequestNumber.enabled = true;
            needNumber = 2;
        }
        else if (progress == 3)
        {
            pickItemSystem.SpawnInUsed(new Vector3(-14.0f, 3.0f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(15.0f, 2.0f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(16.0f, -7.5f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(-11.0f, 5.5f, 0.0f), 1);
            pickItemSystem.SpawnInUsed(new Vector3(-12.0f, -8.0f, 0.0f), 2);
            needNumber = 2;
        }
        else if (progress == 4)
        {
            pickItemSystem.SpawnPickCollect(new Vector3(-13.0f, -9.0f, 0.0f), 0);
            pickItemSystem.SpawnPickCollect(new Vector3(14.5f, -3.0f, 0.0f), 0);
            pickItemSystem.SpawnPickCollect(new Vector3(-12.7f, 2.3f, 0.0f), 2);
            needNumber = 3;
            enemySystem.AddUsedList(new Vector3(0.0f, 10.3f, 0.0f));
        }
        else if (progress == 5) {
            needNumber = 2;
        }
    }

    void OnSpawnNeed() {

        if (progress == 3)
        {
            requestTime += Time.deltaTime;
            if (!craftComplete && requestTime > 13.0f) {
                Vector2 loc = new Vector2(-14.0f, 3.0f);
                Collider2D temp = Physics2D.OverlapCircle(loc, 0.1f);
                if (temp == null) {
                    pickItemSystem.SpawnInUsed(new Vector3(-14.0f, 3.0f, 0.0f), 1);
                    requestTime = 8.0f;
                }

            }
        }
        else if (progress == 5) {
            requestTime += Time.deltaTime;
            if (!craftComplete && requestTime > 8.0f)
            {
                Vector2 loc = new Vector2(-14.0f, -1.0f);
                Collider2D temp = Physics2D.OverlapCircle(loc, -1.0f);
                if (temp == null)
                {
                    pickItemSystem.SpawnInUsed(new Vector3(-14.0f, -1.0f, 0.0f), 1);
                    requestTime = 8.0f;
                }

            }
        }
    }


    void GetInput() {
        if ((!Player.p1controller || !Player.p2controller) && Input.GetKeyDown(KeyCode.Space))
        {
            OnNext();
        }
        if (Player.p1controller)
        {
            if (Input.GetButtonDown(Player.p1joystick + "ButtonA"))
            {
                OnNext();
            }
        }
        if (Player.p2controller)
        {
            if (Input.GetButtonDown(Player.p2joystick + "ButtonA"))
            {
                OnNext();
            }
        }
    }

    void SkipRequest() {
        if (progress > -2) {
            attackComplete = true;
            attackerConfirm.enabled = true;
            craftComplete = true;
            crafterConfirm.enabled = true;
            OnNextTutorial();
        }
        
    }

    public void AttackCompleteRequest() {
        attackComplete = true;
        attackerConfirm.enabled = true;
        if (craftComplete) OnNextTutorial();
    }
    public void CraftCompleteRequest()
    {
        craftComplete = true;
        crafterConfirm.enabled = true;
        if (attackComplete) OnNextTutorial();
    }
    public void CompleteTime(bool _isAttacker, int _times) {

        if (_isAttacker)
        {
            if (attackComplete) return;
            attackerRequestNumber.text = _times.ToString() + "/" + needNumber.ToString();
        }
        else {
            if (craftComplete) return;
            craftRequestNumber.text = _times.ToString() + "/" + needNumber.ToString();
        }
    }

    void TutorialDialogAnimate() {
        if (aniFrameTime > 0.7f) {
            if (aniFirst) requestImage.sprite = allRequestImage_1[progress];
            else requestImage.sprite = allRequestImage_0[progress];
            aniFrameTime = 0.0f;
            aniFirst = !aniFirst;
        }
        aniFrameTime += Time.deltaTime;
    }


    IEnumerator RequestImgFade() {
        changeRequest = true;
        if (progress > 0) {
            requestImageLast.enabled = true;
            requestImageLast.transform.position = currentRequestPos;
            requestImageLast.sprite = allRequestImage_0[progress - 1];
            requestImageLast.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            requestImageLast.CrossFadeAlpha(0.0f, 0.5f, true);
        }
        if (progress < 6) {
            requestImage.transform.position = lastRequestPos;
            requestImage.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            requestImage.CrossFadeAlpha(1.0f, 0.5f, true);
        }
 
        Vector3 downPos = currentRequestPos - new Vector3(0,lastRequestPos.y - currentRequestPos.y,0);
        while (changeRequest) {
            requestFadeTime += Time.deltaTime * 2.0f;
            if (progress > 0)
                requestImageLast.transform.position = Vector3.Lerp(currentRequestPos, downPos, requestFadeTime);
            if (progress < 6)
                requestImage.transform.position = Vector3.Lerp(lastRequestPos, currentRequestPos, requestFadeTime);
            if (requestFadeTime >= 1.0f)
            {
                requestFadeTime = 0.0f;
                changeRequest = false;
                requestImage.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
                requestImageLast.enabled = false;
            }
            yield return null;
        }
        yield return null;
    }



}
