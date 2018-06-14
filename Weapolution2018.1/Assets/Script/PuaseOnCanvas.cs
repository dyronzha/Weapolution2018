using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuaseOnCanvas : MonoBehaviour {

    int PuaseinFuctionTime = 0;
    int StartinFuctionTime = 0;
    bool isPuase = false;
    public GameObject PuaseCanvasperfab;
    GameObject PuaseCanvas;
    public Transform canvas;
    public AudioSource GameMusic;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        GameMusic = GameObject.Find("map").GetComponent<AudioSource>();
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.P) )
        {
            if (isPuase) OnDisable();
            else OnPuase();          
        }
        StartinFuctionTime = 0;
        PuaseinFuctionTime = 0;
    }

    void OnPuase()
    {
        if (PuaseinFuctionTime == 0 ) {
            Time.timeScale = 0f; //暫停
            PuaseinFuctionTime++;
            isPuase = true;
            Debug.Log("Puase");
            GameMusic.Pause();
            PuaseCanvas = Instantiate(PuaseCanvasperfab, Vector2.zero, Quaternion.identity);
            PuaseCanvas.transform.parent = canvas;
        }          
        
        
    }
    void OnDisable()
    {
        if (StartinFuctionTime == 0)
        {
            Time.timeScale = 1f; //正常
            StartinFuctionTime++;
            isPuase = false;
            GameMusic.UnPause();
            Destroy(PuaseCanvas);
        }
            
        
        
    }

}
