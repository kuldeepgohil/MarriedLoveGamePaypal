using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{  
    public static SoundManager instance; 
    public AudioSource clickAudioPlay;
    public AudioSource discAudioSource;

    public Image soundImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public Button soundToggleButton;
    public bool isSoundOn; 

    public void Awake()
    {
        instance = this;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");

      /*if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);*/

        if (soundToggleButton == null)
        {
            soundToggleButton = GameObject.FindGameObjectWithTag("SoundToggleButton")?.GetComponent<Button>();

            if (soundToggleButton == null)
            {
                soundToggleButton = GetComponentInChildren<Button>();
            } 
        }  

        if (soundToggleButton != null)
        {
            soundToggleButton.onClick.AddListener(ToggleSound);
        }
        else
        {
            Debug.LogError("SoundToggleButton is not assigned or found.");
        }

        SetupMusicButton();  

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMusicButton(); // Reassign button when a new scene loads 
        ApplySoundState();
    }

    private void ApplySoundState()
    {
        // Turn off sound if it's disabled
        if (!isSoundOn)
        {
            clickAudioPlay.Stop();  // Stop any playing audio when sound is off 
            //clickAudioPlay.Stop();
             discAudioSource.Stop(); // If discAudioSource is global, stop it too
        }  

    }
      
    private void SetupMusicButton()
    {
        if (soundToggleButton == null)
        {
            GameObject buttonObject = GameObject.FindGameObjectWithTag("SoundToggleButton");
            if (buttonObject != null)
            {
                soundToggleButton = buttonObject.GetComponent<Button>();
                soundImage = buttonObject.GetComponent<Image>();
            } 
        }
        if (soundToggleButton != null)
        {
            soundToggleButton.onClick.RemoveAllListeners();
            soundToggleButton.onClick.AddListener(ToggleSound);
        }  
        UpdateSoundButtonImage();
        //UpdateMusicUI(); // Update UI based on the current state
    }

    public void Update()
    {
        /*if (Input.GetMouseButtonDown(0) && isSoundOn)
        {
            clickAudioPlay.Play();
        }*/
    }

    public void BtnClickSound()
    {
        if (isSoundOn)
        {
            clickAudioPlay.Play();
        } 
    }

    public void DiscAudioPlay()
    { 
        if (isSoundOn)
        {
            discAudioSource.Play();
        }  
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn; 
        PlayerPrefs.SetInt("SoundState", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSoundButtonImage();  

    }
        
    private void UpdateSoundButtonImage()
    {
        if (isSoundOn)
        {
            soundImage.sprite = soundOnSprite; 
        }
        else
        {
            soundImage.sprite = soundOffSprite; 
        }
    }

}
