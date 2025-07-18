/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    private bool isMusicOn;

    public Image musicImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    public Button musicToggleButton;

    private void Awake()
    {  
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (musicClips.Length > 0)
        {
            PlayRandomMusic();
        }

        LoadMusicPreference();
        SetupMusicButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMusicButton(); // Reassign button when a new scene loads
    }

    private void SetupMusicButton()
    {
        if (musicToggleButton == null)
        {
            GameObject buttonObject = GameObject.FindGameObjectWithTag("MusicToggleButton");
            if (buttonObject != null)
            {
                musicToggleButton = buttonObject.GetComponent<Button>();
                musicImage = buttonObject.GetComponent<Image>();
            }
        }

        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.RemoveAllListeners();
            musicToggleButton.onClick.AddListener(ToggleMusic);
        }

        UpdateMusicUI(); // Update UI based on the current state
    }

    [ContextMenu("change")]
    public void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);
            audioSource.clip = musicClips[randomIndex];
            audioSource.Play();
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            audioSource.Play();
            Debug.Log("Music started.");
        }
        else
        {
            audioSource.Pause();
            Debug.Log("Music paused.");
        }

        UpdateMusicUI();
        SaveMusicPreference();
    }

    private void LoadMusicPreference()
    {
        // Load the saved music state; default to true if no value is found
        isMusicOn = PlayerPrefs.GetInt("MusicState", 1) == 1;

        if (isMusicOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void SaveMusicPreference()
    {
        // Save the music state (1 for on, 0 for off)
        PlayerPrefs.SetInt("MusicState", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateMusicUI()
    {
        if (musicImage != null)
        {
            musicImage.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }

  

}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    private bool isMusicOn = true; // Default to true for every game start

    public Image musicImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    public Button musicToggleButton;

    private void Awake()
    {
        //GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicManager");

        //if (objs.Length > 1)
        //{
        //    Destroy(this.gameObject);
        //}

        //DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {  

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (musicClips.Length > 0)
        {
            PlayRandomMusic();
        }

        SetupMusicButton(); 
    }

    public void Update()
    {
        // Check if music is not playing and trigger PlayRandomMusic
        if (isMusicOn && !audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMusicButton(); // Reassign button when a new scene loads
    }

    private void SetupMusicButton()
    {
        if (musicToggleButton == null)
        {
            GameObject buttonObject = GameObject.FindGameObjectWithTag("MusicToggleButton");
            if (buttonObject != null)
            {
                musicToggleButton = buttonObject.GetComponent<Button>();
                musicImage = buttonObject.GetComponent<Image>();
            }
        } 
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.RemoveAllListeners();
            musicToggleButton.onClick.AddListener(ToggleMusic);
        } 

        UpdateMusicUI(); // Update UI based on the current state 

    }

    [ContextMenu("change")]
    public void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);
            audioSource.clip = musicClips[randomIndex];
            audioSource.Play();
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            audioSource.Play();
            Debug.Log("Music started.");
        }
        else
        {
            audioSource.Pause();
            Debug.Log("Music paused.");
        }

        UpdateMusicUI();
    }

    private void UpdateMusicUI()
    {
        if (musicImage != null)
        {
            musicImage.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
