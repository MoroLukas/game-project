using UnityEngine;
using UnityEngine.AI;

/*this script was made following this tutorial 2026 | 2D NavMesh | 2D Pathfinding | Unity Game Engine (Simple & Easy) 
 but using chatgpt to intergate with the SlimeScript from my group*/


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

    [Header("NavMesh Movement")]
    private NavMeshAgent agent;

    [Header("Dash Attack")]
    public float attackDelay = 1f;
    public float dashForce = 6f;
    public float dashDuration = 0.2f;

    private Rigidbody2D rb;
    private float attackTimer;
    private float spriteTimer;

    private bool isDashing;
    private float dashTimer;

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
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Fixes for 2D + NavMesh
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        spriteTimer += Time.deltaTime;

        // NAVMESH CHASE (only when not dashing)
        if (!isDashing)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        HandleSprite();

        if (attackTimer >= attackDelay && !isDashing)
        {
            StartDash();
            attackTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // sync NavMesh position to Rigidbody
            agent.nextPosition = rb.position;
        }

        if (isDashing)
        {
            rb.linearVelocity *= 0.95f;

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
                EndDash();
        }
    }

    void StartDash()
    {
        Vector3 nextPos = agent.steeringTarget;
        Vector2 direction = (nextPos - transform.position).normalized;

        // stop navmesh
        agent.isStopped = true;
        agent.ResetPath();

        isDashing = true;
        dashTimer = dashDuration;

        rb.linearVelocity = direction * dashForce;

        spriteRenderer.sprite =
            (direction.x < 0) ? slime_left_still : slime_right_still;
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;

        agent.isStopped = false;
    }

    void HandleSprite()
    {
        if (spriteTimer < 0.25f) return;

        Vector2 vel = isDashing ? rb.linearVelocity : agent.velocity;

        if (vel.x < 0)
        {
            spriteRenderer.sprite =
                (spriteRenderer.sprite == slime_left_still)
                ? slime_left_walk
                : slime_left_still;
        }
        else if (vel.x > 0)
        {
            spriteRenderer.sprite =
                (spriteRenderer.sprite == slime_right_still)
                ? slime_right_walk
                : slime_right_still;
        }

        spriteTimer = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 dir = (player.position - transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(dir * knockback, ForceMode2D.Impulse);

            rb.AddForce(-dir * knockback / 2f, ForceMode2D.Impulse);

            attackTimer = 0f;

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