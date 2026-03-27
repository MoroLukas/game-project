using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Movement : MonoBehaviour
{
    public Transform bulletPoint;
    public GameObject bulletPrefab;

    bool canFire = true;
    float waitingTime = 0.5f;
    float timer;

    public Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position; //impostazione della direzione verso il mouse
        
        

        Vector3 scale = Vector3.one;

        //sistemazione della dirazione della sprite
        if(direction.x > 0)
        {
            scale.x = -1f;
            transform.right = direction;
        }
        else
        {
            scale.x = 1f; 
            transform.right = -direction;
        }

        transform.localScale = scale;


        //istanziazione del proiettile
        if (Mouse.current.leftButton.wasPressedThisFrame && canFire)
        {
            Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
            canFire = false;
        }

        if (!canFire)//tempo di attesa per sparare
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
