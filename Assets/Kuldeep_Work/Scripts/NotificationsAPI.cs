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
using Unity.VisualScripting;


public class NotificationsAPI : MonoBehaviour
{  

    public static NotificationsAPI ins;

    [System.Serializable]
    public class Datum
    {
        public string _id;
        public string image;
        public string type;
        public string title;
        public string description;
        public string link;
        public bool status;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<Datum> data;
    }


    [Header("Prefab and Parent Setup")]
    public GameObject notificationsPrefab;
    public Transform parentTransform;
    public GameObject mainImage;  


    [Header("API URL")]
    string apiUrl;
    public string authToken;

    private int minCharacterLimit = 165;
    private int maxCharacterLimit = 2000;

    public void Awake()
    {
        ins = this;
    }

    public void Start()
    {
       StartCoroutine(NotificationsRequest());
    }
    public void NotiEvent()
    {   
       StartCoroutine(NotificationsRequest());
    }

    public IEnumerator NotificationsRequest()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        string auth = PlayerPrefs.GetString("SaveLoginToken");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/notifications";
        UnityWebRequest www = UnityWebRequest.Get(createuserprofileRequestUrl);

        www.SetRequestHeader("auth", auth);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Notifications error=" + www.downloadHandler.text);
            var data = www.downloadHandler.text; 
        }
        else
        { 
            Debug.Log("Get_Notifications_Response=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userData = JsonUtility.FromJson<Root>(data);
            Root responseData = JsonUtility.FromJson<Root>(data);

            foreach (var notifications in responseData.data)
            {
                if(notifications.type== "event")
                {  
                    GameObject faqItem = Instantiate(notificationsPrefab, parentTransform);
                    SetupNotificationsItem(faqItem, notifications.title, notifications.description);
                    StartCoroutine(GetNotificationsImage(faqItem.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>(),
                        commonURLScript.imgURL + notifications.image));

                    faqItem.gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (notifications.link== "")
                        {
                            Application.OpenURL("https://www.google.com/");
                            Debug.Log("Null URL");
                        }
                        else
                        {
                            Application.OpenURL(notifications.link);
                        }
                    });

                }
            }     

        }

    }

    IEnumerator GetNotificationsImage(Image pp, string url)
    {
        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;


        if (w.error != null)
        {
            Debug.Log("error ");
            //show default image
            //allgameList[i].banner = defaultIcon;
            // pp.sprite = Sprite.Create(defalutAvatar, new Rect(0, 0, defalutAvatar.width, defalutAvatar.height), Vector2.zero);

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

    private void SetupNotificationsItem(GameObject faqItem, string question, string answer)
    {
        // Get references to UI elements in the prefab

        Text questionText = faqItem.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text answerText = faqItem.transform.GetChild(1).gameObject.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();

        Button toggleButton = faqItem.transform.GetChild(1).gameObject.GetComponent<Button>();

        // Assign question text
        questionText.text = question;

        // Handle answer text and toggle functionality
        string fullAnswer = answer;
        bool isExpanded = false;

        string readMoreText = "<b><size=18>Read More...</size></b>";

        // Display initial limited text
        answerText.text = fullAnswer.Length > minCharacterLimit
            ? fullAnswer.Substring(0, minCharacterLimit) + "                   " + readMoreText
            : fullAnswer;

        // Add listener to the toggle button
        toggleButton.onClick.AddListener(() =>
        {
            isExpanded = !isExpanded;
            answerText.text = isExpanded
                ? (fullAnswer.Length > maxCharacterLimit ? fullAnswer.Substring(0, maxCharacterLimit) : fullAnswer)
                : (fullAnswer.Length > minCharacterLimit ? fullAnswer.Substring(0, minCharacterLimit) + "                   " + readMoreText : fullAnswer);
            //: (fullAnswer.Length > minCharacterLimit ? fullAnswer.Substring(0, minCharacterLimit) : fullAnswer);
            StartCoroutine(FixSize());

        });
    }

    IEnumerator FixSize()
    {
        parentTransform.GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForSeconds(.1f);
        parentTransform.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

}