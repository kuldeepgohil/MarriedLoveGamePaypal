using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadGameElement : MonoBehaviour
{
    public string gameId;
    public GameData loaddedData;   
    public string auth;

    public void Start()
    {
        auth = PlayerPrefs.GetString("SaveLoginToken");
    }


    public void LoadGame()
    {  

        PlayerPrefs.SetString("GameID",gameId);

        PlayerPrefs.SetInt("maleCurIndexWayPointIndex", loaddedData.maleCurWayPointIndex);
        PlayerPrefs.SetInt("femaleCurIndexWayPointIndex", loaddedData.femaleCurWayPointIndex);

        PlayerPrefs.SetInt("maleCurIndex", loaddedData.maleCurIndex);
        PlayerPrefs.SetInt("femaleCurIndex", loaddedData.femaleCurIndex);
        PlayerPrefs.SetString("lastTurn", loaddedData.lastTurn);

        PlayerPrefs.SetInt("gamelevelIndex", loaddedData.gameLevel);
        PlayerPrefs.SetInt("maleDresLevelIndex", loaddedData.maleDressLevel);
        PlayerPrefs.SetInt("femaleDresLevelIndex", loaddedData.femaleDressLevel);

        PlayerPrefs.SetString("lovesetupid", loaddedData.lovesetupid);

        string activitiesBeforeClimexS = PlayerPrefs.GetString("ActivitiesC");
        int  activitiesBeforeClimex = int.Parse(activitiesBeforeClimexS);

        PlayerPrefs.SetInt("activitiesBeforeClimex", loaddedData.activityBeforeClimax);
        PlayerPrefs.SetString("location", loaddedData.location);
        PlayerPrefs.SetString("toylist", loaddedData.toys);
        
       /* CoinManager.instance.malePoint=loaddedData.malePoint;
        CoinManager.instance.feMalePoint=loaddedData.feMalePoint;*/

        PlayerPrefs.SetInt("malePointss", loaddedData.malePoint); 
        PlayerPrefs.SetInt("feMalePointss", loaddedData.feMalePoint); 

        StartCoroutine(LoadGamelovesetupSubmitRequest());   

    }   

    IEnumerator LoadGamelovesetupSubmitRequest()
    { 
        WWWForm form = new WWWForm();

        int gameLevel = loaddedData.gameLevel;
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

        int maleDressLevel = loaddedData.maleDressLevel;
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


        int femaleDressLevel = loaddedData.femaleDressLevel;
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

        form.AddField("level_id", gameLevelID);
        form.AddField("no_of_activities", loaddedData.activityBeforeClimax);
        form.AddField("toys", loaddedData.toys);
        form.AddField("locations", loaddedData.location);
        form.AddField("male_dress_level_id", maleDressLevelID);
        form.AddField("female_dress_level_id", femaleDressLevelID);

        string lovesetupSubmitRequest = commonURLScript.url + "/api/user/set-love-setup";
        UnityWebRequest request = UnityWebRequest.Post(lovesetupSubmitRequest, form);

        request.SetRequestHeader("auth", auth);  
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("ErrorLoveResponse= " + request.downloadHandler.text);
            string jsonresponse = request.downloadHandler.text;
            LoveSetupRoot loveError = JsonUtility.FromJson<LoveSetupRoot>(jsonresponse);
              
          /*  ErrorMsg.text = loveError.message;
            ErrorPopup.SetActive(true);*/
        }
        else
        {
            Debug.LogError("LoveResponse= " + request.downloadHandler.text);
            //Debug.Log("love setup done is call ...");
            SceneManager.LoadScene("Game Scene");
        }

    }

}