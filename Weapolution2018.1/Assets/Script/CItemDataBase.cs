using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CItemDataBase {
    static string path;
    static string jsonString;
    public static CItem[] items;
    //public static CItem2[] items2;
    public GameObject CraftSystem;
    public static Sprite fail_sprite;
    public static List<Sprite> spriteList = new List<Sprite>();
    

    
    static TextAsset dataText;

    public CItemDataBase() {
    }

    public static void SetItemDataBase(string stage)
    {
        dataText = (TextAsset)Resources.Load("TextCsv/CraftDataBase" + stage);//+ StageManager.currentStage
        string[] st = dataText.text.Split('\n');
        Debug.Log("st.Length" + st.Length + "   " + st[0]);
        List<string[]> data = new List<string[]>();
        //Debug.Log("st.Length" + st.Length);
        items = new CItem[st.Length - 2];
        for (int i = 0; i < st.Length - 2; i++)
        {
            
            data.Add(st[i].Split(','));
            //Debug.Log(data[i][0]);
            items[i] = new CItem(data[i][0], data[i][1], data[i][2], data[i][3], data[i][4], data[i][5], data[i][6], data[i][7], data[i][8]);
            //Debug.Log(items[i].name);
        }

    }
    public static void SetSpriteList(string Stage)
    {
        fail_sprite = Resources.Load<Sprite>("image/Stage/fail");
        spriteList = new List<Sprite>();
        for (int id = 0; id < items.Length; id++)
        {
            spriteList.Add(Resources.Load<Sprite>("image/Stage/" + Stage + "/CraftElement/" + items[id].image));
            //Debug.Log(items[id].image);
        }
    }

    //public static void SetItemDataBase2()
    //{
    //    if (items2 == null)
    //    {
    //        CItemDataBase.path = Application.dataPath + "/StreamingAssets/ItemList.json";
    //        jsonString = File.ReadAllText(path);
    //        items2 = JsonHelper.getJsonArray<CItem>(jsonString);
    //        Debug.Log("static items");
    //    }
    //}
}


[System.Serializable]
public class CItem
{
    public string name;
    public int id;
    public int elementID;
    public int craftingID;
    public string image;
    public int attack;
    public int durability;
    public int ani_type;
    public int audio_source;
    public CItem(string _name, string _id, string _elementID, string _craftingID, string _image, string _attack, string _durability, string _aniType, string _audio)
    {
        name = _name;
        id = int.Parse(_id);
        elementID = int.Parse(_elementID);
        craftingID = int.Parse(_craftingID);
        image = _image;
        attack = int.Parse(_attack);
        durability = int.Parse(_durability);
        ani_type = int.Parse(_aniType);
        audio_source = int.Parse(_audio);
    }
}

//public class JsonHelper
//{
//    public static T[] getJsonArray<T>(string json)
//    {
//        //string newJson = "{ \"array\": " + json + "}";
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//        return wrapper.Items;
//    }

//    [System.Serializable]
//    private class Wrapper<T>
//    {
//        public T[] Items;
//    }
//}

//public class CItem2
//{
//    public string name;
//    public int id {
//        get { return id; }
//        set { }
//    }
//    public int craftingID;
//    public string image;
//    public int attack;
//    public int ani_type;
//}


//[System.Serializable]
//public class CItemList {
//    public List<CItem> itemlist;
//}


// public CItemList items = new CItemList();

//void Awake () {
//       path = Application.streamingAssetsPath + "/ItemList.json";
//       jsonString = File.ReadAllText(path);
//       Debug.Log(jsonString);
//       items = JsonHelper.getJsonArray<CItem>(jsonString);
//       Debug.Log(items[3].attack);

//       CraftSystem.GetComponent<CraftSystem>().SetItemList(items);

//JsonUtility.FromJsonOverwrite(jsonString,items);
//Debug.Log(items.itemlist.Count);
//}