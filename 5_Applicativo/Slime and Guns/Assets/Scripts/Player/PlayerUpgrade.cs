using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    private PlayerMovement PlayerMovement;

    Vector3 scale;
    Vector3 initaialScale;

    float scaleMultiplier = 1.4f;
    float speedAddition = 1f;

    float maxScale = 3.92f;
    float minScale = 1.0f;

    float minSpeed = 1f;
    float maxSpeed = 3f;

    private void Start()
    {
        scale = Vector3.one;

        PlayerMovement = GetComponent<PlayerMovement>();
    }
    public void enlargementUpgrade()
    {
        initaialScale = transform.localScale;

        if (initaialScale.x < maxScale && initaialScale.y < maxScale) //lascia che il personaggio possa solo ringrandirsi due volte
        { 
            scale.x = initaialScale.x * scaleMultiplier;
            scale.y = initaialScale.y * scaleMultiplier;
        }


        if (PlayerMovement.speed > minSpeed)//serve a fere in modo che non rimanga mai immobile
        {
            PlayerMovement.speed -= speedAddition;
        }

        transform.localScale = scale;
    }

    public void shrinkingUpgrade()
    {
        initaialScale = transform.localScale;

        if (initaialScale.x > minScale && initaialScale.y > minScale) //fa in modo che non diventi troppo piccolo
        {
            scale.x = initaialScale.x / scaleMultiplier;
            scale.y = initaialScale.y / scaleMultiplier;
        }

        if (PlayerMovement.speed < maxSpeed) //fa in modo che non diventi troppo veloce
        {
            PlayerMovement.speed += speedAddition;
        }

        transform.localScale = scale;
    }
}
