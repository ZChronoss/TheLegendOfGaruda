using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthPotion : MonoBehaviour, IDataPersistence
{
    public int maxPotion = 2;
    public int potions;

    public int healAmount = 1;
    public float healTime = 1.6f;

    private SpriteRenderer spriteRenderer;

    public HealPotionUI potionUI;
    public PlayerHealth playerHealth;

    [SerializeField] private InputActionAsset input;
    TouchingDirections touchDir;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load saved maxPotion, default 3
        //maxPotion = PlayerPrefs.GetInt("maxPotion", 2); 
        //ResetPotions();
        //potionUI.UpdateHPotions(potions);
    }

    private void Awake()
    {
        potionUI = FindAnyObjectByType<HealPotionUI>();
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

    public void LoadData(GameData data)
    {
        this.potions = data.hPotionAmount;
        this.maxPotion = data.maxHPotion;

        potionUI.SetMaxHealPotions(maxPotion);
        potionUI.UpdateHPotions(potions);
    }

    public void SaveData(GameData data)
    {
        data.hPotionAmount = this.potions;
        data.maxHPotion = this.maxPotion;
    }
}
