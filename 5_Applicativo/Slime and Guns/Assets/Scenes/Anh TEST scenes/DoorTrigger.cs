using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner; // Assign your spawner in Inspector
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            spawner.ActivateSpawner();
            Debug.Log("Player passed through trigger, spawner activated!");
        }
    }
}