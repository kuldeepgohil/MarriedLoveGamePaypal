using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FeedBackSupportAPI : MonoBehaviour
{
    public Dropdown catagoryDropdown;
    public InputField discriptionIF;
    public Button submitButton;

    public Transform listElementPerent;
    public Root allRequestRoot;

    public GameObject requestListElement;

    public Sprite selectedImage;
    public Sprite deselectedImage;

    public GameObject formPannel;
    public GameObject requestListPannel;

    public Button formButton;
    public Button requestListButton;

    public Text popUpcatagoryText;
    public Text popUpDiscriptionText;
    public Text popUpresponceText;
    public GameObject requestDetailsPopup;


    public GameObject errorPopup;
    public Text errorText;

    // Start is called before the first frame update
    void Awake()
    {
        submitButton.onClick.AddListener(CreateTickitRequest);
        GetAllRequestList();
    }

    void CreateTickitRequest()
    {
        StartCoroutine(CreateTickit());
    }

    IEnumerator CreateTickit()
    {
        if(catagoryDropdown.captionText.text == catagoryDropdown.options[0].text)
        {
            errorText.text = "Please Select Valid Catagory";
            errorText.gameObject.SetActive(true);
            errorPopup.SetActive(true);
        }
        else if (discriptionIF.text.Length == 0)
        {
            errorText.text = "Please Select Valid Discription";
            errorText.gameObject.SetActive(true);
            errorPopup.SetActive(true);
        }
        else
        {
            string catagory = catagoryDropdown.captionText.text;
            string discription = discriptionIF.text;
            string usertoken = PlayerPrefs.GetString("SaveLoginToken");

            WWWForm form = new WWWForm();

            form.AddField("subject", catagory);
            form.AddField("description", discription);

            string url = commonURLScript.url + "/api/user/create-ticket";

            UnityWebRequest www = UnityWebRequest.Post(url, form);

            Debug.Log("Auth : " + usertoken);

            www.SetRequestHeader("auth", usertoken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error =" + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Tickit Generated Success" + www.downloadHandler.text);
                GetAllRequestList();
                gameObject.SetActive(false);
            }
        }
    }

    public void GetAllRequestList()
    {
        StartCoroutine(GetAllRequestListAPI());
    }

    IEnumerator GetAllRequestListAPI()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        string ProfileRequestCodeUrl = commonURLScript.url + "/api/user/get-all-tickets";

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Fetched Successfully" + www.downloadHandler.text);
            allRequestRoot = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            if(allRequestRoot.data.Count > 0)
            {
                Debug.Log("Child : " + listElementPerent.transform.childCount);

                //listElementPerent.gameObject.SetActive(false);

                for (int a = listElementPerent.transform.childCount; a > 0; a--)
                {
                    Destroy(listElementPerent.GetChild(a - 1).gameObject);
                }

                for(int i = 0; i < allRequestRoot.data.Count; i++)
                {
                    GameObject element = Instantiate(requestListElement);

                    FeedBackRequestElement tempElement = element.GetComponent<FeedBackRequestElement>();

                    tempElement.id = allRequestRoot.data[i]._id;
                    tempElement.numberText.text = (i + 1).ToString();
                    tempElement.GetDetails();
                    tempElement.viewBTN.onClick.AddListener(delegate { OnViewButtonClick(tempElement.subject, tempElement.discription, tempElement.responce); });

                    element.transform.SetParent(listElementPerent);
                    element.transform.position = Vector3.zero;
                }

                //listElementPerent.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("No Data Found");
            }
        }
    }

    public void ClearFields()
    {
        discriptionIF.text = string.Empty;
        catagoryDropdown.value = 0;
    }

    public void PannelButtonClick(Button btn)
    {
        if (btn == formButton)
        {
            formPannel.SetActive(true);
            requestListPannel.SetActive(false);

            formButton.image.sprite = selectedImage;
            requestListButton.image.sprite = deselectedImage;

            ClearFields();
        }
        else
        {
            GetAllRequestList();
            formPannel.SetActive(false);
            requestListPannel.SetActive(true);

            formButton.image.sprite = deselectedImage;
            requestListButton.image.sprite = selectedImage;
        }
    }

    public void OnViewButtonClick(string catagory, string discription, string responce)
    {
        //view button click event here
        popUpcatagoryText.text = catagory;
        popUpDiscriptionText.text = "<b> <size=35>Discription : </size> </b>" + discription;
        popUpresponceText.text = "<b> <size=35>Responce : </size> </b>" + responce;

        requestDetailsPopup.SetActive(true);
    }

    [Serializable]
    public class RequestElement
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
    }

    [Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<RequestElement> data ;
    }
}
