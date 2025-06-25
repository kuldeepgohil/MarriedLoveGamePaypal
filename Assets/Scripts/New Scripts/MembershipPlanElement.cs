using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MembershipPlanElement : MonoBehaviour
{
    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";
    //public static string communUrl = "https://trqqxw6z-3057.inc1.devtunnels.ms"; 
    //public static string communUrl = "https://58f7-122-164-17-137.ngrok-free.app";

    public string id;
    public string type;
    public Text priceText;
    public Text discriptionText;
    public float monthlyCost;
    public float yearlyCost;
    public float lifetimeCost;
    public bool isLifeTime;
    public Button buyButton;
    public Dropdown dropdown;

    public GameObject errorPopup;
    public Text errorText;

    public Root orderDetails;
    public GameObject browserPrefab;

    private string approvalUrl;
    public Button paypalButtn;
    public Button AuthorizenBtn;

    public Text anniversaryGift;
    public Text memberOnlySpecials;
    public Text educationalInstructionalVideos;
    public Text discountOnToyPurchases;
    public Text annualPlansavings;


    private void Awake()
    {
        buyButton.onClick.AddListener(OpenPaymentMethodScreen);
    }

    public void SetupElement()
    {
        priceText.text = "$ " + monthlyCost.ToString();
        type = "monthly";

        if (isLifeTime)
        {
            dropdown.options.RemoveAt(0);
            dropdown.options.RemoveAt(0);
            dropdown.captionText.text = "LifeTime";
            dropdown.options.Add(new Dropdown.OptionData() { text = "LifeTime" });
            priceText.text = "$ " + lifetimeCost.ToString();
            type = "lifetime";
        }
    }

    public void OnDropDownValueChange(Dropdown dropdown)
    {
        string value = dropdown.captionText.text;
        
        if(value == dropdown.options[0].text)
        {
            priceText.text = "$ " + monthlyCost.ToString();
            type = "monthly";
        }
        else if(value == dropdown.options[1].text)
        {
            priceText.text = "$ " + yearlyCost.ToString();
            type = "yearly";
        }
        else
        {
            priceText.text = "$ " + lifetimeCost.ToString();
            type = "lifetime";
        }
    }

    public void OpenPaymentMethodScreen()
    {
        paypalButtn.onClick.RemoveAllListeners();
        paypalButtn.onClick.AddListener(delegate { PaymentMethodType.instance.OnButtonClick("paypal"); });
        paypalButtn.onClick.AddListener(BuyButtonClick);

        AuthorizenBtn.onClick.RemoveAllListeners();
        AuthorizenBtn.onClick.AddListener(delegate { PaymentMethodType.instance.OnButtonClick("authorize"); });
        AuthorizenBtn.onClick.AddListener(BuyButtonClick);
    }

    public void BuyButtonClick()
    {
        StartCoroutine(BuyMembership());
        MyMembershipPlanCheck.Instance.StartMembershipCheck();
    }
    IEnumerator BuyMembership()
    {   
        string membershipPlanId = id;
        string type = this.type;
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");

        string paymentMethod = PaymentMethodType.instance.selectedPaymentMethod;
        Debug.Log(paymentMethod);

        WWWForm form = new WWWForm();

        form.AddField("membershipPlanId", membershipPlanId);
        form.AddField("type", type);
        form.AddField("paymentMethod",paymentMethod);


        Debug.Log(membershipPlanId);
        Debug.Log(type);
        Debug.Log(paymentMethod);


        string url = communUrl + "/api/user/buy-membership-plan";
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        Debug.Log("Auth : " + usertoken);
        www.SetRequestHeader("auth", usertoken);


        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {   


            Debug.Log("Error =" + www.downloadHandler.text);
            orderDetails = JsonUtility.FromJson<Root>(www.downloadHandler.text);
            PlanPurchaseManager.instance.VerifyPanelScreen.SetActive(false);

            errorText.text = orderDetails.message;
            errorPopup.SetActive(true);
                        

        }
        else
        {
            Debug.Log("Data fetch Success" + www.downloadHandler.text);
            orderDetails = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            PlayerPrefs.SetString("MembershipPlanID", id);
            PlayerPrefs.SetString("MembershipOrderID", orderDetails.data.orderId);
            PlayerPrefs.SetString("MembershipOrderType", type);
           
            string approvalUrl = orderDetails.data.approvalUrl;
            Debug.Log("Approval URL: " + approvalUrl);

            if (paymentMethod == "authorize" && !string.IsNullOrEmpty(orderDetails.data.approvalUrl))
            {
                //yield return new WaitForSeconds(2f);
                Application.OpenURL(approvalUrl); 
            }
            else if (paymentMethod == "paypal" && !string.IsNullOrEmpty(orderDetails.data.approvalUrl))
            {
                Debug.LogError("Opening Approval URL: " + approvalUrl);
                
                Application.OpenURL(approvalUrl);
            }
            else
            {
                Debug.LogWarning("Approval URL is empty or null.");
                errorText.text = "Payment link is invalid.";
                errorPopup.SetActive(true);
            }
            PlanPurchaseManager.instance.loaderScreen.SetActive(true);
        }    

    }

    [System.Serializable]
    public class Data
    {
        public string orderId;
        public string approvalUrl;
        public string token;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }  
}