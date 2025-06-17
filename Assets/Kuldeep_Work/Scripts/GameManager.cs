using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] place;
    public GameObject[] placeFemale;
    public Sprite[] placeSprite;

    public string[] placeName;
    public string[] spaceName;  

    public string[] deskName;

    public Image placePopUpBg;
    public Image ywllowplacePopUpBg;


    public Text placeNameText;
    public Text SpaceNameText;

    public Text deskNameText;

    public GameObject playerMale;
    public GameObject playerFemale;


    private bool isMale;
    private bool isFemale;

    private FollowThePath playerMovement;
    public Turn curTurn;

    [SerializeField] public List<string> usedCard;

    public List<string> activityDeckCardMale;
    public List<string> secretDeckCardMale;
    public List<string> showMeandTeaseMeDeckCardMale;
    
    public List<string> activityDeckCardFeMale;
    public List<string> secretDeckCardFeMale;
    public List<string> showMeandTeaseMeDeckCardFeMale;

    public int maleCurWayPointIndex;
    public int femaleCurWayPointIndex;

    public int maleCurIndex;
    public int femaleCurIndex;
    public string lastTrun;
      
    public string usedCards;

    public int gameLevel;
    public int maleDressLevel;
    public int femaleDressLevel;

    public int activitiesBeforeClimex;
    public string location;
    public string toys;

    public int malePoints;
    public int feMalePoints;

    public string gameID;

    public string gameLevelName;

    public string loveSetupId;

    [Header("Game Over")]
    public bool maleOrgasm;
    public bool feMaleOrgasm;

    public GameObject gameoverPanel;  

    public void Awake()
    {
        Instance = this;  
    }

    public void Start()
    { 

        isMale = true;
        isFemale = false;
        curTurn = Turn.Female;
        
        if (playerMale != null)
        {
            playerMovement = playerMale.GetComponent<FollowThePath>();
        }

        gameoverPanel.SetActive(false); 

        gameID = null;
        LoadGame();

        maleOrgasm = false;
        feMaleOrgasm = false;

        gameLevel = PlayerPrefs.GetInt("gamelevelIndex");
        gameLevelName = PlayerPrefs.GetString("plan_name");
        maleDressLevel = PlayerPrefs.GetInt("maleDresLevelIndex");
        femaleDressLevel = PlayerPrefs.GetInt("femaleDresLevelIndex");
        loveSetupId = PlayerPrefs.GetString("loveSetupId");

    }

    public void SkipTurn()
    {
        if (curTurn == Turn.Male)
        {
            Debug.Log("Skipping Male's turn...");
            curTurn = Turn.Female;  //test

        }
        else if (curTurn == Turn.Female)
        {
            Debug.Log("Skipping Female's turn...");
            curTurn = Turn.Male;  //test
        }  
        
    } 

    public void Update()
    { 
        if (curTurn == Turn.Female && playerMale.GetComponent<FollowThePath>().curIndex == 3)
        { 
            if (maleDressLevel == 5)
            {
                Debug.Log("punishmentRemainsImage is call ......");
                //open punishment for male  
                UIManager.Instance.punishmentRemainsImage.SetActive(true);
                UIManager.Instance.congratsdresslevelupImage.SetActive(false);
                UIManager.Instance.HisLevelUpBtn.SetActive(false);
                UIManager.Instance.HerLevelDownBtn.SetActive(true);
            }
            else 
            {
                Debug.Log("congratsdresslevelupImage is call ......");
                UIManager.Instance.congratsdresslevelupImage.SetActive(true);
                UIManager.Instance.punishmentRemainsImage.SetActive(false);
                UIManager.Instance.teasemeforfemaleImage.SetActive(false);
                UIManager.Instance.HisLevelUpBtn.SetActive(true);
                UIManager.Instance.HerLevelDownBtn.SetActive(false);
                UIManager.Instance.outSideFirePitOkBtn.SetActive(false);
            }    

        }
        else if (curTurn == Turn.Male && playerFemale.GetComponent<FollowThePath>().curIndex == 4)
        {
            Debug.Log("female her closet is call ....");

            if(femaleDressLevel == 5)
            {  
                Debug.Log("punishmentRemainsImage is call ......1111");
                //open show me tease me for female  
                UIManager.Instance.punishmentRemainsImage.SetActive(false);
                UIManager.Instance.congratsdresslevelupImage.SetActive(false);
                UIManager.Instance.teasemeforfemaleImage.SetActive(true);
                UIManager.Instance.HisLevelUpBtn.SetActive(false);
                UIManager.Instance.HerLevelDownBtn.SetActive(true);
            }
            else
            {
                Debug.Log("congratsdresslevelupImage is call ......222");
                UIManager.Instance.congratsdresslevelupImage.SetActive(true);
                UIManager.Instance.punishmentRemainsImage.SetActive(false);
                UIManager.Instance.teasemeforfemaleImage.SetActive(false);
                UIManager.Instance.HisLevelUpBtn.SetActive(true);
                UIManager.Instance.HerLevelDownBtn.SetActive(false);
                UIManager.Instance.outSideFirePitOkBtn.SetActive(false);
            }  

        }

        malePoints = CoinManager.instance.malePoint;
        feMalePoints = CoinManager.instance.feMalePoint;  

        maleCurIndex = playerMale.GetComponent<FollowThePath>().curIndex;
        femaleCurIndex = playerFemale.GetComponent<FollowThePath>().curIndex;

        maleCurWayPointIndex=playerMale.GetComponent<FollowThePath>().waypointIndex; 
        femaleCurWayPointIndex=playerFemale.GetComponent<FollowThePath>().waypointIndex;

        lastTrun = curTurn.ToString();
        string activitiesBeforeClimexS = PlayerPrefs.GetString("ActivitiesC");
        activitiesBeforeClimex = int.Parse(activitiesBeforeClimexS);

        location = PlayerPrefs.GetString("location");
        toys = PlayerPrefs.GetString("toylist"); 

        usedCards = GetCardsAPI.Instance.cardjson;

 /*     usedCard.Clear(); 
        usedCard.AddRange(activityDeckCardMale);
        usedCard.AddRange(secretDeckCardMale);
        usedCard.AddRange(showMeandTeaseMeDeckCardMale);
        usedCard.AddRange(activityDeckCardFeMale);
        usedCard.AddRange(secretDeckCardFeMale);
        usedCard.AddRange(showMeandTeaseMeDeckCardFeMale);*/

        if (usedCard.Count > 0)
        {  
            string usedCardJson = "[";  // Start with an opening square bracket for the JSON array

            foreach (string cardId in usedCard)
            {
                usedCardJson += $"\"{cardId}\", ";  // Add each card ID wrapped in double quotes and separated by commas
            }

            if (usedCardJson.EndsWith(", "))
            {
                usedCardJson = usedCardJson.Substring(0, usedCardJson.Length - 2); // Remove the trailing comma and space
            } 

            usedCardJson += "]";  // Close the JSON array
            usedCards = usedCardJson;

           // Debug.Log("Used Cards JSON: " + usedCards);  

        }
        else
        {   
            usedCards = "[]";  
        }   

        if(gameID==null)
        {
            UIManager.Instance.savePopup.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
        else 
        {
            UIManager.Instance.savePopup.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.SetActive(false);
        }  

    }

    public IEnumerator gameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Dashboard");
    }

    public void LoadGame()
    {  
        if (!PlayerPrefs.HasKey("maleCurIndex"))
        {
            return;
        }

        playerMale.GetComponent<FollowThePath>().curIndex = PlayerPrefs.GetInt("maleCurIndex"); 
        playerMale.GetComponent<FollowThePath>().waypointIndex = PlayerPrefs.GetInt("maleCurIndexWayPointIndex");

        if (playerMale.GetComponent<FollowThePath>().curIndex > 0)
        {
            playerMale.transform.position = place[playerMale.GetComponent<FollowThePath>().curIndex - 1].transform.position;
        }

        playerFemale.GetComponent<FollowThePath>().curIndex = PlayerPrefs.GetInt("femaleCurIndex"); 
        playerFemale.GetComponent<FollowThePath>().waypointIndex = PlayerPrefs.GetInt("femaleCurIndexWayPointIndex");

        if (playerFemale.GetComponent<FollowThePath>().curIndex > 0)
        {
            playerFemale.transform.position = placeFemale[playerFemale.GetComponent<FollowThePath>().curIndex - 1].transform.position;
        }

        if (PlayerPrefs.GetString("lastTurn") == "Female")
        {
            //changhe 
            Debug.Log("load game female  turn is call ....");
            curTurn = Turn.Female;

            UIManager.Instance.maleHighLightBorder.SetActive(true);
            UIManager.Instance.feMaleHighLightBorder.SetActive(false);

            UIManager.Instance.playerTurnText.text = PlayerPrefs.GetString("FeMaleFirstName") + " Make Your Move!";
            UIManager.Instance.loadingScreen.SetActive(true);
            UIManager.Instance.playerTurnPanel.SetActive(true);

        }
        else
        {
            //changhe 
            Debug.Log("load game male turn is call ....");
            curTurn = Turn.Male;

            UIManager.Instance.maleHighLightBorder.SetActive(false);
            UIManager.Instance.feMaleHighLightBorder.SetActive(true);

            UIManager.Instance.playerTurnText.text = PlayerPrefs.GetString("MaleFirstName") + " Make Your Move!";
            UIManager.Instance.loadingScreen.SetActive(true);
            UIManager.Instance.playerTurnPanel.SetActive(true);

        }

        maleCurWayPointIndex = PlayerPrefs.GetInt("maleCurIndexWayPointIndex");
        femaleCurWayPointIndex = PlayerPrefs.GetInt("femaleCurIndexWayPointIndex");

        CoinManager.instance.malePoint = PlayerPrefs.GetInt("malePointss");
        CoinManager.instance.feMalePoint = PlayerPrefs.GetInt("feMalePointss");

        gameLevel = PlayerPrefs.GetInt("gamelevelIndex");
        maleDressLevel = PlayerPrefs.GetInt("maleDresLevelIndex");
        femaleDressLevel = PlayerPrefs.GetInt("femaleDresLevelIndex");   

        loveSetupId= PlayerPrefs.GetString("loveSetupId");

        // Debug.Log("looad game  + femaleCurWayPointIndex  " + femaleCurWayPointIndex);

        string activitiesBeforeClimexS = PlayerPrefs.GetString("ActivitiesC");
        activitiesBeforeClimex = int.Parse(activitiesBeforeClimexS);

        malePoints = PlayerPrefs.GetInt("malePointss");
        feMalePoints = PlayerPrefs.GetInt("feMalePointss");

        location = PlayerPrefs.GetString("location");
        toys = PlayerPrefs.GetString("toylist");


        if (PlayerPrefs.HasKey("GameID"))
        {
            gameID = PlayerPrefs.GetString("GameID");
            Debug.LogError("gameID"+ gameID);
        }
        else
        {
            gameID = null;
        }

        PlayerPrefs.DeleteKey("maleCurIndex");
        PlayerPrefs.DeleteKey("femaleCurIndex");
        PlayerPrefs.DeleteKey("lastTurn");
        PlayerPrefs.DeleteKey("WaypointIndex_playerMale");
        PlayerPrefs.DeleteKey("WaypointIndex_playerFemale");
        PlayerPrefs.DeleteKey("malePointss");
        PlayerPrefs.DeleteKey("feMalePointss"); 
        PlayerPrefs.DeleteKey("GameID");

    }

}

[System.Serializable]
public class CardList
{
    public List<string> cardList;
}



public enum Turn
{
    Male,
    Female
}