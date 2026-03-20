using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] heartImages; // array di sprite cuori
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    void Start()
    {
        playerHealth.OnHealthChanged += UpdateHearts;
        UpdateHearts(playerHealth.currentHP, playerHealth.maxHP);
    }

    void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < max)
            {
                heartImages[i].gameObject.SetActive(true);
                heartImages[i].sprite = i < current ? fullHeart : emptyHeart;
            }
            else
            {
                heartImages[i].gameObject.SetActive(false); // nasconde cuori extra
            }
        }
    }
}