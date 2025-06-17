using UnityEngine;
using UnityEngine.UI;

public class TextLimit : MonoBehaviour
{
    // Reference to the Text component
    public Text textComponent;

    // Set the character limit
    public int characterLimit = 10;
    public int characterLimits = 15;

    // Set the text with a limit
    public string fullText;

    void Start()
    {
        if (textComponent != null)
        {
            // Limit the text length to the specified character limit
            textComponent.text = fullText.Length > characterLimit ? fullText.Substring(0, characterLimit) : fullText;
        }
    }


    public void clickkk()
    {
        if (textComponent != null)
        {
            // Limit the text length to the specified character limit
            textComponent.text = fullText.Length > characterLimit ? fullText.Substring(0, characterLimits) : fullText;
        }
    }
}
