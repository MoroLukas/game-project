using UnityEngine;
using UnityEngine.UI; 

public class Crosshair : MonoBehaviour
{
    public RectTransform crosshair;
    public float expandAmount = 10f;
    public float shrinkSpeed = 5f;
    private float currentSize = 0f;
    void Update()
    {
        if (currentSize > 0)
        {
            currentSize -= shrinkSpeed * Time.deltaTime;
            crosshair.sizeDelta = new Vector2(20 + currentSize, 20 + currentSize);
        }
    }
    public void Shoot()
    {
        currentSize = expandAmount;
    }
}