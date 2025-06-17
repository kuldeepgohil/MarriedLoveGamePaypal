using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;   


public class checkProfile : MonoBehaviour
{  
    public static checkProfile ins;

    public GameObject profilepop;
    public bool isBothProfileCreated;

    private void Awake()
    {
        ins = this;
       
    }  

    [System.Serializable] 
    public class Data
    {
        public bool isBothProfileCreated;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }

    private void Start()
    {
        isBothProfileCreated = false;
        checkuserprofile();
    }

    public void checkuserprofile()
    {
        StartCoroutine(checkuserprofileRequest());
    }

    IEnumerator checkuserprofileRequest()
    {
        string auth = PlayerPrefs.GetString("SaveLoginToken");
        string checkprofileRequestUrl = commonURLScript.url + "/api/user/check-profile";
        UnityWebRequest www = UnityWebRequest.Get(checkprofileRequestUrl);

        www.SetRequestHeader("auth", auth);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("checkprofileerror=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userData = JsonUtility.FromJson<Root>(data);

            //kuldeep code


            if (userData.data.isBothProfileCreated == false)
            {
                profilepop.SetActive(true);
            }
            else
            { 
                profilepop.SetActive(false);
            } 

            isBothProfileCreated =false;

        }
        else
        {
            //Debug.Log("Get_Profile_Response=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userData = JsonUtility.FromJson<Root>(data);

            //Debug.Log("both proflie is done");
              
            if (userData.data.isBothProfileCreated == false)
            {
                profilepop.SetActive(true);
            }
            else
            {
                profilepop.SetActive(false);
            } 

            isBothProfileCreated = true;


        }

    } 

}
