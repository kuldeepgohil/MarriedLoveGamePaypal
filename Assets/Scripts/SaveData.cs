using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    string auth;

    public void SaveGame(string gameName, string gameData)
    {
        StartCoroutine(SaveGameAPI(gameName, gameData));
    }

    IEnumerator SaveGameAPI(string gameName, string gameData)
    {
        string userId = PlayerPrefs.GetString("userIDText");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/save-game";
        WWWForm form = new WWWForm();
        auth = PlayerPrefs.GetString("SaveLoginToken");

        form.AddField("gameName", gameName);
        form.AddField("gameData", gameData);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.LogError(www.error);
            Debug.Log("Error in save Game" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Game Saved : " + www.downloadHandler.text);
            SceneManager.LoadScene("Dashboard");
        } 
    } 

    public void UpdateGame(string gameName, string gameData,string ID)
    {
        StartCoroutine(UpdateGameAPI(gameName, gameData,ID));
    }

    IEnumerator UpdateGameAPI(string gameName, string gameData,string gameID)
    {
        string userId = PlayerPrefs.GetString("userIDText");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/save-game";
        WWWForm form = new WWWForm();
        auth = PlayerPrefs.GetString("SaveLoginToken");

        //form.AddField("gameName", gameName);
        form.AddField("gameData", gameData);

        /*string gameIDS = PlayerPrefs.GetString("GameID");
        Debug.LogError(gameIDS);*/

        gameID = GameManager.Instance.gameID;  

        form.AddField("id",gameID);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.LogError(www.error);
            Debug.Log("Error in save Game" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Game Saved : " + www.downloadHandler.text);
            SceneManager.LoadScene("Dashboard");
        }
    }

}
