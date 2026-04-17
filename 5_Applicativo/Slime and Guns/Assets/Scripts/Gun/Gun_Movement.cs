using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Movement : MonoBehaviour
{
    public Transform playerTransform;
    public Transform bulletPoint;
    public GameObject bulletPrefab;

    bool canFire = true;
    float waitingTime = 0.5f;
    float timer;

    public float distanceFromPlayer = 0.5f;
    Vector3 baseLocalPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseLocalPos = transform.localPosition; //posizione per muovere la pistola
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        Vector3 scale = Vector3.one;

        scale.y = 2f;

        Vector3 targetPos;

        if (direction.x > 0)
        {
            scale.x = -2f;
            transform.right = direction;
            targetPos = baseLocalPos  + new Vector3(0.35f, 0, 0); ;

        }
        else
        {
            scale.x = 2f;
            transform.right = -direction;
            targetPos = baseLocalPos;
        }

        transform.localScale = scale;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 10f * Time.deltaTime);


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
