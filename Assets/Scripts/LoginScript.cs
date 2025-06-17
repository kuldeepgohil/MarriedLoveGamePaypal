using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LoginScript : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;

    public Button Showpwdbtn;

    public Sprite Image1;
    public Sprite Image2;

    public Text EnterAllTxt;
    public Text loginSuccessTxt_;
    string userIDText;
    public GameObject Errorpopup;
    public GameObject LogoutErrorPopup;

    public string saveduserName;
    public string savedPwd;

    public string logoutNameData;
    public string logoutPwdData;

    public GameObject signupscreen;
    public GameObject forgotscreen;
    public GameObject planPurchasePannel;


    //-----------------Get Profile-----------------------------
    [System.Serializable]
    public class Data
    {
        public string _id;
        public string fullName;
        public string email;
        public bool is_active;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
        public object otp;
        public object otp_expire_at;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }  

    //-----------Login------------------------------------------------
    [System.Serializable]
    public class LoginData
    {
        public string _id;
        public string token;
        public string user_name;
        public List<Product> products;
        public PlanDetails plan_details;
    } 

    [System.Serializable]
    public class PlanDetails
    {
        public string plan_name;
    }

    [System.Serializable]
    public class Product
    {
        public string product_id;
        public string product_name;
        public List<string> product_tag;
    }


    [System.Serializable]
    public class LoginRoot
    {
        public int status;
        public string message;
        public LoginData data;
    }



    void Start()
    {
        PlayerPrefs.DeleteAll();
        loginButton.onClick.AddListener(LoginRequest);  
    }

    void LoginRequest()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        PlayerPrefs.SetString("usernamedd", username);
        PlayerPrefs.SetString("pswdd", password);
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match matchEmail = regex.Match(username);

        Regex regex1 = new Regex(@"^[1-9]\d{12}$");
        Match matchNumber = regex1.Match(username);  

        if (username.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Email ID";
            Errorpopup.SetActive(true);

        }
        else if (!matchEmail.Success)
        {
            EnterAllTxt.text = "Please Enter valid Email ID";
            Errorpopup.SetActive(true);

        }
        else if (passwordInput.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Password";
            Errorpopup.SetActive(true);
        }
        else
        {
            StartCoroutine(SendLoginRequest(username, password));
        } 
        
    }

    IEnumerator Enteralldata()
    {
        yield return new WaitForSeconds(5f);
        EnterAllTxt.gameObject.SetActive(false);
    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        string email2 = username;
        string fcm = PlayerPrefs.GetString("fcmToken");
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email2);

        string NumberCheck = username;
        Regex regex1 = new Regex(@"^[1-9]\d{12}$");
        Match match1 = regex1.Match(NumberCheck);

        Debug.Log(commonURLScript.url);

        string LoginCodeUrl = commonURLScript.url + "/api/user/login";

        Debug.Log(LoginCodeUrl);

        WWWForm form = new WWWForm();

        form.AddField("email", username);
        form.AddField("password", password);

        using (UnityWebRequest request = UnityWebRequest.Post(LoginCodeUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("login error=" + request.downloadHandler.text);
                LoginRoot userMessage = JsonUtility.FromJson<LoginRoot>(request.downloadHandler.text);
                EnterAllTxt.text = userMessage.message;
                Errorpopup.SetActive(true);
            }
            else
            {
                Debug.Log("loginsuces=" + request.downloadHandler.text);
                loginSuccessTxt_.color = Color.green;
                loginSuccessTxt_.text = "Login Successful";
                loginSuccessTxt_.gameObject.SetActive(true);
                var data = request.downloadHandler.text;
                LoginRoot userIdData = JsonUtility.FromJson<LoginRoot>(data);
                PlayerPrefs.SetString("SaveLoginId", userIdData.data._id);
                PlayerPrefs.SetString("SaveLoginToken", userIdData.data.token);

                Debug.LogError(userIdData.data.plan_details.plan_name);
                
                if(userIdData.data.plan_details.plan_name == null)
                {

                    //EnterAllTxt.text = "Please Purchase plane";
                    //Errorpopup.SetActive(true);  
                    //planPurchasePannel.SetActive(true);
                    SceneManager.LoadScene("Dashboard");

                }
                else
                {
                    StartCoroutine(SucessMsg());
                    PlayerPrefs.SetString("plan_name", userIdData.data.plan_details.plan_name);
                }
            }
        }
    }  
    IEnumerator SucessMsg()
    {
        yield return new WaitForSeconds(.3f);
        PlayerPrefs.SetInt("SaveLogin", 1);
        loginSuccessTxt_.color = Color.red;
        loginSuccessTxt_.gameObject.SetActive(false);



        StartCoroutine(SendProfileRequest());
    }

    IEnumerator SendProfileRequest()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");
        string ProfileRequestCodeUrl = commonURLScript.url + "/api/user/profile";

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {  
            Debug.Log("error=" + www.downloadHandler.text);
            Root userDetails = JsonUtility.FromJson<Root>(www.downloadHandler.text);  
        }
        else
        {
            var data = www.downloadHandler.text;
            Debug.Log("userDetails=" + data);  

            Root userDetails = JsonUtility.FromJson<Root>(data);
            string userID = userDetails.data._id;  

            PlayerPrefs.SetString("userIDText", userID);
            PlayerPrefs.SetString("userFullName", userDetails.data.fullName);
            PlayerPrefs.SetString("usermobileEmail", userDetails.data.email);

         /*   Debug.LogError(userDetails.data.plan_name);
            PlayerPrefs.SetString("plan_name",userDetails.data.plan_name); */

            if(PlayerPrefs.GetString("plan_name") == "Null")
            {
                Debug.Log("sadjhfuijndsafjunsafdhn");
                // open plan purchase pannel
               // planPurchasePannel.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("Dashboard");
                planPurchasePannel.SetActive(false);
            }
        }  
    }

    public void ShowPassword()
    {
        if (passwordInput.contentType == InputField.ContentType.Password)
        {
            passwordInput.contentType = InputField.ContentType.Standard;
            passwordInput.ForceLabelUpdate();
            Showpwdbtn.GetComponent<Image>().sprite = Image1;
        }
        else
        {
            passwordInput.contentType = InputField.ContentType.Password;
            passwordInput.ForceLabelUpdate();
            Showpwdbtn.GetComponent<Image>().sprite = Image2;
        }  
    }  

    public void changeScreen()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        signupscreen.SetActive(true);
    }
        
    public void clickforgotScreen()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        forgotscreen.SetActive(true);
    }

}