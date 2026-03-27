using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWithAStarAlgorithm : MonoBehaviour
{
    // --- Pathfinding ---
    public Node currentNode;
    public List<Node> path = new List<Node>();

    // --- Movement ---
    public float speed = 3f;

    // --- Combat ---
    public Transform player;
    public float attackDelay = 1f;
    public float knockback = 4f;

    // --- Visuals ---
    public Sprite slime_left_still;
    public Sprite slime_left_walk;
    public Sprite slime_right_still;
    public Sprite slime_right_walk;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    private float attackTimer;
    private float animTimer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Collider is trigger to avoid physical push with player
        col.isTrigger = true;

        // Kinematic rigidbody for controlled path movement
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        animTimer += Time.deltaTime;

        MoveAlongPath();
        Animate();
    }

    void MoveAlongPath()
    {
        if (path == null || path.Count == 0)
        {
            GenerateNewPath();
            return;
        }

        Node nextNode = path[0];
        Vector3 targetPosition = nextNode.transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Flip sprite based on movement direction
        if (targetPosition.x < transform.position.x)
            sr.sprite = slime_left_walk;
        else
            sr.sprite = slime_right_walk;

        // Reached current node
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentNode = nextNode;
            path.RemoveAt(0);
        }
    }

    void GenerateNewPath()
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        if (allNodes.Length == 0) return;

        Node randomTarget = allNodes[Random.Range(0, allNodes.Length)];
        path = AStarManager.instance.GeneratePath(currentNode, randomTarget);
    }

    void Animate()
    {
        if (animTimer < 0.25f) return;

        if (sr.sprite == slime_left_still)
            sr.sprite = slime_left_walk;
        else if (sr.sprite == slime_left_walk)
            sr.sprite = slime_left_still;
        else if (sr.sprite == slime_right_still)
            sr.sprite = slime_right_walk;
        else if (sr.sprite == slime_right_walk)
            sr.sprite = slime_right_still;

        animTimer = 0f;
    }

    // --- Trigger-based attack ---
    private void OnTriggerStay2D(Collider2D other)
    {
        if (player == null) return;

        if (other.transform == player)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                // Apply knockback to player
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                playerRb.linearVelocity = Vector2.zero;
                playerRb.AddForce(direction * knockback, ForceMode2D.Impulse);

                // Optional: small recoil for enemy
                rb.MovePosition(transform.position - (Vector3)(direction * knockback * 0.5f));

                // Deal damage
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                ph?.TakeDamage(1);

                attackTimer = 0f;
            }
        }
    }
}