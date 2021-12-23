using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playAudio(AudioClip ac, float sound)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = ac;
        GetComponent<AudioSource>().volume = sound;
        GetComponent<AudioSource>().Play(0);
    }
    public void stopAudio()
    {
        GetComponent<AudioSource>().Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
