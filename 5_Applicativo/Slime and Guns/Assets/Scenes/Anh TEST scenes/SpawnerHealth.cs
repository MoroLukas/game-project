using UnityEngine;

public class SpawnerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    public int maxHealth = 10;

    private int currentHealth;

    [Header("Effects")]
    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
