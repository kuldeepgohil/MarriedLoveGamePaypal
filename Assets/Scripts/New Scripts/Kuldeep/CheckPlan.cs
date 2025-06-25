using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using ZenFulcrum.VR.OpenVRBinding;

public class CheckPlan : MonoBehaviour
{

    public GameObject planPurchasePannel;


    public void Start()
    {
        if (PlayerPrefs.HasKey("plan_name"))
        {
            Debug.Log("Plan Name : " + PlayerPrefs.GetString("plan_name"));

            if (string.IsNullOrEmpty(PlayerPrefs.GetString("plan_name")))
            {

                planPurchasePannel.SetActive(true);
                Debug.Log("true is call ...");
            }
            else
            {

                planPurchasePannel.SetActive(false);

                Debug.Log("false is call ...");

            }
        }
        else
        {
            planPurchasePannel.SetActive(true);
        }
    }

    private void Update() 
    {
       

    }

}
