using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPlane : MonoBehaviour
{

    public GameObject planPurchasePannel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("plan_name") == "Null")
        {
            Debug.Log("sadjhfuijndsafjunsafdhn");
            planPurchasePannel.SetActive(true);
        }
        else
        {
            
            planPurchasePannel.SetActive(false);
        }
    }
}
