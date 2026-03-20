using UnityEngine;

public class enemy : MonoBehaviour
{
    public int maxHits = 3;
    private int currentHits = 0;

    public GameObject deathEffect;
    public GameObject SmallStainPrefab;
    public GameObject StainPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            currentHits++;

            if (currentHits == 1)
            {
                Instantiate(SmallStainPrefab, transform.position, Quaternion.identity);
            }
            else if (currentHits == 2)
            {
                Instantiate(StainPrefab, transform.position, Quaternion.identity);
                Destroy(SmallStainPrefab);
            }
            else if (currentHits >= maxHits)
            {
                if (deathEffect != null)
                    Instantiate(deathEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

}
