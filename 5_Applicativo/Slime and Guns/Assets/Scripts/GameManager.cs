using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;      // Disattiva VSync
        Application.targetFrameRate = 60;    // Blocca a 60 FPS
    }
}