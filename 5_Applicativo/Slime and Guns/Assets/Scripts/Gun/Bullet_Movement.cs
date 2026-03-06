using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet_Movement : MonoBehaviour
{
    Rigidbody2D rb;

    public float posX;
    public float posY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;


        posX = direction.x;
        posY = direction.y;

        rb.linearVelocity = new Vector2 (posX, posY).normalized * 5f;

        Destroy(gameObject, 4f);
    }
}

