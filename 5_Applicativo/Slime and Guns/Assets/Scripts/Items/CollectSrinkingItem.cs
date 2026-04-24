using UnityEngine;

public class CollectSrinkingItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("you got the srinking Item"); //visualizzazione su console
            Destroy(gameObject);

            collision.GetComponent<PlayerUpgrade>().shrinkingUpgrade();
        }
    }
}
