using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundData : MonoBehaviour
{
    
    public static SoundData instance;

    public bool onoffData;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundData");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    public void Update()
    {
        onoffData = SoundManager.instance.isSoundOn;
    }

}

