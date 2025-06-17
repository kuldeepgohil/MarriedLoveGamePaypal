using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class faqScript : MonoBehaviour
{
    [Header("Prefab and Parent Setup")]
    public GameObject faqPrefab;         // Reference to the FAQ prefab
    public Transform parentTransform;    // Parent transform to hold instantiated prefabs

    [Header("API URL")]
    string apiUrl; // Replace with your API endpoint

    // Character limits for toggling
    private int minCharacterLimit = 170;
    private int maxCharacterLimit = 2000;

    void Start()
    {
        apiUrl = commonURLScript.url + "/api/user/faqs";
        StartCoroutine(FetchFAQData());
    }

    // Coroutine to fetch FAQ data from API
    private IEnumerator FetchFAQData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                FAQResponse faqResponse = JsonUtility.FromJson<FAQResponse>(request.downloadHandler.text);

                // Instantiate FAQ items
                foreach (var faq in faqResponse.data)
                {
                    if (faq.is_active && !faq.is_deleted)
                    {
                        GameObject faqItem = Instantiate(faqPrefab, parentTransform);
                        SetupFAQItem(faqItem, faq.question, faq.answer);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to fetch FAQ data: " + request.error);
            }
        }
    }

    // Setup method for each FAQ item
    private void SetupFAQItem(GameObject faqItem, string question, string answer)
    {
        // Get references to UI elements in the prefab
        Text questionText = faqItem.transform.Find("QuestionText").GetComponent<Text>();
        Text answerText = faqItem.transform.Find("AnswerText").GetComponent<Text>();
        Button toggleButton = faqItem.transform.GetComponent<Button>();

        // Assign question text
        questionText.text = question;

        // Handle answer text and toggle functionality
        string fullAnswer = answer;
        bool isExpanded = false;

        string readMoreText = "<b><size=18>Read More...</size></b>";

        // Display initial limited text
        answerText.text = fullAnswer.Length > minCharacterLimit
            ? fullAnswer.Substring(0, minCharacterLimit) + "                   " + readMoreText
            : fullAnswer;

        // Add listener to the toggle button
        toggleButton.onClick.AddListener(() =>
        {
            isExpanded = !isExpanded;
            answerText.text = isExpanded
                ? (fullAnswer.Length > maxCharacterLimit ? fullAnswer.Substring(0, maxCharacterLimit) : fullAnswer)
                : (fullAnswer.Length > minCharacterLimit ? fullAnswer.Substring(0, minCharacterLimit) + "                   " + readMoreText : fullAnswer);
                //: (fullAnswer.Length > minCharacterLimit ? fullAnswer.Substring(0, minCharacterLimit) : fullAnswer);
            StartCoroutine(FixSize());
        });
    }

    IEnumerator FixSize()
    {
        parentTransform.GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForSeconds(.1f);
        parentTransform.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

}

// Class to represent the JSON structure of the API response
[System.Serializable]
public class FAQResponse
{
    public List<FAQData> data;
}

// Class to represent each FAQ item in the response
[System.Serializable]
public class FAQData
{
    public string _id;
    public string question;
    public string answer;
    public bool is_active;
    public bool is_deleted;
}
