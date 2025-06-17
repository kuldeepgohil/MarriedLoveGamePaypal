using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GamePlayUIAnimation : MonoBehaviour
{ 
    public static GamePlayUIAnimation ins;
    public GameObject popUpImage;

    public void Awake()
    {
        ins = this;
    }

    public void OpenPopUp(GameObject popup)
    {
        popup.transform.DOScale(Vector3.one, 1f); 
        popup.SetActive(true);
    }
    
    public void ClosePopup(GameObject popup)
    {
        popup.transform.DOScale(Vector3.zero, 1f); 
        popup.SetActive(false);
    }

}
