using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DeleteGameAPI : MonoBehaviour
{

    public static DeleteGameAPI instance;

    public GameObject loadGame;
    public string auth;

    public void Awake()
    {
        instance = this;
    }

    public void DeleteGameBtnClick(string gameID)
    {
        Debug.LogError(gameObject.transform.parent.GetComponent<LoadGameElement>().gameId); 

        gameID = this.gameObject.transform.parent.GetComponent<LoadGameElement>().gameId;
        StartCoroutine(DeleteGame(gameID));
    } 

    public IEnumerator DeleteGame(string gameID)
    {
        string userId = PlayerPrefs.GetString("userIDText", ""); // Default to empty string if not found
        auth = PlayerPrefs.GetString("SaveLoginToken", ""); // Default to empty string if not found

        Debug.LogError(auth);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(auth))
        {
            Debug.LogError($"Missing required PlayerPrefs values: userIDText='{userId}', SaveLoginToken='{auth}'");
            yield break;
        }

        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/delete-game";
        WWWForm form = new WWWForm();
        form.AddField("id", gameID);

        UnityWebRequest www = UnityWebRequest.Post(createuserprofileRequestUrl, form);
        www.SetRequestHeader("auth", auth);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError($"Error in delete Game: {www.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"Delete Game successful: {www.downloadHandler.text}");
            DestroyObject(this.gameObject.transform.parent.gameObject); 
            StartCoroutine(LoadGame.instance.GetSavedGames());
        }
    }


}
