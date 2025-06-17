using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_Script : MonoBehaviour
{
    public Image handleImg;

    public Sprite on_Img, Off_Img;

    private void Awake()
    {
        OnValueChange(gameObject.GetComponent<Toggle>().isOn);
    }

    public void OnValueChange(bool value)
    {
        handleImg.rectTransform.anchoredPosition = value ? new Vector2(30, 0) : new Vector2(-30, 0);
        handleImg.sprite = value ? on_Img : Off_Img;
    }
}
