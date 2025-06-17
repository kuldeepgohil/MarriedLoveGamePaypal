using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

public class toysListing : MonoBehaviour
{

    //-----------------------------------------------------

    public GameObject togglePrefab; // Assign a Toggle prefab with a Text component
    public Transform toggleParent; // Assign the parent transform for toggles
    public Button saveButton;

    private List<string> selectedIds = new List<string>();

    //-----------------------------------------------------





    [System.Serializable]
    public class Datum
    {
        public string _id;
        public string name;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<Datum> data;
    }


    [System.Serializable]
    private class SelectedIdList
    {
        public string[] selectedIds;
    }
    public void gettoyslist()
    {
        StartCoroutine(gettoyslistrequest());
    }

    IEnumerator gettoyslistrequest()
    {
        WWWForm form = new WWWForm();
        string auth = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3MWY2NWI0ZDc3ZDcwYTdjMWEzMjE3NyIsImlhdCI6MTczMDExMDkyNiwiZXhwIjoxNzMwNzE1NzI2fQ.nZ4DzAKFDtHrvP4PYxeQlEAjKiqLegObmF1eIHxDkc8";
        //string auth = PlayerPrefs.GetString("SaveLoginToken");
        string gettoyslistrequestUrl = commonURLScript.url + "/api/user/toys";




        UnityWebRequest www = UnityWebRequest.Get(gettoyslistrequestUrl);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        //Debug.Log(www.downloadHandler.text);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Response=" + www.downloadHandler.text);

            string jsonresponse = www.downloadHandler.text;
            Root response = JsonUtility.FromJson<Root>(jsonresponse);

            // Generate toggles for each item
            foreach (Datum item in response.data)
            {
                GameObject newToggle = Instantiate(togglePrefab, toggleParent);
                Toggle toggle = newToggle.GetComponent<Toggle>();
                Text toggleText = newToggle.GetComponentInChildren<Text>();
                toggleText.text = item.name;

                toggle.onValueChanged.AddListener((isSelected) => OnToggleValueChanged(isSelected, item._id));
            }

            // Add save button listener
            saveButton.onClick.AddListener(SaveSelection);
        }
    }


    private void OnToggleValueChanged(bool isSelected, string id)
    {
        if (isSelected)
        {
            if (!selectedIds.Contains(id))
                selectedIds.Add(id);
        }
        else
        {
            selectedIds.Remove(id);
        }
    }

    private void SaveSelection()
    {
        StartCoroutine(SendSelectionToAPI());
    }

    private IEnumerator SendSelectionToAPI()
    {
        string url = "your_api_endpoint_here";

        // Convert the selected IDs to JSON format
        SelectedIdList selectedIdList = new SelectedIdList { selectedIds = selectedIds.ToArray() };
        string jsonBody = JsonUtility.ToJson(selectedIdList);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
    }

}
