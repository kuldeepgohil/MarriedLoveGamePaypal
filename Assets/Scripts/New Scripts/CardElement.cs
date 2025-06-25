using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using ZenFulcrum.EmbeddedBrowser;

public class CardElement : MonoBehaviour
{
    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";
    //public static string communUrl = "https://trqqxw6z-3057.inc1.devtunnels.ms";


    public string id;
    public string deckType;
    public string deckName;
    public string title;
    public string discription;
    public float price;
    public string imageURL;

    public Text typeText;
    public Text nameText;
    public Text titleText;
    public Text discriptionText;
    public Text priceText;
    public Image cardImage;

    public GameObject errorPopup;
    public Text errorText;
    public Button buyButton;

    public CardDetailsRoot cardDetails;
    public GameObject browserPrefab;

    public GameObject titlePannel;
    public GameObject imagePannel;
    public GameObject discriptionPannel;

    private void OnEnable()
    {
        //StartCoroutine(GetCardImage(cardImage, communUrl + "/api/images/" + imageURL));
    }

    private void Start()
    {
        StartCoroutine(GetCardImage(cardImage, communUrl + "/api/images/" + imageURL));
    }

    public void SetupCard()
    {
        typeText.text = deckType;
        nameText.text = deckName;
        titleText.text = title;
        discriptionText.text = discription;
        priceText.text = "<b>$ " + price.ToString() + "</b>";
        StartCoroutine(SetupDiscription());
    }

    IEnumerator SetupDiscription()
    {
        discriptionPannel.SetActive(true);
        yield return new WaitForSeconds(.1f);
        discriptionPannel.SetActive(false);
    }

    public void OpenPaymentMethodPannel()
    {
        CardDeckBuyManager.instance.paymentScreen.SetActive(true);  

        CardDeckBuyManager.instance.paypalButtn.onClick.RemoveAllListeners();

        CardDeckBuyManager.instance.paypalButtn.onClick.AddListener(delegate { PaymentMethodType.instance.OnButtonClick("paypal"); });
        CardDeckBuyManager.instance.paypalButtn.onClick.AddListener(BuyButtonClick);


        
        CardDeckBuyManager.instance.authorizeButton.onClick.RemoveAllListeners();

        CardDeckBuyManager.instance.authorizeButton.onClick.AddListener(delegate { PaymentMethodType.instance.OnButtonClick("authorize"); });
        CardDeckBuyManager.instance.authorizeButton.onClick.AddListener(BuyButtonClick);


    }

    public void InfoButtonClick()
    {
        if (discriptionPannel.activeSelf)
        {
            discriptionPannel.SetActive(false);
            titlePannel.SetActive(true);
            imagePannel.SetActive(true);
        }
        else
        {
            discriptionPannel.SetActive(true);
            titlePannel.SetActive(false);
            imagePannel.SetActive(false);
        }
    }

    public void BuyButtonClick()
    {
        StartCoroutine(PurchaseCard());
        UserJustBuyCards.Instance.StartUserJustBuyCards();
    }

    IEnumerator PurchaseCard()
    {
        string cardId = id;
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");

        string paymentMethod = PaymentMethodType.instance.selectedPaymentMethod;
        Debug.Log(paymentMethod);

        WWWForm form = new WWWForm();

        form.AddField("card_id", cardId);
        form.AddField("paymentMethod", paymentMethod); //static for PAYPAL

        string url = communUrl + "/api/user/purchase-card";

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("Auth : " + usertoken);

        www.SetRequestHeader("auth", usertoken);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {  
            Debug.Log("Error =" + www.downloadHandler.text);

            cardDetails = JsonUtility.FromJson<CardDetailsRoot>(www.downloadHandler.text);
            CardDeckBuyManager.instance.VerifyPanelScreen.SetActive(false);

            errorText.text = cardDetails.message;
            errorPopup.SetActive(true); 

        }
        else
        {
            Debug.Log("Data fetch Success" + www.downloadHandler.text);
            cardDetails = JsonUtility.FromJson<CardDetailsRoot>(www.downloadHandler.text);

            PlayerPrefs.SetString("CardID", id);
            PlayerPrefs.SetString("CardOrderID", cardDetails.data.orderId);
           
            string approvalUrl = cardDetails.data.approvalUrl;
            Debug.Log("Approval URL: " + approvalUrl);
              
            if (paymentMethod == "authorize" && !string.IsNullOrEmpty(cardDetails.data.approvalUrl))
            {
                yield return new WaitForSeconds(2f);
                Application.OpenURL(approvalUrl);
            }
            else if (paymentMethod == "paypal" && !string.IsNullOrEmpty(cardDetails.data.approvalUrl))
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
        }

        CardDeckBuyManager.instance.loaderScreen.SetActive(true);

    }

    IEnumerator GetCardImage(Image pp, string url)
    {
        Debug.Log("asduygasfduafasdf");

        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;


        if (w.error != null)
        {
            Debug.Log("error ");
            //show default image
            //allgameList[i].banner = defaultIcon;
        }
        else
        {
            if (w.isDone)
            {
                Texture2D tx = w.texture;
                pp.sprite = Sprite.Create(tx, new Rect(0f, 0f, tx.width, tx.height), Vector2.zero, 10f);
            }
        }
    }

    public void CheckCardAlreadyExist()
    {
        Debug.Log("Checking list");
        if (CardDeckBuyManager.instance.userCardIdList.Contains(id))
        {
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class Data
    {
        public string orderId;
        public string approvalUrl;
    }

    [System.Serializable]
    public class CardDetailsRoot
    {
        public int status;
        public string message;
        public Data data;
    }
}
