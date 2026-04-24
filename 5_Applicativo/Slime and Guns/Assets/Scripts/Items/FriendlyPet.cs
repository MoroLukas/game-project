using UnityEngine;

public class FriendlyPet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireRate = 2f; // tempo tra un colpo e l'altro
    private float fireCooldown = 0f;

    public float detectionRadius = 5f;
    public LayerMask enemyLayer;

    private Transform targetEnemy;


    // Update is called once per frame
    void Update()
    {
        // Cerca un nemico nel raggio
        FindEnemy();

        // Se c'è un nemico → ruota verso di lui e spara
        if (targetEnemy != null)
        {
            RotateTowardsEnemy();
            Shoot();
        }

        // Aggiorna cooldown
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

    void RotateTowardsEnemy()
    {
        Vector2 direction = targetEnemy.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        if (fireCooldown <= 0f)
        {
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            fireCooldown = fireRate;
        }
    }

    // Per vedere il raggio in scena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
