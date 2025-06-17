using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class ShopifySignin : MonoBehaviour
{

    public UnityEngine.UI.Button sendVerificationCodeButton;
    public InputField EmailNumberInputField;
    string IDText;
    string userMessageText;
    private string verificationCode; 
    public GameObject SignupPopup;
    public GameObject LoginPopup;
    public Text EnterAllTxt;
    public Text EnterOtpTxt;
    public GameObject ErrorPopup;
    public GameObject getOtpScreen;

    [Header("Verify UI")]
    public UnityEngine.UI.Button verifyButton;
    public InputField verificationCodeInputField1;
    public GameObject verifyPopup;

    public GameObject resetPasswordPopup;
    public Text successMsgtext;

    public Text urlLink;

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public string data;
    }  

    private void Start()
    {
        sendVerificationCodeButton.onClick.AddListener(SendVerificationCode);
        verifyButton.onClick.AddListener(VerifyCode);
    }

    private void SendVerificationCode()
    {
        string email = EmailNumberInputField.text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        Regex alphanumericRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&+=!])");
        Regex spaceRegex = new Regex(@"\s");

        string NumberCheck = EmailNumberInputField.text;
        Regex regex1 = new Regex(@"^[1-9]\d{9}$");
        Match match1 = regex1.Match(NumberCheck);

        if (EmailNumberInputField.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Email ID";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (!match.Success)
        {
            EnterAllTxt.text = "Please Enter Valid Email ID";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(SendVerificationCodeRequest());
        } 

    }

    private IEnumerator SendVerificationCodeRequest()
    {
        string emailIDNumber = EmailNumberInputField.text;
        string sendVerificationCodeUrl = commonURLScript.url + "/api/user/signup-shopify";

        WWWForm form = new WWWForm();

        form.AddField("email", emailIDNumber);
        UnityWebRequest www = UnityWebRequest.Post(sendVerificationCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {  
            Debug.Log("register=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
                  
            Root userMessage = JsonUtility.FromJson<Root>(data);
            userMessageText = userMessage.message; 

            EnterAllTxt.text = "Looks like you’re new here! Tap below to join Shopify";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);  

            urlLink.text = "Create Account here...";
            urlLink.gameObject.SetActive(true);

        }
        else
        {
            var data = www.downloadHandler.text;
            Root userIdData = JsonUtility.FromJson<Root>(data);
            IDText = userIdData.data;  
            Debug.LogError(userIdData.data);  
            PlayerPrefs.SetString("userIDText", IDText);  

            if(userIdData.message== "User already exist. Please login")
            {
                successMsgtext.gameObject.SetActive(true);
                successMsgtext.text = userIdData.message;
                StartCoroutine(msgCllose());

            }
            else
            {
                getOtpScreen.SetActive(true);  
            }

        }  

    }

    IEnumerator msgCllose()
    {
        yield return new WaitForSeconds(1f);
        successMsgtext.gameObject.SetActive(false);
        EmailNumberInputField.text = "";
        SignupPopup.SetActive(false);
        LoginPopup.SetActive(true);
    }

    public void changeScreen()
    {
        EmailNumberInputField.text = "";
    } 

    public void clickBack()
    {
        EmailNumberInputField.text = "";
        SignupPopup.SetActive(false);
        LoginPopup.SetActive(true);
    }

    private void VerifyCode()
    {
        if (verificationCodeInputField1.text.Length == 0) 
        {
            EnterOtpTxt.text = "Please Enter valid code";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else
        {
            StartCoroutine(VerifyCodeRequest());
        }  
    }

    private IEnumerator VerifyCodeRequest()
    {
        string enteredCode = verificationCodeInputField1.text;
        string userID = PlayerPrefs.GetString("userIDText");
        string verifyCodeUrl = commonURLScript.url + "/api/user/verify-account";

        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("otp", enteredCode);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            EnterOtpTxt.text = "Invalid Otp";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else
        {
            verifyPopup.SetActive(false);
            SignupPopup.SetActive(false);
            resetPasswordPopup.SetActive(true);
            changeScreen();
        } 

    } 
    IEnumerator EnterProperOtp()
    {
        yield return new WaitForSeconds(1f);
        EnterOtpTxt.gameObject.SetActive(false);
    }   

    public void shopifyUrlOpen()
    {
        //Application.OpenURL("https://marriedlovegames.com/password");
    }
}