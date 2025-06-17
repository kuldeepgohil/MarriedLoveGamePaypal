using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class RegisterScript : MonoBehaviour
{

    public InputField verificationCodeInputField1;
    public Button sendVerificationCodeButton;
    public Button verifyButton;

    public InputField firstNameField;
    public InputField lastNameField;
    public InputField EmailNumberInputField;
    public InputField passwordField;
    string IDText;
    string userMessageText;
    private string verificationCode;
    public GameObject verifyPopup;
    public GameObject SignupPopup;
    public GameObject LoginPopup;

    public Text EnterAllTxt;
    public Button Showpwdbtn;
    public Sprite Image1;
    public Sprite Image2;

    public Text EnterOtpTxt;

    public float timeRemaining = 10.0f;
    bool startTime = false;

    public Text ResendBtn;
    public Text MsgText;
    public Button resendButton;

    public GameObject ErrorPopup;

    public GameObject VerificationCodeTxt;
    public GameObject VerificationReCodeTxt;

   

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
        
        if (firstNameField.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter First Name";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);

        }
        //else if (lastNameField.text.Length == 0)
        //{
        //    EnterAllTxt.text = "Please Enter Last Name";
        //    EnterAllTxt.gameObject.SetActive(true);
        //    ErrorPopup.SetActive(true);
        //}
        else if (EmailNumberInputField.text.Length == 0)
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
        else if (passwordField.text.Length == 0 || passwordField.text.Length! < 6 || passwordField.text.Length! > 12 || !alphanumericRegex.IsMatch(passwordField.text) || spaceRegex.IsMatch(passwordField.text))
        {
            EnterAllTxt.text = "Password Length must be 6-12 Characters that contains atleast 1 Upper, 1 Lower, 1 Special Character , 1 Numeric, No WhiteSpace Allowed";
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
        string fullName = firstNameField.text;
        //string lastName = lastNameField.text;
        string password = passwordField.text;
        string sendVerificationCodeUrl = commonURLScript.url+"/api/user/signup";

        Debug.Log("URL " + sendVerificationCodeUrl);

        WWWForm form = new WWWForm();

        form.AddField("fullName", fullName);
        //form.AddField("first_name", firstName);
        //form.AddField("last_name", lastName);
        form.AddField("email", emailIDNumber);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(sendVerificationCodeUrl, form);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
            Debug.Log("register=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;

            Root userMessage = JsonUtility.FromJson<Root>(data);

            Debug.Log("ajfhndshfpoas "+userMessage);

            userMessageText = userMessage.message;

            EnterAllTxt.text = userMessageText;
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            var data = www.downloadHandler.text;
            Root userIdData = JsonUtility.FromJson<Root>(data);
            IDText = userIdData.data;

            Debug.Log("Id : " + IDText);

            PlayerPrefs.SetString("userIDText", IDText);
            verifyPopup.SetActive(true);
        }
    }

    IEnumerator VerificationCodeTxtClose()
    {
        yield return new WaitForSeconds(2f);
        VerificationCodeTxt.SetActive(false);
    }

    IEnumerator VerificationReCodeTxtClose()
    {
        yield return new WaitForSeconds(2f);
        VerificationReCodeTxt.SetActive(false);
    }
    private void VerifyCode()
    {
        if(verificationCodeInputField1.text.Length == 0) //check length of code
        {
            //fill all
            EnterOtpTxt.text = "Please Enter valid code";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else
        {
            StartCoroutine(VerifyCodeRequest());
        }  
    }

    IEnumerator EnterProperOtp()
    {
        yield return new WaitForSeconds(1f);
        EnterOtpTxt.gameObject.SetActive(false);
    }
    private IEnumerator VerifyCodeRequest()
    {
        string enteredCode = verificationCodeInputField1.text;
        string userID = PlayerPrefs.GetString("userIDText");
        string verifyCodeUrl = commonURLScript.url + "/api/user/verify-account";


        Debug.Log("Id on OTP : " + userID);
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("otp", enteredCode);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            EnterOtpTxt.text = "Invalid Otp";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());

            Debug.Log("Error : " + www.error);
        }
        else
        {
            Debug.Log("Success...!");
            verifyPopup.SetActive(false);
            SignupPopup.SetActive(false);
            LoginPopup.SetActive(true);
            changeScreen();
        }
    }

    public void ShowPassword()
    {
        if (passwordField.contentType == InputField.ContentType.Password)
        {
            passwordField.contentType = InputField.ContentType.Standard;
            passwordField.ForceLabelUpdate();
            Showpwdbtn.GetComponent<Image>().sprite = Image1;
        }
        else
        {
            passwordField.contentType = InputField.ContentType.Password;
            passwordField.ForceLabelUpdate();
            Showpwdbtn.GetComponent<Image>().sprite = Image2;
        }
    }

    private void ResendCode()
    {
        StartCoroutine(ResendCodeRequest());
    }

    private IEnumerator ResendCodeRequest()
    {
        string userID = IDText;
        string verifyCodeUrl = commonURLScript.url + "/api/user/resendOtp";

        WWWForm form = new WWWForm();
        form.AddField("id", userID);


        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.LogError(www.error);
            //Debug.Log("verifyforget error=" + www.downloadHandler.text);
        }
        else
        {
            ////Debug.Log("Verification successful");
            timeRemaining = 10.0f;
            startTime = true;
            MsgText.gameObject.SetActive(true);
            ResendBtn.gameObject.SetActive(false);
            VerificationReCodeTxt.SetActive(true);
            StartCoroutine(VerificationReCodeTxtClose());
            //Debug.Log("verifyforget =" + www.downloadHandler.text);
        } 
    }


    public GameObject planPurchasePannel;
    private string data;

    public void changeScreen()
    {
        EmailNumberInputField.text = "";
        verificationCodeInputField1.text = "";
        firstNameField.text = "";
        lastNameField.text = "";
        passwordField.text = "";
    }

    void Update()
    {
        if (startTime)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //int timer = int.Parse(timeRemaining.ToString());
                MsgText.text = timeRemaining.ToString("0");
            }

            if (timeRemaining < 0)
            {
                startTime = false;
                MsgText.gameObject.SetActive(false);
                ResendBtn.gameObject.SetActive(true);
            }
        }
    }

    public void clickBack()
    {
        EmailNumberInputField.text = "";
        verificationCodeInputField1.text = "";

        firstNameField.text = "";
        lastNameField.text = "";
        passwordField.text = "";

        verifyPopup.SetActive(false);
        SignupPopup.SetActive(false);
        LoginPopup.SetActive(true);
    }  


}