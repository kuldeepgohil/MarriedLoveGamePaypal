using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.UI;


public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType type;

    public Image buyBtn;
    public Image tutorialBtn;
    public Image onlyTutorialBtn;

    private void Start()
    {
        if(type == ButtonType.Buy)
        {
            buyBtn.gameObject.SetActive(false);
            tutorialBtn.gameObject.SetActive(false);
        }
        else if(type == ButtonType.Tutorial)
        {
            buyBtn.gameObject.SetActive(false);
            tutorialBtn.gameObject.SetActive(false);
        }
        else
        {
            onlyTutorialBtn.gameObject.SetActive(false);
        }
      
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("The cursor entered the selectable UI element.");

        if (type == ButtonType.Buy)
        {
            buyBtn.gameObject.SetActive(true);
            tutorialBtn.gameObject.SetActive(false);
        }
        else if(type == ButtonType.Tutorial)
        {
            buyBtn.gameObject.SetActive(false);
            tutorialBtn.gameObject.SetActive(true);
        }
        else
        {
            onlyTutorialBtn.gameObject.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("The cursor exit the selectable UI element.");

        if (type == ButtonType.Buy)
        {
            buyBtn.gameObject.SetActive(false);
            tutorialBtn.gameObject.SetActive(false);
        }
        else if (type == ButtonType.Tutorial)
        {
            buyBtn.gameObject.SetActive(false);
            tutorialBtn.gameObject.SetActive(false);
        }
        else
        {
            onlyTutorialBtn.gameObject.SetActive(false);
        }
    }  

    public enum ButtonType
    {
        Buy,
        Tutorial,
        onlytutorial
    };  
    
}

