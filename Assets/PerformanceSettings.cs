using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 


    

    static void Init()
    {
        QualitySettings.vSyncCount = 0;

#if UNITY_ANDROID
        // Android Settings
        Application.targetFrameRate = 120;
        QualitySettings.SetQualityLevel(1); // Low
        QualitySettings.antiAliasing = 0;   // No AA
        QualitySettings.pixelLightCount = 0;
        QualitySettings.realtimeReflectionProbes = false;
        Debug.Log("✅ Android Performance Mode");

#elif UNITY_STANDALONE_WIN
        // PC Settings
        Application.targetFrameRate = 144;
        QualitySettings.SetQualityLevel(5); // Ultra
        QualitySettings.antiAliasing = 4;   // 4x AA
        QualitySettings.pixelLightCount = 4;
        QualitySettings.realtimeReflectionProbes = true;
        Debug.Log("✅ PC Ultra Mode");
#endif
    }
}