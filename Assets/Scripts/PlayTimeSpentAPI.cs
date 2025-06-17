using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayTimeSpentAPI : MonoBehaviour
{
    public static PlayTimeSpentAPI instance;
    public string auth;

    private float elapsedTime;
    private bool isGameRunning;
    private float timeToTriggerRequest = 1000f; // Time interval for sending requests in seconds
    private float nextRequestTime;

    public void Awake()
    {  

        instance = this;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GamePlayTimeCount");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }
    
    void Start()
    {
        // Load elapsedTime from PlayerPrefs
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
        isGameRunning = true;
        auth = PlayerPrefs.GetString("SaveLoginToken", "");
        nextRequestTime = elapsedTime + timeToTriggerRequest; // Schedule next request based on elapsed time
    }

    void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();

            // Check if it's time to send a PlayTimRequest
            if (elapsedTime >= nextRequestTime)
            {
                StartCoroutine(PlayTimRequest(Mathf.FloorToInt(300f))); // Pass elapsed time as integer
                nextRequestTime += timeToTriggerRequest; // Schedule next request
               // nextRequestTime = timeToTriggerRequest; // Schedule next request
            }
        }
    }

    void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
       // Debug.Log($"Elapsed Time: {minutes}m {seconds}s");
    }

    public void StopTimer()
    {
        isGameRunning = false;
        SaveElapsedTime(); // Save the current elapsed time
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        nextRequestTime = timeToTriggerRequest; // Reset request timer
        SaveElapsedTime(); // Save the reset elapsed time
        UpdateTimeDisplay();
    }

    public void SaveElapsedTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save(); // Ensure changes are saved to disk
       // Debug.Log("ElapsedTime saved: " + elapsedTime);
    }

    public IEnumerator PlayTimRequest(int GamePlayTime)
    {

        if (string.IsNullOrEmpty(auth))
        {
            Debug.LogWarning("Auth token is null or empty. Skipping API request.");
            yield break; // Exit the coroutine early
        }

        yield return new WaitForSeconds(1);

        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/game-time";
        WWWForm form = new WWWForm();
        auth = PlayerPrefs.GetString("SaveLoginToken", "");

        form.AddField("gamePlayTime", GamePlayTime);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError(www.error);
            //Debug.Log($"Error in gamePlayTime: {www.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"gamePlayTime successfully sent: {www.downloadHandler.text}");
        }  

    }

    private void OnApplicationQuit()
    {
        SaveElapsedTime(); // Save elapsed time when the application quits
    }  

}

