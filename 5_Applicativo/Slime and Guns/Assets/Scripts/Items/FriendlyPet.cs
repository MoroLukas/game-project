using UnityEngine;

public class FriendlyPet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireRate = 2f; // tempo tra un colpo e l'altro
    private float fireCooldown = 0f;

    public float detectionRadius = 2.5f;
    public LayerMask enemyLayer;

    private Transform targetEnemy;


    // Update is called once per frame
    void Update()
    {
        // Cerca un nemico nel raggio di visione
        FindEnemy();


        if (targetEnemy != null)
        {
            Shoot();
        }

        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
    }

    void FindEnemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectionRadius, enemyLayer);

        if (enemy != null)
            targetEnemy = enemy.transform;
        else
            targetEnemy = null;
    }


    void Shoot()
    {
        if (fireCooldown <= 0f)
        {
            Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
            fireCooldown = fireRate;
        }
    }
}
