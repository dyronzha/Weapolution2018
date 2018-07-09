using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPickItemSystem : MonoBehaviour {
    //public List<GameObject> free_list = new List<GameObject>();
    //public List<GameObject> used_list = new List<GameObject>();
    int free_num, used_num, locationID = 0;
    
    int totalNum;
    float spawnTime = 0.0f;
    Vector3[] locations;
    CPickCollection fireTree;
    //RestoreHealth restoreHealth;

    public bool test = false;
    public Transform usedCollectList, freeCollectList;
    public List<CPickItem> freePickItemList = new List<CPickItem>(), 
                                    usedPickItemList = new List<CPickItem>();
    public int[] collectItemType;
    public int[] typeCollectNum;
    public int[] typeCollectMaxNum;
    public float[] typeOppunity;
    public string stage;
    public LayerMask mask;


    void Awake () {
        //Debug.Log("adsadasdasdsadsadsad" + (StageManager.nextStage - 3));
        CItemDataBase.SetItemDataBase(stage);
        CItemDataBase.SetSpriteList(stage);
        Transform tempFree = transform.GetChild(0);
        //restoreHealth = GameObject.Find("RestoreHealth").GetComponent<RestoreHealth>();
        //Transform tempUsed = transform.GetChild(1);
        freeCollectList = transform.GetChild(2);
        usedCollectList = transform.GetChild(3);
        used_num = 0;
        free_num = tempFree.childCount;
        
        for (int i = 0; i < free_num; i++) {
            freePickItemList.Add(tempFree.GetChild(i).GetComponent<CPickItem>());
            freePickItemList[i].SetSystem(this);
            freePickItemList[i].gameObject.SetActive(false);
        }

        locations = new Vector3[freeCollectList.childCount];
        for (int i = 0; i < freeCollectList.childCount; i++)
        {
            freeCollectList.GetChild(i).GetComponent<CPickCollection>().pickitem_system = this;
            freeCollectList.GetChild(i).gameObject.SetActive(false);
            locations[i] = freeCollectList.GetChild(i).position;
        }
    }

    private void Start()
    {
        //SpawnInUsed(new Vector3(-7, 0, 0), 5);
        //SpawnInUsed(new Vector3(-10, -6, 0), 5);

        //SpawnInUsed(new Vector3(-7, 3, 0), 6);
        //SpawnInUsed(new Vector3(-5, 3, 0), 7);
        //SpawnInUsed(new Vector3(-4, 3, 0), 8);
        //SpawnInUsed(new Vector3(-2, -6, 0), 9);
        //SpawnInUsed(new Vector3(0, -6, 0), 10);
        //SpawnInUsed(new Vector3(15, -9.8f, 0), 3);

        //SpawnInUsed(new Vector3(-5, 0, 0), 5);
        //SpawnInUsed(new Vector3(-5, 0.7f, 0), 5);
        // SpawnInUsed(new Vector3(-3, 0.3f, 0), 5);
        // SpawnInUsed(new Vector3(-3, 0.7f, 0), 1);
        // SpawnInUsed(new Vector3(-3, 0, 0), 1);
        //if (test) {
        //    SpawnInUsed(new Vector3(-14, -8, 0), 1);
        //    SpawnInUsed(new Vector3(6, 2, 0), 5);
        //} 
        if (!test)
        {
            if (StageManager.currentStage == 4) {
                SpawnPickCollect(locations[0], 0);
                SpawnPickCollect(locations[4], 0);
                SpawnPickCollect(locations[7], 2);
                SpawnPickCollect(locations[5], 2);
            }
            else if (StageManager.currentStage == 5) {
                SpawnPickCollect(locations[0], 0);
                SpawnPickCollect(locations[4], 0);
                SpawnPickCollect(locations[7], 1);
                SpawnPickCollect(locations[5], 1);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyDown(KeyCode.H)) SpawnPickCollectInMap();
        if (test) return;
        //if (Input.GetKeyDown(KeyCode.L)) SetThunderLoc() ;
        
        spawnTime += Time.deltaTime;
        if (spawnTime > 8.0f) {
            spawnTime = 0.0f;
            SpawnPickCollectInMap();
        }
	}

    void SpawnPickCollectInMap() {

        if (totalNum >= locations.Length)return;
        float opturity = Random.Range(0.0f, 1.0f);
        //Debug.Log(tree_num + "   " + rock_num +"   " + mush_num + "   " + locations.Length);

        for (int i = 0; i < typeOppunity.Length; i++) {
            if (opturity <= typeOppunity[i] && CheckPickCollectLoc())
            {
                if (i == 0)
                {
                    if (typeCollectNum[i] < typeCollectMaxNum[i])
                    {
                        SpawnPickCollect(locations[locationID], 0);
                        return;
                    }
                    else opturity = typeOppunity[1];
                }
                else if (i == 1)
                {
                    if (typeCollectNum[i] < typeCollectMaxNum[i]) {
                        SpawnPickCollect(locations[locationID], 1);
                        return;
                    }
                    else opturity = typeOppunity[2];
                }
                else if (i == 2) {
                    if (typeCollectNum[i] < typeCollectMaxNum[i]) {
                        SpawnPickCollect(locations[locationID], 2);
                        return;
                    }
                    else opturity = typeOppunity[0];
                }
                
            }
        }
        //else if (opturity <= 1.0f)
        //{
        //    if (tree_num < 5 && CheckPickCollectLoc()) SpawnPickCollect(locations[locationID], 1, 10);
        //}

    }


    bool CheckPickCollectLoc()
    {
        Collider2D detect;
        locationID = Random.Range(0, locations.Length - 1);
        for (int i = 0; i < locations.Length; i++)
        {
            int tempID = (locationID + i) % (locations.Length);
            detect = Physics2D.OverlapCircle(locations[tempID], 0.4f,mask);
            if (detect == null)
            {
                locationID = tempID;
                Debug.Log("detect null" + tempID);
                return true;
            }
            else Debug.Log("detect true" + locations[i]);
        }
        return false;
    }



    public CPickItem SpawnInUsed(Vector3 pos, int id) {
        
        if (free_num <= 0) return null;
        freePickItemList[0].gameObject.SetActive(true);
        freePickItemList[0].SetPickItem(id);
        freePickItemList[0].SetShadowEnable(true);
        freePickItemList[0].transform.position = pos;
        freePickItemList[0].transform.parent = transform.GetChild(1);
        usedPickItemList.Add(freePickItemList[0]);
        freePickItemList.RemoveAt(0);     
        free_num--;
        used_num++;
        //Debug.Log(usedPickItemList[usedPickItemList.Count - 1].name);
        return usedPickItemList[usedPickItemList.Count - 1];
    }

    //public void RecyleUsedList(GameObject pickItem) {
    //    pickItem.transform.parent = usedPickItemList;
    //}

    public void AddFreeList(CPickItem pickItem) {
        pickItem.transform.position = transform.GetChild(0).position;
        pickItem.transform.parent = transform.GetChild(0);
        freePickItemList.Add(pickItem);
        usedPickItemList.Remove(pickItem);
        free_num++;
        used_num--;
    }

    public void SpawnPickCollect(Vector3 pos, int _type)
    {
        Transform spawned;
        spawned = freeCollectList.GetChild(0);
        spawned.parent = usedCollectList;
        spawned.gameObject.SetActive(true);
        spawned.position = pos;
        spawned.GetComponent<CPickCollection>().InitCollects(_type, collectItemType[_type]);
        for (int i = 0; i < typeCollectNum.Length; i++) {
            if (_type == i) {
                typeCollectNum[i]++;
                break;
            } 
        }
        totalNum++;
    }
    public void RecyclePickCollect(GameObject _pickCollect) {
        _pickCollect.GetComponent<COutLine>().SetOutLine(false);
        _pickCollect.transform.position = freeCollectList.position;
        _pickCollect.transform.parent = freeCollectList;
        for (int i = 0; i < typeCollectNum.Length; i++)
        {
            if (_pickCollect.GetComponent<CPickCollection>().GetCollectType() <= i) {
                typeCollectNum[i]--;
                break;
            }
        }
        totalNum--;
        //_pickCollect.gameObject.SetActive(false);
        
        Debug.Log("recycle" + _pickCollect.GetComponent<CPickCollection>().GetCollectType());
    }

    public Vector3 SetThunderLoc() {
        for (int i = 0; i < usedCollectList.childCount; i++)
        {
            CPickCollection pickCollection = usedCollectList.GetChild(i).GetComponent<CPickCollection>();
            if (pickCollection.CanOnFire() )//&& Random.Range(0.0f, 1.0f) > 0.85f)
            {
                fireTree = pickCollection;
                fireTree.StartFire();
                return pickCollection.transform.position;
            }
        }
        return new Vector3(-60.0f,0.0f,0.0f);
    }

    //public void RestoreHealth(Vector3 _orinigPos) {
    //    restoreHealth.SetRestore(_orinigPos);
    //}

}
