using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{  


    public Text maleDressLevelT;
    public Text maleDressLevel;

    public Text feMaleDressLevelT;
    public Text feMaleDressLevel;

    public Text gameLevelT;
    public Text gameLevelss;

    public Text climaxNumberT;
    public Text climaxNumber;

    public Text malePointT;
    public Text malePoint;

    public Text feMalePointT;
    public Text feMalePoint;

    public void Update()
    {  
        maleDressLevelT.text = PlayerPrefs.GetString("MaleFirstName").ToUpper() + " Dress Level";
        feMaleDressLevelT.text = PlayerPrefs.GetString("FeMaleFirstName").ToUpper() + " Dress Level";

        gameLevelT.text = " Your Game Level";
        climaxNumberT.text = " Climax Number";

        malePointT.text = PlayerPrefs.GetString("MaleFirstName").ToUpper() + " Points";
        feMalePointT.text = PlayerPrefs.GetString("FeMaleFirstName").ToUpper() + " Points";

        SetTXt();

    }

    public void SetTXt()
    {
        int maleDressindex = GameManager.Instance.maleDressLevel;
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

       // Debug.Log("Male dress : " + maleDressLevelID);
        maleDressLevel.text = maleDressLevelID;

        int femaleDressindex = GameManager.Instance.femaleDressLevel;
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

        //Debug.Log("Female dress : " + femaleDressLevelID);
        feMaleDressLevel.text = femaleDressLevelID;


       int gameLevelIndex = GameManager.Instance.gameLevel;
       string gameLevelID;

        switch (gameLevelIndex)
        {
            case 1:
                {
                    gameLevelID = StaticId.Instance.gamelevelNameONE;
                    break;
                }
            case 2:
                {
                    gameLevelID = StaticId.Instance.gamelevelNameTWO;
                    break;
                }
            case 3:
                {
                    gameLevelID = StaticId.Instance.gamelevelNameThree;
                    break;
                }
            case 4:
                {
                    gameLevelID = StaticId.Instance.gamelevelNameFour;
                    break;
                }
            default:
                {
                    gameLevelID = StaticId.Instance.gamelevelNameFive;
                    break;
                }

        }

        gameLevelss.text = gameLevelID;  
        climaxNumber.text = GameManager.Instance.activitiesBeforeClimex.ToString();   

        /*malePoint.text = "0";
        feMalePoint.text = "0"; */   
        
        malePoint.text = CoinManager.instance.malePoint.ToString();
        feMalePoint.text = CoinManager.instance.feMalePoint.ToString();
    }

}
