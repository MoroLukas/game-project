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
    }

    void Update()
    {
        if (!targetSet)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 origin = transform.position;
            Vector2 direction = (worldPos - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, 100f);

            if (hit.collider != null)
            {
                targetPoint = hit.point;
                enemyhit = true;
            }
            else
            {
                targetPoint = worldPos;

                enemyhit = false;
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
    private void OnDestroy()
    {
        if (!enemyhit)
        { Instantiate(ColorPrefab, transform.position, Quaternion.identity); }

    }
}