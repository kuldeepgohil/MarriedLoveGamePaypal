using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentMethodType : MonoBehaviour
{
    public static PaymentMethodType instance;  
    public string selectedPaymentMethod;

    public void Awake()
    {
        instance = this;
    }

    public void OnButtonClick(string message)
    {
        selectedPaymentMethod = $"{message}"; // String formatted
        Debug.Log(selectedPaymentMethod);
    }  

}