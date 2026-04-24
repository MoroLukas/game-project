using UnityEngine;

public class SpawnerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHits = 3;
    private int currentHits = 0;
    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject SmallStainPrefab;
    public GameObject StainPrefab;
    public Transform stainPoint;




    public void TakeHit()
    {
        currentHits++;

        if (currentHits == 1)
        {
            Instantiate(SmallStainPrefab, stainPoint.position, Quaternion.identity, stainPoint);
        }
        else if (currentHits == 2)
        {
            Instantiate(StainPrefab, stainPoint.position, Quaternion.identity, stainPoint);
        }
        else if (currentHits >= maxHits)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, stainPoint.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
