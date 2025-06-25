using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PaymentMethodType : MonoBehaviour
{
    public static PaymentMethodType instance;  
    public string selectedPaymentMethod;

    public Button paypalButton;
    public Button authorizeButton;

    public void Awake()
    {
        instance = this;

        StartCoroutine(ActivePaymentMethodAPI());
    }

    public void OnButtonClick(string message)
    {
        selectedPaymentMethod = $"{message}";
        Debug.Log(selectedPaymentMethod);
    }

    IEnumerator ActivePaymentMethodAPI()
    {
        string url = commonURLScript.url + "/api/user/get-avaible-payment-method";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Data : " + www.downloadHandler.text);

            Root data = JsonUtility.FromJson<Root>(www.downloadHandler.text);

            if (!data.data.paypal)
            {
                paypalButton.interactable = false;
            }
            if (!data.data.authorize)
            {
                authorizeButton.interactable = false;
            }
        }
    }

    [Serializable]
    public class Data
    {
        public string _id;
        public bool paypal;
        public bool authorize;
        public DateTime updatedAt;
    }

    [Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }
}