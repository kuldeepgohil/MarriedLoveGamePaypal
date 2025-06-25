using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordRulesBtnManager : MonoBehaviour
{
    public InputField passwordIF;
    public GameObject eyeButton;
    public GameObject ruleButton;

    private void Start()
    {
        ruleButton.SetActive(true);
        eyeButton.SetActive(false);
        passwordIF.onValueChanged.AddListener(delegate { OnValueChange(passwordIF.text); });
    }

    public void OnValueChange(string text)
    {
        if(text.Length == 0)
        {
            eyeButton.SetActive(false);
            ruleButton.SetActive(true);
        }
        else
        {
            eyeButton.SetActive(true);
            ruleButton.SetActive(false);
        }
    }
}