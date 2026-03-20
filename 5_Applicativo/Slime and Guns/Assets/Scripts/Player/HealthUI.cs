using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] heartImages; // array di sprite cuori
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite halfHeart;

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
        // max e current sono in mezzi cuori, quindi il numero di icone è max/2 arrotondato su
        int totalIcons = Mathf.CeilToInt(max / 2f);

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i >= totalIcons)
            {
                heartImages[i].gameObject.SetActive(false);
                continue;
            }

            heartImages[i].gameObject.SetActive(true);

            // quanti mezzi cuori "cadono" in questo slot?
            int hpForThisSlot = current - (i * 2);

            if (hpForThisSlot >= 2)
                heartImages[i].sprite = fullHeart;
            else if (hpForThisSlot == 1)
                heartImages[i].sprite = halfHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }
}