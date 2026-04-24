using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 4f;
    private Vector2 direction;
    private Rigidbody2D rb;
    public GameObject ColorPrefab;
    private bool playerHit = false;

    void Start()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Se colpisce il player
        if (collision.CompareTag("Player"))
        {
            playerHit = true;
            //collision.GetComponent<SlimeScript>().TakeHit();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("enemy") || collision.CompareTag("bullet"))
        {
            //se colpisce un nemico
        } else
        {
            // Se colpisce un muro 
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Solamente se non ha colpito un nemico
        if (!playerHit)
        {
            Instantiate(ColorPrefab, transform.position, Quaternion.identity);
        }
    }

}