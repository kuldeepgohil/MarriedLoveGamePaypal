using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanPurchaseManager : MonoBehaviour
{
    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";
    //public static string communUrl = "https://trqqxw6z-3057.inc1.devtunnels.ms";
    //public static string communUrl = "https://58f7-122-164-17-137.ngrok-free.app";

    public static PlanPurchaseManager instance;
    public Root orderDetails;

    public GameObject loaderScreen;

    public GameObject memberShipScreen;  
    public GameObject paymentScreen;

    //kuldeep code 
    public GameObject VerifyPanelScreen;

    public string purchaseType;   

    public GameObject errorPopup;
    public Text errorText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void OnBackButtonClick()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }

    public void VerifyOrder()
    {
        if (purchaseType == "Membership")
        {
           StartCoroutine(VerifyMembershipPirchase());
                    
        }  
        else
        {
            Debug.Log("please card select");
        }
    }

    /* IEnumerator VerifyMembershipPirchase()  
     {


         Debug.Log("VerifyMembershipPirchase is call ... ");

         string paymentMethod = PaymentMethodType.instance.selectedPaymentMethod;
         Debug.Log(paymentMethod);

         string membershipPlanId = PlayerPrefs.GetString("MembershipPlanID");
         string orderId = PlayerPrefs.GetString("MembershipOrderID");
         string type = PlayerPrefs.GetString("MembershipOrderType");
         string usertoken = PlayerPrefs.GetString("SaveLoginToken");



         Debug.Log("plan ID " + membershipPlanId);
         Debug.Log("order ID " + orderId);
         Debug.Log("plan type " + type);

         WWWForm form = new WWWForm();

         form.AddField("membershipPlanId", membershipPlanId);
         form.AddField("type", type);
         form.AddField("orderId", orderId);

         string url = communUrl + "/api/user/comfirm-buy-membership-plan";

         UnityWebRequest www = UnityWebRequest.Post(url, form);

         Debug.Log("Verification Auth : " + usertoken);

         www.SetRequestHeader("auth", usertoken);

         yield return www.SendWebRequest();

         if (www.isNetworkError || www.isHttpError)
         {
             Debug.Log("Error =" + www.downloadHandler.text);

             errorText.text = "Please Payment First";
             errorPopup.SetActive(true);


         }
         else
         {
             Debug.Log("Order Data fetch Success" + www.downloadHandler.text);
             orderDetails = JsonUtility.FromJson<Root>(www.downloadHandler.text);

             if (orderDetails.status == 201)
             {
                 //Purchse success please login again
                 PlayerPrefs.SetString("plan_name", orderDetails.data.plan_name);
                // SceneManager.LoadScene("Dashboard");

                 memberShipScreen.SetActive(false);
                 paymentScreen.SetActive(false);

                 PlayerPrefs.DeleteKey("MembershipPlanID");
                 PlayerPrefs.DeleteKey("MembershipOrderID");
             }
         }

         VerifyPanelScreen.SetActive(false);

        // loaderScreen.SetActive(false);
     }*/

    IEnumerator VerifyMembershipPirchase()
    {
        Debug.Log("VerifyMembershipPirchase is call ... ");

        string paymentMethod = PaymentMethodType.instance.selectedPaymentMethod;
        Debug.Log("Selected Payment Method: " + paymentMethod);

        string url = communUrl + "/api/user/comfirm-buy-membership-plan";
        UnityWebRequest www;

        string usertoken = PlayerPrefs.GetString("SaveLoginToken");

        if (paymentMethod == "paypal")
        {
            string membershipPlanId = PlayerPrefs.GetString("MembershipPlanID");
            string orderId = PlayerPrefs.GetString("MembershipOrderID");
            string type = PlayerPrefs.GetString("MembershipOrderType");

            Debug.Log("PayPal plan ID: " + membershipPlanId);
            Debug.Log("PayPal order ID: " + orderId);
            Debug.Log("PayPal plan type: " + type);

            WWWForm form = new WWWForm();
            form.AddField("membershipPlanId", membershipPlanId);
            form.AddField("type", type);
            form.AddField("orderId", orderId);

            www = UnityWebRequest.Post(url, form);
        }
        else if (paymentMethod == "authorize")
        {
            Debug.Log("Authorize.net payment selected. Sending auth and paymentMethod only.");


            string membershipPlanIds = PlayerPrefs.GetString("MembershipPlanID");
            string type = PlayerPrefs.GetString("MembershipOrderType");


            WWWForm form = new WWWForm();
            form.AddField("paymentMethod", paymentMethod);
            form.AddField("membershipPlanId", membershipPlanIds);
            form.AddField("type", type);

            Debug.Log("Kuldeep "+paymentMethod);

            www = UnityWebRequest.Post(url, form);
        }
        else
        {
            Debug.LogError("Unknown payment method selected.");
            errorText.text = "Invalid payment method selected.";
            errorPopup.SetActive(true);
            yield break;
        }

        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.downloadHandler.text);
            errorText.text = "Please complete the payment first.";
            errorPopup.SetActive(true);
        }
        else
        {
            Debug.Log("Order Data fetch Success: " + www.downloadHandler.text);
            orderDetails = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            if (orderDetails.status == 201)
            {
                PlayerPrefs.SetString("plan_name", orderDetails.data.plan_name);
                memberShipScreen.SetActive(false);
                paymentScreen.SetActive(false);

                PlayerPrefs.DeleteKey("MembershipPlanID");
                PlayerPrefs.DeleteKey("MembershipOrderID");
            }
        }

        VerifyPanelScreen.SetActive(false);
    }

    [System.Serializable]
    public class Data
    {
        public string user_id;
        public string membership_plan_id;
        public bool is_active;
        public int plan_amount;
        public DateTime plan_start_date;
        public DateTime plan_end_date;
        public DateTime plan_ends_on;
        public string plan_type;
        public string order_id;
        public string plan_name;
        public string order_status;
        public DateTime order_date;
        public string _id;
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