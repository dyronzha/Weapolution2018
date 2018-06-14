using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMusic : MonoBehaviour {

    public List<AudioClip> CharacterVoice;
    AudioSource MainVoice;
    // Use this for initialization
    void Start () {
        MainVoice = transform.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void SetAudio(int SetAudioNumber)
    {
        MainVoice.PlayOneShot(CharacterVoice[SetAudioNumber]);
  
    }

}
