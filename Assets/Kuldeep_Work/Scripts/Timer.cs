using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float startTime = 60f; // Set the start time in seconds
    private float currentTime; 
    public bool isTimerRunning = false;

    public AudioSource buzzerSound;

    public void Awake()
    {
        Instance = this;
    } 

    public void Start()
    {
       // currentTime = startTime;  
    
    } 

    public void SetTime(string activityTimer)
    {
        // Try to parse the string value into a float for the countdown timer
        if (float.TryParse(activityTimer, out float parsedTime))
        {
            startTime = parsedTime; // Set the start time
            currentTime = startTime; // Reset the current time
                                     //isTimerRunning = true;  // Start the timer 
            UpdateTimerDisplay(currentTime);
        }
        else
        {
            Debug.LogWarning("Invalid activityTimer format. Using default startTime.");
        }
    }  

    void Update()
    {
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // Reduce the current time by the time passed since the last frame 
                currentTime = Mathf.Max(currentTime, 0);
                UpdateTimerDisplay(currentTime);

            }
            else
            {
                currentTime = 0;
                isTimerRunning = false;
                UpdateTimerDisplay(currentTime);
                TimerEnded(); // Optional function call when the timer ends  
                currentTime = startTime;  
            }  

            if (currentTime <= 5f && currentTime >= 1)
            {
                Debug.LogError("sound play ");
                if (!buzzerSound.isPlaying)
                {
                    buzzerSound.Play();
                }
            }
            else
            {  

                Debug.LogError("sound pause "); 

                if (buzzerSound.isPlaying)
                {
                    buzzerSound.Stop();
                } 
                
                buzzerSound.loop = false;
                buzzerSound.Stop();  

            }

        } 

    } 

    public void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        UIManager.Instance.timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Display in MM:SS format
    }

    private void TimerEnded()
    {   
       // UIManager.Instance.isPunishmentRoom = false;
          Debug.Log("Timer has ended!");


        if (!buzzerSound.isPlaying)
        {
            Debug.Log("Stopping buzzer sound in TimerEnded.");
            buzzerSound.Stop();
        }

        if (GameManager.Instance.playerMale.GetComponent<FollowThePath>().curIndex==18&& GameManager.Instance.curTurn == Turn.Female)
        {
            UIManager.Instance.isPunishmentRoom = true;
        } 
        
        if(GameManager.Instance.playerFemale.GetComponent<FollowThePath>().curIndex == 18 && GameManager.Instance.curTurn==Turn.Male)
        {
            UIManager.Instance.isPunishmentRoom = true;
        }

        if(UIManager.Instance.isPunishmentRoom)
        {
            UIManager.Instance.isPunishmentRoom = false;
            Debug.Log("PunishmentRoom is call ...direate ");
            GamePlayUIAnimation.ins.ClosePopup(UIManager.Instance.perFormActivity);
            UIManager.Instance.SatisfiedYesBtn(); 
        }
        else
        {
            Debug.Log("PunishmentRoom is call ... in direate ");
            GamePlayUIAnimation.ins.ClosePopup(UIManager.Instance.perFormActivity);
            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.satisfiedPopup);
        }

       


    }

   


}
