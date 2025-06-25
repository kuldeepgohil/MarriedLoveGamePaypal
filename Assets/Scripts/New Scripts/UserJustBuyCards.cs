using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UserJustBuyCards : MonoBehaviour
{
    public static UserJustBuyCards Instance;

    public Root userJustBuyPlans;
    private Coroutine membershipCheckCoroutine;
    private Coroutine pendingStatusTimerCoroutine;

    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";

    public GameObject CardDeckBuyPannel;
    public GameObject paymentScreen;

    public GameObject cardMessage;
    public Text cardMessageMessageText;

    public void Awake()
    {
        Instance = this;
    }

    void Start() { }

    public void StartUserJustBuyCards()
    {
        InvokeRepeating("RunCardStatusCheck", 5f, 5f);
    }

    void RunCardStatusCheck()
    {
        if (membershipCheckCoroutine != null)
        {
            StopCoroutine(membershipCheckCoroutine);
        }
        membershipCheckCoroutine = StartCoroutine(UserJustBuyPlansPlans());
    }

    IEnumerator UserJustBuyPlansPlans()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        Debug.Log(usertoken);

        string userJustBuyCodeUrl = communUrl + "/api/user/user-recent-buy-cards";
        Debug.Log(userJustBuyCodeUrl);

        UnityWebRequest www = UnityWebRequest.Get(userJustBuyCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Data : " + www.downloadHandler.text);
            userJustBuyPlans = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            CardDeckBuyManager.instance.loaderScreen.SetActive(false);
            CardDeckBuyPannel.SetActive(false);
            paymentScreen.SetActive(false);

            string cardStatus = userJustBuyPlans.data.status.ToLower();

            if (cardStatus == "pending")
            {
                CardDeckBuyManager.instance.loaderScreen.SetActive(true);

                if (pendingStatusTimerCoroutine == null)
                {
                    pendingStatusTimerCoroutine = StartCoroutine(StopCardStatusCheckAfterDelay(120f)); // 2 minutes
                }
            }
            else if (cardStatus == "success" || cardStatus == "failed")
            {
                CardDeckBuyManager.instance.loaderScreen.SetActive(false);
                CardDeckBuyPannel.SetActive(false);
                paymentScreen.SetActive(false);

                if (cardStatus == "success")
                {
                    cardMessageMessageText.text = "Your heart just subscribed to happiness. Welcome aboard!";
                }
                else
                {
                    cardMessageMessageText.text = "Something went wrong... but love deserves a second chance. Try the payment again.";
                }

                cardMessage.SetActive(true);
                StartCoroutine(HideCardMessagePanelAfterDelay(10f));

                CancelInvoke("RunCardStatusCheck");

                if (membershipCheckCoroutine != null)
                {
                    StopCoroutine(membershipCheckCoroutine);
                    membershipCheckCoroutine = null;
                }

                if (pendingStatusTimerCoroutine != null)
                {
                    StopCoroutine(pendingStatusTimerCoroutine);
                    pendingStatusTimerCoroutine = null;
                }
            }
        }
    }

    IEnumerator StopCardStatusCheckAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CardDeckBuyPannel.SetActive(false);
        paymentScreen.SetActive(false);

        cardMessageMessageText.text = "You’re on the edge of something beautiful... finalize your payment to cross over.";
        cardMessage.SetActive(true);
        StartCoroutine(HideCardMessagePanelAfterDelay(10f));

        CancelInvoke("RunCardStatusCheck");

        if (membershipCheckCoroutine != null)
        {
            StopCoroutine(membershipCheckCoroutine);
            membershipCheckCoroutine = null;
        }

        pendingStatusTimerCoroutine = null;
        Debug.Log("Stopped card check after 2 minutes of pending status.");

        CardDeckBuyManager.instance.loaderScreen.SetActive(false);
    }

    IEnumerator HideCardMessagePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardMessage.SetActive(false);
    }

    void OnDestroy()
    {
        CancelInvoke("RunCardStatusCheck");

        if (membershipCheckCoroutine != null)
        {
            StopCoroutine(membershipCheckCoroutine);
            membershipCheckCoroutine = null;
        }

        if (pendingStatusTimerCoroutine != null)
        {
            StopCoroutine(pendingStatusTimerCoroutine);
            pendingStatusTimerCoroutine = null;
        }
    }

    [System.Serializable]
    public class CardDeckId
    {
        public string _id;
        public string name;
        public bool status;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class CardId
    {
        public string _id;
        public string name;
        public CardDeckId card_deck_id;
        public string unique_id;
        public string category;
        public int price;
        public bool status;
        public bool is_deleted;
        public string description;
        public string image;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
        public string title;
    }

    [System.Serializable]
    public class Data
    {
        public string _id;
        public string user_id;
        public CardId card_id;
        public bool is_active;
        public bool is_deleted;
        public string category;
        public string unique_id;
        public int amount;
        public string status;
        public string order_id;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }




}