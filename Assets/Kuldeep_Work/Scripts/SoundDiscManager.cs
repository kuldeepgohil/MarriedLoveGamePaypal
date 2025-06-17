using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDiscManager : MonoBehaviour
{
    public static SoundDiscManager instance;
    public AudioSource discAudioSource;  

    public void Awake()
    {
        instance = this;
    } 

    public void DiscAudioPlay()
    {
        if (SoundManager.instance.isSoundOn)
        {
            discAudioSource.Play();
        }
    } 

}
