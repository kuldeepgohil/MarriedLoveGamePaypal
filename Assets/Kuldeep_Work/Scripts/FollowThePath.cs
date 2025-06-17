using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FollowThePath : MonoBehaviour
{  
   
    public static FollowThePath instance;   

    public Transform[] waypoints;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    public int waypointIndex = 0;

    public Vector3 finalPos;
    public bool canMove;
    public int curIndex;

    public string cardCategory;


    public void Awake()
    {
        instance = this;

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[waypointIndex].position;
        }

        finalPos = Vector3.zero;
        curIndex = 0;  

    } 

    private void Start()
    {
        //if (waypoints.Length > 0)
        //{
        //    transform.position = waypoints[waypointIndex].position;
        //}

        //finalPos = Vector3.zero;
        //curIndex = 0; 
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {  
        if (!canMove)
        {
            return;
        }  

        UIManager.Instance.isSatisfiedNo = false;

        if (!UIManager.Instance.isSatisfiedNo)
        {  
            if (waypointIndex < waypoints.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);

                if (transform.position == waypoints[waypointIndex].position)
                {
                    if (transform.position == finalPos)
                    {
                        canMove = false;
                        Debug.Log("Player is reached finalpos");

                        if (curIndex == 3 && GameManager.Instance.curTurn == Turn.Male)
                        {
                            //free turn PopUp Handle 
                            Debug.Log("curindex 3 is call ...");
                            UIManager.Instance.youGetTurnPopup.SetActive(true);
                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.youGetTurnPopup);
                            StartCoroutine(HandleTurnPopupFeMale());
                            return;
                        }
                        else if (curIndex == 4 && GameManager.Instance.curTurn == Turn.Female)
                        {
                            //free turn PopUp Handle 
                            Debug.Log("curindex 4 is call ...");
                            UIManager.Instance.youGetTurnPopup.SetActive(true);
                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.youGetTurnPopup);
                            StartCoroutine(HandleTurnPopupMale());
                            return;
                        }

                        else if (curIndex == 11)
                        {
                            Debug.Log("CurIndex 11 is call ...");  //front porch  
                            UIManager.Instance.congratsdresslevelupImage.SetActive(false);
                            UIManager.Instance.punishmentRemainsImage.SetActive(false);
                            UIManager.Instance.HisLevelUpBtn.SetActive(false);
                            UIManager.Instance.HisSecrectRoomBtn.SetActive(false);
                            UIManager.Instance.HerLevelDownBtn.SetActive(false);
                            UIManager.Instance.HerSecrectRoomBtn.SetActive(false);
                            UIManager.Instance.messageTxt.gameObject.SetActive(false);
                            UIManager.Instance.outSideFirePitOkBtn.SetActive(false); 
                            UIManager.Instance.FrontPorchShowMyCardBtn.SetActive(true);
                            
                            GameManager.Instance.ywllowplacePopUpBg.sprite = GameManager.Instance.placeSprite[curIndex - 1];
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);

                            UIManager.Instance.mainPopup.SetActive(true);
                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup);
                            return;

                        }
                        else if (curIndex == 16)
                        {  

                            Debug.Log(" CurIndex 16 is call ...");  //outside fire pit 

                            UIManager.Instance.congratsdresslevelupImage.SetActive(false);
                            UIManager.Instance.punishmentRemainsImage.SetActive(false);
                            UIManager.Instance.HisLevelUpBtn.SetActive(false);
                            UIManager.Instance.HisSecrectRoomBtn.SetActive(false);
                            UIManager.Instance.HerLevelDownBtn.SetActive(false);        
                            UIManager.Instance.HerSecrectRoomBtn.SetActive(false);
                            UIManager.Instance.messageTxt.gameObject.SetActive(false);
                            UIManager.Instance.FrontPorchShowMyCardBtn.SetActive(false); 

                            UIManager.Instance.outSideFirePitOkBtn.SetActive(true);
                            GameManager.Instance.ywllowplacePopUpBg.sprite = GameManager.Instance.placeSprite[curIndex - 1]; 

                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);

                            UIManager.Instance.mainPopup.SetActive(true);
                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup); 
                            return;

                        }

                        if (curIndex == 3 || curIndex == 4) 
                        {
                            Debug.Log("curIndex is 3 and 4 is call...");  

                            GameManager.Instance.ywllowplacePopUpBg.sprite = GameManager.Instance.placeSprite[curIndex - 1];
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true); 
                            UIManager.Instance.mainPopup.SetActive(true); 
                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup);

                        }   
                        else if(curIndex==9)
                        {   

                            Debug.Log("Player ElectricalRoom in is call .... ");

                            if (CoinManager.instance.malePoint >= 50  || CoinManager.instance.feMalePoint >= 50)  
                            {
                                Debug.Log("Male player has 100 or more points. Condition is true.");

                                if (GameManager.Instance.curTurn == Turn.Male)
                                {
                                    Debug.Log("Player ElectricalRoom in is call  and male turn");
                                    UIManager.Instance.femaleYellowCard.SetActive(true);
                                    UIManager.Instance.maleYellowCard.SetActive(false);

                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.SetActive(true);
                                    GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup);

                                    CheckCoin();
                                }
                                else
                                {
                                    Debug.Log("Player ElectricalRoom in is call  and female turn");
                                    UIManager.Instance.maleYellowCard.SetActive(true);
                                    UIManager.Instance.femaleYellowCard.SetActive(false);

                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                                    UIManager.Instance.mainPopup.SetActive(true);
                                    GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup);

                                    CheckCoin();

                                }

                            }
                            else
                            {
                                Debug.Log("Male player has less than 100 points. Condition is false.");
                                UIManager.Instance.retryPanel.SetActive(true);
                                StartCoroutine(UIManager.Instance.WaitRetryTurn());
                                return;
                            }  

                        } 
                        else
                        {     
                            Debug.LogError("Another Index is call ...");  

                            GameManager.Instance.placePopUpBg.sprite = GameManager.Instance.placeSprite[curIndex-1]; 
                            GameManager.Instance.placeNameText.text= GameManager.Instance.placeName[curIndex - 1];  
                            GameManager.Instance.deskNameText.text= GameManager.Instance.deskName[curIndex - 1];     
                            GameManager.Instance.SpaceNameText.text= GameManager.Instance.spaceName[curIndex - 1];

                            if (curIndex == 14)
                            {

                                Debug.LogError("14141414141414141");
                                cardCategory = "6731e6cb2fbc87f8c64bb9a7 ";   //Show Me and Tease Me Deck - sun room    
                                PlayerPrefs.SetString("CardCategory", cardCategory);

                                string maleName = PlayerPrefs.GetString("MaleFirstName");
                                string femaleName = PlayerPrefs.GetString("FeMaleFirstName");

                                if (GameManager.Instance.curTurn == Turn.Male)
                                {    
                                    Debug.LogError("14141414141414141"+"xdhbfhjzsdbfhjkbxzdhfbj");
                                    UIManager.Instance.maleName.text = femaleName;
                                    UIManager.Instance.femaleName.text = maleName;
                                }
                                else
                                {
                                    Debug.LogError("14141414141414141"+"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                                    UIManager.Instance.maleName.text = maleName;
                                    UIManager.Instance.femaleName.text = femaleName;
                                }  

                            }
                            else if (curIndex == 18)
                            {   
                                Debug.Log("Punishment Deck  is calll ");
                                cardCategory = "672e037c319d1f832d39769d";  // Punishment Deck 
                                PlayerPrefs.SetString("CardCategory", cardCategory);
                            }
                            else
                            {
                                cardCategory = "672e01e2319d1f832d397644";  //Activity Deck  
                                PlayerPrefs.SetString("CardCategory", cardCategory);
                            } 

                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); 
                            UIManager.Instance.mainPopup.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);   

                            UIManager.Instance.mainPopup.SetActive(true);
                            UIManager.Instance.maleYellowCard.SetActive(false);
                            UIManager.Instance.femaleYellowCard.SetActive(false);

                            GamePlayUIAnimation.ins.OpenPopUp(UIManager.Instance.mainPopup);
                            //Debug.Log("Card Deck ID : " + PlayerPrefs.GetString("CardCategory"));
                            StartCoroutine(GetCardsAPI.Instance.GetCardsRequest());  

                        } 

                    }
                    else  
                    {
                        if (waypoints[waypointIndex] == waypoints[waypoints.Length - 1])
                        {
                            waypointIndex = 0;
                        }
                        else
                        {
                            waypointIndex ++;
                        } 
                        
                    }  

                    UIManager.Instance.issHisMoveBackSpaces = false;
                }
            }
        } 
        else
        {
            transform.position = finalPos;
            canMove = false;
            UIManager.Instance.mainPopup.SetActive(true); 
            UIManager.Instance.isSatisfiedNo = false;
        }

    }  
     
    public IEnumerator HandleTurnPopupFeMale()
    {
        
        yield return new WaitForSeconds(3f);
        UIManager.Instance.youGetTurnPopup.SetActive(false);
        if (GameManager.Instance.curTurn == Turn.Female)
        {
            Debug.Log("Resetting turn to Female and handling movement");

            // Highlight the female's turn in the UI
            UIManager.Instance.maleHighLightBorder.SetActive(true);
            UIManager.Instance.feMaleHighLightBorder.SetActive(false);

            // Ensure the female player can move
            DiceManager.instance.femalePlayer.canMove = true;

            // Check if the final position is set and initiate movement
            if (DiceManager.instance.femalePlayer.finalPos != Vector3.zero)
            {
                DiceManager.instance.femalePlayer.Move();
            }
            else
            {
                Debug.LogWarning("Female player's final position is not set!");
            }  

        }
        else
        {
            Debug.LogError("It's not Female's turn. Male turn should be handled elsewhere.");
            // Reset UI for the male's turn
            GameManager.Instance.curTurn = Turn.Female;   
            UIManager.Instance.maleHighLightBorder.SetActive(true);
            UIManager.Instance.feMaleHighLightBorder.SetActive(false);
        } 

        // Hide any unnecessary panels
        UIManager.Instance.discPanel.SetActive(false); 

    }

    public IEnumerator HandleTurnPopupMale()
    { 
        yield return new WaitForSeconds(3f);
        UIManager.Instance.youGetTurnPopup.SetActive(false);

        if (GameManager.Instance.curTurn == Turn.Male)
        {
            Debug.Log("Resetting turn to Female and handling movement");

            // Highlight the female's turn in the UI
            UIManager.Instance.maleHighLightBorder.SetActive(true);
            UIManager.Instance.feMaleHighLightBorder.SetActive(false);

            // Ensure the female player can move
            DiceManager.instance.femalePlayer.canMove = true;

            // Check if the final position is set and initiate movement
            if (DiceManager.instance.femalePlayer.finalPos != Vector3.zero)
            {
                DiceManager.instance.femalePlayer.Move();
            }
            else
            {
                Debug.LogWarning("Female player's final position is not set!");
            }


        }
        else
        {
            Debug.Log("It's not Female's turn. Male turn should be handled elsewhere.");
            // Reset UI for the male's turn
            UIManager.Instance.maleHighLightBorder.SetActive(false);
            UIManager.Instance.feMaleHighLightBorder.SetActive(true);
            GameManager.Instance.curTurn = Turn.Male;  
        } 
        
        // Hide any unnecessary panels
        UIManager.Instance.discPanel.SetActive(false);

    }

    public void CheckCoin()
    {
        if (CoinManager.instance.malePoint >= 100 || CoinManager.instance.feMalePoint >= 100)
        {
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(true);

            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(true); 
        }
        else if (CoinManager.instance.malePoint <= 100 || CoinManager.instance.feMalePoint <= 100 || 
            CoinManager.instance.malePoint >= 50 || CoinManager.instance.feMalePoint >= 50)
        {

            if (GameManager.Instance.gameLevel >= 5)
            {
                Debug.Log(GameManager.Instance.gameLevel);  
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if(GameManager.Instance.gameLevel <= 1)
            {
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }

            if(GameManager.Instance.maleDressLevel>=5)
            {
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
            if(GameManager.Instance.maleDressLevel <= 1)
            {
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }

            if (GameManager.Instance.femaleDressLevel >= 5)
            {
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (GameManager.Instance.femaleDressLevel <= 1)
            {
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
                
            //UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            // UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            //UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            //UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);  

            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(true);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(true);

            /*UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);*/ 

          /*UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);*/
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(true);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(true);  

        }
        else
        {
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            UIManager.Instance.maleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(false);

            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            UIManager.Instance.femaleYellowCard.gameObject.transform.GetChild(5).gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// OnDrawGizmos code is
    /// </summary>
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Draw waypoints as wireframe cubes
        Gizmos.color = Color.blue;
        foreach (var waypoint in waypoints)
        {
            if (waypoint != null)
                Gizmos.DrawWireCube(waypoint.position, Vector3.one * 0.3f);
        }

        // Draw dotted lines between waypoints
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                DrawDottedLine(waypoints[i].position, waypoints[i + 1].position, 0.2f);
        }

    }

    private void DrawDottedLine(Vector3 start, Vector3 end, float gapLength)
    {
        float distance = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;
        float drawPos = 0f;

        while (drawPos < distance)
        {
            Vector3 segmentStart = start + direction * drawPos;
            drawPos += gapLength;
            Vector3 segmentEnd = start + direction * Mathf.Min(drawPos, distance);
            Gizmos.DrawLine(segmentStart, segmentEnd);
            drawPos += gapLength;
        }

    }  

}
