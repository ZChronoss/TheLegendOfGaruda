using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthPotion : MonoBehaviour
{
    public int maxPotion = 2;
    public int potions;

    public int healAmount = 1;
    public float healTime = 1.6f;

    private SpriteRenderer spriteRenderer;

    public HealPotionUI potionUI;
    public PlayerHealth playerHealth;

    private Rigidbody2D rb;

    [SerializeField] private InputActionAsset input;
    TouchingDirections touchDir;
    Animator animator;

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
        touchDir = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    public void ResetPotions()
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
        if (context.started && playerHealth.health < playerHealth.maxHealth && touchDir.isGrounded)
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
        animator.SetTrigger(AnimationString.heal);
        yield return new WaitForSeconds(healTime);

        potions--;
        playerHealth.Heal(healAmount);
        potionUI.UpdateHPotions(potions);

        input.Enable();
    }
}
