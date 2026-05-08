using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

/* this script was made following this tutorial "Unity Tutorial(2021) -Making an Enemy Spawner" and modified based on my necessity*/

using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private float swarmerInterval = 3.5f;

    private bool isSpawning = false;

    public void ActivateSpawner()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemy(swarmerInterval, swarmerPrefab));
        }
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }
}