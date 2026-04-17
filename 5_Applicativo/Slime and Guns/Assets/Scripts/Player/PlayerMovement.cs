using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Sprite player_left_still;
    public Sprite player_left_walk;
    public Sprite player_right_still;
    public Sprite player_right_walk;

    private SpriteRenderer spriteRenderer;
    public float change_sprite_timer = 0;
    private bool facingRight = true;

    public float speed = 2f;
    public float acceleration = 10f;
    public float deceleration = 6f;

    private Rigidbody2D rb;
    private Vector2 movementInput = Vector2.zero;

    public float invincibilityTime = 1f;
    public float flashInterval = 0.05f;

    private bool isInvincible = false;


    //DASH variables
    public float dashSpeed = 10f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1.0f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 4.0f;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() //usato per aggiornare la logica che non dipende dalla fisica
    {
        // Leggi input
        movementInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementInput.y = 1;
        if (Keyboard.current.sKey.isPressed) movementInput.y = -1;
        if (Keyboard.current.aKey.isPressed) movementInput.x = -1;
        if (Keyboard.current.dKey.isPressed) movementInput.x = 1;

        if (movementInput.x > 0) facingRight = true;
        if (movementInput.x < 0) facingRight = false;

        if (movementInput != Vector2.zero)
        {
            if (change_sprite_timer >= (30 / speed))
            {
                if (facingRight)
                {
                    spriteRenderer.sprite = (spriteRenderer.sprite == player_right_still)
                        ? player_right_walk
                        : player_right_still;

                }
                else
                {
                    spriteRenderer.sprite = (spriteRenderer.sprite == player_left_still)
                        ? player_left_walk
                        : player_left_still;

                }

                change_sprite_timer = 0;
            }
            change_sprite_timer++;
        }
        else
        {
            spriteRenderer.sprite = facingRight ? player_right_still : player_left_still;
        }

        movementInput = movementInput.normalized; // normalizza la velocità, così non va più veloce in diagonale


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//utilizzo del mouse per cambiare sprite
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        if (direction.x > 0) facingRight = true;//cambio posizione seconda del mouse
        if (direction.x < 0) facingRight = false;

        Vector3 scale = Vector3.one;


        if (facingRight){ //cambio sprite a seconda del mouse 
            spriteRenderer.sprite = player_right_still;
        }
        else
        {
            spriteRenderer.sprite = player_left_still;
        }


        if (Keyboard.current.spaceKey.wasPressedThisFrame && dashCooldownTimer <= 0f && movementInput != Vector2.zero) //contrlla che sia schiacciato lo spazio
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashDirection = movementInput; // direzione del dash
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

    }

    void FixedUpdate() //usato per la fisica
    {
        if (isDashing)//per dashing
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
                isDashing = false;

            return;
        }

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

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            StartCoroutine(Flash()); //per farlo lampeggiare quando prende danno
        }
    }

    IEnumerator Flash()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibilityTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}