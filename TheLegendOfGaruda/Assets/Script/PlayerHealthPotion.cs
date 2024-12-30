using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthPotion : MonoBehaviour
{
    public int maxPotion = 2;
    public int potions;

    public int healAmount = 1;
    public float healTime = 0.8f;

    private SpriteRenderer spriteRenderer;

    public HealPotionUI potionUI;
    public PlayerHealth playerHealth;

    private Rigidbody2D rb;

    [SerializeField] private InputActionAsset input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxPotion = PlayerPrefs.GetInt("maxPotion", 2); // Load saved maxPotion, default 3
        potionUI = FindAnyObjectByType<HealPotionUI>();
        ResetPotions();
        potionUI.UpdateHPotions(potions);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void ResetPotions()
    {
        potions = maxPotion;
        potionUI.SetMaxHealPotions(maxPotion);
    }

    public void IncreasePotionAmount(int amount)
    {
        maxPotion += amount;
        potions += amount;

        PlayerPrefs.SetInt("maxPotion", maxPotion);
        PlayerPrefs.Save();

        if (potions > maxPotion)
        {
            potions = maxPotion;
        }

        potionUI.SetMaxHealPotions(maxPotion);
        potionUI.UpdateHPotions(potions);
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && playerHealth.health < playerHealth.maxHealth)
        {
            if (potions > 0)
            {
                StartCoroutine(HealCoroutine());
            }
        }
    }

    private IEnumerator HealCoroutine()
    {
        input.Disable();

        yield return new WaitForSeconds(healTime);

        potions--;
        playerHealth.Heal(healAmount);
        potionUI.UpdateHPotions(potions);

        input.Enable();
    }
}
