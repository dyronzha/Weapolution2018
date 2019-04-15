using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour {
    public static bool timeUp;
    GameObject PauseMenuSystem;
	GameObject BlackScene;
    public List<Image> MenuButton;
    public List<Sprite> Stage01ButtonState;
    public List<Sprite> Stage02ButtonState;
    bool SetReady = false;
    int SelectNum = 0;
    int MaxNum = 3, MinNum = 0;
    int InFuntionTime = 0;
	bool showMenu;
    float clickTime;
    public bool MouseHover = false;
    StageManager StageManagerscript;
    public Image PauseMenuImage;
    List<Sprite> WhichStage;
    public List<Button> PauseMenuButton;
    // Use this for initialization
    void Awake () {
        PauseMenuSystem = GameObject.Find("PauseMenu");
        PauseMenuSystem.SetActive(false);
		StageManagerscript = GameObject.Find ("StageManager").GetComponent<StageManager>();
        BlackScene = GameObject.Find ("PauseBlackScene");
		BlackScene.SetActive(false);
        if (StageManager.currentStage < 7)
            WhichStage = Stage01ButtonState;
        else
            WhichStage = Stage02ButtonState;

    }
    // Update is called once per frame=
    void Update () {
        if (!SetReady)
        {
            SetMenuPic();
        }
        else
        {
            ShowMenu();
            if (showMenu)
            {
                OnControlMenu();
                MouseControl();
                LockOption();
            }
        }
		
	}

    void SetMenuPic()
    {
        PauseMenuImage.sprite = WhichStage[2];
        for (int i = 0; i < 4; i++)
        {
            MenuButton[i].sprite = WhichStage[0];
            PauseMenuButton[i].targetGraphic.GetComponent<Image>().sprite = WhichStage[0];

            SpriteState PauseMenuButtonState = new SpriteState();
            PauseMenuButtonState = PauseMenuButton[i].spriteState;

            PauseMenuButtonState.highlightedSprite = WhichStage[1];
            PauseMenuButtonState.pressedSprite = WhichStage[1];
            PauseMenuButton[i].spriteState = PauseMenuButtonState;
        }
       
        SetReady = true;
    }

    void ShowMenu()
    {

		if (StageManagerscript.inMenuState && !showMenu)
        {
            PauseMenuSystem.SetActive(true);
			BlackScene.SetActive(true);
			showMenu = true;
            //BlackScene.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 100);
        }
        else if(!StageManagerscript.inMenuState && showMenu)
        {
            PauseMenuSystem.SetActive(false);
			BlackScene.SetActive(false);
			showMenu = false;
            SelectNum = 0;
            //BlackScene.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        }

    }


	void MouseControl(){
		if(MouseHover && InFuntionTime == 0){
            for (int i = 0; i < 4; i++)
            {
                MenuButton[i].sprite = WhichStage[0];
            }
            
            SelectNum = 0;
			InFuntionTime++;
		}
		else if(!MouseHover){
			InFuntionTime = 0;
		}
	}

    void OnControlMenu()
    {
        if (Time.time - clickTime > 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {            
                MenuButton[SelectNum].sprite = WhichStage[0]; //idleButton    
                if (SelectNum == MinNum) SelectNum = MaxNum;
                else SelectNum--;
                MenuButton[SelectNum].sprite = WhichStage[1];
                clickTime = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MenuButton[SelectNum].sprite = WhichStage[0];        
                if (SelectNum == MaxNum) SelectNum = MinNum;
                else SelectNum++;
                MenuButton[SelectNum].sprite = WhichStage[1];
                clickTime = Time.time;
            }
        }
    }

	public void ClickOption(int whichButton){ //MouseClick
        //Debug.Log("dfdsfsfsfsf" + whichButton);
		switch(whichButton){
		case 0:
			StageManagerscript.inMenuState = false;
			break;
		case 1:
                StageManager.nextStage = StageManager.currentStage;
                StageManagerscript.ChangeSceneBlackOut();
                break;
		case 2:
                StageManager.nextStage = 0;
                StageManagerscript.ChangeSceneBlackOut();
                break;
		case 3:
                Application.Quit();
                break;
		}
	}

	void LockOption(){ //keyboard
		if(Input.GetKeyDown(KeyCode.Return)){
			switch(SelectNum){
			case 0:
				    StageManagerscript.inMenuState = false;
				    break;
			case 1:
                    StageManager.nextStage = StageManager.currentStage;
                    StageManagerscript.ChangeSceneBlackOut();
                    break;
                case 2:
                    StageManager.nextStage = 0;
                    StageManagerscript.ChangeSceneBlackOut();
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
		}
	}

}
