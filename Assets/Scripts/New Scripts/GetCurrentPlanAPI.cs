using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetCurrentPlanAPI : MonoBehaviour
{
    public Root data;

    public Text planNameText;
    public Text endDateText;
    public Button cancleButton;

    public void Start()
    {
        GetcurrentPlanDetails();
    }

    public void GetcurrentPlanDetails()
    {
        StartCoroutine(GetCurrentPlanAPIRequest());
    }

    IEnumerator GetCurrentPlanAPIRequest()
    {
        string url = commonURLScript.url + "/api/user/my-membership-plan";

        UnityWebRequest www = UnityWebRequest.Get(url);
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        www.SetRequestHeader("auth", usertoken);

        yield return www.SendWebRequest();

        Debug.Log("checking plan...!");


        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Data : " + www.downloadHandler.text);
            data = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            if(data.data.plan_name == null)
            {
                planNameText.text = "No plan, no romance. Get started now!";
                cancleButton.interactable = false;
            }
            else
            {
                DateTime date = DateTime.Parse(data.data.plan_end_date, null, System.Globalization.DateTimeStyles.RoundtripKind);
                planNameText.text = "<b><color=#F58359>" + data.data.membership_plan_id.description + "</color></b>" + " active Let the romance begin!";
                endDateText.text = "Hearts will part soon! \n" + "Your current plan stays active till <b><color=#F58359>" + date.Date.ToString("dd-MM-yyyy") + "</color></b>.\n" + "Still want to cancel?";
                cancleButton.interactable = true;
            }
        }
    }

    [Serializable]
    public class Data
    {
        public string _id;
        public string user_id;
        public MembershipPlanId membership_plan_id;
        public bool is_active;
        public double plan_amount;
        public string plan_start_date;
        public string plan_end_date;
        public string plan_ends_on;
        public string plan_type;
        public bool is_lifetime;
        public string order_id;
        public string plan_name;
        public string order_status;
        public string order_date;
        public bool recurring;
        public string subscription_id;
        public string status;
        public string paymentGetway;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [Serializable]
    public class MembershipPlanId
    {
        public string _id;
        public string description;
    }

    [Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }
}
