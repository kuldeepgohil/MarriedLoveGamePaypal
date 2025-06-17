using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateUserDetailsAPI : MonoBehaviour
{
    public InputField userNameIF;
    public InputField emailIF;

    public Sprite selectedImage;
    public Sprite deselectedImage;

    public GameObject updateDetailsPannel;
    public GameObject changePassPannel;

    public Button updateDetailsButton;
    public Button changePassButton;

    public Button submitBtn;

    public GameObject errorPopup;
    public Text errorText;

    private void Awake()
    {
        submitBtn.onClick.AddListener(UpdateDetailsButtonClick);
    }

    public void PannelButtonClick(Button btn)
    {
        if (btn == updateDetailsButton)
        {
            updateDetailsPannel.SetActive(true);
            changePassPannel.SetActive(false);

            updateDetailsButton.image.sprite = selectedImage;
            changePassButton.image.sprite = deselectedImage;
        }
        else
        {
            updateDetailsPannel.SetActive(false);
            changePassPannel.SetActive(true);

            updateDetailsButton.image.sprite = deselectedImage;
            changePassButton.image.sprite = selectedImage;
        }
    }

    public void UpdateDetailsButtonClick()
    {
        StartCoroutine(UpdateUsetDetails());
    }

    IEnumerator UpdateUsetDetails()
    {
        string userName = userNameIF.text;
        string email = emailIF.text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match matchEmail = regex.Match(email);

        string usertoken = PlayerPrefs.GetString("SaveLoginToken");

        if (email.Length != 0 && !matchEmail.Success)
        {
            errorText.text = "Please Enter valid Email ID";
            errorPopup.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();

            form.AddField("email", email);
            form.AddField("fullName", userName);

            string url = commonURLScript.url + "/api/user/update-user";

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
                Debug.Log("Details changed successfully=" + www.downloadHandler.text);
                gameObject.SetActive(false);
            }
        }

        ClearFields();
    }

    public void ClearFields()
    {
        userNameIF.text = string.Empty;
        emailIF.text = string.Empty;
    }
}