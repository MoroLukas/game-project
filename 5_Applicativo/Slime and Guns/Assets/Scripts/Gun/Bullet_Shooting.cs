using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet_Shooting : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bulletPrefab;

    bool canFire = true;
    float waitingTime = 0.5f;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && canFire) 
        {
           Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
           canFire = false;
        }

        if (!canFire)
        {
            timer = Time.time;

            if (timer > waitingTime)
            {
                canFire = true;
                timer = 0;
            }
        }
    }
}
