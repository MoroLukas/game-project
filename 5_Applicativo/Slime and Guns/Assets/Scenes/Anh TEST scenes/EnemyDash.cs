using UnityEngine;
using UnityEngine.AI;

/*quessto script è fatto con " 2026 | 2D NavMesh | 2D Pathfinding | Unity Game Engine (Simple & Easy) "
però incoporato con un script di un membro della squadra e retouch with chatpt per fare codice più pulita*/

public class SlimeEnemy : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite slime_left_still;
    public Sprite slime_left_walk;
    public Sprite slime_right_still;
    public Sprite slime_right_walk;

    private SpriteRenderer spriteRenderer;

    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float attackDelay = 1f;

    private NavMeshAgent agent;
    private float timerAttack;
    private float timerSprite;

    [Header("Combat")]
    public float knockback = 4f;

    [Header("Health")]
    public int maxHits = 3;
    private int currentHits = 0;

    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject SmallStainPrefab;
    public GameObject StainPrefab;
    public Transform stainPoint;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure it has the 'Player' tag.");
        }
    }

    void Update()
    {
        timerAttack += Time.deltaTime;
        timerSprite += Time.deltaTime;

        // Movement (NavMesh)
        agent.SetDestination(player.position);

        // Sprite animation
        if (timerSprite >= 0.25f)
        {
            if (agent.velocity.x < 0)
            {
                spriteRenderer.sprite =
                    spriteRenderer.sprite == slime_left_still ? slime_left_walk : slime_left_still;
            }
            else if (agent.velocity.x > 0)
            {
                spriteRenderer.sprite =
                    spriteRenderer.sprite == slime_right_still ? slime_right_walk : slime_right_still;
            }

            timerSprite = 0;
        }

        // Attack timing
        if (timerAttack >= attackDelay)
        {
            Attack();
            timerAttack = 0;
        }
    }

    void Attack()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Simulate dash by boosting agent velocity
        agent.velocity = direction * agent.speed * 3f;

        // Set sprite direction
        if (direction.x < 0)
            spriteRenderer.sprite = slime_left_still;
        else
            spriteRenderer.sprite = slime_right_still;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.position - transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(direction * knockback, ForceMode2D.Impulse);

            timerAttack = 0;

            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            ph?.TakeDamage(1);
        }
    }

    public void TakeHit()
    {
        currentHits++;

        if (currentHits == 1)
        {
            Instantiate(SmallStainPrefab, stainPoint.position, Quaternion.identity, stainPoint);
        }
        else if (currentHits == 2)
        {
            Instantiate(StainPrefab, stainPoint.position, Quaternion.identity, stainPoint);
        }
        else if (currentHits >= maxHits)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, stainPoint.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}