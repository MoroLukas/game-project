using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.RuleTile.TilingRuleOutput.Transform;

public class Gun_Movement : MonoBehaviour
{
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

        Vector3 direction = mousePos - transform.position;
        
        

        Vector3 scale = Vector3.one;

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

        
    }
}
