using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCutturCustomSizeHandler : MonoBehaviour
{

    public float sizeMultiplier;
    public RectTransform selectionRect;

    // Start is called before the first frame update
    void Start()
    {
        sizeMultiplier = 1.0f;
        selectionRect.sizeDelta = new Vector2(248, 424) * sizeMultiplier;
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Plus();
            selectionRect.sizeDelta = new Vector2(248, 424) * sizeMultiplier;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Minus();
            selectionRect.sizeDelta = new Vector2(248, 424) * sizeMultiplier;
        }
    }

    public void Plus()
    {
        sizeMultiplier += .2f;
    }
    public void Minus()
    {
        sizeMultiplier -= .2f;
    }
}
