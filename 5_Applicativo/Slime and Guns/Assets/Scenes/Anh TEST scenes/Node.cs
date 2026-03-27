using System.Collections.Generic;
using UnityEngine;



public class Node : MonoBehaviour
{

    //this code was made with "easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]"
    //but i ask Ai to upgrade it to a version where the nodes are auto connected between them.
    public Node cameFrom;
    public List<Node> connections = new List<Node>();

    public float gScore;
    public float hScore;

    public float neighborRadius = 2f;
    public LayerMask wallLayer; // assign in Inspector

    public float FScore()
    {
        return gScore + hScore;
    }

    void Start()
    {
        FindNeighbors();
    }

    void FindNeighbors()
    {
        connections.Clear();

        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node n in allNodes)
        {
            if (n == this) continue;

            Vector2 dir = n.transform.position - transform.position;
            float dist = dir.magnitude;

            if (dist <= neighborRadius)
            {
                // 🔥 check for wall
                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    dir.normalized,
                    dist,
                    wallLayer
                );

                // only connect if nothing blocks the path
                if (hit.collider == null)
                {
                    connections.Add(n);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // connections
        Gizmos.color = Color.blue;
        foreach (Node n in connections)
        {
            if (n != null)
                Gizmos.DrawLine(transform.position, n.transform.position);
        }

        // radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
    }
}