using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    private PlayerMovement PlayerMovement;

    Vector3 scale;
    Vector3 initaialScale;

    private void Start()
    {
        scale = Vector3.one;
    }
    public void enlargementUpgrade()
    {
        initaialScale = transform.localScale;

        scale.x = initaialScale.x * 1.4f;
        scale.y = initaialScale.y * 1.4f;

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerMovement.speed -= 1f;

        transform.localScale = scale;
    }

    public void shrinkingUpgrade()
    {
        initaialScale = transform.localScale;

        scale.x = initaialScale.x / 1.4f;
        scale.y = initaialScale.y / 1.4f;

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerMovement.speed += 1f;

        transform.localScale = scale;
    }
}
