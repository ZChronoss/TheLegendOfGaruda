using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthPotion : MonoBehaviour
{
    public int maxPotion = 2;
    public int potions;

    public int healAmount = 1;

    private SpriteRenderer spriteRenderer;

    public HealPotionUI potionUI;
    public PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        potionUI = FindAnyObjectByType<HealPotionUI>();
        ResetPotions();
        potionUI.UpdateHPotions(potions);
    }

    void ResetPotions()
    {
        potions = 2;
        potionUI.SetMaxHealPotions(maxPotion);
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && playerHealth.health < playerHealth.maxHealth)
        {
            if (potions > 0)
            {
                potions--;
                playerHealth.Heal(healAmount);
                potionUI.UpdateHPotions(potions);
            }
        }
    }
}
