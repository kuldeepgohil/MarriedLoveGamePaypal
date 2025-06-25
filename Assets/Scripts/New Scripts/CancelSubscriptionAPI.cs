using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CancelSubscriptionAPI : MonoBehaviour
{
    public GameObject popupPannel;
    public Text popupText;
    public GameObject yesBtn;
    public GameObject noBtn;


    public void CancleSubscription()
    {
        StartCoroutine(CancleSubscriptionAPI());
    }

    IEnumerator CancleSubscriptionAPI()
    {
        yesBtn.SetActive(false);
        noBtn.SetActive(false);

        string url = commonURLScript.url + "/api/user/cancel-membership-plan";

        UnityWebRequest www = UnityWebRequest.Get(url);
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        www.SetRequestHeader("auth", usertoken);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);

            if(www.downloadHandler.text == "{\"error\":\"Membership plan not found\"}")
            {
                popupText.text = "Plan is already cancled.";
            }
            else
            {
                popupText.text = "Something went wrong.\nPlease try again latter.";
            }
        }
        else
        {
            Debug.Log("Data : " + www.downloadHandler.text);

            Root data = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            if(data.status == 200)
            {
                Debug.Log("Plan cancle success...!");
                popupText.text = "Plan successfully canceled.";
            }
            else
            {
                Debug.Log("Plan cancle failed....!");
                popupText.text = "Plan cancelation faild.\nPlease try again latter.";
            }
        }

        yield return new WaitForSeconds(3f);
        popupPannel.SetActive(false);
        yesBtn.SetActive(true);
        noBtn.SetActive(true);
    }

    [Serializable]
    public class Root
    {
        public int status;
        public string message;
    }
}