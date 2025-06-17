using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiceManager : MonoBehaviour
{

    public static DiceManager instance;

    bool rot;
    Vector3 curRot;
    public Vector3[] diceRots;

    public FollowThePath malePlayer;
    public FollowThePath femalePlayer;

    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (rot)
        {
            transform.rotation = Quaternion.Euler(curRot.x, curRot.y, curRot.z);
        }
    }

    public void StartRotation()
    {
        rot = false;
        //SoundDiscManager.instance.DiscAudioPlay();  
        //SoundManager.instance.DiscAudioPlay();      
        SoundGameScreen.instance.DiscAudioPlay();
    }

    public void Start()
    {
        if (GameManager.Instance.curTurn == Turn.Female)
        {
            UIManager.Instance.maleHighLightBorder.SetActive(true);
            UIManager.Instance.feMaleHighLightBorder.SetActive(false);   
           // Debug.LogError("hello femlae is call ...");
        }
        else
        {
            //Debug.LogError("hello ");
            UIManager.Instance.maleHighLightBorder.SetActive(false);
            UIManager.Instance.feMaleHighLightBorder.SetActive(true);  
        } 
        UIManager.Instance.discPanel.SetActive(false);  

    }

    public void SetDiceValue()
    {
        
        GetComponent<Animator>().enabled = false;
        int value = Random.Range(0, 5);     
       //int value = 0;
        curRot = diceRots[value];
        rot = true; 
            
        if (GameManager.Instance.curTurn == Turn.Male)           
        {
            //Debug.Log("male move is called...");
            MoveMale(value);
            //MoveMale(0);

            string maleName = PlayerPrefs.GetString("MaleFirstName");
            string femaleName = PlayerPrefs.GetString("FeMaleFirstName");

            UIManager.Instance.maleName.text = femaleName;
            UIManager.Instance.femaleName.text = maleName;
            UIManager.Instance.maleTurnCount++;

            if (UIManager.Instance.isoutSideFirePit && UIManager.Instance.maleTurnCount == 2)
            {
                MoveMalePunishmentRoom();
                UIManager.Instance.isoutSideFirePit = false;
            }
            else
            {
                UIManager.Instance.maleTurnCount = 0;
            }  

            GameManager.Instance.curTurn = Turn.Female;  

        }
        else if (GameManager.Instance.curTurn == Turn.Female)
        {
            //Debug.Log("female move is called...");
            MoveFemale(value);
            //MoveFemale(0);

            string maleName = PlayerPrefs.GetString("MaleFirstName");
            string femaleName = PlayerPrefs.GetString("FeMaleFirstName");
            UIManager.Instance.maleName.text = maleName;    
            UIManager.Instance.femaleName.text = femaleName;   

            UIManager.Instance.feMaleTurnCount++;

            if (UIManager.Instance.isoutSideFirePit && UIManager.Instance.feMaleTurnCount==2)
            { 
                MoveFeMalePunishmentRoom();
                UIManager.Instance.isoutSideFirePit = false;
            }
            else
            {
                 UIManager.Instance.feMaleTurnCount=0;
            }    

            GameManager.Instance.curTurn = Turn.Male; 
            GameManager.Instance.curTurn = Turn.Male; 
            
        }
        else
        {
            Debug.Log("No valid turn detected. Default case...");
            // Handle any fallback case or log an error
        } 

        UIManager.Instance.discPanel.SetActive(true);  
        
    } 

    public void MoveMale(int value)
    {  
        UIManager.Instance.isCoinCount=true; 

        Debug.Log("Movemale is call ..."); 
        
        malePlayer.canMove = true;
        malePlayer.curIndex += value + 1; //test  

        if (malePlayer.curIndex > GameManager.Instance.place.Length)
        {
            malePlayer.curIndex = malePlayer.curIndex % GameManager.Instance.place.Length;
        } 

        malePlayer.finalPos = GameManager.Instance.place[malePlayer.curIndex - 1].transform.position; //remove -1 after test
        GameManager.Instance.curTurn = Turn.Female;  

    } 
    
    public void MoveFemale(int value)
    {
        UIManager.Instance.isCoinCount = true;

        femalePlayer.canMove = true;
        femalePlayer.curIndex += value + 1;


        if (femalePlayer.curIndex > GameManager.Instance.placeFemale.Length)
        {
            Debug.Log("reset");
            femalePlayer.curIndex = femalePlayer.curIndex % GameManager.Instance.placeFemale.Length;
        }

        femalePlayer.finalPos = GameManager.Instance.placeFemale[femalePlayer.curIndex - 1].transform.position;
        GameManager.Instance.curTurn = Turn.Male;

    }

    public void MoveMalePunishmentRoom()
    {
        UIManager.Instance.isCoinCount = false;

        Debug.Log("MoveMalePunishmentRoom is call ...");  

        malePlayer.canMove = true;
        malePlayer.curIndex = GameManager.Instance.place.Length;
        malePlayer.waypointIndex = 65;
        malePlayer.finalPos = GameManager.Instance.place[GameManager.Instance.place.Length - 1].transform.position; 

        GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[malePlayer.curIndex-1];
        GameManager.Instance.placeNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 1];   
        GameManager.Instance.deskNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 1];  
        GameManager.Instance.SpaceNameText.text = GameManager.Instance.spaceName[malePlayer.curIndex - 1];
        UIManager.Instance.isPunishmentRoom = true; 

    }

    public void MoveFeMalePunishmentRoom()
    {
        UIManager.Instance.isCoinCount = false;
        // Debug.Log("MoveFeMalePunishmentRoom is call ...");   

        femalePlayer.canMove = true;  
        femalePlayer.curIndex = GameManager.Instance.placeFemale.Length;
        femalePlayer.waypointIndex = 65;    

        femalePlayer.finalPos = GameManager.Instance.placeFemale[GameManager.Instance.placeFemale.Length - 1].transform.position;
        GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[femalePlayer.curIndex -1]; 

        GameManager.Instance.placeNameText.text = GameManager.Instance.placeName[femalePlayer.curIndex - 1];  

        GameManager.Instance.deskNameText.text = GameManager.Instance.placeName[femalePlayer.curIndex - 1];  

        GameManager.Instance.SpaceNameText.text = GameManager.Instance.spaceName[femalePlayer.curIndex - 1];
        UIManager.Instance.isPunishmentRoom = true;    

    }  

    public void MoveFemaleSunRoom() 
    {
        UIManager.Instance.isCoinCount = false;
        Debug.Log("Move FeMale Sunroom is call ...");
        femalePlayer.canMove = true;
        femalePlayer.curIndex = 14;
        femalePlayer.waypointIndex = 49;

        femalePlayer.finalPos = GameManager.Instance.placeFemale[GameManager.Instance.placeFemale.Length -5].transform.position;
        GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[femalePlayer.curIndex - 1];  
        GameManager.Instance.placeNameText.text = GameManager.Instance.placeName[femalePlayer.curIndex - 1];  
        GameManager.Instance.deskNameText.text = GameManager.Instance.placeName[femalePlayer.curIndex - 1];  
        GameManager.Instance.SpaceNameText.text = GameManager.Instance.spaceName[femalePlayer.curIndex - 1];

        string maleName = PlayerPrefs.GetString("MaleFirstName");
        string femaleName = PlayerPrefs.GetString("FeMaleFirstName");

        UIManager.Instance.maleName.text = femaleName;
        UIManager.Instance.femaleName.text = maleName;


    }   

    public void MaleSunRoom()
    {
        UIManager.Instance.isCoinCount = false;
        // Debug.Log("Move Male SunRoom is call ...");

        malePlayer.canMove = true;

        malePlayer.curIndex = 14;
        malePlayer.waypointIndex = 49;

        malePlayer.finalPos = GameManager.Instance.place[GameManager.Instance.place.Length - 5].transform.position;
        GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[GameManager.Instance.place.Length - 5]; 
        GameManager.Instance.placeNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 5];

        GameManager.Instance.deskNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 5];  

        GameManager.Instance.SpaceNameText.text = GameManager.Instance.spaceName[malePlayer.curIndex - 5];

        UIManager.Instance.maleHighLightBorder.SetActive(true);
        UIManager.Instance.feMaleHighLightBorder.SetActive(false);

        string maleName = PlayerPrefs.GetString("MaleFirstName");
        string femaleName = PlayerPrefs.GetString("FeMaleFirstName");

        UIManager.Instance.maleName.text = maleName;
        UIManager.Instance.femaleName.text = femaleName;  

    }

    public void DiceBtnClick()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("Rot");

      
    }

    public void MoveMalePunishmentRoomHisMoveBackSpacesBtnClick()   
    {
        UIManager.Instance.isCoinCount = false;
        // Debug.Log("MoveMalePunishmentRoomHisMoveBackSpacesBtnClick is call ..."); 
        malePlayer.canMove = true;
        malePlayer.curIndex = GameManager.Instance.place.Length;
        malePlayer.waypointIndex = 65;
        malePlayer.finalPos = GameManager.Instance.place[GameManager.Instance.place.Length - 1].transform.position;
        GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[malePlayer.curIndex - 1];
        GameManager.Instance.placeNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 1];  
        GameManager.Instance.deskNameText.text = GameManager.Instance.placeName[malePlayer.curIndex - 1];  
        GameManager.Instance.SpaceNameText.text = GameManager.Instance.spaceName[malePlayer.curIndex - 1];
        UIManager.Instance.isPunishmentRoom = true;  
        
    }

}