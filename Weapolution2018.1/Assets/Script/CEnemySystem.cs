using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemySystem : MonoBehaviour {
    bool hasInit;
    int enemyNumber, deathNumber = 0;
    Transform freeEnemyIn, UsedEnemyIn;    
    //一維為左右，二維是可不可用, true為右
    bool[]canAttackLoc = new bool[2] {  true, true };
    public bool test, bossAppear;
    public float spawnLocY;
    //public Vector2 InsidefieldMinLimit, InsidefieldMaxLimit;
    public GameObject Inplayer, Outplayer;
    //MonsterVoice audioSource;
    //public List<Transform> InEnemys, OutEnemys;
    //public GameObject forge;
    CEnemy boss;
    MonsterVoice monsterVoice;
    StageManager stageManager;
    //public CPickItemSystem pickItemSystem;
    
    // Use this for initialization
    void Awake()
    {
        enemyNumber = 0;
        Inplayer = GameObject.Find("character1");
        Outplayer = GameObject.Find("character2");
        freeEnemyIn = transform.GetChild(0).Find("InsideFree");
        UsedEnemyIn = transform.GetChild(0).Find("InsideUsed");
        //audioSource = GameObject.Find("MonsterAudio").GetComponent<MonsterVoice>();
        //pickItemSystem = GameObject.Find("PickItemSystem").GetComponent<CPickItemSystem>();
        monsterVoice = GameObject.Find("MonsterAudio").GetComponent<MonsterVoice>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>(); ;

    }
    private void Start()
    {

        CEnemy enemy;
        //boss獨立給玩家參照
        boss = transform.GetChild(0).GetChild(2).GetComponent<CEnemy>();
        boss.player = Inplayer;
        boss.SetEnemySystem(this);
        //所有敵人分別給內外玩家參照
        for (int i = 0; i < freeEnemyIn.childCount; i++)
        {
            enemy =  freeEnemyIn.GetChild(i).GetComponent<CEnemy>();
           enemy.player = Inplayer;
            enemy.SetEnemySystem(this);
           // if (i == 1) InEnemys[i].gameObject.SetActive(true);
            //InEnemys[i].GetComponent<CEnemy>().attackDetect += Inplayer.OnHurt;
        }
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            enemy = (transform.GetChild(1).GetChild(i)).GetComponent<CEnemy>();
            //OutEnemys.Add(enemy.transform);
            enemy.IsOut = true;
            enemy.player = Outplayer;
            enemy.SetEnemySystem(this);
            //OutEnemys[i].GetComponent<CEnemy>().player = Outplayer.gameObject;
            // if (i == 1) OutEnemys[i].gameObject.SetActive(true);
            //InEnemys[i].GetComponent<CEnemy>().attackDetect += Outplayer.OnHurt;
        }
        if (test) return;
       

    }

    // Update is called once per frame
    void Update()
    {
        if (test) return;
        if (StageManager.timeUp) return;
        if (!hasInit) MonstersInit();

        //if (StageManager.currentStage == 4)
        //{
        //    if (deathNumber >= 3)
        //    {
        //        if (!bossAppear)
        //        {
        //            bossAppear = true;
        //            boss.gameObject.SetActive(true);
        //        }

        //    }
        //    //if (deathNumber <= 1)
        //    //{
        //    //    if (enemyNumber < 3)
        //    //    {
        //    //        AddUsedList(new Vector3(Random.Range(-7.0f, 8.0f), spawnLocY, 0));
        //    //        //AddUsedList(new Vector3(Random.Range(-12.0f, 12.0f), spawnLocY, 0));
        //    //    }
        //    //}
        //    //else if (deathNumber >= 3)
        //    //{
        //    //    if (!bossAppear)
        //    //    {
        //    //        bossAppear = true;
        //    //        boss.gameObject.SetActive(true);
        //    //    }

        //    //}
        //}

        //else if (StageManager.currentStage == 5)
        //{
        //    if (deathNumber <= 1)
        //    {
        //        if (enemyNumber < 2)
        //        {
                  
        //            //AddUsedList(new Vector3(Random.Range(-12.0f, 12.0f), spawnLocY, 0));
                    
        //                AddUsedList(new Vector3(Random.Range(-12.0f, 12.0f), spawnLocY, 0));
        //            //AddUsedList(new Vector3(Random.Range(-7.0f, 8.0f), spawnLocY, 0));
        //        }
        //    }
        //    else if (deathNumber >= 3)
        //    {
        //        if (!bossAppear)
        //        {
        //            bossAppear = true;
        //            boss.gameObject.SetActive(true);
        //        }

        //    }
        //}
        if (Input.GetKeyDown(KeyCode.K)) {
            boss.gameObject.SetActive(true);
            //forge.GetComponent<CForge>().ShowUp();
        } 
    }

    void MonstersInit() {
        hasInit = true;
        if (StageManager.currentStage == 6)
        {
            //AddUsedList(new Vector3(-12.0f, spawnLocY, 0));
            //AddUsedList(new Vector3(0.0f, spawnLocY, 0));
            //AddUsedList(new Vector3(12.0f, spawnLocY, 0));
            AddUsedList(new Vector3(-5.8f, spawnLocY, 0));
            AddUsedList(new Vector3(6.6f, spawnLocY, 0));
            AddUsedList(new Vector3(-0.5f, spawnLocY, 0));
        }
        else if (StageManager.currentStage == 7)
        {
            AddUsedList(new Vector3(-12.0f, spawnLocY, 0));
            //AddUsedList(new Vector3(0.0f, spawnLocY, 0));
            AddUsedList(new Vector3(12.0f, spawnLocY, 0));
        }
    }

    public void  AddFreeList(Transform trans)
    {
        Debug.Log("recycle enemy" + enemyNumber);
        CEnemy temp = trans.GetComponent<CEnemy>();
        float side = temp.whichSide;
        trans.parent = freeEnemyIn;
        trans.gameObject.SetActive(false);
        enemyNumber--;
        //deathNumber++;
        if (StageManager.currentStage <7) {
            if (Mathf.Abs(side) > 0.5f)
            {    //如果回收的敵人有占住一個攻擊位置，把位置給其他人
                if (side > 0.1f) canAttackLoc[1] = true;
                else if (side < -0.1f) canAttackLoc[0] = true;
                for (int i = 0; i < UsedEnemyIn.childCount; i++)
                {
                    temp = UsedEnemyIn.GetChild(i).GetComponent<CEnemy>();
                    if (Mathf.Abs(temp.whichSide) < 0.2f)
                    {
                        Debug.Log("get position");
                        if (canAttackLoc[0])
                        {
                            Debug.Log("left");
                            temp.whichSide = -1.0f;
                            canAttackLoc[0] = false;
                        }
                        else if (canAttackLoc[1])
                        {
                            Debug.Log("right");
                            temp.whichSide = 1.0f;
                            canAttackLoc[1] = false;
                        }
                    }
                }
            }
        }
    }

    public void AddUsedList(Vector3 pos)
    {
        Debug.Log("ememy respawn");
        Transform temp = freeEnemyIn.GetChild(0);
        temp.parent = UsedEnemyIn;
        temp.position = pos;
        temp.gameObject.SetActive(true);
        enemyNumber++;
        if (StageManager.currentStage < 7) {
            if (canAttackLoc[0])
            {
                temp.GetComponent<CEnemy>().whichSide = -1.0f;
                canAttackLoc[0] = false;
            }
            else if (canAttackLoc[1])
            {
                temp.GetComponent<CEnemy>().whichSide = 1.0f;
                canAttackLoc[1] = false;
            }
            else temp.GetComponent<CEnemy>().whichSide = 0.0f;
        }
    }

    public int GetDeathNumber() {
        return deathNumber;
    }
    //public void playSound(int id,float volume) {
    //    audioSource.SetAudio(id, volume);
    //}

    public void PlaySound(int id, float volume) {
        monsterVoice.SetAudio(id, volume);
    }

    public void RespawnEnemy() {
        deathNumber++;
        if (StageManager.currentStage == 6)
        {
            if (deathNumber >= 3)
            {
                if (!bossAppear)
                {
                    bossAppear = true;
                    boss.gameObject.SetActive(true);
                }
            }
            //else {
            //    if (deathNumber <= 0)
            //        AddUsedList(new Vector3(Random.Range(-7.0f, 8.0f), spawnLocY, 0));
            //}
        }

        else if (StageManager.currentStage == 7) {
            if (deathNumber >= 4)
            {
                if (!bossAppear)
                {
                    bossAppear = true;
                    boss.gameObject.SetActive(true);
                }
            }
            else {
                if (deathNumber <= 2)
                    AddUsedList(new Vector3(Random.Range(-12.0f, 12.0f), spawnLocY, 0));
            } 
        }
        
    }

    public void NextStage() {
        StartCoroutine(stageManager.SlowDown(1.5f, true));
    }

}
