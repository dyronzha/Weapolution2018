using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPickCollection : MonoBehaviour {
    bool ToFire = false;
    int type, itemTypes;
    float fireTime = 0.0f;
    SpriteRenderer img;
    Animator animator;
    LevelHeight levelHieght;
    public float throwSpeed, throwHeight;
    public CPickItemSystem pickitem_system = null;
    public int[] colliderType;
    public bool isOnFire = false, isOnCollect = false;
    public List<Sprite> appearences = new List<Sprite>();
    public BoxCollider2D[] colliders = new BoxCollider2D[2];

	// Use this for initialization
	void Awake () {        
        img = transform.GetChild(0).GetComponent<SpriteRenderer>();
        colliders = transform.GetComponents<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
        levelHieght = GetComponent<LevelHeight>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ToFire) {
            fireTime += Time.deltaTime;
            if (fireTime >= 0.6f) SetFireOn();
        }
	}

    public void InitCollects(int _type, int _itemType) {
        if(StageManager.currentStage ==5)levelHieght.SetHeight();
        img.color = new Color(1,1,1,0);
        type = _type;
        itemTypes = _itemType;
        img.sprite = appearences[type];
        StartCoroutine(ShowUp());
    }

    IEnumerator ShowUp() {
        float showTime = 0.0f;
        while (showTime < 1.0f) {
            img.color = new Color(1,1,1,showTime);
            showTime += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            if (type < colliderType[i])
            {  //以type id大小來區分要用哪個碰撞器
                colliders[i].enabled = true;
                break;
            }
        }
        yield return null;
    }

    //public void SetCollect(int id) {

    //    colliders[id<2 ? 0 : 1].enabled = true;
    //    colliders[id > 2 ? 0 : 1].enabled = false;
    //    type = id;
    //}

    public bool IsHealthCollect() {
        if (CItemDataBase.items[itemTypes].elementID < -20) return true;
        else return false; ;
    }

    public void ThrowItemOut() {
        if (CItemDataBase.items[itemTypes].elementID < -20)
        {
            pickitem_system.RestoreHealth(transform.position);
        }
        else
        {
            int random = Random.Range(2, 4);
            CPickItem tempItem;
            for (int i = 0; i < random; i++)
            {
                tempItem = pickitem_system.SpawnInUsed(transform.position + new Vector3(0, throwHeight, 0), itemTypes);
                if (tempItem == null) break;
                //tempItem = pickitem_system.usedList.GetChild(pickitem_system.usedList.childCount - 1);
                Vector3 throwWay = new Vector3(0, -1.0f, 0);
                float angle = 0.0f;
                if (i <= 0) angle = (Random.Range(0, 10) > 6 ? 1.0f : -1.0f) * Random.Range(5.0f, 90.0f);
                else
                {
                    float offset = (Random.Range(0, 10) > 6 ? 1.0f : -1.0f) * Random.Range(15.0f, 30.0f);
                    angle = (Mathf.Abs(angle += offset) < 90.0f) ? angle : (Mathf.Sign(offset) * 90.0f - offset);
                }
                throwWay = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)) * throwWay;
                tempItem.SetFall(0.5f, throwWay, throwSpeed);
                Debug.Log("throw out   " + throwWay);
            }
        }
        pickitem_system.RecyclePickCollect(this.gameObject);
        ResetTree();
    }

    public int GetCollectType() {
        return type;
    }

    public bool CanOnFire() {
        if (type < 1 && !isOnFire && !isOnCollect) return true;
        else return false;
    }

    public void StartFire() {
        ToFire = true;
        isOnFire = true;
    }

    public void SetFireOn() {
        itemTypes = 3;
        animator.Play("OnFire");
        ToFire = false;
    }

    public void ResetTree()
    {
        if (isOnFire) {
            this.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            fireTime = 0.0f;
            isOnFire = false;
            ToFire = false;
            animator.SetTrigger("endFire");
        }

        for (int i = 0; i<colliders.Length; i++) {
            colliders[i].enabled = false;
        }
        type = 0;
        itemTypes = 0;
        isOnCollect = false;
        gameObject.SetActive(false);
}

}
