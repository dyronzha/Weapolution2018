using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPDialog : MonoBehaviour {

    bool show = false;
    int dialogLine;
    string[] currentDialog;
    Text content;
    StageManager stageManager;

    static bool hasChoose = false;

    [HideInInspector]
    public bool start = false;

    public string[] dialog;

    // Use this for initialization
    private void Awake()
    {
        content = transform.Find("Content").GetComponent<Text>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

    }
    void Start () {
        currentDialog = dialog;
        if (StageManager.currentStage == 1) {
            content.text = currentDialog[0];
            if (!hasChoose) hasChoose = true;
            else {
                this.gameObject.SetActive(false);
                start = true;
            } 
        } 
        else {
            this.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
	}
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("p1ButtonA")
              || Input.GetButtonDown("p2ButtonA") || Input.GetButtonDown("p3ButtonA") || Input.GetButtonDown("p4ButtonA"))
        {
            if (dialogLine < currentDialog.Length - 1)
            {
                dialogLine++;
                content.text = currentDialog[dialogLine];
            }
            else
            {
                if (StageManager.currentStage == 1)
                {
                    this.gameObject.SetActive(false);
                    start = true;
                }
                else {
                    StageManager.nextStage = 1;
                    stageManager.ChangeSceneBlackOut();
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("p1ButtonB")
              || Input.GetButtonDown("p2ButtonB") || Input.GetButtonDown("p3ButtonB") || Input.GetButtonDown("p4ButtonB")) {
            if (StageManager.currentStage == 1)
            {
                start = true;
                this.gameObject.SetActive(false);
            }
            else
            {
                StageManager.nextStage = 1;
                stageManager.ChangeSceneBlackOut();
            }
        }
    }

    public void SetOn(string team) {
        content.text = team + currentDialog[0];
        gameObject.SetActive(true);
    }
}
