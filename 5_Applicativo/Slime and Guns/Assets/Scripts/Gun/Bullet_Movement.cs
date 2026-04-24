using UnityEngine;
using UnityEngine.InputSystem;


public class Bullet_Movement : MonoBehaviour
{
    public float speed = 10f;
    public GameObject ColorPrefab;

    private Rigidbody2D rb;
    private bool enemyHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        Vector2 direction = (mousePos - transform.position).normalized;

        rb.linearVelocity = direction * speed;

        // Ignora il player all'istanziazione per evitare confilitto
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(
                GetComponent<Collider2D>(),
                player.GetComponent<Collider2D>()
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Quando "colpisce" se stesso
        if (collision.CompareTag("Player"))
            return;

        // Se colpisce un nemico
        if (collision.CompareTag("enemy"))
        {
            enemyHit = true;
            collision.GetComponent<SlimeScript>().TakeHit();
            Destroy(gameObject);
        }
        else
        {
            // Se colpisce un muro 
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Solamente se non ha colpito un nemico
        if (!enemyHit)
        {
            Instantiate(ColorPrefab, transform.position, Quaternion.identity);
        }
    }
}
