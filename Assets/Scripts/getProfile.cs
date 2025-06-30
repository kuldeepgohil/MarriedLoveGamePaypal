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
using UnityEngine.SceneManagement;

public class getProfile : MonoBehaviour
{
    public static getProfile ins;

    public Image maleProfilePic;
    public Image femaleProfilePic;
    public Texture2D maleDefalutAvatar;
    public Texture2D femaleDefalutAvatar;


    public GameObject createUploadTitleMaleTxt; 
    public GameObject createUploadTitleFeMaleTxt;
    public GameObject UpdateUploadTitleMaleTxt;
    public GameObject UpdateUploadTitleFeMaleTxt;


    public GameObject createUploadTitleMaleImaage;
    public GameObject createUploadTitleFeMaleImage;

    public GameObject UpdateUploadTitleMaleTxtImage;
    public GameObject UpdateUploadTitleFeMaleImage;


    [System.Serializable]
    public class Activity
    {
        public string activityName;
        public string activityId;
        public bool isGiving;
        public bool isReceiving;
    }

    [System.Serializable]
    public class Datum
    {
        public string _id;
        public int profile_type;
        public string first_name;
        public string last_name;
        public string gender;
        public string user_id;
        public string dob;
        public string anniversary_date;
        public List<string> toys;
        public List<string> locations;
        public List<Activity> activity;
        public DateTime createdAt;
        public DateTime updatedAt;
        public string image;
        public int __v;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<Datum> data;
    } 

    public Text MaleName;
    public Text FeMaleName;

    public Text maleChName;
    public Text feMaleChName;
   
    private void Start()
    {
        getuserdata();
    }

    private void Awake()
    {
        ins = this;
    }

    public void getuserdata()
    {
        StartCoroutine(getuserDataRequest());

    }

    IEnumerator getuserDataRequest()
    {
        string auth = PlayerPrefs.GetString("SaveLoginToken");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/get-profile";
        UnityWebRequest www = UnityWebRequest.Get(createuserprofileRequestUrl);

        www.SetRequestHeader("auth", auth);

      //  Debug.LogError(auth);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log("getprofileerror=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
        }
        else
        {
            Debug.Log("Get_Profile_Response=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root userData = JsonUtility.FromJson<Root>(data);
            Root responseData = JsonUtility.FromJson<Root>(data);

            foreach (var profile in responseData.data)
            {
                if (profile.gender.Equals("male", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(profile.image))
                    {
                       // Debug.Log("Male profile image is missing.");
                        maleChName.text = profile.first_name.ToUpper().Substring(0, 1); // Show first letter
                        maleChName.gameObject.SetActive(true); // Ensure it is visible 
                        maleProfilePic.GetComponent<Image>().enabled = false;

                        createUploadTitleMaleTxt.gameObject.GetComponent<Text>().color = Color.white;
                        UpdateUploadTitleMaleTxt.gameObject.GetComponent<Text>().color = Color.white;

                        createUploadTitleMaleImaage.gameObject.GetComponent<Image>().color = Color.white;
                        UpdateUploadTitleMaleTxtImage.gameObject.GetComponent<Image>().color = Color.white;

                    }
                    else
                    {
                        createUploadTitleMaleTxt.gameObject.GetComponent<Text>().color = Color.green;
                        UpdateUploadTitleMaleTxt.gameObject.GetComponent<Text>().color = Color.green;

                        createUploadTitleMaleImaage.gameObject.GetComponent<Image>().color = Color.green;
                        UpdateUploadTitleMaleTxtImage.gameObject.GetComponent<Image>().color = Color.green;

                        maleProfilePic.GetComponent<Image>().enabled = true;
                        //Debug.Log("Male profile image exists: " + profile.image);
                        maleChName.gameObject.SetActive(false); // Hide placeholder
                    }

                }   
                else if (profile.gender.Equals("female", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(profile.image))
                    {
                        createUploadTitleFeMaleTxt.gameObject.GetComponent<Text>().color = Color.white;
                        UpdateUploadTitleFeMaleTxt.gameObject.GetComponent<Text>().color = Color.white;

                        createUploadTitleFeMaleImage.gameObject.GetComponent<Image>().color = Color.white;
                        UpdateUploadTitleFeMaleImage.gameObject.GetComponent<Image>().color = Color.white;

                        femaleProfilePic.GetComponent<Image>().enabled = false;
                       // Debug.Log("Female profile image is missing.");
                        feMaleChName.text = profile.first_name.ToUpper().Substring(0, 1);
                        feMaleChName.gameObject.SetActive(true);
                    }
                    else
                    {
                        createUploadTitleFeMaleTxt.gameObject.GetComponent<Text>().color = Color.green;
                        UpdateUploadTitleFeMaleTxt.gameObject.GetComponent<Text>().color = Color.green;

                        createUploadTitleFeMaleImage.gameObject.GetComponent<Image>().color = Color.green;
                        UpdateUploadTitleFeMaleImage.gameObject.GetComponent<Image>().color = Color.green;

                        femaleProfilePic.GetComponent<Image>().enabled = true;
                        feMaleChName.gameObject.SetActive(false);

                    }
                }
            }

            DateTime MaleAniDateString = DateTime.Parse(userData.data[0].anniversary_date).ToUniversalTime();
            string MaleAniDate = MaleAniDateString.ToString("yyyy-MM-dd");
            PlayerPrefs.SetString("MaleAniDate", MaleAniDate);
                
            DateTime FeMaleAniDateString = DateTime.Parse(userData.data[1].anniversary_date).ToUniversalTime();
            string FeMaleAniDate = FeMaleAniDateString.ToString("yyyy-MM-dd");
            PlayerPrefs.SetString("FeMaleAniDate", FeMaleAniDate);

            DateTime MaleBDateString = DateTime.Parse(userData.data[0].dob).ToUniversalTime();
            string MaleBDate = MaleBDateString.ToString("yyyy-MM-dd");
            PlayerPrefs.SetString("MaleBDate", MaleBDate);


            DateTime FeMaleBDateString = DateTime.Parse(userData.data[0].dob).ToUniversalTime();
            string FeMaleBDate = FeMaleBDateString.ToString("yyyy-MM-dd");
            PlayerPrefs.SetString("FeMaleBDate", FeMaleBDate);


            MaleName.text = userData.data[0].first_name + " " + userData.data[0].last_name;
            FeMaleName.text = userData.data[1].first_name + " " + userData.data[1].last_name;

            PlayerPrefs.SetString("MaleUserID", userData.data[0]._id);
            PlayerPrefs.SetString("FeMaleUserID", userData.data[1]._id);

            PlayerPrefs.SetString("MaleFirstName", userData.data[0].first_name);
            PlayerPrefs.SetString("FeMaleFirstName", userData.data[1].first_name);

            PlayerPrefs.SetString("MaleLastName", userData.data[0].last_name);
            PlayerPrefs.SetString("FeMaleLastName", userData.data[1].last_name);

            PlayerPrefs.SetString("MaleProfilePic", commonURLScript.imgURL + userData.data[0].image);
            PlayerPrefs.SetString("FemaleProfilePic", commonURLScript.imgURL + userData.data[1].image);

            StartCoroutine(GetuserProfile(maleProfilePic, PlayerPrefs.GetString("MaleProfilePic")));
            StartCoroutine(GetuserProfile(femaleProfilePic, PlayerPrefs.GetString("FemaleProfilePic")));
        }
    }

    IEnumerator GetuserProfile(Image pp, string url)
    {
        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;


        if (w.error != null)
        {
            //Debug.Log("error ");
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

    public class DateConverter
    {
        public static string ConvertToDate(string dateString)
        {
            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return date.ToString("yyyy-MM-dd");
        } 

    }

    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void DeleteAccount()
    {
        Debug.Log("Delete Account...");

        //Temp Code
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
