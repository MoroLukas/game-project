using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public Sprite slime_left_still;
    public Sprite slime_left_walk;
    public Sprite slime_right_still;
    public Sprite slime_right_walk;
    private SpriteRenderer spriteRenderer;

    public float speed = 5f;
    public Transform player;
    public float attackDelay = 1f; // secondi

    private Rigidbody2D rb;
    private float timerAttack;
    private float timerSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timerAttack += Time.deltaTime;
        timerSprite += Time.deltaTime;

        if (timerSprite >= 0.25)
        {
            if (spriteRenderer.sprite == slime_left_still)
            {
                spriteRenderer.sprite = slime_left_walk;
            }
            else if (spriteRenderer.sprite == slime_left_walk)
            {
                spriteRenderer.sprite = slime_left_still;
            }
            else if (spriteRenderer.sprite == slime_right_walk)
            {
                spriteRenderer.sprite = slime_right_still;
            }
            else if (spriteRenderer.sprite == slime_right_still)
            {
                spriteRenderer.sprite = slime_right_walk;
            }
            timerSprite = 0;
        }

        if (timerAttack >= attackDelay)
        {
            Attack();
            timerAttack = 0;
        }
    }

    void FixedUpdate() // Rallenta gradualmente dopo l'attacco
    {
        rb.linearVelocity *= 0.95f;
    }

    void Attack()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x < 0)
        {
            spriteRenderer.sprite = slime_left_still;
        }
        else
        {
            spriteRenderer.sprite = slime_right_still;
        }
        rb.linearVelocity = direction * speed;
    }
}