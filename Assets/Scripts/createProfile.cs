using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine.XR;
using SFB;

[Serializable]
public class createProfile : MonoBehaviour
{
    public GameObject MainPanel;
    string auth;
    public InputField FirstName;
    public InputField LastName;   

    public InputField safeWord;
    public string dobText;
    public string anni_Text;

    public GameObject ErrorPopup;
    public Text EnterAllTxt;

    public GameObject availToyDrop;
    public GameObject roomlocationDrop;
    public GameObject activityDrop;
    public Text MessageText;

    public DateTime birthDate;
    public DateTime aniDate;

    public Toggle termAndCondition;

    public Button buyBtntest;

    //-------Toy-------------------------------------------------
    string formattedToysIDsProfile;
    public Toggle togglePrefab;
    public Transform toggleParent;
    private Dictionary<string, Toggle> toggles = new Dictionary<string, Toggle>(); // Use string keys for UUIDs
    public List<toyData> toys = new List<toyData>();
    public List<string> selectedToyIDs = new List<string>(); // List to store selected toy IDs
    public List<string> selectedToyNames = new List<string>(); // List to store selected toy Names
    public string toySelectionText;
    public Text toySelectionTitleText;

    //-------location-------------------------------------------------
    string formattedlocationIDsProfile;
    public Toggle locationtogglePrefab;
    public Transform locationtoggleParent;
    private Dictionary<string, Toggle> locationtoggles = new Dictionary<string, Toggle>(); // Use string keys for UUIDs
    public List<locationDatum> locations = new List<locationDatum>();
    public List<string> selectedLocationIDs = new List<string>(); // List to store selected
    public List<string> selectedLocationNames = new List<string>(); // List to store selected
    public string locationSelectionText;
    public Text locationSelectionTitleText;


    //--------------------Activity------------------------------------
    public GameObject activitytogglePrefab; // Assign a toggle prefab in the inspector
    public Transform activitytoggleContainer; // A parent container in the UI
    public List<Activity> activities = new List<Activity>();
    public List<Activity> selectedActivities = new List<Activity>();
    string activityjson;
    [SerializeField] public List<ActivityDataToSend> activityDataToSend = new List<ActivityDataToSend>();
    public string activitySelectionText;
    public Text activitySelectionTitleText;

    //-------Profile Pic-----------------------------------------------
    public Button uploadBtn;
    public byte[] imageByte;
    public int uploadValue;

    //--------Select All toggle---------------------------------------------------------
    public Toggle toySelectAllToggle;
    public Toggle locationSelectAllToggle;
    public Toggle actvitySelectAllToggle;

    [System.Serializable]
    public class ActivityDataToSend
    {
        public string activityId;
        public bool isGiving;
        public bool isReceiving;

        public ActivityDataToSend(string id, bool giving, bool receiving)
        {
            activityId = id;
            isGiving = giving;
            isReceiving = receiving;
        } 
    } 

    private void Start()
    {
        
        auth = PlayerPrefs.GetString("SaveLoginToken");

        Debug.LogError(auth);


        imageByte = null;
        //-------Toy-------------------------------------------------
        StartCoroutine(GetToys());

        //---------location-----------------------------------------------

        StartCoroutine(GetLocation());
        //-----------activity---------------------------------------------
        StartCoroutine(FetchActivitiesFromAPI());

        

    }

    public void UploadProfilePic()
    {


        var extensions = new[] {
        new ExtensionFilter("Image Files", "png", "jpg"),
   // new ExtensionFilter("All Files", "*"),
};

        var paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", extensions, false);


        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutine(new Uri(paths[0]).AbsoluteUri));
        }

        /*  NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
          {
              if (path != null)
              {
                  imageByte = System.IO.File.ReadAllBytes(path);
                  uploadBtn.GetComponent<Image>().color = Color.green;
                  uploadValue = 1;
                  Debug.Log("upload image is call .... ");
              }
          });*/

    }

    private IEnumerator OutputRoutine(string path)
    {
        var loader = new WWW(path);
        yield return loader;


        string cleanPath = path.Replace("file:///", "");
        cleanPath = cleanPath.Replace("%20", " ");

        Debug.Log("Path : " + cleanPath);

        imageByte = System.IO.File.ReadAllBytes(cleanPath);

        uploadBtn.GetComponent<Image>().color = Color.green;
        uploadBtn.transform.GetChild(0).GetComponent<Text>().color = Color.green;

        uploadValue = 1;
        Debug.Log("upload image is call .... ");
    }


    private Texture2D MakeTextureReadable(Texture2D texture)
    {
        // Create a new texture that is readable
        Texture2D readableTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        readableTexture.SetPixels(texture.GetPixels()); // Copy pixels from the original texture
        readableTexture.Apply(); // Apply changes
        return readableTexture;
    }

    public void clickAvailToy()
    {
        //availToyDrop.SetActive(!availToyDrop.activeSelf);
        availToyDrop.SetActive(true);
    }
    public void clickroomlocation()
    {
        //roomlocationDrop.SetActive(!roomlocationDrop.activeSelf);
        roomlocationDrop.SetActive(true);
    }

    public void clickactivity()
    {
        //activityDrop.SetActive(!activityDrop.activeSelf);
        activityDrop.SetActive(true);
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
        // Assuming the JSON is an array of Toy objects
        toyRoot toysdata = JsonUtility.FromJson<toyRoot>(json);
        // Add each toy to the toysList
        toys.AddRange(toysdata.data);
        //Debug.Log("Toys loaded: " + toys.Count);
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
                    selectedToyNames.Add(toyName); // Add Name when selected
                    if (selectedToyIDs.Count == toys.Count) //select all
                    {
                        toySelectAllToggle.isOn = true;
                    }
                }
                else
                {
                    selectedToyIDs.Remove(toyID); // Remove ID when deselected
                    selectedToyNames.Remove(toyName); // Remove Name when selected
                    toySelectAllToggle.isOn = false; //select all
                }

                SendSelectedIDsToAPI();

                if(selectedToyNames.Count > 0)
                {
                    toySelectionText = null;

                    for (int i =0; i< selectedToyNames.Count; i++)
                    {
                        toySelectionText += selectedToyNames[i] + ", "; 
                    }

                    if(toySelectionText.Length < 20)
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
                    toySelectionTitleText.text = "Select Available Toys";
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
                    Debug.Log(toy.video_document_url);
                }
            });

            // Use string ID directly as key in the dictionary
            toggles.Add(toyID, toggleInstance);
        }

    }

    private void SendSelectedIDsToAPI()
    {
        // Build the formatted string manually
        formattedToysIDsProfile = "[" + string.Join(",", selectedToyIDs.ConvertAll(id => $"'{id}'")) + "]";
        Debug.Log("Selected Toy IDs sent to API: " + formattedToysIDsProfile);
    }

    IEnumerator GetToySImage(string url, Image img)
    {
        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("error ");
            //show default image
            //allgameList[i].banner = defaultIcon;
            // pp.sprite = Sprite.Create(defalutAvatar, new Rect(0, 0, defalutAvatar.width, defalutAvatar.height), Vector2.zero);

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
        // Assuming the JSON is an array of Toy objects
        LocationRoot locationdata = JsonUtility.FromJson<LocationRoot>(json);
        // Add each toy to the toysList
        locations.AddRange(locationdata.data);
        //Debug.Log("Location loaded: " + locations.Count);
        CreateToggleslocation(locations);
    }

    private void CreateToggleslocation(List<locationDatum> locs)
    {
        //select all

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
            string locName = loca.name;
            toggleInstance.onValueChanged.AddListener((isSelected) =>
            {
                if (isSelected)
                {
                    selectedLocationIDs.Add(toyID); // Add ID when selected
                    selectedLocationNames.Add(locName); // Add Name when selected
                    if (selectedLocationIDs.Count == locs.Count) //select all
                    {
                        locationSelectAllToggle.isOn = true;
                    }
                }
                else
                {
                    selectedLocationIDs.Remove(toyID); // Remove ID when deselected
                    selectedLocationNames.Remove(locName); // Remove Name when selected
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

                    if(locationSelectionText.Length < 20)
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

            // Use string ID directly as key in the dictionary
            locationtoggles.Add(toyID, toggleInstance);
        }
    }

    private void SendSelectedIDsLocation()
    {
        formattedlocationIDsProfile = "[" + string.Join(",", selectedLocationIDs.ConvertAll(id => $"'{id}'")) + "]";
        Debug.Log("Selected Toy IDs sent to API: " + formattedlocationIDsProfile);
    } 
    public void createuserprofile(int profile_type)
    {
        int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        int dob = int.Parse(birthDate.ToString("yyyyMMdd"));
        int age = (now - dob) / 10000;

        if (FirstName.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter First Name";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (LastName.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Last Name";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (safeWord.text.Length == 0)
        {
            EnterAllTxt.text = "Please Enter Safe Word";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (age < 18)
        {
            EnterAllTxt.text = "You Must Be 18+ To Play.";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else if (aniDate <= birthDate)
        {
            EnterAllTxt.text = "Please Enter Valid Anniversary Date";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        } 
        else if(selectedToyIDs.Count==0)
        {
            EnterAllTxt.text = "Please Select Toys";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }  
        else if (selectedLocationIDs.Count==0)
        {
            EnterAllTxt.text = "Please Select Locations";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);

        } 
        else if (selectedActivities.Count==0)
        {
            EnterAllTxt.text = "Please Select Activities";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }   
        else if (termAndCondition.isOn==false)
        {
            EnterAllTxt.text = "Please Agree With Terms & Conditions.";
            EnterAllTxt.gameObject.SetActive(true);
            ErrorPopup.SetActive(true);
        }
        else
        {
            StartCoroutine(createuserprofileRequest(profile_type));
        }
    }


    IEnumerator createuserprofileRequest(int profile_typeValue)
    {
        string userId = PlayerPrefs.GetString("userIDText");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/create-profile";
        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);
        form.AddField("profile_type", profile_typeValue);
        form.AddField("first_name", FirstName.text);
        form.AddField("last_name", LastName.text);
        form.AddField("dob", dobText);
        form.AddField("anniversary_date", anni_Text);
        form.AddField("toys", formattedToysIDsProfile);
        form.AddField("locations", formattedlocationIDsProfile);
        form.AddField("activity", activityjson);

        if(imageByte != null)
        {
            form.AddBinaryData("image", imageByte, $"xyz.png", "image/png");
        }
        else
        {
            Debug.Log("null image");
        }


        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth",auth); 

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.LogError(www.error);
            Debug.Log("createprofileError=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root resetpwdData = JsonUtility.FromJson<Root>(data);
            Debug.Log("data:" + data);

            EnterAllTxt.text = resetpwdData.message;
            ErrorPopup.SetActive(true);
        }
        else
        {
            Debug.Log("createprofilerespose=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            MessageText.gameObject.SetActive(true);
            MessageText.text = "Profile Created Successfully";
            StartCoroutine(closePanel());
        }
    }


    IEnumerator closePanel()
    {
        yield return new WaitForSeconds(3f);

        uploadBtn.GetComponent<Image>().color = Color.white;

        MessageText.gameObject.SetActive(false);
        MainPanel.SetActive(false);

        getProfile.ins.getuserdata();
        checkProfile.ins.checkuserprofile();
        imageByte = null;
    } 

    public void savedata() // close popup
    {
        availToyDrop.SetActive(false);
        roomlocationDrop.SetActive(false);
        activityDrop.SetActive(false);
        imageByte = null;
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
    //--------------------------------------------
    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public string data;
    }
   

    //--------Activity-----------------------------------------------------------------------.

    IEnumerator FetchActivitiesFromAPI()
    {
        string activitylistRequestUrl = commonURLScript.url + "/api/user/activities";
        UnityWebRequest request = UnityWebRequest.Get(activitylistRequestUrl);
        request.SetRequestHeader("auth",auth);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Activity Response=" +jsonResponse);
            APIResponse apiResponse = JsonUtility.FromJson<APIResponse>(jsonResponse);

            foreach (ActivityData activityData in apiResponse.data)
            {
                Activity activity = new Activity
                {
                    Id = activityData._id,
                    Name = activityData.name,
                    Giving = false,
                    Receiving = false,
                    url = activityData.video_document_url
                };
                activities.Add(activity);
            }

            CreateToggleUI();
        }
        else
        {
            Debug.LogError("Error fetching activity data: " + request.error);
        }
    }

    void CreateToggleUI()
    {
        actvitySelectAllToggle.onValueChanged.AddListener((isSelected) =>
        {
            if (isSelected)
            {
                List<Toggle> temp = actvitySelectAllToggle.transform.parent.transform.parent.GetComponentsInChildren<Toggle>().ToList();
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
                List<Toggle> temp = actvitySelectAllToggle.transform.parent.transform.parent.GetComponentsInChildren<Toggle>().ToList();

                bool a = true;

                for (int i = 0; i < temp.Count; i++)
                {
                    if (i > 0)
                    {
                        if (!temp[i].isOn)
                        {
                            a = false;
                            return;
                        }
                    }
                }

                if (a)
                {
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



        foreach (Activity activity in activities)
        {
            GameObject toggleObject = Instantiate(activitytogglePrefab, activitytoggleContainer);
            Toggle givingToggle = toggleObject.transform.Find("GivingToggle").GetComponent<Toggle>();
            Toggle receivingToggle = toggleObject.transform.Find("ReceivingToggle").GetComponent<Toggle>();
            Text toggleLabel = toggleObject.transform.Find("ActivityLabel").GetComponent<Text>();

            //Button tutorialBtn = toggleObject.transform.Find("Tutorial Btn").GetComponent<Button>();
            Button tutorialBtn = toggleObject.transform.GetChild(3).GetComponent<Button>();

            toggleLabel.text = activity.Name;
            givingToggle.isOn = activity.Giving;
            receivingToggle.isOn = activity.Receiving;


            tutorialBtn.onClick.AddListener(() =>
            {
                if (activity.url=="")
                {
                    Application.OpenURL("https://www.google.com/");
                    Debug.Log("s URL");
                    StartCoroutine(UserDataAPI.instance.ClickTipsUrl(activity.Id));
                }
                else
                {
                    Application.OpenURL(activity.url);
                    Debug.Log("s URL"+activity.url);
                    StartCoroutine(UserDataAPI.instance.ClickTipsUrl(activity.Id));
                }
            });
           

            givingToggle.onValueChanged.AddListener((bool isOn) =>
            {
                activity.Giving = isOn;
                SendAllDataToAPI();


                activitySelectionText = null;
                selectedActivities.Clear();
                for (int i = 0; i < activities.Count; i++)
                {
                    if (activities[i].Giving || activities[i].Receiving)
                    {
                        selectedActivities.Add(activities[i]);
                    }
                }

                if(selectedActivities.Count > 0)
                {
                    for(int i = 0; i< selectedActivities.Count; i++)
                    {
                        activitySelectionText += selectedActivities[i].Name + ", ";
                    }

                    if(activitySelectionText.Length < 20)
                    {
                        activitySelectionText = activitySelectionText + "";
                    }
                    else
                    {
                        activitySelectionText = activitySelectionText.Substring(0, 20) + "...";
                    }

                    activitySelectionTitleText.text = activitySelectionText;
                }
                else
                {
                    activitySelectionTitleText.text = "Select Activity";
                }

                if (!isOn)
                {
                    actvitySelectAllToggle.isOn = false;
                }
                else
                {

                    List<Toggle> temp = actvitySelectAllToggle.transform.parent.transform.parent.GetComponentsInChildren<Toggle>().ToList();

                    bool a = true;

                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (!temp[i].isOn)
                            {
                                a = false;
                            }
                        }
                    }

                    if (a)
                    {
                        actvitySelectAllToggle.isOn = true;
                    }
                }
            });

            receivingToggle.onValueChanged.AddListener((bool isOn) =>
            {
                activity.Receiving = isOn;
                SendAllDataToAPI();

                activitySelectionText = null;
                selectedActivities.Clear();
                for (int i = 0; i < activities.Count; i++)
                {
                    if (activities[i].Giving || activities[i].Receiving)
                    {
                        selectedActivities.Add(activities[i]);
                    }
                }

                if (selectedActivities.Count > 0)
                {
                    for (int i = 0; i < selectedActivities.Count; i++)
                    {
                        activitySelectionText += selectedActivities[i].Name + ", ";
                    }

                    if (activitySelectionText.Length < 20)
                    {
                        activitySelectionText = activitySelectionText + "";
                    }
                    else
                    {
                        activitySelectionText = activitySelectionText.Substring(0, 20) + "...";
                    }
                }
                else
                {
                    activitySelectionTitleText.text = "Select Activity";
                }

                if (!isOn)
                {
                    actvitySelectAllToggle.isOn = false;
                }
                else
                {

                    List<Toggle> temp = actvitySelectAllToggle.transform.parent.transform.parent.GetComponentsInChildren<Toggle>().ToList();

                    bool a = true;

                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i > 0)
                        {
                            if (!temp[i].isOn)
                            {
                                a = false;
                            }
                        }
                    }

                    if (a)
                    {
                        actvitySelectAllToggle.isOn = true;
                    }
                }
            });
        }
    }

    [ContextMenu("Test")]
    public void testTemp()
    {
        Application.OpenURL("https://vimeo.com/456237534?autoplay=1&muted=1&stream_id=Y2xpcHN8MTIxMjI1Njg0fGlkOmRlc2N8W10%3D");
    }
   
    void SendAllDataToAPI()
    {
        ////new
        activityDataToSend.Clear();

        foreach (Activity activity in activities)
        {
            activityDataToSend.Add(new ActivityDataToSend(activity.Id, activity.Giving, activity.Receiving));
        }

        string activityDataJson = "";
        foreach (var activityData in activityDataToSend)
        {
            activityDataJson += JsonUtility.ToJson(activityData, true) + ",\n";
        } 

        if (activityDataJson.EndsWith(",\n"))
        {
            activityDataJson = activityDataJson.Substring(0, activityDataJson.Length - 2);
        }


        Debug.Log("Test: {\n" + activityDataJson + "\n}");
        activityjson = "[ \n" + activityDataJson + "\n]";
        Debug.Log("Sending JSON data to API: " + activityjson);
        
    }  

    [System.Serializable]
    private class ListWrapper
    {
        public List<ActivityDataToSend> list;
        public ListWrapper(List<ActivityDataToSend> dataList)
        {
            list = dataList;
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<ActivityDataToSend> list;
        public Wrapper(List<ActivityDataToSend> dataList)
        {
            list = dataList;
        }
    }
    
    //--------Activity-----------------------------------------------------------------------

    [System.Serializable]
    public class Activity
    {
        public string Id;
        public string Name;
        public bool Giving;
        public bool Receiving;
        public string url;
    }

    [System.Serializable]
    public class APIResponse
    {
        public int status;
        public string message;
        public List<ActivityData> data;
    }

    [System.Serializable]
    public class ActivityData
    {
        public string _id;
        public string name;
        public string video_document_url;
    }
}