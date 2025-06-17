using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChangePasswordAPI : MonoBehaviour
{
    public InputField oldPassIF;
    public InputField newPassIF;
    public InputField confirmPassIF;

    public Button confirmButton;

    public Sprite image1;
    public Sprite image2;

    public GameObject errorPopup;
    public Text errorText;

    private void Awake()
    {
        confirmButton.onClick.AddListener(RequestForChangePass);
    }

    public void RequestForChangePass()
    {
        StartCoroutine(ChangePassword());
    }

    IEnumerator ChangePassword()
    {
        Regex alphanumericRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&+=!])");

        Regex spaceRegex = new Regex(@"\s");

        //if (oldPassIF.text.Length == 0 || oldPassIF.text.Length! < 6 || oldPassIF.text.Length! > 12 || !alphanumericRegex.IsMatch(oldPassIF.text) || spaceRegex.IsMatch(oldPassIF.text))
        //{
        //    errorText.text = "Password Length must be 6-12 Characters that contains atleast 1 Upper, 1 Lower, 1 Special Character , 1 Numeric, No WhiteSpace Allowed";
        //    errorText.gameObject.SetActive(true);
        //    errorPopup.SetActive(true);
        //}
        if (newPassIF.text.Length == 0 || newPassIF.text.Length! < 6 || newPassIF.text.Length! > 12 || !alphanumericRegex.IsMatch(newPassIF.text) || spaceRegex.IsMatch(newPassIF.text))
        {
            errorText.text = "Password Length must be 6-12 Characters that contains atleast 1 Upper, 1 Lower, 1 Special Character , 1 Numeric, No WhiteSpace Allowed";
            errorText.gameObject.SetActive(true);
            errorPopup.SetActive(true);
        }
        else if (confirmPassIF.text != newPassIF.text)
        {
            errorText.text = "Password dose not matched";
            errorText.gameObject.SetActive(true);
            errorPopup.SetActive(true);
        }
        else
        {
            string oldpass = oldPassIF.text;
            string newpass = newPassIF.text;
            string confirmpass = confirmPassIF.text;
            string usertoken = PlayerPrefs.GetString("SaveLoginToken");

            WWWForm form = new WWWForm();

            form.AddField("old_password", oldpass);
            form.AddField("new_password", newpass);
            form.AddField("confirm_password", confirmpass);

            string url = commonURLScript.url + "/api/user/change-password";

            UnityWebRequest www = UnityWebRequest.Post(url, form);

            Debug.Log("Auth : " + usertoken);

            www.SetRequestHeader("auth", usertoken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error =" + www.downloadHandler.text);

                Root root = JsonUtility.FromJson<Root>(www.downloadHandler.text);

                errorText.text = root.message;
                errorText.gameObject.SetActive(true);
                errorPopup.SetActive(true);
            }
            else
            {
                Debug.Log("Pass changed successfully=" + www.downloadHandler.text);
                ClearFields();
                gameObject.SetActive(false);
            }
        }
    }

    public void ClearFields()
    {
        oldPassIF.text = string.Empty;
        newPassIF.text = string.Empty;
        confirmPassIF.text = string.Empty;
    }

    public void ShowPassword(Image img)
    {
        InputField field = img.transform.parent.GetComponent<InputField>();

        if (field.contentType == InputField.ContentType.Password)
        {
            field.contentType = InputField.ContentType.Standard;
            field.ForceLabelUpdate();
            img.sprite = image1;
        }
        else
        {
            field.contentType = InputField.ContentType.Password;
            field.ForceLabelUpdate();
            img.sprite = image2;
        }
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public object data;
    }
}
