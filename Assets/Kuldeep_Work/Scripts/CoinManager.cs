using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{

    public static CoinManager instance;

    public int malePoint;
    public int feMalePoint;


    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
      /*  malePoint = 50;
        feMalePoint = 50;*/
    }

    public void Update()
    {
        CheckCoinStatus();
    }

    private void CheckCoinStatus()
    {
        /*if (malePoint <= 0)
        {
           // Debug.Log("Male points are zero!");
            //malePoint = 0; // Ensure it doesn't go below zero
        } 
        
        if (feMalePoint <= 0)
        {
            //Debug.Log("Female points are zero!");
            //feMalePoint = 0; // Ensure it doesn't go below zero
        }*/

        malePoint = Mathf.Clamp(malePoint, 0, int.MaxValue);
        feMalePoint = Mathf.Clamp(feMalePoint, 0, int.MaxValue);


    }

    //male

    public void ActivitySpaceMale()
    {
        //malePoint += 10; 

        malePoint = Mathf.Clamp(malePoint + 10, 0, int.MaxValue);

    }

    public void SunroomSpaceMale()
    {
        //malePoint += 20;

        malePoint = Mathf.Clamp(malePoint + 20, 0, int.MaxValue);

    }
    public void PunishRoomSpaceMale()
    {
       // malePoint -= 10;

        malePoint = Mathf.Clamp(malePoint - 10, 0, int.MaxValue);

    }

    //female  

    public void ActivitySpaceFeMale()
    {
        //feMalePoint += 10;

        feMalePoint = Mathf.Clamp(feMalePoint + 10, 0, int.MaxValue);

    }
    public void SunroomSpaceFeMale()
    {
        //feMalePoint += 20;

        feMalePoint = Mathf.Clamp(feMalePoint + 20, 0, int.MaxValue);


    }
    public void PunishRoomSpaceFeMale()
    {
       // feMalePoint -= 10;

        feMalePoint = Mathf.Clamp(feMalePoint - 10, 0, int.MaxValue);

    }


    /// <summary>
    /// Electrical Room Option Costs male: 
    /// </summary>  

    public void UpgradeGameStageOneLevelMale()
    {
        //malePoint -= 50; 

        malePoint = Mathf.Clamp(malePoint - 50, 0, int.MaxValue);

    }
    public void LowerGameStageOneLevelMale()
    {
        //malePoint -= 100;

        malePoint = Mathf.Clamp(malePoint - 100, 0, int.MaxValue);

    }
    public void ForceNonActivePlayertoRemoveClothingMale()
    {
        //malePoint -= 50;  

        malePoint = Mathf.Clamp(malePoint - 50, 0, int.MaxValue);


    }
    public void ActivePlayerCanPutBackOnOnePieceofClothingMale()
    {
       // malePoint -= 100;

        malePoint = Mathf.Clamp(malePoint - 100, 0, int.MaxValue);

    }

    public void SendNonActivePlayertoSunroomMale()
    {
       // malePoint -=50;

        malePoint = Mathf.Clamp(malePoint - 50, 0, int.MaxValue);

    }

    public void SendNonActivePlayertoPunishRoomMale()
    {
        //malePoint -= 50;
        malePoint = Mathf.Clamp(malePoint - 50, 0, int.MaxValue);

    }

    /// <summary>
    /// Electrical Room Option Costs female: 
    /// </summary>  


    public void UpgradeGameStageOneLevelFeMale()
    {
       // feMalePoint -= 50;
        feMalePoint = Mathf.Clamp(feMalePoint - 50, 0, int.MaxValue);

    }
    public void LowerGameStageOneLevelFeMale()
    {
        //feMalePoint -= 100;

        feMalePoint = Mathf.Clamp(feMalePoint - 100, 0, int.MaxValue);

    }
    public void ForceNonActivePlayertoRemoveClothingFeMale()
    {
       // feMalePoint -= 50;
        feMalePoint = Mathf.Clamp(feMalePoint - 50, 0, int.MaxValue);

    }

    public void ActivePlayerCanPutBackOnOnePieceofClothingFeMale()
    {
        //feMalePoint -= 100;
        feMalePoint = Mathf.Clamp(feMalePoint - 100, 0, int.MaxValue);

    }

    public void SendNonActivePlayertoSunroomFeMale()
    {
      //  feMalePoint -= 50;

        feMalePoint = Mathf.Clamp(feMalePoint - 50, 0, int.MaxValue);

    }

    public void SendNonActivePlayertoPunishRoomFeMale()
    {
       // feMalePoint -= 50;

        feMalePoint = Mathf.Clamp(feMalePoint - 50, 0, int.MaxValue);

    }

}
