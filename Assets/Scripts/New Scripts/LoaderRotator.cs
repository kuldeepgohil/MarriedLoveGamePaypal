using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRotator : MonoBehaviour
{
    public RectTransform loader;

    public void Update()
    {
        loader.Rotate(new Vector3(0,0,1), 2.5f);
    }
}
