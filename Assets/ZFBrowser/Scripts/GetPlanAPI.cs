using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GetPlanAPI : MonoBehaviour
{
    public static string communUrl = "https://romantic-blessinggame.appworkdemo.com";
    //public static string communUrl = "https://trqqxw6z-3057.inc1.devtunnels.ms";

    //public static string communUrl = "https://58f7-122-164-17-137.ngrok-free.app";

    public Root availablePlans;

    public MembershipPlanElement[] planElemnts;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMembershipPlans());
    }

    IEnumerator GetMembershipPlans()
    {
        string usertoken = PlayerPrefs.GetString("SaveLoginToken");  

        Debug.Log(usertoken);
        Debug.Log("asjidbfapio");

        string ProfileRequestCodeUrl = communUrl + "/api/user/membership-plans";

        UnityWebRequest www = UnityWebRequest.Get(ProfileRequestCodeUrl);
        www.SetRequestHeader("auth", usertoken);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {

            Debug.Log("Data : " + www.downloadHandler.text);

            availablePlans = JsonUtility.FromJson<Root>(www.downloadHandler.text);


            Debug.LogError(availablePlans.data+ "availablePlans is call...");


            for(int i = 0; i < planElemnts.Length; i++)
            {
                planElemnts[i].id = availablePlans.data[i]._id;
                //planElemnts[i].titleText.text = availablePlans.data[i].title;
                //planElemnts[i].discriptionText.text = availablePlans.data[i].description;
                planElemnts[i].monthlyCost = availablePlans.data[i].monthly_price;
                planElemnts[i].yearlyCost = availablePlans.data[i].yearly_price;
                planElemnts[i].lifetimeCost = availablePlans.data[i].lifetimePrice;
                planElemnts[i].isLifeTime = availablePlans.data[i].is_lifetime;

                /*  planElemnts[i].anniversaryGift.text = availablePlans.data[i].anniversary_gift ? "Yes" : "No";

                  planElemnts[i].memberOnlySpecials.text = availablePlans.data[i].member_only_specials ? "Yes" : "No";

                  planElemnts[i].educationalInstructionalVideos.text = availablePlans.data[i].educational_instructional_videos ? "Yes" : "No";

                  planElemnts[i].discountOnToyPurchases.text = availablePlans.data[i].discount.ToString();

                  planElemnts[i].annualPlansavings.text = availablePlans.data[i].annual_plan_savings;*/

                planElemnts[i].anniversaryGift.text = "<b><color=#F58359>Anniversary Gift :</color></b>\n" + (availablePlans.data[i].anniversary_gift ? "Yes" : "No");
                planElemnts[i].memberOnlySpecials.text = "<b><color=#F58359>Member Only Specials :</color></b>\n" + (availablePlans.data[i].member_only_specials ? "Yes" : "No");
                planElemnts[i].educationalInstructionalVideos.text = "<b><color=#F58359>Educational & Instructional Videos :</color></b>\n" + (availablePlans.data[i].educational_instructional_videos ? "Yes" : "No");
                planElemnts[i].discountOnToyPurchases.text = "<b><color=#F58359>Discount On Toy Purchases :</color></b>\n" + availablePlans.data[i].discount.ToString() + "%";
                planElemnts[i].annualPlansavings.text = "<b><color=#F58359>Annual Plan Savings :</color></b>\nSAVE $" + availablePlans.data[i].annual_plan_savings;
                planElemnts[i].SetupElement();
            }
        }
    }

    public void OnBackButtonClick()
    {
        if (!PlayerPrefs.HasKey("plan_name"))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoginScene");
        }
    }


    [System.Serializable]
    public class Datum
    {
        public string _id;
        public string title;
        public string description;
        public int monthly_price;
        public int yearly_price;
        public int lifetimePrice;
        public bool is_lifetime;
        public bool is_active;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;

        public bool anniversary_gift;
        public int discount;
        public bool educational_instructional_videos;
        public bool member_only_specials;
        public string annual_plan_savings;


    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<Datum> data;
    }
}
