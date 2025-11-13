using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextProcessor : MonoBehaviour, IPointerClickHandler
{
    private static readonly string urlPattern = @"(https?://[^\s]+)";

    public TMP_Text uiText; // Reference to your TMP_Text component

    // Method to format the text, highlighting URLs and making them clickable
    public string FormatTextWithLinks(string inputText)
    {
        string formattedText = Regex.Replace(inputText, urlPattern, match =>
        {
            // Wrap URLs in <link> tags to make them clickable
            return $"<link={match.Value}><color=blue><u>{match.Value}</u></color></link>";
        });

        return formattedText;
    }

    public void SetupText()
    {
        if (uiText == null)
            uiText = GetComponent<TMP_Text>(); // Make sure the TMP_Text is referenced

        // Example backend text with a URL
        string backendText = uiText.text;
        uiText.text = FormatTextWithLinks(backendText);
    }

    // Method to open a URL in the browser
    private void OpenURL(string url)
    {
        // Check if URL is valid
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("The URL is empty or invalid!");
            return;
        }

        Application.OpenURL(url); // This should open the URL in the browser
    }

    // Pointer click event handler
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Clicked"); // Debug message to confirm the click

        // Check if the click intersects with a link
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(uiText, eventData.position, null);

        // If a URL was clicked
        if (linkIndex != -1)
        {
            // Get the link ID (URL)
            string url = uiText.textInfo.linkInfo[linkIndex].GetLinkID();
            Debug.Log("Opening URL: " + url); // Log the URL being opened
            OpenURL(url); // Open the URL in the browser
        }
        else
        {
            Debug.LogWarning("No URL clicked at this position.");
        }
    }
}