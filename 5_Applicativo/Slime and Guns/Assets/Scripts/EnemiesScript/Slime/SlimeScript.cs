using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public Sprite slime_left_still;
    public Sprite slime_left_walk;
    public Sprite slime_right_still;
    public Sprite slime_right_walk;
    private SpriteRenderer spriteRenderer;

    public float speed = 2f;
    public Transform player;
    public float attackDelay = 1f; // secondi

    private Rigidbody2D rb;
    private float timerAttack;
    private float timerSprite;

    public float knockback = 4f;

    public float wallCheckDistance = 1f;
    public LayerMask wallLayer;

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

        // Controllo se c'è un muro davanti
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

        if (hit.collider != null)
        {
            Vector2 dirX = new Vector2(direction.x, 0);
            Vector2 dirY = new Vector2(0, direction.y);

            bool blockedX = Physics2D.Raycast(transform.position, dirX, wallCheckDistance, wallLayer);
            bool blockedY = Physics2D.Raycast(transform.position, dirY, wallCheckDistance, wallLayer);

            if (!blockedX)
            {
                direction = dirX;
            }
            else if (!blockedY)
            {
                direction = dirY;
            }
            
        }

        direction = direction.normalized;

        // Sprite
        if (direction.x < 0)
            spriteRenderer.sprite = slime_left_still;
        else if (direction.x > 0)
            spriteRenderer.sprite = slime_right_still;

        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {

            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            Vector2 direction = (player.position - transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(direction * knockback, ForceMode2D.Impulse);

            rb.AddForce(-direction * knockback/2, ForceMode2D.Impulse);
            timerAttack = 0;
            
            PlayerMovement p = collision.gameObject.GetComponent<PlayerMovement>();

            if (p != null)
            {
                p.TakeDamage();
            }
        }

    }

}