using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Sprite player_left_still;
    public Sprite player_left_walk;
    private SpriteRenderer spriteRenderer;
    public float change_sprite_timer = 0;

    public float speed = 2f;           
    public float acceleration = 10f;   
    public float deceleration = 6f; 

    private Rigidbody2D rb;
    private Vector2 movementInput = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Leggi input
        movementInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementInput.y = 1;
        if (Keyboard.current.sKey.isPressed) movementInput.y = -1;
        if (Keyboard.current.aKey.isPressed) movementInput.x = -1;
        if (Keyboard.current.dKey.isPressed) movementInput.x = 1;

        if (movementInput != Vector2.zero) 
        { 
            if (change_sprite_timer >= (120 / speed)) {
                if (spriteRenderer.sprite == player_left_still)
                {
                    spriteRenderer.sprite = player_left_walk;

                }
                else
                {
                    spriteRenderer.sprite = player_left_still;
                }
                change_sprite_timer = 0;
            }
            change_sprite_timer++;
        }
        else
        {
            change_sprite_timer = 0;
            spriteRenderer.sprite = player_left_still;
        }

            movementInput = movementInput.normalized; // direzione
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = movementInput * speed;


        if (movementInput != Vector2.zero)
        {
            rb.linearVelocity += (targetVelocity - rb.linearVelocity) * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            rb.linearVelocity -= rb.linearVelocity * deceleration * Time.fixedDeltaTime;
        }
    }
}