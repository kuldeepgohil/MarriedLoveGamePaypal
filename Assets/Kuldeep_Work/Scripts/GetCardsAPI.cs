using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using Random = UnityEngine.Random;
using UnityEngine.Analytics;

public class GetCardsAPI : MonoBehaviour
{
    [System.Serializable]
    public class Activity
    {
        public string activityName;
        public string activityId;
        public bool giving;
        public bool receiving;
    }

    [System.Serializable]
    public class Data
    {
        public string _id;
        public string card_number;
        public int activity_timer;
        public string gender;
        public string activity_description;
        public string plan;
        public List<Activity> activity;
        public string orgasm_allowed;
        public bool remove_clothing;
        public bool game_level_up;
        public bool is_active;
        public bool is_paid;
        public string url;
        public DateTime createdAt;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public Data data;
    }

    public static GetCardsAPI Instance;
    string auth;

    public List<Data> getcardsAPIData = new List<Data>();

    public string cardjson;

    [SerializeField] public List<Data> usedCardDataToSend = new List<Data>();

    public Text activityText;
    public Text orgasmAllowedText;
    public Text removeClothingText;
    public Text gameLevelUpText;

    public string activityTimer;
    public string currentCardID;

    public Button urlBtn;
    public Button retryImageBtn;
    public Button startImageBtn;

    public GameObject timertex;

    public ScrollRect scrollRect;

    public Button okRepetCard;
    
    public VerticalLayoutGroup verticalLayoutGroup;

    [Header("Plan")]
    public string planName;
    int count = 0;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        auth = PlayerPrefs.GetString("SaveLoginToken");
        Debug.LogError(auth);

    }

    public IEnumerator GetCardsRequest()
    {
        yield return new WaitForSeconds(0.5f);

        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/cards";
        WWWForm form = new WWWForm();

        int gameLevel = GameManager.Instance.gameLevel;
        string gameLevelID;

        switch (gameLevel)
        {
            case 1:
                {
                    gameLevelID = StaticId.Instance.level1;
                    break;
                }
            case 2:
                {
                    gameLevelID = StaticId.Instance.level2;
                    break;
                }
            case 3:
                {
                    gameLevelID = StaticId.Instance.level3;
                    break;
                }
            case 4:
                {
                    gameLevelID = StaticId.Instance.level4;
                    break;
                }
            default:
                {
                    gameLevelID = StaticId.Instance.level5;
                    break;
                }
        }

        int maleDressLevel = GameManager.Instance.maleDressLevel;
        string maleDressLevelID; 

        switch (maleDressLevel)
        {
            case 1:
                {
                    maleDressLevelID = StaticId.Instance.mlevel1;
                    break;
                }
            case 2:
                {
                    maleDressLevelID = StaticId.Instance.mlevel2;
                    break;
                }
            case 3:
                {
                    maleDressLevelID = StaticId.Instance.mlevel3;
                    break;
                }
            case 4:
                {
                    maleDressLevelID = StaticId.Instance.mlevel4;
                    break;
                }
            default:
                {
                    maleDressLevelID = StaticId.Instance.mlevel5;
                    break;
                }   
        }

        int femaleDressLevel = GameManager.Instance.femaleDressLevel;
        string femaleDressLevelID;

        switch (femaleDressLevel)
        {
            case 1:
                {
                    femaleDressLevelID = StaticId.Instance.felevel1;
                    break;
                }
            case 2:
                {
                    femaleDressLevelID = StaticId.Instance.felevel2;
                    break;
                }
            case 3:
                {
                    femaleDressLevelID = StaticId.Instance.felevel3;
                    break;
                }
            case 4:
                {
                    femaleDressLevelID = StaticId.Instance.felevel4;
                    break;
                }
            default:
                {
                    femaleDressLevelID = StaticId.Instance.felevel5;
                    break;
                }
        }

        string SelectedLevelId = PlayerPrefs.GetString("SelectedLevelId");
        form.AddField("levelId", gameLevelID);

        string CardCategory = PlayerPrefs.GetString("CardCategory").Trim();
        form.AddField("cardCategory", CardCategory);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            form.AddField("gender", "female");
            string femaledressId = PlayerPrefs.GetString("FeMaleDressLevelId");
            form.AddField("dressLevelId", femaleDressLevelID);
        }
        else
        {
            form.AddField("gender", "male");
            string maledressId = PlayerPrefs.GetString("MaleDressLevelId");
            form.AddField("dressLevelId", maleDressLevelID);
        }

        if (GameManager.Instance.usedCard.Count > 0)
        {
            form.AddField("cardList", GameManager.Instance.usedCards);
        }
        else
        {
            form.AddField("cardList", "");
        } 

        if (GameManager.Instance.activitiesBeforeClimex <= 0)
        { 
            int randomNumber = Random.Range(1, 6);

            if (randomNumber == 4)
            {
                form.AddField("orgasm", "yes");
                //Debug.LogError("orgasm yes is called...");
            }
            else
            {
                form.AddField("orgasm", "no");
            }   

        }
        else
        {
            form.AddField("orgasm", "no");
            //Debug.LogError("orgasm no is call ...");
        }

        string lovesetupid = PlayerPrefs.GetString("loveSetupId");
        Debug.LogError(lovesetupid + " get card love setup id is call ....");
        form.AddField("loveSetupId", lovesetupid);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("GetCardsError=" + www.downloadHandler.text);
            var data = www.downloadHandler.text;
            Root resetpwdData = JsonUtility.FromJson<Root>(data);
            Debug.Log("data:" + data);
            Debug.Log("Error:" + www.error);

            activityText.text = resetpwdData.message.ToString();
            orgasmAllowedText.gameObject.SetActive(false);
            removeClothingText.gameObject.SetActive(false);
            gameLevelUpText.gameObject.SetActive(false);

        }
        else  
        {  
            var data = www.downloadHandler.text;
            Root resetpwdData = JsonUtility.FromJson<Root>(data);
            Debug.LogError("Get card API " + data);

            if (resetpwdData.message == "No any matching card found")
            {     
                Debug.LogError("No any matching card found 222222222222"); 

                if(GameManager.Instance.usedCard.Count==0)
                {
                    Debug.LogError("No any matching card found 3333333333333"); 
                    verticalLayoutGroup.padding.top = 250;
                    activityText.text = "No Cards Match Game and Profile Settings";
                    StartCoroutine(Onn());
                    okRepetCard.gameObject.SetActive(false);
                }
                else
                {  
                    //check for which list for clear  
                    //GameManager.Instance.usedCard.Clear();

                    if(GameManager.Instance.curTurn == Turn.Male && GameManager.Instance.playerFemale.GetComponent<FollowThePath>().curIndex==18)
                    {
                        GameManager.Instance.secretDeckCardFeMale.Clear();
                    }
                    else if (GameManager.Instance.curTurn == Turn.Male && GameManager.Instance.playerFemale.GetComponent<FollowThePath>().curIndex == 14)
                    {
                        GameManager.Instance.showMeandTeaseMeDeckCardFeMale.Clear();
                    }
                    else
                    {
                        GameManager.Instance.activityDeckCardFeMale.Clear();
                    }

                    if (GameManager.Instance.curTurn == Turn.Female && GameManager.Instance.playerMale.GetComponent<FollowThePath>().curIndex == 18)
                    {
                        GameManager.Instance.secretDeckCardMale.Clear();
                    }
                    else if (GameManager.Instance.curTurn == Turn.Female && GameManager.Instance.playerMale.GetComponent<FollowThePath>().curIndex == 14)
                    {
                        GameManager.Instance.showMeandTeaseMeDeckCardMale.Clear();
                    }
                    else
                    {
                        GameManager.Instance.activityDeckCardMale.Clear();
                    }

                    verticalLayoutGroup.padding.top = 250;
                    activityText.text = "You Don't Have New Cards The Old Cards Will Repeat";
                    StartCoroutine(Onn());
                    okRepetCard.gameObject.SetActive(true);  
                    GamePlayUIAnimation.ins.ClosePopup(UIManager.Instance.perFormActivity);   

                }

                Debug.Log("Turn change please ");
               
                timertex.gameObject.SetActive(false);
                retryImageBtn.gameObject.SetActive(true);
                startImageBtn.gameObject.SetActive(false);
                urlBtn.gameObject.SetActive(false); 
                orgasmAllowedText.gameObject.SetActive(false);
                removeClothingText.gameObject.SetActive(false);
                gameLevelUpText.gameObject.SetActive(false); 

            }
            else
            {
                if (StaticId.Instance.ActivityName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Male)
                {
                    GameManager.Instance.activityDeckCardFeMale.Add(resetpwdData.data._id.ToString());
                }
                else if (StaticId.Instance.ShowmeTeasemeName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Male)
                {
                    GameManager.Instance.showMeandTeaseMeDeckCardFeMale.Add(resetpwdData.data._id.ToString());
                }
                else if (StaticId.Instance.PunishmentName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Male)
                {
                    GameManager.Instance.secretDeckCardFeMale.Add(resetpwdData.data._id.ToString());
                }

                if (StaticId.Instance.ActivityName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Female)
                {
                    GameManager.Instance.activityDeckCardMale.Add(resetpwdData.data._id.ToString());
                }
                else if (StaticId.Instance.ShowmeTeasemeName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Female)
                {
                    GameManager.Instance.showMeandTeaseMeDeckCardMale.Add(resetpwdData.data._id.ToString());
                }
                else if (StaticId.Instance.PunishmentName == resetpwdData.data.card_number.Substring(0, 3) && GameManager.Instance.curTurn == Turn.Female)
                {
                    GameManager.Instance.secretDeckCardMale.Add(resetpwdData.data._id.ToString());
                }

                GameManager.Instance.usedCard.Clear();
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.activityDeckCardMale);
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.secretDeckCardMale);
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.showMeandTeaseMeDeckCardMale);
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.activityDeckCardFeMale);
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.secretDeckCardFeMale);
                GameManager.Instance.usedCard.AddRange(GameManager.Instance.showMeandTeaseMeDeckCardFeMale);

                verticalLayoutGroup.padding.top = 30;
                timertex.gameObject.SetActive(true);
                retryImageBtn.gameObject.SetActive(false);
                startImageBtn.gameObject.SetActive(true);
                urlBtn.gameObject.SetActive(true);
                orgasmAllowedText.gameObject.SetActive(true);
                removeClothingText.gameObject.SetActive(true);
                gameLevelUpText.gameObject.SetActive(true);
                okRepetCard.gameObject.SetActive(false);

                if (resetpwdData.data == null)
                {
                    string originalStrings = resetpwdData.message.ToString();
                    activityText.text = originalStrings;
                }
                if (resetpwdData.data.activity == null || resetpwdData.data.activity.Count == 0)
                {
                    string originalStringss = resetpwdData.message.ToString();
                    activityText.text = originalStringss;
                }
                else
                { 
                    string originalString = resetpwdData.data.activity_description.ToString();
                    //Debug.LogError(originalString);
                    string player1Name = PlayerPrefs.GetString("MaleFirstName");
                    string player2Name = PlayerPrefs.GetString("FeMaleFirstName");

                    GameManager.Instance.activitiesBeforeClimex--;
                    PlayerPrefs.SetString("ActivitiesC", GameManager.Instance.activitiesBeforeClimex.ToString());  

                    string updatedString = originalString.Replace("<male>", player1Name).Replace("<female>", player2Name);
                    activityText.text = updatedString;
                    StartCoroutine(Onn());

                    currentCardID = resetpwdData.data._id;
                    orgasmAllowedText.gameObject.SetActive(true);
                    removeClothingText.gameObject.SetActive(true);
                    gameLevelUpText.gameObject.SetActive(true);

                    if (resetpwdData.data.orgasm_allowed.ToString() == "both" || resetpwdData.data.orgasm_allowed.ToString() == "neither")
                    {
                        orgasmAllowedText.text = "OrgasmAllowed: " +
                   "<color=white><size=35>" + resetpwdData.data.orgasm_allowed.ToString().ToUpper() + "</size></color>";
                    }
                    else if (resetpwdData.data.orgasm_allowed.ToString() == "male")
                    {
                        GameManager.Instance.maleOrgasm=true;
                        orgasmAllowedText.text = "OrgasmAllowed: " +
                "<color=white><size=35>" + PlayerPrefs.GetString("MaleFirstName").ToUpper() + "</size></color>";

                    }
                    else                 
                    {
                        GameManager.Instance.feMaleOrgasm=true;
                        orgasmAllowedText.text = "OrgasmAllowed: " +
                "<color=white><size=35>" + PlayerPrefs.GetString("FeMaleFirstName").ToUpper() + "</size></color>";  

                    } 
                    
                    if (resetpwdData.data.remove_clothing.ToString() == "False")
                    {
                        removeClothingText.gameObject.SetActive(false);
                    }
                    else
                    {   

                        removeClothingText.gameObject.SetActive(true);

                        if (GameManager.Instance.curTurn == Turn.Male)
                        {
                            Debug.Log("female dress level update is call ...");
                            FemaleDressLevelUp();
                        }
                        else
                        {
                            Debug.Log("male dress level update is call ...");
                            MaleDressLevelUP();
                        } 
                        
                    } 

                    if (resetpwdData.data.game_level_up.ToString() == "False")
                    {
                        gameLevelUpText.gameObject.SetActive(false);
                    }
                    else
                    {
                        gameLevelUpText.gameObject.SetActive(true);
                        planName = PlayerPrefs.GetString("plan_name");
                        Debug.LogError(planName);

                        if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
                        {
                            Debug.Log("basic is call ..."); 

                            if (count < 2)
                            {
                                count++;
                                GameManager.Instance.gameLevel++;
                            }

                        }
                        else if (planName.Contains("advance") || planName.Contains("Advance")) //advnce plne
                        {
                            Debug.Log("advance is call ..."); 

                            if (count < 4)
                            {
                                count++;
                                GameManager.Instance.gameLevel++;
                            }

                        } 
                        else if (planName.Contains("premium") || planName.Contains("Premium"))
                        {
                            if (count < 5)
                            {
                                count++;
                                GameManager.Instance.gameLevel++;
                            }
                        }
                        else
                        {
                            Debug.LogError("no plane found ");
                        }

                    } 
                    
                    urlBtn.onClick.AddListener(() =>
                    {
                        if (resetpwdData.data.url == "")
                        {
                            Application.OpenURL("https://www.google.com/");
                            Debug.Log("Null URL");
                        }
                        else
                        {
                            Application.OpenURL(resetpwdData.data.url);
                        } 

                    }); 
                    activityTimer = resetpwdData.data.activity_timer.ToString();
                    Timer.Instance.SetTime(activityTimer);
                }  
            }
        }
    }

    [ContextMenu("on")]
    public IEnumerator Onn()
    {
        yield return new WaitForSeconds(2.3f);
        scrollRect.verticalNormalizedPosition = 1;
    }
    
    public void MaleDressLevelUP()
    {
        if (GameManager.Instance.maleDressLevel < 5)
        {
            int maleDressindex = GameManager.Instance.maleDressLevel;
            string currentMaleDressLevelID;

            switch (maleDressindex)
            {
                case 1:
                    {
                        currentMaleDressLevelID = StaticId.Instance.maleDress1;
                        break;
                    }
                case 2:
                    {
                        currentMaleDressLevelID = StaticId.Instance.maleDress2;
                        break;
                    }
                case 3:
                    {
                        currentMaleDressLevelID = StaticId.Instance.maleDress3;
                        break;
                    }
                case 4:
                    {
                        currentMaleDressLevelID = StaticId.Instance.maleDress4;
                        break;
                    }
                default:
                    {
                        currentMaleDressLevelID = StaticId.Instance.maleDress5;
                        break;
                    }
            }

            GameManager.Instance.maleDressLevel++;
            maleDressindex = GameManager.Instance.maleDressLevel;
            string maleDressLevelID;

            switch (maleDressindex)
            {
                case 1:
                    {
                        maleDressLevelID = StaticId.Instance.maleDress1;
                        break;
                    }
                case 2:
                    {
                        maleDressLevelID = StaticId.Instance.maleDress2;
                        break;
                    }
                case 3:
                    {
                        maleDressLevelID = StaticId.Instance.maleDress3;
                        break;
                    }
                case 4:
                    {
                        maleDressLevelID = StaticId.Instance.maleDress4;
                        break;
                    }
                default:
                    {
                        maleDressLevelID = StaticId.Instance.maleDress5;
                        break;
                    }

            }

            //messageTxt.text = currentMaleDressLevelID + " To " + maleDressLevelID;


        }

    }  

    public void FemaleDressLevelUp()
    {

        if (GameManager.Instance.femaleDressLevel < 5)
        {
            int femaleDressindex = GameManager.Instance.femaleDressLevel;
            string currentfemaleDressLevelID;

            switch (femaleDressindex)
            {
                case 1:
                    {
                        currentfemaleDressLevelID = StaticId.Instance.femaleDress1one;
                        break;
                    }
                case 2:
                    {
                        currentfemaleDressLevelID = StaticId.Instance.femaleDress2one;
                        break;
                    }
                case 3:
                    {
                        currentfemaleDressLevelID = StaticId.Instance.femaleDress3one;
                        break;
                    }
                case 4:
                    {
                        currentfemaleDressLevelID = StaticId.Instance.femaleDress4one;
                        break;
                    }
                default:
                    {
                        currentfemaleDressLevelID = StaticId.Instance.femaleDress5one;
                        break;
                    }
            }

            GameManager.Instance.femaleDressLevel++;
            femaleDressindex = GameManager.Instance.femaleDressLevel;
            string femaleDressLevelID;

            switch (femaleDressindex)
            {
                case 1:
                    {
                        femaleDressLevelID = StaticId.Instance.femaleDress1one; 
                        break;
                    }
                case 2:
                    {
                        femaleDressLevelID = StaticId.Instance.femaleDress2one;
                        break;
                    }
                case 3:
                    {
                        femaleDressLevelID = StaticId.Instance.femaleDress3one;
                        break;
                    }
                case 4:
                    {
                        femaleDressLevelID = StaticId.Instance.femaleDress4one;
                        break;
                    }
                default:
                    {
                        femaleDressLevelID = StaticId.Instance.femaleDress5one;
                        break;
                    }
            }

            //messageTxt.text = currentfemaleDressLevelID + " To " + femaleDressLevelID;


        }

    }

    

}