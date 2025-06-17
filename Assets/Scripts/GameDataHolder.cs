using System;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections;



public class GameDataHolder : MonoBehaviour
{
    private SaveData saveData;

    public InputField saveGameNameInput;
    public Text errorTxt;
    public Text saveGameNameText;
    public GameData gameData;
   
    // Start is called before the first frame update
    void Awake()
    {
        saveData = GetComponent<SaveData>();
    }

    public void SetupDataAndSave()
    {
        gameData.maleCurWayPointIndex = GameManager.Instance.maleCurWayPointIndex;
        gameData.femaleCurWayPointIndex = GameManager.Instance.femaleCurWayPointIndex;
        gameData.maleCurIndex = GameManager.Instance.maleCurIndex;
        gameData.femaleCurIndex = GameManager.Instance.femaleCurIndex;  
        gameData.lastTurn = GameManager.Instance.lastTrun;

        gameData.usedCards = GameManager.Instance.usedCards;

        gameData.maleDressLevel = GameManager.Instance.maleDressLevel;
        gameData.femaleDressLevel = GameManager.Instance.femaleDressLevel;
        gameData.gameLevel = GameManager.Instance.gameLevel;

        gameData.lovesetupid=GameManager.Instance.loveSetupId;

        gameData.location = GameManager.Instance.location;
        gameData.toys = GameManager.Instance.toys;
        gameData.activityBeforeClimax = GameManager.Instance.activitiesBeforeClimex;
        
        gameData.malePoint = GameManager.Instance.malePoints;
        gameData.feMalePoint = GameManager.Instance.feMalePoints;

        gameData.gameId=GameManager.Instance.gameID;

        string gameDataJSON = JsonConvert.SerializeObject(gameData, Formatting.None);
        Debug.Log(gameDataJSON);


        if(saveGameNameInput.gameObject.activeSelf == true)
        {
            if (saveGameNameInput.text.Length == 0)
            {
                Debug.Log("SDFsadf");
                errorTxt.gameObject.SetActive(true);
                StartCoroutine(onOfftxt());
            }
            else
            {
                saveData.SaveGame(saveGameNameText.text, gameDataJSON);
            }
        }
        
        if (saveGameNameInput.gameObject.activeSelf==false)
        {
            Debug.Log("update game is call ...");
            saveData.UpdateGame(saveGameNameText.text,gameDataJSON,gameData.gameId);
        }

    }
    public IEnumerator onOfftxt()
    {
        yield return new WaitForSeconds(1f);
        errorTxt.gameObject.SetActive(false);
    }
}

[Serializable]
public class GameData
{  

    public string gameId;

    //test
    public int maleCurWayPointIndex;
    public int femaleCurWayPointIndex;

    public int maleCurIndex;
    public int femaleCurIndex;
    public string lastTurn;
    public string usedCards;
    public int maleDressLevel;
    public int femaleDressLevel;
    public int gameLevel;

    public string lovesetupid;

    public string location;
    public string toys;
    public int activityBeforeClimax;  

    public int malePoint;
    public int feMalePoint;


}