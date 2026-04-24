using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;

    public float range = 8f;
    public float fireRate = 1f;

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= range)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        // direzione verso player
        Vector2 direction = (player.position - transform.position).normalized;

        // crea proiettile nella posizione della torretta
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // lo manda verso il player
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
}