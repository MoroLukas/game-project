using UnityEngine;

public class BulletMovement2 : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float detectionRadius = 10f;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Transform targetEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        FindClosestEnemy();
    }

    void Update()
    {
        if (targetEnemy == null)
        {
            FindClosestEnemy();
        }
        else
        {
            // direzione verso il nemico
            Vector2 direction = (targetEnemy.position - transform.position).normalized;

            // Rotazione verso il nemico
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0, 0, angle),
                rotateSpeed * Time.deltaTime
            );

            rb.linearVelocity = transform.right * speed;
        }
    }

    void FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRadius,
            enemyLayer
        );

        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetEnemy = enemy.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            SlimeScript slime = collision.GetComponent<SlimeScript>();

            if (slime != null)
            {
                slime.TakeHit();
            }

            Destroy(gameObject);
        }
    }
}