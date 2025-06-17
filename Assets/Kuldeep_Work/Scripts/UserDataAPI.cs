using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;  


public class UserDataAPI : MonoBehaviour
{
     
    public static UserDataAPI instance;
    public string auth;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        auth = PlayerPrefs.GetString("SaveLoginToken");
       // Debug.LogError(auth);
    }

    public IEnumerator ClickToysUrl(string toyId)
    {
        yield return new WaitForSeconds(1);

        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/click-toys";
        WWWForm form = new WWWForm();
        auth = PlayerPrefs.GetString("SaveLoginToken");

        form.AddField("toyId", toyId);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.Log("Error in Click Toy Url " + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Click Toy Url   : " + www.downloadHandler.text);
        } 
    }

    public IEnumerator ClickTipsUrl(string ActivityId)
    {
        yield return new WaitForSeconds(1);

        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/click-tips";
        WWWForm form = new WWWForm();
        auth = PlayerPrefs.GetString("SaveLoginToken");

        form.AddField("activityId", ActivityId);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.Log("Error in Click ActivityId Url " + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Click ActivityId Url   : " + www.downloadHandler.text);
        }
    }


}

