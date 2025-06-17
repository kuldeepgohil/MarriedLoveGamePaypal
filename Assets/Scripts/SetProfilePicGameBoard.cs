using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetProfilePicGameBoard : MonoBehaviour
{
    public Image maleProfilePic;
    public Image femaleProfilePic;

    public Texture2D maleDefalutAvatar;
    public Texture2D femaleDefalutAvatar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetuserProfile(maleProfilePic, PlayerPrefs.GetString("MaleProfilePic"), maleDefalutAvatar));
        StartCoroutine(GetuserProfile(femaleProfilePic, PlayerPrefs.GetString("FemaleProfilePic"), femaleDefalutAvatar));
    }

    IEnumerator GetuserProfile(Image pp, string url, Texture2D defalutAvatar)
    {
        string spriteurl = url;
        WWW w = new WWW(spriteurl);
        yield return w;


        if (w.error != null)
        {
            Debug.Log("error ");
            //show default image
            //allgameList[i].banner = defaultIcon;
            pp.sprite = Sprite.Create(defalutAvatar, new Rect(0, 0, defalutAvatar.width, defalutAvatar.height), Vector2.zero);

        }
        else
        {
            if (w.isDone)
            {
                Texture2D tx = w.texture;
                pp.sprite = Sprite.Create(tx, new Rect(0f, 0f, tx.width, tx.height), Vector2.zero, 10f);
            }
        }
    }
}
