using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDeckBuyManager : MonoBehaviour
{
    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";
    //public static string communUrl = "https://trqqxw6z-3057.inc1.devtunnels.ms";


    // public static string communUrl = "https://58f7-122-164-17-137.ngrok-free.app";

    public static CardDeckBuyManager instance;

    public Sprite selectedImage;
    public Sprite deselectedImage;

    public GameObject cardBuyPannel;
    public GameObject availableCardPannel;

    public Button cardBuyButton;
    public Button availableCardButton;

    public List<string> userCardIdList;
    public CardListRoot cardList;
    public UserCardRoot userCardList;

    public GameObject cardPrefab;
    public Transform cardListPerent;
    public Transform userCardListPerent;

    public CardOrderRoot cardOrderDetails;

    public GameObject errorPopUp;
    public Text errorText;

    public GameObject loaderScreen;
    public GameObject paymentScreen;
    public Button paypalButtn;
    public Button authorizeButton;

    public GameObject VerifyPanelScreen;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GetCardList();
        GetUserCardList();
    }

    public void PannelButtonClick(Button btn)
    {
        if(btn == cardBuyButton)
        {
            cardBuyPannel.SetActive(true);
            availableCardPannel.SetActive(false);

            cardBuyButton.image.sprite = selectedImage;
            availableCardButton.image.sprite = deselectedImage;
        }
        else
        {
            cardBuyPannel.SetActive(false);
            availableCardPannel.SetActive(true);


            cardBuyButton.image.sprite = deselectedImage;
            availableCardButton.image.sprite = selectedImage;
        }
    }

    void GetCardList()
    {
        StartCoroutine(GetCardListAPI());
    }
        
    IEnumerator GetCardListAPI()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        string ProfileRequestCodeUrl = communUrl + "/api/user/cards-list";

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            cardList = JsonUtility.FromJson<CardListRoot>(www.downloadHandler.text);

            if(cardList.message.Count > 0)
            {
                for (int a = cardListPerent.transform.childCount; a > 0; a--)
                {
                    Destroy(cardListPerent.GetChild(a - 1).gameObject);
                }

                for (int i = 0; i < cardList.message.Count; i++)
                {
                    GameObject card = Instantiate(cardPrefab);
                    card.transform.SetParent(cardListPerent);
                    card.transform.localScale = Vector3.one;

                    card.GetComponent<CardElement>().id = cardList.message[i]._id;
                    card.GetComponent<CardElement>().deckType = cardList.message[i].card_deck_id.name;
                    card.GetComponent<CardElement>().deckName = cardList.message[i].name;
                    card.GetComponent<CardElement>().title = cardList.message[i].title;
                    card.GetComponent<CardElement>().discription = cardList.message[i].description;
                    card.GetComponent<CardElement>().imageURL = cardList.message[i].image;
                    card.GetComponent<CardElement>().price = cardList.message[i].price;

                    card.GetComponent<CardElement>().SetupCard();

                    card.GetComponent<CardElement>().discriptionPannel.SetActive(true);
                    yield return new WaitForSeconds(.1f);
                    card.GetComponent<CardElement>().discriptionPannel.SetActive(false);

                    card.GetComponent<CardElement>().errorPopup = errorPopUp;
                    card.GetComponent<CardElement>().errorText = errorText;
                    card.GetComponent<CardElement>().buyButton.onClick.AddListener(card.GetComponent<CardElement>().OpenPaymentMethodPannel);

                    yield return new WaitForSeconds(.5f);
                    card.GetComponent<CardElement>().CheckCardAlreadyExist();
                }
            }
        }
    }

    void GetUserCardList()
    {
        StartCoroutine(GetUserCardListAPI());
    }

    IEnumerator GetUserCardListAPI()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        string ProfileRequestCodeUrl = communUrl + "/api/user/user-cards";

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            userCardList = JsonUtility.FromJson<UserCardRoot>(www.downloadHandler.text);

            
            for (int a = userCardListPerent.transform.childCount; a > 0; a--)
            {
                Destroy(userCardListPerent.GetChild(a - 1).gameObject);
                userCardIdList.Clear();
            }

            if (userCardList.message.paidCards.Count > 0)
            {
                for (int i = 0; i < userCardList.message.paidCards.Count; i++)
                {
                    GameObject card = Instantiate(cardPrefab);
                    card.transform.SetParent(userCardListPerent);
                    card.transform.localScale = Vector3.one;

                    card.GetComponent<CardElement>().id = userCardList.message.paidCards[i]._id;
                    card.GetComponent<CardElement>().deckType = userCardList.message.paidCards[i].card_id.card_deck_id.name;
                    card.GetComponent<CardElement>().deckName = userCardList.message.paidCards[i].card_id.name;
                    card.GetComponent<CardElement>().title = userCardList.message.paidCards[i].card_id.title;
                    card.GetComponent<CardElement>().discription = userCardList.message.paidCards[i].card_id.description;
                    card.GetComponent<CardElement>().imageURL = userCardList.message.paidCards[i].card_id.image;
                    card.GetComponent<CardElement>().price = userCardList.message.paidCards[i].card_id.price;


                    card.GetComponent<CardElement>().buyButton.gameObject.SetActive(false);
                    userCardIdList.Add(userCardList.message.paidCards[i].card_id._id);

                    card.GetComponent<CardElement>().SetupCard();

                    card.GetComponent<CardElement>().discriptionPannel.SetActive(true);
                    yield return new WaitForSeconds(.1f);
                    card.GetComponent<CardElement>().discriptionPannel.SetActive(false);
                }
            }

            if (userCardList.message.freeCards.Count > 0)
            {
                for (int i = 0; i < userCardList.message.freeCards.Count; i++)
                {
                    GameObject card = Instantiate(cardPrefab);
                    card.transform.SetParent(userCardListPerent);
                    card.transform.localScale = Vector3.one;

                    card.GetComponent<CardElement>().id = userCardList.message.freeCards[i]._id;
                    card.GetComponent<CardElement>().deckType = userCardList.message.freeCards[i].card_deck_id.name;
                    card.GetComponent<CardElement>().deckName = userCardList.message.freeCards[i].name;
                    card.GetComponent<CardElement>().title = userCardList.message.freeCards[i].title;
                    card.GetComponent<CardElement>().discription = userCardList.message.freeCards[i].description;
                    card.GetComponent<CardElement>().imageURL = userCardList.message.freeCards[i].image;
                    //card.GetComponent<CardElement>().price = userCardList.message.freeCards[i].card_id.price;

                    card.GetComponent<CardElement>().SetupCard();

                    card.GetComponent<CardElement>().buyButton.gameObject.SetActive(false);
                    userCardIdList.Add(userCardList.message.freeCards[i]._id);
                }
            }
        }
    }

    public void RefreshCardList()
    {
        GetCardList();
        GetUserCardList();
    }

    public void VerifyOrder()
    {
        StartCoroutine(VerifyCardPurchase());
    }

    IEnumerator VerifyCardPurchase()
    {
        string cardId = PlayerPrefs.GetString("CardID");
        string cardOrderId = PlayerPrefs.GetString("CardOrderID");
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");

        WWWForm form = new WWWForm();

        form.AddField("card_id", cardId);
        form.AddField("order_id", cardOrderId);

        string url = communUrl + "/api/user/comfirm-purchase-card";

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("Verification Auth : " + usertoken);

        www.SetRequestHeader("auth", usertoken);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error =" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Order Data fetch Success" + www.downloadHandler.text);
            cardOrderDetails = JsonUtility.FromJson<CardOrderRoot>(www.downloadHandler.text);

            if (cardOrderDetails.status == 201)
            {
                //Purchse success refresh CardList
                GetCardList();
                GetUserCardList();

                PlayerPrefs.DeleteKey("CardID");
                PlayerPrefs.DeleteKey("CardOrderID");

                yield return new WaitForSeconds(2f);
            }

        }

        loaderScreen.SetActive(false);  

        VerifyPanelScreen.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ScrollLeft(ScrollRect scrollRect)
    {
        if(scrollRect.horizontalNormalizedPosition >= 0)
        {
            scrollRect.horizontalNormalizedPosition -= .1f;
        }
    }

    public void ScrollRight(ScrollRect scrollRect)
    {
        if (scrollRect.horizontalNormalizedPosition <= 1)
        {
            scrollRect.horizontalNormalizedPosition += .1f;
        }
    }

    [System.Serializable]
    public class CardListMessage
    {
        public string _id;
        public string name;
        public CardDeckId card_deck_id;
        public string unique_id;
        public string category;
        public float price;
        public bool status;
        public bool is_deleted;
        public string description;
        public string image;
        public string title;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class CardListRoot
    {
        public int status;
        public List<CardListMessage> message;
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
        public float price;
        public bool status;
        public bool is_deleted;
        public string description;
        public string image;
        public string title;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class FreeCard
    {
        public string _id;
        public string name;
        public CardDeckId card_deck_id;
        public string unique_id;
        public string category;
        public float price;
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
    public class UserCardMessage
    {
        public List<PaidCard> paidCards;
        public List<FreeCard> freeCards;
    }

    [System.Serializable]
    public class PaidCard
    {
        public string _id;
        public string user_id;
        public CardId card_id;
        public bool is_active;
        public bool is_deleted;
        public string category;
        public string unique_id;
        public int amount;
        public string image;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class UserCardRoot
    {
        public int status;
        public UserCardMessage message;
    }

    [System.Serializable]
    public class CardOrderData
    {
        public string user_id;
        public string card_id;
        public bool is_active;
        public bool is_deleted;
        public string category;
        public string unique_id;
        public int amount;
        public string _id;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class CardOrderRoot
    {
        public int status;
        public string message;
        public CardOrderData data;
    }
}
