using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class GameSpaceThemAPI : MonoBehaviour
{

    public static GameSpaceThemAPI ins;

    public List<Image> imagePlace;
    public List<Sprite> gameSpaceImages = new List<Sprite>();  
    public float[] weightAndHeight;
    Sprite sprite;


    [System.Serializable]
    public class Datum
    {
        public string _id;
        public string name;
        public string image;
        public bool status;
        public bool is_deleted;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
    }

    [System.Serializable]
    public class Root
    {
        public int status;
        public string message;
        public List<Datum> data;
    }

    public void Awake()
    {
        ins = this;
    }

    void Start()
    {
        StartCoroutine(FetchGameSpaces());
    }

    IEnumerator FetchGameSpaces()
    {  

        string auth = PlayerPrefs.GetString("SaveLoginToken");
        string createuserprofileRequestUrl = commonURLScript.url + "/api/user/game-spaces";
        UnityWebRequest www = UnityWebRequest.Get(createuserprofileRequestUrl);
        www.SetRequestHeader("auth", auth);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("FetchGameSpaces error=" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("FetchGameSpaces Response=" + www.downloadHandler.text);
            string jsonResponse = www.downloadHandler.text;
            Root root = JsonUtility.FromJson<Root>(jsonResponse);
            Debug.Log("Number of game spaces: " + root.data.Count);

            for (int i = 0; i < root.data.Count; i++)
            {
                StartCoroutine(DownloadImage(root.data[i].image, i)); 
               
            }  

        }  

    }

    IEnumerator DownloadImage(string imageUrl, int index)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture("https://romantic-blessinggame.appworkdemo.com/api/images/" + imageUrl);
        yield return imageRequest.SendWebRequest();

        if (imageRequest.isNetworkError || imageRequest.isHttpError)
        {
            Debug.LogError("Image download error: " + imageRequest.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),100f);

            while (gameSpaceImages.Count <= index)
            {
                gameSpaceImages.Add(null); 
            } 

            gameSpaceImages[index] = sprite;  

            if (index < imagePlace.Count)
            {
                imagePlace[index].sprite = sprite;
            }

            AssignSpritesToRenderers();
            TestT();  

        }

    }

    void AssignSpritesToRenderers()
    {
        for (int i = 0; i < imagePlace.Count && i < gameSpaceImages.Count; i++)
        {
            imagePlace[i].sprite = gameSpaceImages[i];
        }
      
    }

    void TestT()
    {

        for (int i = 0; i <= gameSpaceImages.Count; i++)
        {

            if (i == 0)
            {
                // GameManager.Instance.placeSprite[0] = gameSpaceImages[0];
            }

            else if (i == 1)
            {
                GameManager.Instance.placeSprite[0] = gameSpaceImages[1];

            }

            else if (i == 3)
            {
                GameManager.Instance.placeSprite[1] = gameSpaceImages[2];

            }
            else if (i == 4)
            {
                GameManager.Instance.placeSprite[4] = gameSpaceImages[3];

            }

            else if (i == 5)
            {
                GameManager.Instance.placeSprite[5] = gameSpaceImages[4];

            }
            else if (i == 6)
            {
                GameManager.Instance.placeSprite[6] = gameSpaceImages[5];

            }
            else if (i == 7)
            {
                GameManager.Instance.placeSprite[7] = gameSpaceImages[6];

            }
            else if (i == 8)
            {
                GameManager.Instance.placeSprite[8] = gameSpaceImages[7];

            }
            else if (i == 9)
            {
                GameManager.Instance.placeSprite[9] = gameSpaceImages[8];

            }
            else if (i == 10)
            {
                GameManager.Instance.placeSprite[11] = gameSpaceImages[9];

            }
            else if (i == 11)
            {
                GameManager.Instance.placeSprite[12] = gameSpaceImages[10];

            }
            else if (i == 12)
            {
                GameManager.Instance.placeSprite[13] = gameSpaceImages[11];

            }
            else if (i == 13)
            {
                GameManager.Instance.placeSprite[14] = gameSpaceImages[12];

            }
            else if (i == 14)
            {
                GameManager.Instance.placeSprite[16] = gameSpaceImages[13];
            }
            else if (i == 15)
            {
                GameManager.Instance.placeSprite[17] = gameSpaceImages[14];

            }
        }

    }


    


} 