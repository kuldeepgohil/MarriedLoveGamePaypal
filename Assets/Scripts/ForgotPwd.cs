using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class ForgotPwd : MonoBehaviour
{
    public InputField EmailMobileInputField;
    public InputField verificationCodeInputField1;

    public Button sendVerificationCodeButton;
    public Button verifyButton;
    public Button resendButton;

    string forgetIDText;
    string userIDText;
    string userMsgTxt;

    private string verificationCode;

    public GameObject ResetPopup;
    public GameObject forgotPopup;
    public Text EnterAllTxt;
    public Text EnterOtpTxt;

    public float timeRemaining = 10.0f;
    bool startTime = false;

    public Text ResendBtn;
    public Text MsgText;

    public GameObject verifyPopup;
    [System.Serializable]
    public class ForgotRoot
    {
        public int status;
        public string message;
        public string data;
    }





    private void Start()
    {
        sendVerificationCodeButton.onClick.AddListener(SendVerificationCode);
        verifyButton.onClick.AddListener(VerifyCode);
        //resendButton.onClick.AddListener(ResendCode);
        //MsgText.gameObject.SetActive(false);
        //ResendBtn.gameObject.SetActive(false);
    }



    private void SendVerificationCode()
    {
        string email = EmailMobileInputField.text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match matchEmail = regex.Match(email);

    

        Regex regex1 = new Regex(@"^[1-9]\d{12}$");
        Match matchNumber = regex1.Match(email);


        if (EmailMobileInputField.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Email ID";
            EnterAllTxt.gameObject.SetActive(true);
            StartCoroutine(Enteralldata());

        }
        else if (!matchEmail.Success)
        {
            EnterAllTxt.text = "Please Enter Valid Email ID";
            EnterAllTxt.gameObject.SetActive(true);
            StartCoroutine(Enteralldata());
        }
        else
        {
            StartCoroutine(SendVerificationCodeRequest());
        }
    }

    IEnumerator Enteralldata()
    {
        yield return new WaitForSeconds(2.5f);
        EnterAllTxt.gameObject.SetActive(false);
    }

    IEnumerator SucessMsg()
    {
        yield return new WaitForSeconds(1f);
        EnterAllTxt.color = Color.red;
        EnterAllTxt.gameObject.SetActive(false);
    }

    IEnumerator EnterProperOtp()
    {
        yield return new WaitForSeconds(2.5f);
        EnterOtpTxt.gameObject.SetActive(false);
    }  

    private IEnumerator SendVerificationCodeRequest()
    {   
        string emailPhone = EmailMobileInputField.text;

        string email_ = emailPhone;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email_);

        string NumberCheck = emailPhone;
        Regex regex1 = new Regex(@"^[1-9]\d{9}$");
        Match match1 = regex1.Match(NumberCheck);

        string forgotpasswordCodeUrl = commonURLScript.url + "/api/user/forgot-password"; //check
        WWWForm form = new WWWForm();
        form.AddField("email", emailPhone);

        UnityWebRequest www = UnityWebRequest.Post(forgotpasswordCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error); //---------------

            var data = www.downloadHandler.text;
            //Debug.Log("Data= " + data);
                
            ForgotRoot userMsg = JsonUtility.FromJson<ForgotRoot>(data);
            userMsgTxt = "There isn’t any account associated with this email";
            EnterAllTxt.text = userMsgTxt;
            EnterAllTxt.gameObject.SetActive(true);
            StartCoroutine(Enteralldata());
        }
        else
        {

            var data = www.downloadHandler.text;
            //Debug.Log("forgotpwd= " + data);

            ForgotRoot userforgotIdData = JsonUtility.FromJson<ForgotRoot>(data);
            forgetIDText = userforgotIdData.data;
            PlayerPrefs.SetString("forgetIDText", forgetIDText);
            EnterAllTxt.color = Color.green;
            EnterAllTxt.text = "Otp Sent Successfully";
            EnterAllTxt.gameObject.SetActive(true);
            StartCoroutine(SucessMsg());
            //timeRemaining = 10.0f;
            //startTime = true;
            //MsgText.gameObject.SetActive(true);
            //ResendBtn.gameObject.SetActive(false);
            verifyPopup.SetActive(true);
        }
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
    private void VerifyCode()
    {
        if (verificationCodeInputField1.text.Length == 0)
        {
            //fill all
            EnterOtpTxt.text = "Please Enter valid code";
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else if (verificationCodeInputField1.text.Length < 4)
        {
            //fill all
            EnterOtpTxt.text = "Code Length must be four digit";
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

        string userforgotID = forgetIDText;
        string verifyCodeUrl = commonURLScript.url + "/api/user/verify-otp";

        WWWForm form = new WWWForm();
        form.AddField("id", userforgotID);
        form.AddField("otp", enteredCode);

        UnityWebRequest www = UnityWebRequest.Post(verifyCodeUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            Debug.Log("verifyforget error=" + www.downloadHandler.text);
            otpRoot userMsg = JsonUtility.FromJson<otpRoot>(www.downloadHandler.text);

            string msg = userMsg.message;

            EnterOtpTxt.text = msg;
            EnterOtpTxt.gameObject.SetActive(true);
            StartCoroutine(EnterProperOtp());
        }
        else
        {
            string response = www.downloadHandler.text;
            //if (response == "Verification successful")
            //{
            Debug.Log(response);

            ResetPopup.SetActive(true);
            //forgotPopup.SetActive(false);
            //Debug.Log("verifyforget =" + www.downloadHandler.text);
            //changeScreen();
            //}
            //else
            //{
            //    //Debug.Log("Verification failed");
            //    // Add additional logic here to handle failed verification
            //}
        }
    }


    private void ResendCode()
    {

        StartCoroutine(ResendCodeRequest());

    }

    private IEnumerator ResendCodeRequest()
    {
        string userID = userIDText;
        string verifyCodeUrl = commonURLScript.url + "/api/user/resend-otp";

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


            //Debug.Log("verifyforget =" + www.downloadHandler.text);

        }
    }

    public void closeScreen()
    {
        EmailMobileInputField.text = "";
        verificationCodeInputField1.text = "";
        verifyPopup.SetActive(false);
        forgotPopup.SetActive(false);
    }



    [System.Serializable]
    public class otpRoot
    {
        public int status;
        public string message;
    }


}




