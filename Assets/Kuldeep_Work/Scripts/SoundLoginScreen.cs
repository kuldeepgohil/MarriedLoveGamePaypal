using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoginScreen : MonoBehaviour
{
    public static SoundLoginScreen instance;
    public AudioSource clickAudioSource;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // clickAudioSource.Play();
        }   
    } 

    public void PlayBtnSound()
    {
       clickAudioSource.Play();
    }

}
