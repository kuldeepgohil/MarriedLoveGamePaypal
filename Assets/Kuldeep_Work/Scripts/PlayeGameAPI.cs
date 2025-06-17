using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayeGameAPI : MonoBehaviour
{ 
    public static PlayeGameAPI instance;
    public string auth;

   


    public void Awake()
    {
        instance = this;
    }

    void Start()
    {    
       
           StartCoroutine(PlayeGame());
        

    }
        
    public IEnumerator PlayeGame()
    {  

        auth = PlayerPrefs.GetString("SaveLoginToken");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/played-game";
        UnityWebRequest www = UnityWebRequest.Get(createuserprofileRequestUrl);

        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Play Game error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("PlayGame Response=" + www.downloadHandler.text);
            string jsonResponse = www.downloadHandler.text;
            Root root = JsonUtility.FromJson<Root>(jsonResponse);  
           
        }  

    }  

}
