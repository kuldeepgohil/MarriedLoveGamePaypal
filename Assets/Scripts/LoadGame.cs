using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadGame : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public string _id;
        public string user_id;
        public string game_data;
        public string game_name;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [Serializable]
    public class SaveData
    {
        public int status;
        public string message;
        public List<Data> data;
    }

    public GameObject listElement;
    public Transform listPerent;
    public SaveData saveData;
    string auth;

    public static LoadGame instance;

    public void Awake()
    {  
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        auth = PlayerPrefs.GetString("SaveLoginToken");
        StartCoroutine(GetSavedGames());
    }

    public IEnumerator GetSavedGames()  
    {
        string levelRequestUrl = commonURLScript.url + "/api/user/save-games";  

        using (UnityWebRequest request = UnityWebRequest.Get(levelRequestUrl))
        {
            request.SetRequestHeader("auth", auth);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                var jsonResponse = request.downloadHandler.text;
                saveData = JsonUtility.FromJson<SaveData>(jsonResponse);
                Debug.Log("Save Data = " + request.downloadHandler.text);
                ListSaveGames(); 
                
            }
            else
            {
                Debug.LogError("Error fetching Savedata: " + request.error);
            }
        }
    }

    public void ListSaveGames()
    {
        if(saveData.data.Count > 0)
        {
            for(int i =0; i< saveData.data.Count; i++)
            {
                GameObject element = Instantiate(listElement, listPerent);
                element.transform.GetChild(0).GetComponent<Text>().text = saveData.data[i].game_name;
                element.transform.GetComponent<LoadGameElement>().gameId= saveData.data[i]._id;
                element.GetComponent<LoadGameElement>().loaddedData = JsonUtility.FromJson<GameData>(saveData.data[i].game_data);
            }  
        }
    }
}


