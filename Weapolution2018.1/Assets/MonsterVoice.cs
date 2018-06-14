using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVoice : MonoBehaviour {

    public List<AudioClip> MonsterEffectVoice;
    AudioSource MainVoice;
    // Use this for initialization

    void Start () {
        MainVoice = transform.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {
       
    }
    public void SetAudio(int SetAudioNumber, float SetVolumeValue)
    {
        MainVoice.PlayOneShot(MonsterEffectVoice[SetAudioNumber], SetVolumeValue);

    }
}
