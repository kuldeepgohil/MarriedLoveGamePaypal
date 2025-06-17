using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FeedBackRequestElement : MonoBehaviour
{
    public string number;
    public string id;
    public string userId;
    public string subject;
    public string discription;
    public string responce;
    public string status;

    public Text numberText;
    public Text subjectText;
    public Text discriptionText;
    public Text statusText;
    public Button viewBTN;

    public TicketRoot ticketDetails;

    public void GetDetails()
    {
        StartCoroutine(GetTicketDetails());
    }

    IEnumerator GetTicketDetails()
    {
        Debug.Log("asidugafdoiuswgfa");

        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        string ProfileRequestCodeUrl = commonURLScript.url + "/api/user/get-ticket/" + id;

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Fetched Successfully " + www.downloadHandler.text);
            ticketDetails = JsonUtility.FromJson<TicketRoot>(www.downloadHandler.text);

            userId = ticketDetails.data.user_id;
            subject = ticketDetails.data.subject;
            discription = ticketDetails.data.description;
            responce = ticketDetails.data.response;
            status = ticketDetails.data.status;
            subjectText.text = subject;
            discriptionText.text = discription;
            statusText.text = status;
        }
    }

    [Serializable]
    public class TicketData
    {
        public string _id;
        public string user_id;
        public string subject;
        public string description;
        public string status;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
        public string response;
    }

    [Serializable]
    public class TicketRoot
    {
        public int status;
        public string message;
        public TicketData data;
    }
}
