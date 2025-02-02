using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public int maxHealth = 3;
    public int health;
    public float invincibilityDuration = 1.5f;
    private float invincibilityDeltaTime = 0.4f;
    private SpriteRenderer spriteRenderer;
    HealthUI healthUI;
    private HitFlash HitFlash;
    private bool isInvincible = false;

    private bool isDead = false;
    GameOverController gameOverController;

    [Header("Game Over Sound Effect")]
    [SerializeField] private AudioClip gameOverSFX;

    void Awake()
    {
        //maxHealth = PlayerPrefs.GetInt("MaxHealth", 3);
        healthUI = FindAnyObjectByType<HealthUI>();
        // MARK: Set current healthnya ada di function ResetHealth()
        spriteRenderer = GetComponent<SpriteRenderer>();
        HitFlash = GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }

        gameOverController = FindAnyObjectByType<GameOverController>();
        gameOverController.gameObject.SetActive(false);
    }

    public void IncreaseHealthPoint(int amount)
    {
        maxHealth += amount;
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        
        healthUI.SetMaxHeart(maxHealth);
        healthUI.UpdateHearts(health);
    }

    public void Heal(int amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
         
        healthUI.UpdateHearts(health);
    }

    public void takeDamage(int damage){
        if(!isInvincible){
            HitFlash.TriggerFlash(0.1f);
            FindAnyObjectByType<HitStop>().Stop(0.05f);
            health -= damage;
            if (healthUI){
                healthUI.UpdateHearts(health);
            }
            if(health <= 0 && !isDead){
                isDead = true;

                // Play game over sfx
                SFXManager.instance.PlaySFXClip(gameOverSFX, transform, 0.5f);

                // Display game over screen
                gameOverController.gameObject.SetActive(true);
                gameOverController.GameOver();
                // Jangan pake Destroy karena tombol retry pas game over gabisa
                // TODO - Disable input & tambahin animasi player death
                // Destroy(gameObject);
                StartCoroutine(HandlePlayerDeath());
            }
            else
            {
                StartCoroutine(BecomeTemporarilyInvincible(invincibilityDuration));
            }
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }

    public IEnumerator BecomeTemporarilyInvincible(float invincibilityDuration)
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            HitFlash.TriggerFlash(0.2f);
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        isInvincible = false;
    }

    public void becomeInvulnerable()
    {
        isInvincible = true;

        HitFlash.giveOutline();
    }

    public void removeInvulnerable()
    {
        isInvincible = false;

        HitFlash.removeOutline();
    }

    public void ResetHealth()
    {
        // TODO: Ini cuma buat test healing system, kalo udah mau release jangan lupa di max Health
        health = maxHealth;
        if(healthUI){
            healthUI.SetMaxHeart(maxHealth);
        }
    }

    public void LoadData(GameData data)
    {
        this.health = data.healthAmount;
        this.maxHealth = data.maxHealth;
        this.gameObject.SetActive(true);
        removeInvulnerable();
        isDead = false;

        healthUI.SetMaxHeart(maxHealth);
        healthUI.UpdateHearts(health);
    }

    public void SaveData(GameData data)
    {
        data.healthAmount = this.health;
        data.maxHealth = this.maxHealth;

        data.playerScene = SceneManager.GetActiveScene().name;
    }
}
