using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundGameScreen : MonoBehaviour
{ 
    public static SoundGameScreen instance;
    public AudioSource clickAudioSource;
    public AudioSource discAudioSource;
    public bool isSoundGame; 

    public void Awake()
    {
        instance = this;  

    }

    public void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundData");

        bool test = objs[0].gameObject.GetComponent<SoundData>().onoffData;

        isSoundGame = test;

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
        if (isSoundGame)
        {
            clickAudioSource.Play();
        }
    } 

    public void DiscAudioPlay()
    {
        if (isSoundGame)
        {
            discAudioSource.Play();
        }

    }

}
