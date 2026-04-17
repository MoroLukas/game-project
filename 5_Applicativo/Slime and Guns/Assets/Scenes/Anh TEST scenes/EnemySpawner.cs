using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmerPrefab;

    [SerializeField]
    private float swarmerInterval = 3.5f;


    void Start()
    {
        StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));

    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
