using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createProfileBtns : MonoBehaviour
{ 

    public GameObject MaleProfilePanel;
    public GameObject FemaleProfilePanel;

    public void clickMaleProfile()
    {
        MaleProfilePanel.SetActive(true);
    }
    public void clickFemaleProfile()
    { 
        FemaleProfilePanel.SetActive(true); 
    } 

}
