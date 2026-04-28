using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;

    public void Awak()
    {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        display_Text.text = "FPS : " + fps.ToString();
    }  

}