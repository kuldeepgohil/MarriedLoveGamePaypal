using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;


public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public GameObject mainPopup;
    public GameObject perFormActivity;
    public GameObject satisfiedPopup;
    public GameObject savePopup;

    public Text timerTxt;

    public GameObject maleHighLightBorder;
    public GameObject feMaleHighLightBorder;

    public GameObject maleHighOrageBorder;
    public GameObject FemaleHighOrageBorder;

    public GameObject discPanel;

    public bool isSatisfiedNo;

    public bool isPunishmentRoom;

    public Text performToXYZ;
    public Text maleName; 
    public Text femaleName; 

    public bool issHisMoveBackSpaces;

    public GameObject HisLevelUpBtn;
    public GameObject HisSecrectRoomBtn;    
    public GameObject HerLevelDownBtn; 
    public GameObject HerSecrectRoomBtn;   
    public GameObject FrontPorchShowMyCardBtn;
    public GameObject outSideFirePitOkBtn;  
    
    [Header("Her And His Closet")]
    public Text messageTxt;

    public GameObject youGetTurnPopup;

    public int turnCount;
    public bool isoutSideFirePit;

    public int maleTurnCount;
    public int feMaleTurnCount;

    public GameObject maleYellowCard;
    public GameObject femaleYellowCard;

    public GameObject playerTurnPanel;
    public Text playerTurnText;

    public GameObject gameInfoPanel;

    public GameObject congratsdresslevelupImage;
    public GameObject punishmentRemainsImage;

    public GameObject teasemeforfemaleImage;

    public Text maleMessageText;
    public Text feMaleMessageText;

    public GameObject retryPanel;

    public bool isCoinCount;

    public GameObject content;

    public GameObject loadingScreen;
    public UnityEngine.UI.Slider loadingSlider;
    public float loadingSpeed = 0.5f;

    public Text loadingTxt;
    public float blinkDuration;


    [Header("Plan")]
    public string planName;
    int count = 0;

    public void Awake()
    {
        Instance = this;
        isSatisfiedNo=false;
    }

    public void Start()
    {    
        if(GameManager.Instance.curTurn==Turn.Female)
        {
            Debug.LogError("load game male turn is call ....");
            playerTurnText.text= PlayerPrefs.GetString("MaleFirstName")  + " Make Your Move!";
        }
        else
        {
            playerTurnText.text= PlayerPrefs.GetString("FeMaleFirstName")  + " Make Your Move!";
        }
        messageTxt.text = "";
        turnCount = 0;

        StartCoroutine(UIManager.Instance.AnimateLoadingScreen());
    }

    public IEnumerator AnimateLoadingScreen()          
    {
        loadingScreen.SetActive(true);
        Color customColor1;
        Color customColor2 = Color.white;

        if (!ColorUtility.TryParseHtmlString("#FB7100", out customColor1))
        {
            Debug.LogError("Invalid color code!");
        }

        /*   Sequence rainbowSequence = DOTween.Sequence();
           rainbowSequence.Append(loadingTxt.DOColor(customColor1, blinkDuration)).Append(loadingTxt.DOColor(customColor2, blinkDuration))
               .SetLoops(-1, LoopType.Restart) 
               .SetEase(Ease.Linear); */


        float speed = 1f; // Adjust the speed (higher is faster)
        Sequence rainbowSequence = DOTween.Sequence();
        rainbowSequence
            .Append(loadingTxt.DOColor(customColor1, 1f)) // Default duration
            .Append(loadingTxt.DOColor(customColor2, 1f)) // Default duration
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetSpeedBased(); // Speeds up the sequence


        float progress = 0;
        while (progress < 0.97f)
        {
            progress += Time.deltaTime * loadingSpeed;
            loadingSlider.value = progress;
            yield return null;
        }

        loadingSlider.value = 1;
        loadingScreen.SetActive(false);
        GamePlayUIAnimation.ins.OpenPopUp(playerTurnPanel);
        StartCoroutine(playerTurnPanelOnOff());  

    } 

    public IEnumerator playerTurnPanelOnOff()
    {
        yield return new WaitForSeconds(5f);
        GamePlayUIAnimation.ins.ClosePopup(playerTurnPanel);
    }   
        
    public void ShowMycardBtnClick()
    {  
        mainPopup.SetActive(false);
        perFormActivity.SetActive(true);
        isSatisfiedNo = false;
        GamePlayUIAnimation.ins.ClosePopup(mainPopup);
        GamePlayUIAnimation.ins.OpenPopUp(perFormActivity);

        // SoundManager.instance.BtnClickSound();

        SoundGameScreen.instance.PlayBtnSound();

    }

    public void perFormActivityStartBtnClick()
    {
        Debug.Log("perFormActivityStartBtnClick is call ...");
        Timer.Instance.isTimerRunning = true;
       // GameManager.Instance.usedCard.Add(GetCardsAPI.Instance.currentCardID);
        //SoundManager.instance.BtnClickSound(); 
        SoundGameScreen.instance.PlayBtnSound();
    } 

    public void SatisfiedYesBtn()
    {
        SoundGameScreen.instance.PlayBtnSound();
        //SoundManager.instance.BtnClickSound();
        mainPopup.SetActive(false);
        perFormActivity.SetActive(false);
        satisfiedPopup.SetActive(false);

        GamePlayUIAnimation.ins.ClosePopup(satisfiedPopup);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
           
            if (GameManager.Instance.playerFemale.GetComponent<FollowThePath>().curIndex == 14 && isCoinCount==true)
            {
                CoinManager.instance.SunroomSpaceFeMale();
                Debug.LogError(CoinManager.instance.feMalePoint+ "SunroomSpaceFeMale");
                UIManager.Instance.isCoinCount = false;
            }
            else if (GameManager.Instance.playerFemale.GetComponent<FollowThePath>().curIndex == 18 && isCoinCount == true)
            {
                CoinManager.instance.PunishRoomSpaceFeMale();
                Debug.LogError(CoinManager.instance.feMalePoint + "PunishRoomSpaceFeMale");
                UIManager.Instance.isCoinCount = false;
            }
            else  
            {
                CoinManager.instance.ActivitySpaceFeMale();
                Debug.LogError(CoinManager.instance.feMalePoint+ "ActivitySpaceFeMale");
                UIManager.Instance.isCoinCount = false;
            }

        }
        else 
        {   
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);

            if (GameManager.Instance.playerMale.GetComponent<FollowThePath>().curIndex == 14 && isCoinCount == true)
            {
                CoinManager.instance.SunroomSpaceMale();
                Debug.LogError(CoinManager.instance.malePoint + "SunroomSpaceMale");
                UIManager.Instance.isCoinCount = false;
            }
            else if (GameManager.Instance.playerMale.GetComponent<FollowThePath>().curIndex == 18 && isCoinCount == true)
            {
                CoinManager.instance.PunishRoomSpaceMale();
                Debug.LogError(CoinManager.instance.malePoint + "PunishRoomSpaceMale");
                UIManager.Instance.isCoinCount = false;
            }
            else
            {
                CoinManager.instance.ActivitySpaceMale();
                Debug.LogError(CoinManager.instance.malePoint + "ActivitySpaceMale");
                UIManager.Instance.isCoinCount = false;
            } 

        } 

        discPanel.SetActive(false);

        if (GameManager.Instance.maleOrgasm == true && GameManager.Instance.feMaleOrgasm == true)
        {
            GameManager.Instance.gameoverPanel.SetActive(true);
            GamePlayUIAnimation.ins.OpenPopUp(GameManager.Instance.gameoverPanel);
            StartCoroutine(gameOver());
        }
        else
        {
            GameManager.Instance.gameoverPanel.SetActive(false);
        }  

    }
    public IEnumerator gameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Dashboard");
    }

    public void SatisfiedNoBtn()
    {
        SoundGameScreen.instance.PlayBtnSound();

        //SoundManager.instance.BtnClickSound();
        mainPopup.SetActive(false);
        perFormActivity.SetActive(false);
        satisfiedPopup.SetActive(false);  

        isSatisfiedNo = true;

        GamePlayUIAnimation.ins.ClosePopup(satisfiedPopup);
        GamePlayUIAnimation.ins.OpenPopUp(mainPopup);

        if (GameManager.Instance.curTurn == Turn.Female)
        {
            DiceManager.instance.MoveFeMalePunishmentRoom();

            string maleName = PlayerPrefs.GetString("MaleFirstName");
            string femaleName = PlayerPrefs.GetString("FeMaleFirstName"); 

            UIManager.Instance.maleName.text = maleName;
            UIManager.Instance.femaleName.text = femaleName;

            if (GameManager.Instance.curTurn == Turn.Male)
            {
                GameManager.Instance.curTurn = Turn.Female;
            }
            else if (GameManager.Instance.curTurn == Turn.Female)
            {
                GameManager.Instance.curTurn = Turn.Male;
            } 
        }
        else
        {
            DiceManager.instance.MoveMalePunishmentRoom();
            string maleName = PlayerPrefs.GetString("MaleFirstName");
            string femaleName = PlayerPrefs.GetString("FeMaleFirstName");
            UIManager.Instance.maleName.text = femaleName;
            UIManager.Instance.femaleName.text = maleName;

            if (GameManager.Instance.curTurn == Turn.Male)
            {
                GameManager.Instance.curTurn = Turn.Female;
            }
            else if (GameManager.Instance.curTurn == Turn.Female)
            {
                GameManager.Instance.curTurn = Turn.Male;
            }

        }

        if (GameManager.Instance.maleOrgasm == true && GameManager.Instance.feMaleOrgasm == true)
        {
            GameManager.Instance.gameoverPanel.SetActive(true);
            GamePlayUIAnimation.ins.OpenPopUp(GameManager.Instance.gameoverPanel);
            StartCoroutine(gameOver());
        }
        else
        {
            GameManager.Instance.gameoverPanel.SetActive(false);
        }

    } 

    /// <summary>
    /// Player send Satisfied point count 
    /// </summary>

    public void SavesaveBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        //SoundManager.instance.BtnClickSound();
        GamePlayUIAnimation.ins.ClosePopup(savePopup); 
    } 

    public void SaveExitBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        //SoundManager.instance.BtnClickSound();
        GamePlayUIAnimation.ins.ClosePopup(savePopup);
        SceneManager.LoadScene("Dashboard");
    }

    public void CancelBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        //SoundManager.instance.BtnClickSound();
        GamePlayUIAnimation.ins.OpenPopUp(savePopup);
    }  
      
    public void HisClosetRemoveClouthBtn()
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("boy click remove btn click is call....");  

        StartCoroutine(PopUpCloseTime(mainPopup));
        
        messageTxt.gameObject.SetActive(true);

        messageTxt.text = "";    

        discPanel.SetActive(false);  

        HisLevelUpBtn.gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;

        if (GameManager.Instance.curTurn == Turn.Female)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);  

            Debug.Log("femnale level is up......");

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

                messageTxt.text = currentMaleDressLevelID + " To " +maleDressLevelID;

            }
          
        }
        else
        {  
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);

            Debug.Log("male level is up......");

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

                messageTxt.text = currentfemaleDressLevelID + " To " + femaleDressLevelID;

            }

        }

    } 

   
    public void HisMoveBackSpacesBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("boy click  HisMoveBackSpaces btn click is call....");   
        mainPopup.SetActive(false);
        discPanel.SetActive(false);
        DiceManager.instance.MoveMalePunishmentRoom();     
    }

    public void HerMoveBackSpacesBtnClick() 
    {
        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("giri click  HisMoveBackSpaces btn click is call....");
        mainPopup.SetActive(false);
        discPanel.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            DiceManager.instance.MoveFemaleSunRoom();
        }
        else
        {
            DiceManager.instance.MoveMalePunishmentRoom();
        }

    }

    public void FrontPorchShowMyCardBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("FrontPorchShowMyCardBtnClick is call ....");
        GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.perFormActivity);

        mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        mainPopup.SetActive(false);

        perFormActivity.SetActive(true);
        StartCoroutine(GetCardsAPI.Instance.GetCardsRequest());

    }
    
    public void OutSideFirePitOkBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("OutSideFirePitOkBtnClick is call ....");
       
        isSatisfiedNo = true;  
        isoutSideFirePit=true;

        mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);

        GamePlayUIAnimation.ins.OpenPopUp(mainPopup);

        if (GameManager.Instance.curTurn == Turn.Female)
        {
            DiceManager.instance.MoveMalePunishmentRoom();
            maleTurnCount++;
        }
        else
        {
            DiceManager.instance.MoveFeMalePunishmentRoom();  
            feMaleTurnCount ++;
        }  

    }     

    public IEnumerator PopUpCloseTime(GameObject popup)
    {
        yield return new WaitForSeconds(3f);
        popup.SetActive(false);
        messageTxt.gameObject.SetActive(false);
        HisLevelUpBtn.gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }  
 
    public IEnumerator youGetTurnPopupONoff()
    {
        yield return new WaitForSeconds(5f);
        youGetTurnPopup.SetActive(false);
    }

    /// <summary>
    /// Electrical Male Room Rule Setup.........................
    /// </summary>  
    
    public void ElectricalRoomMaleChangTheGameLevelUpBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("ElectricalRoom Male ChangTheGameLevel Up Btn Click");
        Debug.Log("MALE LEVEL IS UP...."); 
        mainPopup.gameObject.SetActive(false);

        //test
        /*  if (GameManager.Instance.gameLevel < 5)
          {
              GameManager.Instance.gameLevel++;
          }*/

        planName = PlayerPrefs.GetString("plan_name");
        Debug.LogError(planName);

        if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
        {
            Debug.Log("basic is call ...");
            if (count < 2)
            {
                count++;
                GameManager.Instance.gameLevel++; //test
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
           

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        } 

        CoinManager.instance.UpgradeGameStageOneLevelMale();
        discPanel.SetActive(false);  

    }

    public void ElectricalRoomMaleChangTheGameLevelLowBtnClick()
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom Male ChangTheGameLevel low Btn Click");
        Debug.Log("MALE LEVEL IS UP....");  

        mainPopup.gameObject.SetActive(false);

        /* if (GameManager.Instance.gameLevel < 5)
         {
             GameManager.Instance.gameLevel++;
         } */


        /*if(GameManager.Instance.gameLevelName== "basic")
        {
            if (GameManager.Instance.gameLevel < 2)
            {
                GameManager.Instance.gameLevel++;
            }

        }   
        else if(GameManager.Instance.gameLevelName == "advance")
        {
            if (GameManager.Instance.gameLevel < 4)
            {
                GameManager.Instance.gameLevel++;
            }

        }
        else
        {
            if (GameManager.Instance.gameLevel < 5)
            {
                GameManager.Instance.gameLevel++;
            }
        }*/


        planName = PlayerPrefs.GetString("plan_name");
        Debug.LogError(planName);

        if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
        {
            Debug.Log("basic is call ...");
            if (count < 2)
            {
                count++;
            }
        }
        else if (planName.Contains("advance") || planName.Contains("Advance")) //advnce plne
        {
            Debug.Log("advance is call ...");
            if (count < 4)
            {
                count++;
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


        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        } 

        CoinManager.instance.LowerGameStageOneLevelMale();
        discPanel.SetActive(false);  
    }

    public void ElectricalRoomMaleForceAPlayerToRemoveAPieceOfClothing()
    {
        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom Male  Force a player to remove a piece of clothing  Btn Click");
        Debug.Log("MALE CLICK FORCE A PLAYER BTN CLICK ");
        mainPopup.gameObject.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        } 

        CoinManager.instance.ForceNonActivePlayertoRemoveClothingMale();
        discPanel.SetActive(false); 

    }

    public void ElectricalRoomMaleForceAPlayerToRemoveOnePieceofClothing()
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom Male  Force a player to remove OnePiece of Clothing Btn Click");
        Debug.Log("MALE CLICK FORCE A PLAYER BTN CLICK ");
        mainPopup.gameObject.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        } 

        CoinManager.instance.ActivePlayerCanPutBackOnOnePieceofClothingMale();
        discPanel.SetActive(false);
    }

    public void ElectricalRoomMaleSunroomBtnClick()
    {
        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom Male Sunroom Btn Click");    
        maleYellowCard.SetActive(false);
        isSatisfiedNo = true;
        DiceManager.instance.MoveFemaleSunRoom();
        CoinManager.instance.SendNonActivePlayertoSunroomMale();

    }

    public void ElectricalRoomMaleSecretPlayRoomBtnClik()
    {
        SoundGameScreen.instance.PlayBtnSound(); 

        Debug.Log("ElectricalRoom Male Secret PlayRoom Btn Clik");
        maleYellowCard.SetActive(false);
        DiceManager.instance.MoveFeMalePunishmentRoom();
        CoinManager.instance.SendNonActivePlayertoPunishRoomMale();
    }

    /// <summary>
    /// Electrical FeMale Room Rule Setup.....
    /// </summary>

    public void ElectricalRoomFeMaleChangTheGameLevelUpBtnClick()
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom FeMale ChangTheGameLevel Up Btn Click");
        Debug.Log("feMALE LEVEL IS UP....");
        mainPopup.gameObject.SetActive(false);
       
        
        //test code  
      /*  if (GameManager.Instance.gameLevel<5)
        {   
           GameManager.Instance.gameLevel++; 
        }*/


        planName = PlayerPrefs.GetString("plan_name");
        Debug.LogError(planName);

        if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
        {
            Debug.Log("basic is call ...");
            if (count < 2)
            {
                count++;
                GameManager.Instance.gameLevel++; //test
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

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);   
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        }

        //CoinManager.instance.feMalePoint -= 50;

        CoinManager.instance.UpgradeGameStageOneLevelFeMale();
        discPanel.SetActive(false);

    }

    public void ElectricalRoomFeMaleChangTheGameLevelLowBtnClick()
    {

        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("ElectricalRoom FeMale ChangTheGameLevel low Btn Click");
        Debug.Log("feMALE LEVEL IS UP....");
        mainPopup.gameObject.SetActive(false);

        //test
        /* if (GameManager.Instance.gameLevel < 5)
         {
             GameManager.Instance.gameLevel++;
         } */

        /* if (GameManager.Instance.gameLevelName == "basic")
         {
             if (GameManager.Instance.gameLevel < 2)
             {
                 GameManager.Instance.gameLevel++;
             }

         }
         else if (GameManager.Instance.gameLevelName == "advance")
         {
             if (GameManager.Instance.gameLevel < 4)
             {
                 GameManager.Instance.gameLevel++;
             }

         }
         else
         {
             if (GameManager.Instance.gameLevel < 5)
             {
                 GameManager.Instance.gameLevel++;
             }
         }

         if (GameManager.Instance.curTurn == Turn.Male)
         {
             maleHighLightBorder.SetActive(false);
             feMaleHighLightBorder.SetActive(true);
         }
         else
         {
             maleHighLightBorder.SetActive(true);
             feMaleHighLightBorder.SetActive(false);
         }*/



        planName = PlayerPrefs.GetString("plan_name");
        Debug.LogError(planName);

        if (planName.Contains("basic") || planName.Contains("Basic"))  //basic plne
        {
            Debug.Log("basic is call ...");
            if (count < 2)
            {
                count++;
                GameManager.Instance.gameLevel++; //test
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

        // CoinManager.instance.feMalePoint -= 100;
        CoinManager.instance.LowerGameStageOneLevelFeMale();
        discPanel.SetActive(false);

    }

    public void ElectricalRoomFeMaleForceAPlayerToRemoveAPieceOfClothing()
    {

        SoundGameScreen.instance.PlayBtnSound();
        Debug.Log("ElectricalRoom FeMale  Force a player to remove a piece of clothing  Btn Click");
        Debug.Log("fEMALE CLICK FORCE A PLAYER BTN CLICK ");
        

        mainPopup.gameObject.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        }

        //CoinManager.instance.feMalePoint -= 50;  

        CoinManager.instance.ForceNonActivePlayertoRemoveClothingFeMale();
        discPanel.SetActive(false);

    }

    public void ElectricalRoomFeMaleForceAPlayerToOnePieceofClothingClothing()
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom FeMale  Force a player to remove OnePieceofClothing Btn Click");
        Debug.Log("fEMALE CLICK FORCE A PLAYER BTN CLICK ");
            
        mainPopup.gameObject.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
        }
        else
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
        }

        CoinManager.instance.ActivePlayerCanPutBackOnOnePieceofClothingFeMale();
        discPanel.SetActive(false);

    }

    public void ElectricalRoomFeMaleSunroomBtnClick() 
    {

        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom FeMale Sunroom Btn Click");  
        femaleYellowCard.SetActive(false);
        isSatisfiedNo = true;
        DiceManager.instance.MaleSunRoom();
        //CoinManager.instance.feMalePoint -= 50;

        CoinManager.instance.SendNonActivePlayertoSunroomFeMale();
        Debug.LogError(CoinManager.instance.feMalePoint);

    }

    public void ElectricalRoomFeMaleSecretPlayRoomBtnClik()
    {
        SoundGameScreen.instance.PlayBtnSound();

        Debug.Log("ElectricalRoom FeMale Secret PlayRoom Btn Clik");
        femaleYellowCard.SetActive(false);
        
        //CoinManager.instance.feMalePoint -= 50;
        CoinManager.instance.SendNonActivePlayertoPunishRoomFeMale();
        DiceManager.instance.MoveMalePunishmentRoom();
        Debug.LogError(CoinManager.instance.feMalePoint);  
    }  

    public void InfoGameBtnClick()
    {   

        gameInfoPanel.SetActive(true);
        //SoundManager.instance.BtnClickSound();
        SoundGameScreen.instance.PlayBtnSound();
    }   

    public void InfoGameCancelBtnClick()
    {
        gameInfoPanel.SetActive(false);
        SoundGameScreen.instance.PlayBtnSound();
        //SoundManager.instance.BtnClickSound();
    }   

    public void ReTryBtnClick()
    {
        //SoundManager.instance.BtnClickSound();
        SoundGameScreen.instance.PlayBtnSound();
        GamePlayUIAnimation.ins.ClosePopup(perFormActivity); 

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            maleHighLightBorder.SetActive(true);
            feMaleHighLightBorder.SetActive(false);
            GameManager.Instance.curTurn = Turn.Female;
        }
        else
        {
            maleHighLightBorder.SetActive(false);
            feMaleHighLightBorder.SetActive(true);
            GameManager.Instance.curTurn = Turn.Male;
        } 
        
        discPanel.SetActive(false);  

    }

    public void okRepetCardBtnClick()
    {
        StartCoroutine(GetCardsAPI.Instance.GetCardsRequest());
        GamePlayUIAnimation.ins.OpenPopUp(perFormActivity);
        //SoundManager.instance.BtnClickSound();
        SoundGameScreen.instance.PlayBtnSound(); 
    }

    public IEnumerator WaitRetryTurn()
    {
        yield return new WaitForSeconds(3f);    
        retryPanel.SetActive(false);
        ReTryBtnClick();
        //SoundManager.instance.BtnClickSound();
        SoundGameScreen.instance.PlayBtnSound();
    }  

} 
