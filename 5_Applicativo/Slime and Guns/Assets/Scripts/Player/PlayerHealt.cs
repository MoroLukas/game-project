using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;

    public int maxLives = 6;
    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            damage = -damage;
        }
        currentLives -= damage;

        if (currentLives < 0)
        {
            currentLives = 0;
        }
            

        UpdateHearts();
    }

    public int GetLives(int lives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }else
        {
            if (lives < 0)
            {
                lives = -lives;
            }
            currentLives += lives;
        }
            return currentLives;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i * 2 + 2 <= currentLives)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i * 2 < currentLives)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}