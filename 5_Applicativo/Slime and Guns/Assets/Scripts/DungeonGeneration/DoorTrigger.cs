using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Destinazione: Transform della porta di arrivo")]
    public Transform destination;

    [Header("Tag del giocatore")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Sposta il player all'ingresso dell'altra stanza
        other.transform.position = destination.position;

        // Opzionale: centra la camera
        Camera.main.transform.position = new Vector3(
            destination.position.x,
            destination.position.y,
            Camera.main.transform.position.z);
    }
}