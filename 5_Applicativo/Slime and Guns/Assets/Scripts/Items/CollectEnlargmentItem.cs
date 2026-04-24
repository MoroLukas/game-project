using Unity.VisualScripting;
using UnityEngine;

public class CollectEnlargmentItem : MonoBehaviour
{
    //trigger del collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("you got the entlargment Item"); //visualizzazione su console
            Destroy(gameObject);

            collision.GetComponent<PlayerUpgrade>().enlargementUpgrade();
        }
    }
}
