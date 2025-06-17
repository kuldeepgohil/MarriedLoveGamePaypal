using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

public class loveSetup : MonoBehaviour
{

    public Text ErrorMsg;
    public GameObject ErrorPopup;
    public GameObject LoveSetupPanel;
    public GameObject availToyDrop;
    public GameObject roomlocationDrop;
    public InputField activityNumber;
    public Dropdown leveldropdown; // Drag the Dropdown from the Inspector
    public Dropdown maleDressdropdown; // Drag the Dropdown from the Inspector
    public Dropdown femaleDressdropdown; // Drag the Dropdown from the Inspector
    private List<string> levelIds = new List<string>(); // Store `_id`s for each level
    private List<string> maleDressLevelIds = new List<string>(); // Store `_id`s for each level
    private List<string> femaleDressLevelIds = new List<string>(); // Store `_id`s for each level
    public string defaultLabel = "Select Level"; // Default label text
    string selectedLevelId;
    string selectedMaleLevelId;
    string selectedFemaleLevelId;
    public DressDataRoot dressData;

    //-------Toy-------------------------------------------------
    string formattedToysIDs;
    public Toggle togglePrefab;
    public Transform toggleParent;
    private Dictionary<string, Toggle> toggles = new Dictionary<string, Toggle>(); // Use string keys for UUIDs
    public List<toyData> toys = new List<toyData>();
    public List<string> selectedToyIDs = new List<string>(); // List to store selected toy IDs
    public List<string> selectedToyNames = new List<string>(); // List to store selected toy Names
    public string toySelectionText;
    public Text toySelectionTitleText;

    //-------location-------------------------------------------------
    string formattedlocationIDs;
    public Toggle locationtogglePrefab;
    public Transform locationtoggleParent;
    private Dictionary<string, Toggle> locationtoggles = new Dictionary<string, Toggle>(); // Use string keys for UUIDs
    public List<locationDatum> locations = new List<locationDatum>();
    public List<string> selectedLocationIDs = new List<string>(); // List to store selected
    public List<string> selectedLocationNames = new List<string>(); // List to store selected
    public string locationSelectionText;
    public Text locationSelectionTitleText;
    string auth;

    //--------Select All toggle---------------------------------------------------------
    public Toggle toySelectAllToggle;
    public Toggle locationSelectAllToggle;
     

    [Header("Plan")]
    public string planName;
    int count = 0;


    public string loveSetupId;

    public GameObject lovesetupPanel;

    private void Start()
    {
        auth = PlayerPrefs.GetString("SaveLoginToken");
        leveldropdown.captionText.text = defaultLabel;  

        StartCoroutine(leveldroprequest());
        StartCoroutine(GetToys());
        StartCoroutine(GetLocation());   
        
    }  

    public void clickAvailToy()
    {
        availToyDrop.SetActive(true);
    }
    public void clickroomlocation()
    {
        roomlocationDrop.SetActive(true);
    }

    public IEnumerator leveldroprequest()
    {
        string levelRequestUrl = commonURLScript.url + "/api/user/levels";  

        using (UnityWebRequest request = UnityWebRequest.Get(levelRequestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                var jsonResponse = request.downloadHandler.text;
                var levels = JsonUtility.FromJson<Root>(jsonResponse);
                Debug.Log("LevelResponse = " + request.downloadHandler.text);
                // Populate dropdown and store IDs
                PopulateDropdown(levels.data);
            }
            else
            {
                Debug.LogError("Error fetching levels: " + request.error);
            } 
        } 
        StartCoroutine(Dressleveldroprequest());
    }

    IEnumerator Dressleveldroprequest()
    {
            yield return new WaitForSeconds(3f);
        string levelRequestUrl = commonURLScript.url + "/api/user/dress-levels";  

        using (UnityWebRequest request = UnityWebRequest.Get(levelRequestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                var jsonResponse = request.downloadHandler.text;
                dressData = JsonUtility.FromJson<DressDataRoot>(jsonResponse);
                Debug.Log("DressLevel = " + request.downloadHandler.text);
                // Populate dropdown and store IDs
                PopulateDressLevelDropdown();
            }
            else
            {
                Debug.LogError("Error fetching levels: " + request.error);
            }  
        } 
    }

    void PopulateDropdown(List<Datum> data) 
    {
        leveldropdown.ClearOptions();
        List<string> options = new List<string>();
        options.Add("Select Level");

        planName = PlayerPrefs.GetString("plan_name");
        Debug.LogError(planName);

        foreach (var item in data)
        {
            //if (planName == "basic")  //basic plne
            if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
            {
                Debug.Log("basic is call ...");
                if (count < 2) 
                {
                    options.Add(item.game_level);
                    levelIds.Add(item._id);
                    count++;
                }
            }  
            //else if (planName == "advance") //advnce plne
            else if (planName.Contains("advance") || planName.Contains("Advance")) //advnce plne
            {
                Debug.Log("advance is call ...");
                if (count < 4)
                {
                    options.Add(item.game_level);
                    levelIds.Add(item._id);
                    count++;
                }
            }   
            else if (planName.Contains("premium") || planName.Contains("Premium"))
            {
                Debug.Log("pemium is call ...");
              /*  options.Add(item.game_level); 
                levelIds.Add(item._id);*/

                if (count < 5)
                {
                    options.Add(item.game_level);
                    levelIds.Add(item._id);
                    count++;
                }

            }
            else
            {
                Debug.LogError("no plane found ");
                lovesetupPanel.SetActive(false);
            }
        } 
        
        leveldropdown.AddOptions(options);
        leveldropdown.onValueChanged.AddListener(delegate { OnDropdownChange(); });

        if (options.Count > 0)
        {
            leveldropdown.value = 0;
        }  

    }

    void PopulateDressLevelDropdown()
    {
        maleDressdropdown.ClearOptions();
        femaleDressdropdown.ClearOptions();

        List<string> maleDressOpt = new List<string>();
        List<string> femaleDressOpt = new List<string>();

        maleDressOpt.Add("Select Male Dress Level");
        femaleDressOpt.Add("Select Female Dress Level");

        foreach (var item in dressData.data.dressLevelsMale)
        {
            maleDressLevelIds.Add(item._id);             // Store `_id` for each item
            maleDressOpt.Add(item.dress_code);             // Store `gress code` for each item
        }

        foreach (var item in dressData.data.dressLevelsFemale)
        {
            femaleDressLevelIds.Add(item._id);             // Store `_id` for each item
            femaleDressOpt.Add(item.dress_code);             // Store `gress code` for each item
        }  

        maleDressdropdown.AddOptions(maleDressOpt);
        femaleDressdropdown.AddOptions(femaleDressOpt);

        maleDressdropdown.onValueChanged.AddListener(delegate { OnMaleDressLevelDropdownChange(); });
        femaleDressdropdown.onValueChanged.AddListener(delegate { OnFemaleDressLevelDropdownChange(); });

        if (maleDressOpt.Count > 0)
        {
            maleDressdropdown.value = 0;
            
        }
        if (femaleDressOpt.Count > 0)
        {
            femaleDressdropdown.value = 0; 
        }
    }

    void OnDropdownChange()
    {
        if (leveldropdown.value == 0)
        {
            selectedLevelId = null;
            return;
        }
        selectedLevelId = levelIds[leveldropdown.value-1];
        Debug.Log("selectedId=" + selectedLevelId);
        PlayerPrefs.SetInt("gamelevelIndex", leveldropdown.value);
        
    }

    void OnMaleDressLevelDropdownChange()
    {
        if(maleDressdropdown.value == 0)
        {
            selectedMaleLevelId = null;
            return;
        }
        selectedMaleLevelId = maleDressLevelIds[maleDressdropdown.value - 1];
        Debug.Log("MaleDressSelectedId=" + selectedMaleLevelId);
        PlayerPrefs.SetInt("maleDresLevelIndex", maleDressdropdown.value);
       
    }
    void OnFemaleDressLevelDropdownChange()
    {
        if (femaleDressdropdown.value == 0)
        {
            selectedFemaleLevelId = null;
            return;
        }

        selectedFemaleLevelId = femaleDressLevelIds[femaleDressdropdown.value - 1];
        Debug.Log("FemaleDressSelectedId=" + selectedFemaleLevelId);
        PlayerPrefs.SetInt("femaleDresLevelIndex", femaleDressdropdown.value);
       
    }

    IEnumerator SendSelectedId(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("selectedId", id);

        using (UnityWebRequest request = UnityWebRequest.Post("YOUR_API_URL_FOR_SELECTION", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ID sent successfully.");
            }
            else
            {
                Debug.LogError("Error sending ID: " + request.error);
            }
        }
    }

    //------------Toys------------------------------------------------------

    IEnumerator GetToys()
    {
        string toyapiUrl = commonURLScript.url + "/api/user/toys";
        UnityWebRequest request = UnityWebRequest.Get(toyapiUrl);

        request.SetRequestHeader("auth", auth);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            ParseToys(jsonResponse);
        }

    }

    private void ParseToys(string json)
    {
        toyRoot toysdata = JsonUtility.FromJson<toyRoot>(json);
        toys.AddRange(toysdata.data);
        Debug.Log("Toys loaded: " + toys.Count);
        CreateToggles(toys);
    }

    private void CreateToggles(List<toyData> toys)
    {
        //select all

        toySelectAllToggle.onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                List<Toggle> temp = toySelectAllToggle.transform.parent.GetComponentsInChildren<Toggle>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (i > 0)
                    {
                        temp[i].isOn = true;
                    }
                }
            }
            else
            {
                if (selectedToyNames.Count == toys.Count)
                {
                    List<Toggle> temp = toySelectAllToggle.transform.parent.GetComponentsInChildren<Toggle>().ToList();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i > 0)
                        {
                            temp[i].isOn = false;
                        }
                    }
                }
            }
        });

        foreach (var toy in toys)
        {
            Toggle toggleInstance = Instantiate(togglePrefab, toggleParent);
            toggleInstance.GetComponentInChildren<Text>().text = toy.name;

            Button buyBtn = toggleInstance.transform.GetChild(2).GetComponent<Button>();
            Button tutorialBtn = toggleInstance.transform.GetChild(3).GetComponent<Button>();

            Image toyImage = toggleInstance.transform.GetChild(4).GetComponent<Image>();
            StartCoroutine(GetToySImage(commonURLScript.imgURL + toy.image, toyImage));

            string toyID = toy._id;
            string toyName = toy.name;
            toggleInstance.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    selectedToyIDs.Add(toyID); // Add ID when selected
                    selectedToyNames.Add(toyName);
                    if (selectedToyIDs.Count == toys.Count) //select all
                    {
                        toySelectAllToggle.isOn = true;
                    }
                }
                else
                {
                    selectedToyIDs.Remove(toyID); // Remove ID when deselected
                    selectedToyNames.Remove(toyName);
                    toySelectAllToggle.isOn = false; //select all
                }

                SendSelectedIDsToAPI();

                if (selectedToyNames.Count > 0)
                {
                    toySelectionText = null;

                    for (int i = 0; i < selectedToyNames.Count; i++)
                    {
                        toySelectionText += selectedToyNames[i] + ", ";
                    }

                    if (toySelectionText.Length < 20)
                    {
                        toySelectionText = toySelectionText + "";
                    }
                    else
                    {
                        toySelectionText = toySelectionText.Substring(0, 20) + "...";
                    }

                    toySelectionTitleText.text = toySelectionText;
                }
                else
                {
                    toySelectionTitleText.text = "Available Toys Selection";
                }
            });

            buyBtn.onClick.AddListener(() =>
            {
                if (toy.shop_url == "")
                {
                    Application.OpenURL("https://www.google.com/");
                    Debug.Log("Null URL");
                    StartCoroutine(UserDataAPI.instance.ClickToysUrl(toy._id));
                }
                else
                {
                    Application.OpenURL(toy.shop_url);
                    StartCoroutine(UserDataAPI.instance.ClickToysUrl(toy._id));
                }
            });
            tutorialBtn.onClick.AddListener(() =>
            {
                if (toy.video_document_url == "")
                {
                    Application.OpenURL("https://www.google.com/");
                    Debug.Log("Null URL");
                }
                else
                {
                    Application.OpenURL(toy.video_document_url);
                }
            });
            toggles.Add(toyID, toggleInstance);
        }

    }

    private void SendSelectedIDsToAPI()
    {
        formattedToysIDs = "[" + string.Join(",", selectedToyIDs.ConvertAll(id => $"'{id}'")) + "]";
        PlayerPrefs.SetString("toylist", formattedToysIDs);
        Debug.Log("Selected Toy IDs sent to API: " + formattedToysIDs);
    }


    IEnumerator GetToySImage(string url, Image img)
    {
        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("error ");
        }
        else
        {
            if (w.isDone)
            {
                Texture2D tx = w.texture;
                img.sprite = Sprite.Create(tx, new Rect(0f, 0f, tx.width, tx.height), Vector2.zero, 10f);
            }
        }

    }

    //------------Location------------------------------------------------------

    IEnumerator GetLocation()
    {
        string locationapiUrl = commonURLScript.url + "/api/user/locations";
        UnityWebRequest request = UnityWebRequest.Get(locationapiUrl);

        request.SetRequestHeader("auth", auth);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            Parselocation(jsonResponse);
        }

    }

    private void Parselocation(string json)
    {
        LocationRoot locationdata = JsonUtility.FromJson<LocationRoot>(json);
        locations.AddRange(locationdata.data);
        Debug.Log("Toys loaded: " + locations.Count);
        CreateToggleslocation(locations);
    }

    private void CreateToggleslocation(List<locationDatum> locs)
    {
        locationSelectAllToggle.onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                List<Toggle> temp = locationSelectAllToggle.transform.parent.GetComponentsInChildren<Toggle>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (i > 0)
                    {
                        temp[i].isOn = true;
                    }
                }
            }
            else
            {
                if (selectedLocationNames.Count == locs.Count)
                {
                    List<Toggle> temp = locationSelectAllToggle.transform.parent.GetComponentsInChildren<Toggle>().ToList();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i > 0)
                        {
                            temp[i].isOn = false;
                        }
                    }
                }
            }
        });


        foreach (var loca in locs)
        {
            Toggle toggleInstance = Instantiate(locationtogglePrefab, locationtoggleParent);
            toggleInstance.GetComponentInChildren<Text>().text = loca.name;

            string toyID = loca._id;
            string toyName = loca.name;
            toggleInstance.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    selectedLocationIDs.Add(toyID); // Add ID when selected
                    selectedLocationNames.Add(toyName);
                    if (selectedLocationIDs.Count == locs.Count) //select all
                    {
                        locationSelectAllToggle.isOn = true;
                    }
                }
                else
                {
                    selectedLocationIDs.Remove(toyID); // Remove ID when deselected
                    selectedLocationNames.Remove(toyName);
                    locationSelectAllToggle.isOn = false;
                }

                SendSelectedIDsLocation();

                if (selectedLocationNames.Count > 0)
                {
                    locationSelectionText = null;
                    for (int i = 0; i < selectedLocationNames.Count; i++)
                    {
                        locationSelectionText += selectedLocationNames[i] + ", ";
                    }

                    if (locationSelectionText.Length < 20)
                    {
                        locationSelectionText = locationSelectionText + "";
                    }
                    else
                    {
                        locationSelectionText = locationSelectionText.Substring(0, 20) + "...";
                    }

                    locationSelectionTitleText.text = locationSelectionText;
                }
                else
                {
                    locationSelectionTitleText.text = "Select Locations";
                }

            });
            locationtoggles.Add(toyID, toggleInstance);
        }


    }

    private void SendSelectedIDsLocation()
    {
        formattedlocationIDs = "[" + string.Join(",", selectedLocationIDs.ConvertAll(id => $"'{id}'")) + "]";
        PlayerPrefs.SetString("location", formattedlocationIDs);
        //Debug.Log("Selected Toy IDs sent to API: " + formattedlocationIDs);
    }
    //------------------------------------------------------------------

    public void savedata() // close popup
    {
        availToyDrop.SetActive(false);
        roomlocationDrop.SetActive(false);
    }

    public void lovesetupSubmit()
    {
        if (maleDressdropdown.value == 0)
        {
            Debug.Log("maleDressdropdown select please ");
            ErrorMsg.text = "Please Select Male Dress Level";
            ErrorPopup.SetActive(true);
        }
        else if (femaleDressdropdown.value == 0)
        {
            ErrorMsg.text = "Please Select Female Dress Level";
            ErrorPopup.SetActive(true);
        }
        else if (leveldropdown.value==0)
        {
            ErrorMsg.text = "Please Select Level";
            ErrorPopup.SetActive(true);
        }
        else if(selectedLocationIDs.Count==0)
        {
            ErrorMsg.text = "Please Select Locations";
            ErrorPopup.SetActive(true);
        }  
        else if(activityNumber.text=="")
        {
            ErrorMsg.text = "Please Enter No. of Activities Before Climax ";
            ErrorPopup.SetActive(true);
        }
        else if(selectedToyIDs.Count==0)
        {
            ErrorMsg.text = "Please Select Toys";
            ErrorPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(lovesetupSubmitRequest());
        }

    }

    IEnumerator lovesetupSubmitRequest()
    {
        WWWForm form = new WWWForm();
        Debug.Log(selectedLevelId + "selectedLevelId is call ......... .... ");
          

        PlayerPrefs.SetString("SelectedLevelId", selectedLevelId);
        form.AddField("level_id", selectedLevelId);  

        form.AddField("no_of_activities", activityNumber.text);

        PlayerPrefs.SetString("ActivitiesC", activityNumber.text);

        form.AddField("toys", formattedToysIDs);
        form.AddField("locations", formattedlocationIDs);   

        PlayerPrefs.SetString("MaleDressLevelId", selectedMaleLevelId);  
        form.AddField("male_dress_level_id", selectedMaleLevelId); 


        Debug.Log(selectedMaleLevelId +  selectedFemaleLevelId  + "male and female is call .... " );
        PlayerPrefs.SetString("FeMaleDressLevelId", selectedFemaleLevelId);  
        form.AddField("female_dress_level_id", selectedFemaleLevelId);

        string lovesetupSubmitRequest = commonURLScript.url + "/api/user/set-love-setup";
        UnityWebRequest request = UnityWebRequest.Post(lovesetupSubmitRequest, form);

        request.SetRequestHeader("auth", auth);
        yield return request.SendWebRequest();
          
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("ErrorLoveResponse= " + request.downloadHandler.text);
            string jsonresponse = request.downloadHandler.text;
            LoveSetupRoot loveError = JsonUtility.FromJson<LoveSetupRoot>(jsonresponse);
            ErrorMsg.text = loveError.message;
            ErrorPopup.SetActive(true);   
        }
        else
        {  
            Debug.Log("LoveResponse= " + request.downloadHandler.text);
            string jsonresponse = request.downloadHandler.text;
            LoveSetupRoot loveError = JsonUtility.FromJson<LoveSetupRoot>(jsonresponse);

            loveSetupId = loveError.data.ToString();
            PlayerPrefs.SetString("loveSetupId",loveSetupId);  

            LoveSetupPanel.SetActive(false);
            SceneManager.LoadScene("Game Scene");  

        }

    }

}

//-------DressLevel--------------------------

[Serializable]
public class DressLevelData
{
    public List<DressLevelsMale> dressLevelsMale;
    public List<DressLevelsFemale> dressLevelsFemale;
}

[Serializable]
public class DressLevelsFemale
{
    public string _id;
    public string dress_code;
    public string gender;
}

[Serializable]
public class DressLevelsMale
{
    public string _id;
    public string dress_code;
    public string gender;
}

[Serializable]
public class DressDataRoot
{
    public int status;
    public string message;
    public DressLevelData data;
}

//------Level------------------------

[System.Serializable]
public class Datum
{
    public string _id;
    public int level;
    public string dress_code; //make it seperate for both gender
    public string game_level;
}

[System.Serializable]
public class Root
{
    public int status;
    public List<Datum> data;
}
///-------------toy list------------------------------------
[System.Serializable]
public class toyData
{
    public string _id;
    public string name;
    public string image;
    public string shop_url;
    public string video_document_url;
}

[System.Serializable]
public class toyRoot
{
    public int status;
    public string message;
    public List<toyData> data;

}

//------------------location------------------------------------------

[System.Serializable]
public class locationDatum
{
    public string _id;
    public string name;
}

[System.Serializable]
public class LocationRoot
{
    public int status;
    public string message;
    public List<locationDatum> data;
}

public class LoveSetupRoot
{
    public int status;
    public string message;
    public string data; 
}