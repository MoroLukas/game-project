using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject ColorPrefab;

    public float posX;
    public float posY;


    private Vector2 targetPoint;
    private bool targetSet = false;
    Vector3 mousePos;
    Vector3 worldPos;
    Vector2 origin;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetSet)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;

            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            origin = transform.position;
            direction = (worldPos - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, 100f);

            if (hit.collider != null)
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = worldPos;
            }
            targetSet = true;
        }else{

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
        Instantiate(ColorPrefab, transform.position, Quaternion.identity);
    }
}
