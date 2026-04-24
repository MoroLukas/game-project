using UnityEngine;

public class ColorSplash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 15f); //distrugge l'ogetto dopo 15 secondi
    }
}
