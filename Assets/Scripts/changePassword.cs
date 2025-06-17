using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


public class changePassword : MonoBehaviour
{

    private string AUTH_TOKEN;
    public InputField newPwd, confirmNewPwd;
    public Text EnterAllTxt;
    public Button NewShowpwdbtn;
    public Button ConfirmShowpwdbtn;
    public Sprite Image1;
    public Sprite Image2;
    public GameObject ErrorMsgPopup;
    public Text successMsgtext;
    public GameObject resetPwdPopup;
    public GameObject forgootScreen;  

    public void ChangePwd()
    {

        AUTH_TOKEN = PlayerPrefs.GetString("SaveLoginToken");
        Regex alphanumericRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&+=!])");
        Regex spaceRegex = new Regex(@"\s");  

        if (newPwd.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter New Password";
            ErrorMsgPopup.SetActive(true);
        }
        else if (newPwd.text.Length! < 6 || newPwd.text.Length! > 12 || !alphanumericRegex.IsMatch(newPwd.text) || spaceRegex.IsMatch(newPwd.text))
        {
            EnterAllTxt.text = "Password Length must be 6-12 Characters that contains atleast 1 Upper, 1 Lower, 1 Character , 1 Numeric, No WhiteSpace Allowd";
            ErrorMsgPopup.SetActive(true);
        }
        else if (confirmNewPwd.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Confirm New Password";
            ErrorMsgPopup.SetActive(true);
        }
        else if (confirmNewPwd.text.Length! < 6 || confirmNewPwd.text.Length! > 12 || !alphanumericRegex.IsMatch(confirmNewPwd.text) || spaceRegex.IsMatch(confirmNewPwd.text))
        {
            EnterAllTxt.text = "Confirm Length must be 6-12 Characters that contains atleast 1 Upper, 1 Lower, 1 Character , 1 Numeric, No WhiteSpace Allowd";
            ErrorMsgPopup.SetActive(true);
        }
        else if (newPwd.text != confirmNewPwd.text)
        {
            EnterAllTxt.text = "Password do not match";
            ErrorMsgPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(ChangePwdRequest());           
        }
    }

    private IEnumerator ChangePwdRequest()
    {

        string newPwdTxt = newPwd.text;
        string confirmnewPwdTxt = confirmNewPwd.text;
        string forgotuserid = PlayerPrefs.GetString("forgetIDText");
        string resetRequestUrl = commonURLScript.url+ "/api/user/reset-password";
    
        WWWForm form = new WWWForm();

        Debug.Log("Newpass : " + newPwdTxt);
        Debug.Log("Conf Pass : " + confirmnewPwdTxt);
        Debug.Log("ID : " + forgotuserid);

        form.AddField("new_password", newPwdTxt);
        form.AddField("confirm_password", confirmnewPwdTxt);
        form.AddField("id", forgotuserid);

        UnityWebRequest www = UnityWebRequest.Post(resetRequestUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.LogError(www.error);
            Debug.Log("resetpwdError=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root resetpwdData = JsonUtility.FromJson<Root>(data);
            Debug.Log("data:" + data);
            EnterAllTxt.text = resetpwdData.message;
            ErrorMsgPopup.SetActive(true);
        }
        else
        {
            var data = www.downloadHandler.text;
            Root resetpwdData = JsonUtility.FromJson<Root>(data);
            Debug.Log("data:" + data);
            successMsgtext.gameObject.SetActive(true);
            successMsgtext.text = resetpwdData.message;
            newPwd.text = "";
            confirmNewPwd.text = "";
            StartCoroutine(msgCllose());
        }
    }

    IEnumerator msgCllose()
    {
        yield return new WaitForSeconds(1f);
        successMsgtext.gameObject.SetActive(false);
        forgootScreen.SetActive(false); 
        resetPwdPopup.SetActive(false);
    }
    public void ShowPassword()
    {

        if (newPwd.contentType == InputField.ContentType.Password)
        {
            newPwd.contentType = InputField.ContentType.Standard;
            newPwd.ForceLabelUpdate();
            NewShowpwdbtn.GetComponent<Image>().sprite = Image1;
        }
        else
        {
            newPwd.contentType = InputField.ContentType.Password;
            newPwd.ForceLabelUpdate();
            NewShowpwdbtn.GetComponent<Image>().sprite = Image2;
        }  

    }

    public void ShowConfirmPassword()
    {
        if (confirmNewPwd.contentType == InputField.ContentType.Password)
        {
            confirmNewPwd.contentType = InputField.ContentType.Standard;
            confirmNewPwd.ForceLabelUpdate();
            ConfirmShowpwdbtn.GetComponent<Image>().sprite = Image1;
        }
        else
        {
            confirmNewPwd.contentType = InputField.ContentType.Password;
            confirmNewPwd.ForceLabelUpdate();
            ConfirmShowpwdbtn.GetComponent<Image>().sprite = Image2;
        }
    }

    public void closePopup()
    {
        newPwd.text = "";
        confirmNewPwd.text = "";
        resetPwdPopup.SetActive(false);
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public string data;
    }

}

