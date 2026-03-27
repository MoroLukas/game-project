using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet_Movement : MonoBehaviour
{
    public float speed = 5f;
    public GameObject ColorPrefab;

    public float posX;
    public float posY;

    private Vector2 targetPoint;
    private bool targetSet = false;
    private Rigidbody2D rb;

    bool enemyhit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D playerCol = player.GetComponent<Collider2D>();
            Collider2D bulletCol = GetComponent<Collider2D>();

            if (playerCol != null && bulletCol != null)
                Physics2D.IgnoreCollision(bulletCol, playerCol);
        }
    }

    void Update()
    {
        if (!targetSet)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            worldPos.z = 0;

            Vector2 origin = transform.position;
            Vector2 direction = (worldPos - transform.position).normalized;

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, 100f);

            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    targetPoint = hit.point;
                    break;
                }
            }

            targetSet = true;
        }
        else
        {
            Vector2 direction = targetPoint - (Vector2)transform.position;

            if (direction.magnitude < 0.1f)
            {
                Destroy(gameObject);
                targetSet = false;
                return;
            }

            posX = direction.x;
            posY = direction.y;

            rb.linearVelocity = new Vector2(posX, posY).normalized * 5f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemyhit = true;
            collision.GetComponent<SlimeScript>().TakeHit();
            Destroy(gameObject);
        }
        else
        {
            
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!enemyhit)
        { Instantiate(ColorPrefab, transform.position, Quaternion.identity); }

    }
}