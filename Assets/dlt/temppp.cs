using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temppp : MonoBehaviour
{
    public GameObject pop;  

    public void cclicknew()
    {
        pop.SetActive(true);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
