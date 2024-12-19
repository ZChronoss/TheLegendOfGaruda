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
        potions = 2;
        potionUI.SetMaxHealPotions(maxPotion);
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
