using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPotionUI : MonoBehaviour
{
    public Image healPotionPrefab;
    public Sprite filledHealPotionSprite;
    public Sprite emptyHealPotionSprite;

    private List<Image> healPotions = new List<Image>();

    public void SetMaxHealPotions(int maxHPotions)
    {
        foreach (Image HPotion in healPotions)
        {
            Destroy(HPotion.gameObject);
        }

        healPotions.Clear();

        for (int i = 0; i < maxHPotions; i++)
        {
            Image newHPotions = Instantiate(healPotionPrefab, transform);
            newHPotions.sprite = filledHealPotionSprite;
            healPotions.Add(newHPotions);
        }
    }

    public void UpdateHPotions(int currentHPotions)
    {
        for (int i = 0; i < healPotions.Count; i++)
        {
            if (i < currentHPotions)
            {
                healPotions[i].sprite = filledHealPotionSprite;
            }
            else
            {
                healPotions[i].sprite = emptyHealPotionSprite;
            }


        }
    }
}
