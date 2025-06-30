using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticId : MonoBehaviour
{

    public static StaticId Instance;

    public void Awake()
    {
        Instance = this;
    }

    [Header("Male Dress ID")]
    public string mlevel1 = "673723a4ad0fc74347fb6b29";  
    public string mlevel2 = "673723caad0fc74347fb6b2a";  
    public string mlevel3 = "673723d0ad0fc74347fb6b2c";  
    public string mlevel4 = "673aeb0384df36c9ac514f13";  
    public string mlevel5 = "673aea2784df36c9ac514f10";

    [Header("FeMale Dress ID")]
    public string felevel1 = "673ae99f84df36c9ac514f0f";
    public string felevel2 = "673aeaac84df36c9ac514f11";
    public string felevel3 = "673aeaec84df36c9ac514f12";
    public string felevel4 = "673aeb2484df36c9ac514f14";
    public string felevel5 = "673723d3ad0fc74347fb6b2d";

    [Header("Levels ID")]
    public string level1 = "671f5d5a93074f8291166a4b";
    public string level2 = "671f5dc993074f8291166a4c";
    public string level3 = "671f5dce93074f8291166a4d";
    public string level4 = "671f5ddb93074f8291166a4e";
    public string level5 = "671f5de193074f8291166a4f";

    [Header("Card Deck ID")]
    public string Activity = "672e01e2319d1f832d397644";
    public string Punishment  = "672e037c319d1f832d39769d";
    public string ShowmeTeaseme = "6731e6cb2fbc87f8c64bb9a7";


    [Header("Card Name")]

    public string ActivityName = "ACT";
    public string PunishmentName = "SEC";
    public string ShowmeTeasemeName = "SHO";

    [Header("Male Dress Level Name")]
    public string maleDress1 = "Fully Clothed";
    public string maleDress2 = "Shirt, Pants & Underwear";
    public string maleDress3 = "Pants & Underwear";
    public string maleDress4 = "Underwear";
    public string maleDress5 = "Fully Naked";

    [Header("FeMale Dress Level Name")]  
    public string femaleDress1one = "Fully Clothed";
    public string femaleDress2one = "Shirt/Bra,Pants,Panties";
    public string femaleDress3one = "Shirt/Bra & Panties";
    public string femaleDress4one = "Panties";
    public string femaleDress5one = "Fully Naked";

    [Header("Game Level Name")]  
    public string gamelevelNameONE = "Non-sex activities, lite pain";
    public string gamelevelNameTWO = "Sexual Intercourse Activities";
    public string gamelevelNameThree = "Oral Sex, Light Bondage";
    public string gamelevelNameFour = "Anal Play & Sex";
    public string gamelevelNameFive = "All of previous levels and full bondage/impact play";

}
