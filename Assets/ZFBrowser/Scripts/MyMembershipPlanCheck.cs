using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyMembershipPlanCheck : MonoBehaviour
{

    public static MyMembershipPlanCheck Instance;
    public Root availablePlans;
    private Coroutine membershipCheckCoroutine;
    private Coroutine pendingStatusTimerCoroutine;

    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";

    public GameObject memberShipScreen;
    public GameObject paymentScreen;

    public GameObject showPaymentMessagePanel;
    public Text showPaymentMessageText;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    public void StartMembershipCheck()
    {
        InvokeRepeating("RunMembershipCheck", 5f, 5f);
    }

    void RunMembershipCheck()
    {
        if (membershipCheckCoroutine != null)
        {
            StopCoroutine(membershipCheckCoroutine);
        }
        membershipCheckCoroutine = StartCoroutine(MyMembershipPlans());
    }

    IEnumerator MyMembershipPlans()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        Debug.Log(usertoken);

        string MyMembershipPlansCodeUrl = communUrl + "/api/user/get-recent-buy-membership-plan";
        Debug.Log(MyMembershipPlansCodeUrl);

        UnityWebRequest www = UnityWebRequest.Get(MyMembershipPlansCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
            Debug.Log("plan check error block...");
        }
        else
        {
            Debug.Log("Data : " + www.downloadHandler.text);
            availablePlans = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            PlanPurchaseManager.instance.loaderScreen.SetActive(false);
            memberShipScreen.SetActive(false);
            paymentScreen.SetActive(false);

            PlayerPrefs.SetString("plan_name", availablePlans.data.plan_name);
            string planStatus = availablePlans.data.status.ToLower();

            if (planStatus == "pending")
            {
                PlanPurchaseManager.instance.loaderScreen.SetActive(true);
                if (pendingStatusTimerCoroutine == null)
                {
                    pendingStatusTimerCoroutine = StartCoroutine(StopMembershipCheckAfterDelay(120f)); // 2 mins
                }
            }
            else if (planStatus == "success" || planStatus == "failed")
            {
                PlanPurchaseManager.instance.loaderScreen.SetActive(false);
                memberShipScreen.SetActive(false);
                paymentScreen.SetActive(false);

                if (planStatus == "success")
                {
                    showPaymentMessageText.text = " Your heart just subscribed to happiness. Welcome aboard!";
                }
                else
                {
                    showPaymentMessageText.text = "Something went wrong... but love deserves a second chance. Try the payment again.";
                }

                showPaymentMessagePanel.SetActive(true);
                StartCoroutine(HidePaymentMessagePanelAfterDelay(10f));

                CancelInvoke("RunMembershipCheck");

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

    IEnumerator StopMembershipCheckAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        memberShipScreen.SetActive(false);
        paymentScreen.SetActive(false);

        showPaymentMessageText.text = " You’re on the edge of something beautiful... finalize your payment to cross over.";
        showPaymentMessagePanel.SetActive(true);
        StartCoroutine(HidePaymentMessagePanelAfterDelay(10f));

        CancelInvoke("RunMembershipCheck");

        if (membershipCheckCoroutine != null)
        {
            StopCoroutine(membershipCheckCoroutine);
            membershipCheckCoroutine = null;
        }

        pendingStatusTimerCoroutine = null;
        Debug.Log("Stopped membership check after 2 minutes pending status.");

        PlanPurchaseManager.instance.loaderScreen.SetActive(false);
    }

    IEnumerator HidePaymentMessagePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        showPaymentMessagePanel.SetActive(false);
    }

    void OnDestroy()
    {
        CancelInvoke("RunMembershipCheck");

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
    public class Data
    {
        public string _id;
        public string user_id;
        public string membership_plan_id;
        public bool is_active;
        public int plan_amount;
        public DateTime plan_start_date;
        public DateTime plan_end_date;
        public DateTime plan_ends_on;
        public string plan_type;
        public bool is_lifetime;
        public string order_id;
        public string plan_name;
        public string order_status;
        public DateTime order_date;
        public bool recurring;
        public string subscription_id;
        public string status;
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